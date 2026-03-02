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
        /// 
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// Type name.
        /// </summary>
        /// 
        [Display(Name = "Name")]
        public string? Name { get; set; }

        /// <summary>
        /// Observations (max 1000 chars).
        /// </summary>
        /// 
        [Display(Name = "Observations")]
        public string? Observations { get; set; }

        /// <summary>
        /// Indicates if this inventory type represents On-Hand inventory.
        /// </summary>
        /// 
        [Display(Name = "Is onHand")]
        public bool IsOnHand { get; set; }

        /// <summary>
        /// Indicates if the record is active.
        /// </summary>
        /// 
        [Display(Name = "Is Active")]
        public bool Active { get; set; }

        /// <summary>
        /// Zebra integration association flag/value.
        /// </summary>
        /// 
        [Display(Name = "ZEBRA Code")]
        public int Zebrainvassociated { get; set; }

        /// <summary>
        /// Company id (tenant). Returned for reference (optional).
        /// NOTE: This is NOT accepted from client in Create/Update (STANDARD COMPANYID).
        /// </summary>
        /// 
        [Display(Name = "Company ID")]
        public int Idcompany { get; set; }

        [Display(Name ="Company Name")]
        public string? CompanyName { get; set; }
    }

}