namespace DUNES.API.Data.Audit
{
    /// <summary>
    /// Provides optional audit metadata for an entity so the audit pipeline can store
    /// business-friendly identifiers and module classification in <c>AuditLog</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface is especially useful in DB-First projects: implement it via <c>partial</c> classes
    /// (e.g., <c>EntityName.Audit.cs</c>) to avoid modifying scaffold-generated models.
    /// </para>
    /// <para>
    /// If an entity does not implement <see cref="IAuditMeta"/>, auditing still works normally:
    /// the interceptor will write the audit record using <c>SchemaName</c>, <c>TableName</c>, <c>PrimaryKey</c>,
    /// and JSON snapshots/deltas; only <c>Module</c> and <c>BusinessKey</c> will remain <c>null</c>.
    /// </para>
    /// <para>
    /// Typical examples of <c>BusinessKey</c> values:
    /// <c>Serial=...</c>, <c>Bin=...</c>, <c>OrderNo=...</c>, <c>RMA=...</c>.
    /// </para>
    /// </remarks>
    public interface IAuditMeta
    {

        /// <summary>
        /// Module associated with the table
        /// </summary>
        string? Module { get; }
        /// <summary>
        ///  Human/business identifier for fast searching without needing the database PK
        /// important data example (serial = 10101010 , deliveryid = ab9088 , shipmentnum = 'aw4854859'
        /// </summary>
        string? BusinessKey { get; }
    }
}
