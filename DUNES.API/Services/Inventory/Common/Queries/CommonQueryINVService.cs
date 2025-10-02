using DUNES.API.Models.Inventory;
using DUNES.API.ReadModels.Inventory;
using DUNES.API.Repositories.Inventory.Common.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;

namespace DUNES.API.Services.Inventory.Common.Queries
{
    
    /// <summary>
    /// All common inventory transactions queries
    /// </summary>
    public class CommonQueryINVService : ICommonQueryINVService
    {

        private readonly ICommonQueryINVRepository _repository;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="repository"></param>
        public CommonQueryINVService(ICommonQueryINVRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// get all inventory transactions for a document number
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByDocument(string DocumentNumber, CancellationToken ct)
        {

            var infotransactions = await _repository.GetAllInventoryTransactionsByDocument(DocumentNumber, ct);

            if (infotransactions == null)
            {
                return ApiResponseFactory.NotFound<List<TzebB2bReplacementPartsInventoryLogDto>>(
                   $"There is not transactions for this document ({DocumentNumber}).");
            }

            return ApiResponseFactory.Ok(infotransactions!, "OK");
        }


        /// <summary>
        /// Get all inventory transactions for a Document Number and a search Start Date
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <param name="startDate"></param>
        /// <param name="ct"></param>
        /// <returns></returns>

        public async Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByDocumentStartDate(string DocumentNumber, DateTime startDate, CancellationToken ct)
        {

            var infotransactions = await _repository.GetAllInventoryTransactionsByDocumentStartDate(DocumentNumber, startDate, ct);

            if (infotransactions == null)
            {
                return ApiResponseFactory.NotFound<List<TzebB2bReplacementPartsInventoryLogDto>>(
                   $"There is not transactions for this document ({DocumentNumber}).");
            }

            return ApiResponseFactory.Ok(infotransactions!, "OK");
        }


        /// <summary>
        /// Get all inventory transactions for a Part Number ID
        /// </summary>
        /// <param name="PartNumberId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByPartNumberId(int PartNumberId, CancellationToken ct)
        {
            var infotransactions = await _repository.GetAllInventoryTransactionsByPartNumberId(PartNumberId, ct);

            if (infotransactions == null)
            {
                return ApiResponseFactory.NotFound<List<TzebB2bReplacementPartsInventoryLogDto>>(
                   $"There is not transactions for this Part Number Id ({PartNumberId}).");
            }

            return ApiResponseFactory.Ok(infotransactions!, "OK");
        }

        /// <summary>
        /// Get all Division for a company
        /// </summary>
        /// <param name="CompanyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<TdivisionCompanyDto>>> GetDivisionByCompanyClient(string CompanyClient, CancellationToken ct)
        {
            var infodivisions = await _repository.GetDivisionByCompanyClient(CompanyClient, ct);

            if (infodivisions == null)
            {
                return ApiResponseFactory.NotFound<List<TdivisionCompanyDto>>(
                   $"There is not division for this company ({CompanyClient}).");
            }

            List<TdivisionCompanyDto> listdiv = new List<TdivisionCompanyDto>();
            TdivisionCompanyDto objdet = new TdivisionCompanyDto();

            foreach (var company in infodivisions)
            {
                objdet.CompanyDsc = company.CompanyDsc;
                objdet.DivisionDsc = company.DivisionDsc;

                listdiv.Add(objdet);
            }

            return ApiResponseFactory.Ok(listdiv!, "OK");
        }


        /// <summary>
        /// Get all input - output calls for a pick process
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <param name="processtype"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PickProcessCallsReadDto>> GetAllCalls(string DocumentId,string processtype, CancellationToken ct)
        {

            List<InputCallsDto> listinputcalls = new List<InputCallsDto>();
            List<OutputCallsDto> listoutputcalls = new List<OutputCallsDto>();

            PickProcessCallsReadDto objdto = new PickProcessCallsReadDto();

            var info = await _repository.GetAllCalls(DocumentId, processtype, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<PickProcessCallsReadDto>(
                    $"There is not call information for this Pick Process number ({DocumentId}).");
            }


            List<InputCallsDto> listInputCalls = new List<InputCallsDto>();

            List<OutputCallsDto> listOutputCalls = new List<OutputCallsDto>();


            if (info.inputCalls != null)
            {
                foreach (var call in info.inputCalls)
                {
                    InputCallsDto objin = new InputCallsDto();

                    objin.Id = call.Id;
                    objin.TransactionCode = call.TransactionCode;
                    objin.DateTimeInserted = call.DateTimeInserted;
                    objin.Error = call.Error;
                    objin.Processed = call.Processed;

                    listInputCalls.Add(objin);
                }
            }




            if (info.outputCalls != null)
            {
                foreach (var call in info.outputCalls)
                {
                    OutputCallsDto objout = new OutputCallsDto();



                    objout.Id = call.Id;
                    objout.TypeOfCallId = call.TypeOfCallId;
                    objout.TypeOfCallDescription = call.callName;
                    objout.DateTimeInserted = call.DateTimeInserted;
                    objout.AckReceived = call.AckReceived;
                    objout.Result = call.Result;
                    objout.DateTimeSent = call.DateTimeSent;
                    objout.InProcess = call.InProcess;

                    listOutputCalls.Add(objout);
                }
            }

            objdto.inputCallsList = listInputCalls;
            objdto.outputCallsList = listOutputCalls;

            return ApiResponseFactory.Ok(objdto, "OK");
        }

    }
}
