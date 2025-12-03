using DUNES.API.DTOs.B2B;
using DUNES.API.Models.B2b;
using DUNES.API.ReadModels.B2B;
using DUNES.API.Repositories.B2B.Common.Queries;

using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Services.B2B.Common.Queries
{
    /// <summary>
    /// Provides query-related services for retrieving business data such as repair information.
    /// This layer handles any business logic and validation before returning results.
    /// </summary>
    public class CommonQueryB2BService : ICommonQueryB2BService
    {

        private readonly ICommonQueryB2BRepository _repository;

        /// <summary>
        /// instance iqueryrepository interface
        /// </summary>
        /// <param name="repository"></param>
        public CommonQueryB2BService(ICommonQueryB2BRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// get all date fields from a servtrack order 
        /// </summary>
        /// <param name="refNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<TorderRepairHdrDatesDto>> GetAllDateFieldsRepair(int refNumber, CancellationToken ct)
        {
            var info = await _repository.GetAllDateFieldsRepair(refNumber);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<TorderRepairHdrDatesDto>(
                    $"This Reference number ({refNumber}) doesn't exist in our system.");
            }

            return ApiResponseFactory.Ok(info, "The RMA has records in all 4 tables.");
        }

        /// <summary>
        /// check in the RMA have all records in our database (4 tables)
        /// </summary>
        /// <param name="refNo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> GetAllRMATablesCreatedAsync(int refNo, CancellationToken ct)
        {
            var exists = await _repository.GetAllRMATablesCreatedAsync(refNo);

            if (!exists)
            {
                return ApiResponseFactory.NotFound<bool>($"The RMA {refNo} does not have records in all 4 tables.");
            }

            return ApiResponseFactory.Ok(true, "Reference information retrieved successfully");
        }

        /// <summary>
        /// Displays the 4 tables associated with an order in Servtrack.
        /// _TOrderRepair_Hdr
        /// _TorderRepair_ItemsSerials_Receiving
        /// _TorderRepair_ItemsSerials_Shipping 
        /// _TOrderRepair_Items
        /// </summary>
        /// <param name="refNo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<OrderRepairFourTablesRead>> GetAllTablesOrderRepairCreatedAsync(int refNo, CancellationToken ct)
        {
            var info = await _repository.GetAllTablesOrderRepairCreatedAsync(refNo);

            if (info.OrHdr == null)
            {
                return ApiResponseFactory.NotFound<OrderRepairFourTablesRead>($"The Reference Number {refNo} does not exist in our system.");
            }

            return ApiResponseFactory.Ok(info, "Reference information retrieved successfully");
        }


        /// <summary>
        /// Get current area from sql function
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<AreaNameDto>> GetAreaByFunction(int repairNumber, CancellationToken ct)
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
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<CheckRepairInformationDto>> GetRepairInfoAsync(int repairNumber, CancellationToken ct)
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
        /// get RMA information for a serial number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<TzebInBoundRequestsFile>>> GetRMAReceivingInfo(string serialNumber, CancellationToken ct   )
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
        /// <summary>
        /// Check if a RMA was created correcty in our database (4 tables)
        /// </summary>
        /// <param name="refNo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        async Task<ApiResponse<bool>> ICommonQueryB2BService.GetAllRMATablesCreatedAsync(int refNo, CancellationToken ct)
        {
            // Llamas al repository (que devuelve true o false)
            var exists = await _repository.GetAllRMATablesCreatedAsync(refNo);

            // Si NO existe info en las 4 tablas
            if (!exists)
                return ApiResponseFactory.NotFound<bool>($"The RMA {refNo} does not have records in all 4 tables.");

            // Si existe info en las 4 tablas
            return ApiResponseFactory.Ok(true, $"The RMA {refNo} has records in all 4 tables.");
        }

        /// <summary>
        /// Get info about one repair when it's ready for receive
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<RepairReadyToReceiveDto>>> GetRepairReadyToReceive(string serialNumber, CancellationToken ct)
        {
            string errormessage = string.Empty;

            List<RepairReadyToReceiveDto> listobjresult = new List<RepairReadyToReceiveDto>();

            if (serialNumber.Length > 14)
            {
                return ApiResponseFactory.Fail<List<RepairReadyToReceiveDto>>(
                    "Invalid serial number. It must be up to 14 digits.",
                    "Validation",
                    400
                );
            }


            var data = await _repository.GetRepairReadyToReceive(serialNumber);

            if (data.Count <= 0)
            {
                return ApiResponseFactory.NotFound<List<RepairReadyToReceiveDto>>(
                   $"This serial number ({serialNumber}) doesn't have receiving process pending."
               );


            }
            else
            {
                //we validate the data received

                TorderRepairHdrDatesDto repairinfo = new TorderRepairHdrDatesDto();



                string reference = string.Empty;

                foreach (var c in data)
                {

                    if (c.SerialINBOUND.Trim().ToUpper() == serialNumber.Trim().ToUpper())
                    {
                        RepairReadyToReceiveDto objresult1 = new RepairReadyToReceiveDto();


                        reference = c.Ref_No.Trim();

                        objresult1.Id = c.Id;

                        objresult1.SerialINBOUND = c.SerialINBOUND;

                        objresult1.Ref_No = c.Ref_No;

                        objresult1.Repair_No = c.Repair_No;

                        objresult1.Part_No = c.Part_No;

                        objresult1.Part_DSC = c.Part_DSC;

                        objresult1.SerialRECEIVED = c.SerialRECEIVED;

                        objresult1.UnitID = c.UnitID;

                        objresult1.Company_DSC = c.Company_DSC;

                        objresult1.Division = c.Division;

                        listobjresult.Add(objresult1);

                        repairinfo = await _repository.GetAllDateFieldsRepair(Convert.ToInt32(reference));


                        if (repairinfo != null)
                        {

                            if (repairinfo.CanceledDate != null)
                            {
                                errormessage = "Order Canceled";
                            }

                            if (repairinfo.StopDate != null)
                            {
                                errormessage = "Order Stopped";
                            }

                            if (repairinfo.CloseDate != null)
                            {
                                errormessage = "Order Closed";
                            }
                        }
                    }

                }
            }

            if (!string.IsNullOrEmpty(errormessage))
            {
                return ApiResponseFactory.Fail<List<RepairReadyToReceiveDto>>(errormessage, "Business Rule", 409);
            }
            return ApiResponseFactory.Ok(listobjresult, "Repair ready to receive");
        }
    }
}
