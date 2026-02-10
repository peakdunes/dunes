using System.ComponentModel.DataAnnotations;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Read DTO for Inventory Types (WMS).
    /// </summary>
    public class WMSInventoryTypesReadDTO
    {
        /// <summary>
        /// Database identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Type name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Observations (max 1000 chars).
        /// </summary>
        public string? Observations { get; set; }

        /// <summary>
        /// Indicates if this inventory type represents On-Hand inventory.
        /// </summary>
        public bool IsOnHand { get; set; }

        /// <summary>
        /// Indicates if the record is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Zebra integration association flag/value.
        /// </summary>
        public int Zebrainvassociated { get; set; }

        /// <summary>
        /// Company id (tenant). Returned for reference (optional).
        /// NOTE: This is NOT accepted from client in Create/Update (STANDARD COMPANYID).
        /// </summary>
        public int Idcompany { get; set; }
    }

}