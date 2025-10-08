using DUNES.API.Models.Inventory.ASN;

namespace DUNES.API.ReadModels.Inventory
{
    /// <summary>
    /// Model used for bring ASN Header and detail
    /// </summary>
    public class ASNRead
    {
        /// <summary>
        /// this brings header ASN Information
        /// </summary>
        public required TzebB2bAsnOutHdrDetItemInbConsReqs asnheader { get; set; }


        /// <summary>
        /// this brings ASN item detail
        /// </summary>
        public required List<TzebB2bAsnLineItemTblItemInbConsReqs> asnlistdetail { get; set; }    

    }
}
