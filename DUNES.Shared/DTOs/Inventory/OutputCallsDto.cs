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
        /// 
        [Display(Name ="Call ID")]
        public int Id { get; set; }

        /// <summary>
        /// type of call id
        /// </summary>
        /// 
        [Display(Name = "Type of Call")]
        public int TypeOfCallId { get; set; }

        /// <summary>
        /// type of call description
        /// </summary>
        [MaxLength(200)]
        [Display(Name = "Type Description")]
        public string? TypeOfCallDescription { get; set; }


        /// <summary>
        /// Date time Inserted
        /// </summary>
        /// 
        [Display(Name = "Inserted Date")]
        public DateTime DateTimeInserted { get; set; }

        /// <summary>
        /// call received yes or not 
        /// </summary>
        ///  
        [Display(Name = "Ack Reponse")]
        public bool? AckReceived { get; set; }


        /// <summary>
        /// call result (successful or fail)
        /// </summary>
        /// 
        [Display(Name = "Result")]
        public string? Result { get; set; }

        /// <summary>
        /// Date Time call send
        /// </summary>
        /// 
        [Display(Name = "Sent Date")]
        public DateTime? DateTimeSent { get; set; }


        /// <summary>
        /// call in process yes or not
        /// </summary>
        /// 
        [Display(Name = "Call In Process")]
        public bool InProcess { get; set; }

    }
}
