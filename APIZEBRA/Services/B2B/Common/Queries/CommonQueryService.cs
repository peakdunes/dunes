using APIZEBRA.DTOs.B2B;
using APIZEBRA.Models.B2b;
using APIZEBRA.Repositories.B2B.Common.Queries;
using APIZEBRA.Utils.Responses;

namespace APIZEBRA.Services.B2B.Common.Queries
{
    /// <summary>
    /// Provides query-related services for retrieving business data such as repair information.
    /// This layer handles any business logic and validation before returning results.
    /// </summary>
    public class CommonQueryService : ICommonQueryService
    {

        private readonly ICommonQueryRepository _repository;

        /// <summary>
        /// instance iqueryrepository interface
        /// </summary>
        /// <param name="repository"></param>
        public CommonQueryService(ICommonQueryRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// check in the RMA have all records in our database (4 tables)
        /// </summary>
        /// <param name="refNo"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> GetAllRMATablesCreatedAsync(int refNo)
        {
            var exists = await _repository.GetAllRMATablesCreatedAsync(refNo);

            if (!exists)
            {
                return ApiResponseFactory.NotFound<bool>($"The RMA {refNo} does not have records in all 4 tables.");
            }

            return ApiResponseFactory.Ok(true, "The RMA has records in all 4 tables.");
        }


        /// <summary>
        /// Get current area from sql function
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<AreaNameDto>> GetAreaByFunction(int repairNumber)
        {
            var data = await _repository.GetAreaByFunction(repairNumber);

            if (data == null)
            {
                return ApiResponseFactory.NotFound<AreaNameDto>(
                    $"This repair number ({repairNumber}) doesn't have an AREA defined."
                );
            }

            //Si sí hay info, la devolvemos con ApiResponse.Ok
            return ApiResponseFactory.Ok(data, "Repair information retrieved successfully");
        }

        /// <summary>
        /// return all information about one repair number
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<CheckRepairInformationDto>> GetRepairInfoAsync(int repairNumber)
        {

            var data = await _repository.GetRepairInfoAsync(repairNumber);

            if (data == null)
            {
                return ApiResponseFactory.NotFound<CheckRepairInformationDto>(
                    $"This repair number ({repairNumber}) doesn't have an RMA associated."
                );
            }

            //Si sí hay info, la devolvemos con ApiResponse.Ok
            return ApiResponseFactory.Ok(data, "Repair information retrieved successfully");

        }
        /// <summary>
        /// Get info about one repair when it's ready for receive
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<RepairReadyToReceiveDto>>> GetRepairReadyToReceive(string serialNumber)
        {
            var data = await _repository.GetRepairReadyToReceive(serialNumber);

            if (data.Count <= 0)
            {
                return ApiResponseFactory.NotFound<List<RepairReadyToReceiveDto>>(
                   $"This serial number ({serialNumber}) doesn't have receiving process pending."
               );


            }

            return ApiResponseFactory.Ok(data, "OK");
        }

        /// <summary>
        /// get RMA information for a serial number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<TzebInBoundRequestsFile>>> GetRMAReceivingInfo(string serialNumber)
        {
            var data = await _repository.GetRMAReceivingInfo(serialNumber);

            if (data == null)
            {
                return ApiResponseFactory.NotFound<List<TzebInBoundRequestsFile>>(
                   $"This serial number ({serialNumber}) doesn't have an RMA associated."
               );

            
            }

            return ApiResponseFactory.Ok(data, "OK");
        }

        async Task<ApiResponse<bool>> ICommonQueryService.GetAllRMATablesCreatedAsync(int refNo)
        {
            // Llamas al repository (que devuelve true o false)
            var exists = await _repository.GetAllRMATablesCreatedAsync(refNo);

            // Si NO existe info en las 4 tablas
            if (!exists)
                return ApiResponseFactory.NotFound<bool>($"The RMA {refNo} does not have records in all 4 tables.");

            // Si existe info en las 4 tablas
            return ApiResponseFactory.Ok(true, $"The RMA {refNo} has records in all 4 tables.");
        }
    }
}
