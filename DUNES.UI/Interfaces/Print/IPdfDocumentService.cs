using DUNES.Shared.TemporalModels;

namespace DUNES.UI.Interfaces.Print
{
    public interface IPdfDocumentService
    {

        /// <summary>
        /// create a Packing List
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task CreatePackingListPdfAsync(string fullPath, TorderRepairTm model, CancellationToken ct);
    }
}
