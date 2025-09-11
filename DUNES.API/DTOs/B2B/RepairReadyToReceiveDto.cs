using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.DTOs.B2B
{

    /// <summary>
    /// Reapair order ready to receive DTO table _Trepair
    /// </summary>
   
    public class RepairReadyToReceiveDto
    {

        public int Id { get; set; }

        /// <summary>
        /// serial number inside device
        /// </summary>
        [MaxLength(100)]
        public string SerialINBOUND { get; set; }

        /// <summary>
        /// reference number
        /// </summary>
        [MaxLength(50)]
        public string Ref_No { get; set; }

        /// <summary>
        /// Repair number
        /// </summary>
        [MaxLength(20)]
        public string Repair_No { get; set; }

        /// <summary>
        /// Device type
        /// </summary>
        [MaxLength(50)]
        public string Part_No { get; set; }


        /// <summary>
        /// Device dscription
        /// </summary>
        [MaxLength(250)]
        public string Part_DSC { get; set; }

        /// <summary>
        /// Serial number
        /// </summary>
        [MaxLength(50)]
        public string SerialRECEIVED { get; set; }


        [MaxLength(50)]
        public string UnitID { get; set; }

        /// <summary>
        /// company client
        /// </summary>
        [MaxLength(200)]
        public string Company_DSC { get; set; }

        /// <summary>
        /// Division Company
        /// </summary>
        [MaxLength(200)]
        public string Division { get; set; }


        /// <summary>
        /// Is Unit in stock (what stock)
        /// </summary>
        [MaxLength(200)]
        public string Spare_Pool_id { get; set; }


        /// <summary>
        /// Customer reference
        /// </summary>
        [MaxLength(200)]
        public string Cust_Ref { get; set; }
    }
}
