using DUNES.API.Models.Inventory;

namespace DUNES.API.ReadModels.Inventory
{
    /// <summary>
    /// input and output pickprocess calls 
    /// </summary>
    public class PickProcessCallsRead
    {
        /// <summary>
        /// ZEBRA to Peak list calls
        /// </summary>
      public  List<TzebB2bInbConsReqs>? inputCalls { get; set; }

        /// <summary>
        /// Peak to ZEBRA list calls
        /// </summary>
       public List<TzebB2bOutConsReqs>? outputCalls { get; set; }
    }
}
