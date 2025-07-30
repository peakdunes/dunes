namespace DUNES.API.DTOs.B2B
{

    /// <summary>
    /// TorderRepairHdr Dates DTO
    /// </summary>
    public class TorderRepairHdrDatesDto
    {
        public int RefNo { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? CanceledDate { get; set; }

        public DateTime? StopDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public DateTime? DateSaved { get; set; }

        public DateTime? EmailResponseDateTime { get; set; }

        public DateTime? DateInserted { get; set; }

        public DateTime? ReceivingStartDate { get; set; }

        public DateTime? ReceivingEndDate { get; set; }

    }
}
