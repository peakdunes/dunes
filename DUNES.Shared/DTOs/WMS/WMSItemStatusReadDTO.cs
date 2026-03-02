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
        /// 
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// Status name.
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
        /// Indicates if the record is active.
        /// </summary>
        /// 
        [Display(Name = "Active")]
        public bool Active { get; set; }

        /// <summary>
        /// Company id (tenant). Returned for reference (optional).
        /// NOTE: This is NOT accepted from client in Create/Update (STANDARD COMPANYID).
        /// </summary>
        [Display(Name = "Company ID")]
        public int Idcompany { get; set; }

        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }
    }

}
