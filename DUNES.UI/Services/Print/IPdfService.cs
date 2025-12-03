using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;

namespace DUNES.UI.Services.Print
{
    public interface IPdfService
    {
        /// <summary>
        /// Genera un PDF de Packing List, lo guarda en disco y devuelve la ruta completa.
        /// </summary>
        Task<ApiResponse<string>> GeneratePackingList(string userName, TorderRepairTm model, CancellationToken ct);
    }
}
