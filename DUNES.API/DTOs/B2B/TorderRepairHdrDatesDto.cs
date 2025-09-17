namespace DUNES.API.DTOs.B2B
{

    /// <summary>
    /// TorderRepairHdr Dates DTO
    /// </summary>
    public class TorderRepairHdrDatesDto
    {

        /// <summary>
        /// Reference Number
        /// </summary>
        public int RefNo { get; set; }

        /// <summary>
        /// Date Created
        /// </summary>
        public DateTime? DateCreated { get; set; }
        /// <summary>
        /// Date Canceled
        /// </summary>
        public DateTime? CanceledDate { get; set; }
        /// <summary>
        /// Stop Date
        /// </summary>
        public DateTime? StopDate { get; set; }

        /// <summary>
        /// Close Date
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// Saved Date
        /// </summary>
        public DateTime? DateSaved { get; set; }
        /// <summary>
        /// Email Response Date
        /// </summary>
        public DateTime? EmailResponseDateTime { get; set; }
        /// <summary>
        /// Date Inserted
        /// </summary>
        public DateTime? DateInserted { get; set; }

        /// <summary>
        /// Date Receiving
        /// </summary>
        public DateTime? ReceivingStartDate { get; set; }

        /// <summary>
        /// End Date
        /// </summary>
        public DateTime? ReceivingEndDate { get; set; }

    }
}
