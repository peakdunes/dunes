namespace DUNES.API.DTOs.Inventory
{
    /// <summary>
    /// ZEBRA to PEAK calls table model (_TZEB_B2B_Inb_Cons_Reqs)
    /// </summary>
    public class InputCallsDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Type call (PICK CONFIRM, SHIP CONFIRM)
        /// </summary>
        public string TransactionCode { get; set; } = string.Empty;

        /// <summary>
        /// Date time inserted
        /// </summary>
        public DateTime DateTimeInserted { get; set; }

        /// <summary>
        /// Error string
        /// </summary>
        public string? Error { get; set; }


        /// <summary>
        /// processed true o false
        /// </summary>
        public bool Processed { get; set; }
    }
}
