using DUNES.API.Models.Inventory.Common;

namespace DUNES.API.ReadModels.Inventory
{
    /// <summary>
    /// input and output pickprocess calls 
    /// </summary>
    public class ProcessCallsRead
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
