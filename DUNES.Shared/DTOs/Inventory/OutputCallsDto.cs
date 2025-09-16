using System.ComponentModel.DataAnnotations;

namespace DUNES.Shared.DTOs.Inventory
{

    /// <summary>
    /// calls Peak to ZEBRA model TzebB2bOutConsReqs
    /// </summary>
    public class OutputCallsDto
    {

        /// <summary>
        /// call id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// type of call id
        /// </summary>
        public int TypeOfCallId { get; set; }

        /// <summary>
        /// type of call description
        /// </summary>
        [MaxLength(200)]
        public string? TypeOfCallDescription { get; set; }


        /// <summary>
        /// Date time Inserted
        /// </summary>
        public DateTime DateTimeInserted { get; set; }

        /// <summary>
        /// call received yes or not 
        /// </summary>
        public bool? AckReceived { get; set; }


        /// <summary>
        /// call result (successful or fail)
        /// </summary>
        public string? Result { get; set; }

        /// <summary>
        /// Date Time call send
        /// </summary>
        public DateTime? DateTimeSent { get; set; }


        /// <summary>
        /// call in process yes or not
        /// </summary>
        public bool InProcess { get; set; }

    }
}
