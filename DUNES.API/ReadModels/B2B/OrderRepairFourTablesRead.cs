using DUNES.API.Models.B2b;

namespace DUNES.API.ReadModels.B2B
{

    /// <summary>
    /// Display the most important 4 tables associated with a order in Servtrak
    /// when this order is create from pick process
    /// </summary>
    public class OrderRepairFourTablesRead
    {
        /// <summary>
        /// Order Repair Head
        /// </summary>
        public TorderRepairHdr? OrHdr { get; set; }

        /// <summary>
        /// items detail
        /// </summary>
        public List<TorderRepairItems>? ItemList { get; set; }
        /// <summary>
        /// info by item for receiving
        /// </summary>
        public List<TorderRepairItemsSerialsReceiving>? ReceivingList { get; set;}
        /// <summary>
        /// info by item for shipping
        /// </summary>
        public List<TorderRepairItemsSerialsShipping>? ShippingList { get; set; }
    }
}
