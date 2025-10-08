using DUNES.API.Models.Inventory.PickProcess;

namespace DUNES.API.ReadModels.Inventory
{
    /// <summary>
    /// All information about PICK PROCESS
    /// </summary>
    public class PickProcessRead
    {
        /// <summary>
        /// Pick Process Head
        /// </summary>
        public required TzebB2bPSoWoHdrTblItemInbConsReqsLog pickHdr { get; set; }
        /// <summary>
        /// Pick Process Detail
        /// </summary>
        public required List<TzebB2bPSoLineItemTblItemInbConsReqsLog> pickdetails { get; set; }
    }
}
