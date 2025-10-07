using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ReadModels.Inventory
{
    /// <summary>
    /// all information for a begin ASN Receiving process
    /// </summary>
    public class AsnReceivedHdrLogRead
    {
        /// <summary>
        /// ASN number
        /// </summary>
        [MaxLength(50)]
        public string asnNumber { get; set; } = string.Empty;
        /// <summary>
        /// Transaction Code
        /// </summary>
        [MaxLength(50)]
        public string TransactionCode { get; set; } = string.Empty;

        /// <summary>
        /// Transaction Type
        /// </summary>
        [MaxLength(50)]
        public string TransactionType { get; set; } = string.Empty;

        /// <summary>
        /// Process Name 
        /// </summary>
        [MaxLength(50)]
        public string ProcessName { get; set; } = string.Empty;

        /// <summary>
        /// Transaction Origen (DBK)
        /// </summary>
        [MaxLength(50)]
        public string org3pl { get; set; } = string.Empty;


        /// <summary>
        /// 3pl locator
        /// </summary>
        [MaxLength(50)]
        public string locator3pl { get; set; } = string.Empty;

        /// <summary>
        /// Id RTV
        /// </summary>
        public bool IsRtvPart { get; set; }

        /// <summary>
        /// Is CE Project
        /// </summary>
        public bool IsCePart { get; set; }

    }
}
