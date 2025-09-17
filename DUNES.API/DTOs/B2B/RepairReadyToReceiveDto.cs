using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.DTOs.B2B
{

    /// <summary>
    /// Reapair order ready to receive DTO table _Trepair
    /// </summary>
   
    public class RepairReadyToReceiveDto
    {
        /// <summary>
        /// Table ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// serial number inside device
        /// </summary>
        [MaxLength(100)]
        public string SerialINBOUND { get; set; } = string.Empty;

        /// <summary>
        /// reference number
        /// </summary>
        [MaxLength(50)]
        public string Ref_No { get; set; } = string.Empty;

        /// <summary>
        /// Repair number
        /// </summary>
        [MaxLength(20)]
        public string Repair_No { get; set; } = string.Empty;

        /// <summary>
        /// Device type
        /// </summary>
        [MaxLength(50)]
        public string Part_No { get; set; } = string.Empty;


        /// <summary>
        /// Device dscription
        /// </summary>
        [MaxLength(250)]
        public string Part_DSC { get; set; } = string.Empty;

        /// <summary>
        /// Serial number
        /// </summary>
        [MaxLength(50)]
        public string SerialRECEIVED { get; set; } = string.Empty;


        [MaxLength(50)]
        public string UnitID { get; set; } = string.Empty;

        /// <summary>
        /// company client
        /// </summary>
        [MaxLength(200)]
        public string Company_DSC { get; set; } = string.Empty;

        /// <summary>
        /// Division Company
        /// </summary>
        [MaxLength(200)]
        public string Division { get; set; } = string.Empty;


        /// <summary>
        /// Is Unit in stock (what stock)
        /// </summary>
        [MaxLength(200)]
        public string Spare_Pool_id { get; set; } = string.Empty;


        /// <summary>
        /// Customer reference
        /// </summary>
        [MaxLength(200)]
        public string Cust_Ref { get; set; } = string.Empty;
    }
}
