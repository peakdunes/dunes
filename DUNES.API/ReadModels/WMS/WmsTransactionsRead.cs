using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.ReadModels.WMS
{
    /// <summary>
    /// All WMS Transactions for a document
    /// </summary>
    public class WmsTransactionsRead
    {
        /// <summary>
        /// Header transactions
        /// </summary>
       public List<InventorytransactionHdr>? ListHdr { get; set; }
        /// <summary>
        /// Detail transactions
        /// </summary>
        public List<InventorytransactionDetail>? ListDetail { get; set; }
        /// <summary>
        /// Movement Detail
        /// </summary>
        public List<Inventorymovement>? ListMovement { get; set; }
    }
}
