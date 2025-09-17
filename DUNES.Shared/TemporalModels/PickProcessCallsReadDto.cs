
using DUNES.Shared.DTOs.Inventory;

namespace DUNES.API.ReadModels.Inventory
{
    /// <summary>
    /// input and output pickprocess calls 
    /// </summary>
    public class PickProcessCallsReadDto
    {
        /// <summary>
        /// ZEBRA to Peak list calls
        /// </summary>
        public List<InputCallsDto> inputCallsList { get; set; } = new();

        /// <summary>
        /// Peak to ZEBRA list calls
        /// </summary>
        public List<OutputCallsDto> outputCallsList { get; set; } = new();
    }
}
