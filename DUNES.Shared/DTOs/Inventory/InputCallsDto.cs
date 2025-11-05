using System.ComponentModel.DataAnnotations;

namespace DUNES.Shared.DTOs.Inventory
{
    /// <summary>
    /// ZEBRA to PEAK calls table model (_TZEB_B2B_Inb_Cons_Reqs)
    /// </summary>
    public class InputCallsDto
    {
        /// <summary>
        /// ID
        /// </summary>
        /// 
        [Display(Name = "Call ID")]
        public int Id { get; set; }

        /// <summary>
        /// Type call (PICK CONFIRM, SHIP CONFIRM)
        /// </summary>
        /// 
        [Display(Name = "Transaction Code")]
        public string TransactionCode { get; set; } = string.Empty;

        /// <summary>
        /// Date time inserted
        /// </summary>
        /// 
        [Display(Name = "Date Inserted")]
        public DateTime DateTimeInserted { get; set; }

        /// <summary>
        /// Error string
        /// </summary>
        /// 
        [Display(Name = "Error Message")]

        public string? Error { get; set; }


        [Display(Name = "Is Processed")]

        /// <summary>
        /// processed true o false
        /// </summary>
        public bool Processed { get; set; }
    }
}
