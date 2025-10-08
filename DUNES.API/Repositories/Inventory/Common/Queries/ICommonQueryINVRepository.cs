using DUNES.API.Models.Inventory.Common;
using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;

namespace DUNES.API.Repositories.Inventory.Common.Queries
{ 
    /// <summary>
    /// All common inventory transactions queries
    /// </summary>
    public interface ICommonQueryINVRepository
    {
        /// <summary>
        /// Get all inventory transactions for a Document Number
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<TzebB2bReplacementPartsInventoryLogDto>> GetAllInventoryTransactionsByDocument(string DocumentNumber, CancellationToken ct);


        /// <summary>
        /// Get all inventory transactions for a Document Number and a search Start Date
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <param name="startDate"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<TzebB2bReplacementPartsInventoryLogDto>> GetAllInventoryTransactionsByDocumentStartDate(string DocumentNumber, DateTime startDate, CancellationToken ct);

        /// <summary>
        /// Get all inventory transactions for a Part Number ID
        /// </summary>
        /// <param name="PartNumberId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<TzebB2bReplacementPartsInventoryLogDto>> GetAllInventoryTransactionsByPartNumberId(int PartNumberId, CancellationToken ct);



        /// <summary>
        /// Get all Division for a company
        /// </summary>
        /// <param name="CompanyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<TdivisionCompany>> GetDivisionByCompanyClient(string CompanyClient, CancellationToken ct);



        /// <summary>
        /// Gell all (input, output) calls for an pick process
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <param name="processtype">(ASN-PICKPROCESS-B2B)</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ProcessCallsRead?> GetAllCalls(string DocumentId, string processtype, CancellationToken ct);
    }
}
