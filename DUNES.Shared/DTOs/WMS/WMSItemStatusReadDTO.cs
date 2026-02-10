using System.ComponentModel.DataAnnotations;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Read DTO for Item Status (WMS).
    /// </summary>
    public class WMSItemStatusReadDTO
    {
        /// <summary>
        /// Database identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Status name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Observations (max 1000 chars).
        /// </summary>
        public string? Observations { get; set; }

        /// <summary>
        /// Indicates if the record is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Company id (tenant). Returned for reference (optional).
        /// NOTE: This is NOT accepted from client in Create/Update (STANDARD COMPANYID).
        /// </summary>
        public int Idcompany { get; set; }
    }

}
