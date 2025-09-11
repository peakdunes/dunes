using DUNES.API.Data;
using DUNES.API.DTOs.B2B;
using DUNES.API.Models.B2b;
using DUNES.API.Models.B2B;
using DUNES.API.ReadModels.B2B;
using DUNES.API.Utils.Responses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Repositories.B2B.Common.Queries
{

    /// <summary>
    /// all B2B queries
    /// </summary>
    public class CommonQueryB2BRepository : ICommonQueryB2BRepository
    {

        private readonly AppDbContext _context;
        private readonly appWmsDbContext _wmscontext;

        /// <summary>
        /// initialize dbcontext
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wmscontext"></param>
        public CommonQueryB2BRepository(AppDbContext context, appWmsDbContext wmscontext)
        {
            _context = context;
            _wmscontext = wmscontext;
        }
        /// <summary>
        /// get all date fields for a Reference Number
        /// </summary>
        /// <param name="refNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TorderRepairHdrDatesDto> GetAllDateFieldsRepair(int refNumber)
        {

            TorderRepairHdrDatesDto objdto = new TorderRepairHdrDatesDto();

            var data = await _context.TorderRepairHdr.FirstOrDefaultAsync(x => x.RefNo == refNumber);

            if (data != null)
            {
                objdto.RefNo = data.RefNo;
                objdto.DateCreated = data.DateCreated;
                objdto.CanceledDate = data.CanceledDate;
                objdto.StopDate = data.StopDate;
                objdto.CloseDate = data.CloseDate;
                objdto.DateSaved = data.DateSaved;
                objdto.EmailResponseDateTime = data.EmailResponseDateTime;
                objdto.DateInserted = data.DateInserted;
                objdto.ReceivingStartDate = data.ReceivingStartDate;
                objdto.ReceivingEndDate = data.ReceivingEndDate;

            }

            return objdto;
        }

        /// <summary>
        /// check in the RMA have all records in our database (4 tables)
        /// </summary>
        /// <param name="refNo"></param>
        /// <returns></returns>
        public async Task<bool> GetAllRMATablesCreatedAsync(int refNo)
        {
            return await (
                    from encOrder in _context.TorderRepairHdr
                        join recOrder in _context.TorderRepairItemsSerialsReceiving
                            on encOrder.RefNo equals recOrder.RefNo
                        join shpOrder in _context.TorderRepairItemsSerialsShipping
                            on encOrder.RefNo equals shpOrder.RefNo
                        join itemOrder in _context.TorderRepairItems
                            on encOrder.RefNo equals itemOrder.RefNo
                        where encOrder.RefNo == refNo
                        select encOrder.RefNo
                    ).AnyAsync();


        }



        /// <summary>
        /// Displays the 4 tables associated with an order in Servtrack.
        /// _TOrderRepair_Hdr
        /// _TorderRepair_ItemsSerials_Receiving
        /// _TorderRepair_ItemsSerials_Shipping 
        /// _TOrderRepair_Items
        /// </summary>
        /// <param name="refNo"></param>
        /// <returns></returns>
        public async Task<OrderRepairFourTablesRead> GetAllTablesOrderRepairCreatedAsync(int refNo)
        {

            OrderRepairFourTablesRead objdto = new OrderRepairFourTablesRead();

            objdto.OrHdr = await _context.TorderRepairHdr.FirstOrDefaultAsync(x => x.RefNo == refNo);
            objdto.ItemList = await _context.TorderRepairItems.Where(x => x.RefNo == refNo).ToListAsync();
            objdto.ReceivingList = await _context.TorderRepairItemsSerialsReceiving.Where(x => x.RefNo.Equals(refNo)).ToListAsync();
            objdto.ShippingList = await _context.TorderRepairItemsSerialsShipping.Where(x => x.RefNo.Equals(refNo)).ToListAsync();


            return objdto;

        }


        /// <summary>
        /// Get the current area from the sql
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <returns></returns>
        public async Task<AreaNameDto> GetAreaByFunction(int repairNumber)
        {
            var paramCall = new SqlParameter[] {
                       new SqlParameter() {
                                 ParameterName = "@Repair_No",
                                 SqlDbType =  System.Data.SqlDbType.Int,
                                 Direction = System.Data.ParameterDirection.Input,
                                 Value = Convert.ToInt32(repairNumber)
                             }
                         };

            // var objresult = _context.AreaNamesDto.FromSqlRaw("_SPZEB_B2B_Get_RepairNo_Location @Repair_No", paramCall).ToList();

            var objresult = (await _context.areaNamesDto.FromSqlRaw("_SPZEB_B2B_Get_RepairNo_Location @Repair_No", paramCall)
                .ToListAsync()).FirstOrDefault();


            return objresult;
        }

        /// <summary>
        /// return all information for a repair
        /// </summary>
        /// <param name="RepairNumber"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CheckRepairInformationDto?> GetRepairInfoAsync(int RepairNumber)
        {
            CheckRepairInformationDto objinfo = new CheckRepairInformationDto();

            // Primero validamos si este repair está asociado con un RMA
            objinfo.infofile = await _context.TzebInBoundRequestsFile
                .FirstOrDefaultAsync(x => x.RepairNo == RepairNumber);

            // Si no hay info, devolvemos null 
            if (objinfo.infofile == null)
            {
                return null;
            }

            // Receiving
            objinfo.inforeceiving = await _context.UserMvcReceiving
                .FirstOrDefaultAsync(x => x.Repairno == RepairNumber);

            if (objinfo.inforeceiving != null)
            {
                var infotech = await _context.Ttech
                    .FirstOrDefaultAsync(x => x.Email == objinfo.inforeceiving.User);
                if (infotech != null)
                {
                    objinfo.inforeceiving.username = !string.IsNullOrEmpty(infotech.TtechName)
                        ? infotech.TtechName
                        : objinfo.inforeceiving.User;
                }
                else
                {
                    var infouserdbk = await _context.Tdbkusers
                        .FirstOrDefaultAsync(x => x.Email == objinfo.inforeceiving.User);
                    if (infouserdbk != null)
                    {
                        objinfo.inforeceiving.username = !string.IsNullOrEmpty(infouserdbk.Name)
                            ? infouserdbk.Name
                            : objinfo.inforeceiving.User;
                    }
                }
            }

            // WIP
            objinfo.infotrepair = await _context.Trepair
                .FirstOrDefaultAsync(x => x.RepairNo == RepairNumber);

            var actionsDict = await _context.TrepairActionsCodes
                .ToDictionaryAsync(x => x.ActionId, x => x.ActionDesc);

            var callsDict = await _context.TzebB2bOutBoundRequestsTypeOfCalls
                .ToDictionaryAsync(x => x.Id, x => x.Name);

            var codesDict = await _context.TzebFaultCodes
                .ToDictionaryAsync(x => x.FaultCode, x => x.FaultDesc);

            var actionWorksDict = await _context.TzebWorkCodesActions
                .ToDictionaryAsync(x => x.WorkCodeAction, x => x.WorkDescAction);

            var worksDict = await _context.TzebWorkCodesTargets
                .ToDictionaryAsync(x => x.WorkCodeTarget, x => x.WorkDescTarget);

            var listaction = await _context.TrepairActionsLog
                .Where(x => x.RepairNo == RepairNumber)
                .OrderBy(x => x.ActionDate)
                .ToListAsync();

            foreach (var info in listaction)
            {
                info.actionName = actionsDict.ContainsKey(Convert.ToInt32(info.ActionId))
                    ? actionsDict[Convert.ToInt32(info.ActionId)]
                    : "";
            }

            var infocall = await _context.TzebB2bOutBoundRequestsLog
                .Where(x => x.RepairNo == RepairNumber)
                .OrderBy(x => x.DateTimeInserted)
                .ToListAsync();

            foreach (var info in infocall)
            {
                info.callname = callsDict.ContainsKey(Convert.ToInt32(info.TypeOfCallId))
                    ? callsDict[Convert.ToInt32(info.TypeOfCallId)]
                    : "";
            }

            var infoassig = await _context.UserMvcAssignments
                .FirstOrDefaultAsync(x => x.Repairid == RepairNumber);

            if (infoassig != null)
            {
                var infotech = await _context.Ttech
                    .FirstOrDefaultAsync(x => x.Email == infoassig.Userassigned);
                if (infotech != null && !string.IsNullOrEmpty(infotech.TtechName))
                {
                    infoassig.username = infotech.TtechName;
                }
            }

            var listareas = await _context.MvcChangeAreaLogs
                .Where(x => x.Repairid == RepairNumber).ToListAsync();

            var infocodeslist = await _context.TzebRepairCodes
                .Where(x => x.RepairNo == RepairNumber).ToListAsync();

            List<int> repidlist = infocodeslist.Select(item => item.RepId).ToList();

            foreach (var info in infocodeslist)
            {
                info.codename = codesDict.ContainsKey(info.FaultCode) ? codesDict[info.FaultCode] : "";
                info.actionname = actionWorksDict.ContainsKey(info.WorkCodeAction) ? actionWorksDict[info.WorkCodeAction] : "";
                info.workname = worksDict.ContainsKey(info.WorkCodeTarget) ? worksDict[info.WorkCodeTarget] : "";
            }

            objinfo.faultlistparts = repidlist.Count > 0
                ? await _context.TzebRepairCodesPartNo.Where(x => repidlist.Contains(x.RepId)).ToListAsync()
                : new List<TzebRepairCodesPartNo>();

            // Info preflash
            objinfo.infopreflash = await _context.MvcRepairPreflash
                .FirstOrDefaultAsync(x => x.Repairid == RepairNumber);

            if (objinfo.infopreflash != null)
            {
                var infotech = await _context.Ttech
                    .FirstOrDefaultAsync(x => x.Email == objinfo.infopreflash.User);
                if (infotech != null)
                {
                    objinfo.infopreflash.username = !string.IsNullOrEmpty(infotech.TtechName)
                        ? infotech.TtechName
                        : objinfo.infopreflash.User;
                }
                else
                {
                    var infouserdbk = await _context.Tdbkusers
                        .FirstOrDefaultAsync(x => x.Email == objinfo.infopreflash.User);
                    if (infouserdbk != null)
                    {
                        objinfo.infopreflash.username = !string.IsNullOrEmpty(infouserdbk.Name)
                            ? infouserdbk.Name
                            : objinfo.infopreflash.User;
                    }
                }
            }

            // Inventory transaction WMS
            objinfo.listinventorydetail = await (
                from enc in _wmscontext.InventorytransactionHdr
                    .Where(x => x.Documentreference.Contains(RepairNumber.ToString()))
                join det in _wmscontext.InventorytransactionDetail
                    on enc.Id equals det.Idenctransaction
                select det
            ).ToListAsync();

            objinfo.labelslist = await _context.TzebB2bReplacedPartLabel
                .Where(x => x.RepairNo == RepairNumber).ToListAsync();

            objinfo.listshipping = await _context.TorderRepairItemsSerialsShipping
                .Where(x => x.RepairNo == RepairNumber).ToListAsync();

            // Info hold
            var infohold = await _context.TzebInBoundRequestsFileHoldsLog
                .Where(x => x.RowId == objinfo.infofile.RowId).ToListAsync();

            // Asignación de listas
            objinfo.actionlist = listaction;
            objinfo.assigninfo = infoassig;
            objinfo.changeareaslist = listareas;
            objinfo.calllist = infocall;
            objinfo.listcodes = infocodeslist;
            objinfo.listhold = infohold;

            // Ahora devolvemos SOLO el DTO
            return objinfo;
        }


        /// <summary>
        /// Get basic information about repair number ready to be received
        /// </summary>
        /// <param name="serialnumber"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<RepairReadyToReceiveDto>> GetRepairReadyToReceive(string serialnumber)
        {




            var repairStatuses = await (from enc in _context.TiewRepairStatusZebraBldgReceiving.Where(x => x.SerialInbound.Contains(serialnumber.Trim()))
                                        join det in _context.TorderRepairItemsSerialsReceiving.Where(x => x.DateReceived == null)
                                        on Convert.ToInt32(enc.RepairNo) equals det.RepairNo
                                        select new
                                        {
                                            enc
                                        }).ToListAsync();

            List<RepairReadyToReceiveDto> listcalls = new List<RepairReadyToReceiveDto>();


            foreach (var info in repairStatuses)
            {
                RepairReadyToReceiveDto objdet = new RepairReadyToReceiveDto();

                objdet.Id = 1;
                objdet.SerialINBOUND = info.enc.SerialInbound;
                objdet.Ref_No = info.enc.RefNo;
                objdet.Repair_No = info.enc.RepairNo;
                objdet.Part_No = info.enc.PartNo;
                objdet.Part_DSC = info.enc.PartDsc;
                objdet.SerialRECEIVED = info.enc.SerialReceived;
                objdet.UnitID = info.enc.UnitId;
                objdet.Company_DSC = info.enc.CompanyDsc;
                objdet.Division = info.enc.Division;
                objdet.Spare_Pool_id = info.enc.SparePoolId;
                objdet.Cust_Ref = info.enc.CustRef;

                listcalls.Add(objdet);

            }

            return listcalls;
        }
        /// <summary>
        /// Get info about one repair when it's ready for receive
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public async Task<List<TzebInBoundRequestsFile>> GetRMAReceivingInfo(string serialNumber)
        {
            return await _context.TzebInBoundRequestsFile
                .Where(x => x.SerialNumber == serialNumber && x.DateCancelled == null)
                .ToListAsync();
        }
    }
}
