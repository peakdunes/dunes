namespace DUNES.API.ModelsWMS.Admin
{

    /// <summary>
    /// auditory transactions
    /// </summary>
    public class AuditLog
    {
        /// <summary>
        /// identity id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Event date
        /// </summary>
        public DateTime EventDateUtc { get; set; } 

        /// <summary>
        /// INSERT / UPDATE / DELETE
        /// </summary>
        public string EventType { get; set; } = default!; 

        /// <summary>
        /// schema database
        /// </summary>
        public string SchemaName { get; set; } = "dbo";

        /// <summary>
        /// table name
        /// </summary>
        public string TableName { get; set; } = default!;

        /// <summary>
        /// PK representada como texto para soportar PK compuesta. Ej: "Id=15" o "CompanyId=7|Id=15"
        /// </summary>
        public string? PrimaryKey { get; set; }

        /// <summary>
        /// Usuario autenticado que ejecutó la operación (claim, session, etc.)
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// request transaction ID
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// ipclient
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Nombre de la app/capa (ej. DUNES.API)
        /// </summary>
        public string? AppName { get; set; }

        /// <summary>
        /// Para agrupar múltiples cambios dentro de una misma operación lógica
        /// procesos que incluyen mas de una tabla se guarda un los por cada CUD en cada tabla
        /// y esten numero los correlaciona como una unica transaccion
        /// </summary>
        public Guid? CorrelationId { get; set; }

        /// <summary>
        /// Para minería: módulo funcional (ej. WMS, REPAIR, MASTERS)
        /// </summary>
        public string? Module { get; set; }

        /// <summary>
        /// Para minería: clave de negocio rápida (ej. Serial=..., OrderNo=..., Code=...)
        /// para guardar un numero de serial o un shipment num, o delivery id que nos ayude a la busqueda
        /// </summary>
        public string? BusinessKey { get; set; }

        /// <summary>
        /// Solo para UPDATE: lista de columnas cambiadas (CSV o JSON array si prefieres)
        /// </summary>
        public string? ChangedColumns { get; set; }

        /// <summary>
        /// UPDATE: delta anterior; DELETE: snapshot completo; INSERT: null
        /// </summary>
        public string? JsonOld { get; set; }

        /// <summary>
        /// UPDATE: delta nuevo; INSERT: snapshot completo; DELETE: null
        /// </summary>
        public string? JsonNew { get; set; }
    }
}
