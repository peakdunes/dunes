using DUNES.API.Data.Audit;
using DUNES.API.ModelsWMS.Admin;
using DUNES.Shared.Interfaces.AuditContext;
using DUNES.Shared.Interfaces.RequestInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DUNES.API.Data.Interceptors
{
    /// <summary>
    /// intercepta el savechanges de EntityFramework, filtra los CUD (Create, Update, Delete) y
    /// arma el log que inserta en la tabla de auditoria AuditLog.
    /// </summary>
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IAuditContext _audit;

        // Opcional: campos que NO quieres auditar en UPDATE (típicos)
        private static readonly HashSet<string> _ignoreUpdateProps = new(StringComparer.OrdinalIgnoreCase)
        {
            "UpdatedAt", "UpdatedBy", "ModifiedAt", "ModifiedBy"
        };

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = false
        };

        /// <summary>
        /// ctor
        /// </summary>
        public AuditSaveChangesInterceptor(IAuditContext audit)
        {
            _audit = audit;
        }

        /// <summary>
        /// Sync: intercepta SaveChanges().
        /// </summary>
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            var context = eventData.Context;
            if (context == null) return result;

            AddAuditLogs(context);

            return result;
        }

        /// <summary>
        /// Async: intercepta SaveChangesAsync(). (Este override es CRÍTICO para que se inserte auditoría cuando tu repo usa async).
        /// </summary>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null) return ValueTask.FromResult(result);

            AddAuditLogs(context);

            return ValueTask.FromResult(result);
        }

        private void AddAuditLogs(DbContext context)
        {
            // Importante: DetectChanges para asegurar que EF tenga bien el ChangeTracker
            context.ChangeTracker.DetectChanges();

            var correlationId = _audit.CorrelationId;

            var auditEntries = new List<AuditLog>();

            var entries = context.ChangeTracker.Entries()
                .Where(e =>
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                // No auditamos la propia auditoría
                if (entry.Entity is AuditLog) continue;

                // Si es una entidad sin tabla (query type) o similar, se ignora
                if (entry.Metadata.GetTableName() is null) continue;

                var schema = entry.Metadata.GetSchema() ?? "dbo";
                var table = entry.Metadata.GetTableName()!;

                var eventType = entry.State switch
                {
                    EntityState.Added => "INSERT",
                    EntityState.Modified => "UPDATE",
                    EntityState.Deleted => "DELETE",
                    _ => null
                };

                if (eventType == null) continue;

                var pkText = BuildPrimaryKeyText(entry);

                var module = TryGetModule(entry.Entity);
                var businessKey = TryGetBusinessKey(entry.Entity);

                string? changedColumns = null;
                string? jsonOld = null;
                string? jsonNew = null;

                if (eventType == "INSERT")
                {
                    jsonNew = SerializeSnapshot(entry.CurrentValues, includeAllProperties: true);
                }
                else if (eventType == "DELETE")
                {
                    jsonOld = SerializeSnapshot(entry.OriginalValues, includeAllProperties: true);
                }
                else if (eventType == "UPDATE")
                {
                    var delta = BuildUpdateDelta(entry);
                    if (delta == null)
                    {
                        // No hay cambios reales
                        continue;
                    }

                    changedColumns = string.Join(",", delta.ChangedColumns);
                    jsonOld = JsonSerializer.Serialize(delta.OldValues, _jsonOptions);
                    jsonNew = JsonSerializer.Serialize(delta.NewValues, _jsonOptions);
                }

                if (jsonOld == null && jsonNew == null) continue;

                auditEntries.Add(new AuditLog
                {
                    EventDateUtc = DateTime.UtcNow,
                    EventType = eventType,

                    SchemaName = schema,
                    TableName = table,
                    PrimaryKey = pkText,

                    UserName = _audit.UserName,
                    TraceId = _audit.TraceId,
                    IpAddress = _audit.IpAddress,
                    AppName = _audit.AppName ?? "DUNES.API",
                    CorrelationId = correlationId,

                    Module = module,
                    BusinessKey = businessKey,
                    ChangedColumns = changedColumns,

                    JsonOld = jsonOld,
                    JsonNew = jsonNew
                });
            }

            if (auditEntries.Count > 0)
            {
                context.Set<AuditLog>().AddRange(auditEntries);
            }
        }

        // ---------- Helpers ----------

        private static string? BuildPrimaryKeyText(EntityEntry entry)
        {
            var pk = entry.Metadata.FindPrimaryKey();
            if (pk == null || pk.Properties.Count == 0) return null;

            var parts = pk.Properties
                .OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                .Select(p =>
                {
                    var propName = p.Name;
                    var value = entry.Property(propName).CurrentValue ?? entry.Property(propName).OriginalValue;
                    return $"{propName}={value}";
                });

            return string.Join("|", parts);
        }

        private static string SerializeSnapshot(PropertyValues values, bool includeAllProperties)
        {
            var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

            foreach (var prop in values.Properties)
            {
                dict[prop.Name] = values[prop];
            }

            return JsonSerializer.Serialize(dict, _jsonOptions);
        }

        private static UpdateDelta? BuildUpdateDelta(EntityEntry entry)
        {
            var changed = new List<string>();
            var oldDict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            var newDict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

            foreach (var prop in entry.Properties)
            {
                if (prop.Metadata.IsPrimaryKey()) continue;
                if (_ignoreUpdateProps.Contains(prop.Metadata.Name)) continue;
                if (!prop.IsModified) continue;

                var oldVal = prop.OriginalValue;
                var newVal = prop.CurrentValue;

                if (ValuesEqual(oldVal, newVal)) continue;

                changed.Add(prop.Metadata.Name);
                oldDict[prop.Metadata.Name] = oldVal;
                newDict[prop.Metadata.Name] = newVal;
            }

            if (changed.Count == 0) return null;

            return new UpdateDelta(changed, oldDict, newDict);
        }

        private static bool ValuesEqual(object? a, object? b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;

            if (a is DateTime da && b is DateTime db) return da.ToUniversalTime() == db.ToUniversalTime();
            if (a is decimal ma && b is decimal mb) return ma == mb;

            return a.Equals(b);
        }

        private static string? TryGetModule(object entity)
            => entity is IAuditMeta meta ? meta.Module : null;

        private static string? TryGetBusinessKey(object entity)
            => entity is IAuditMeta meta ? meta.BusinessKey : null;

        private sealed record UpdateDelta(
            List<string> ChangedColumns,
            Dictionary<string, object?> OldValues,
            Dictionary<string, object?> NewValues
        );
    }
}
