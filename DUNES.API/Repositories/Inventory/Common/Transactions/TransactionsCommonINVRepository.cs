using DUNES.API.Data;
using DUNES.API.Models.Inventory.Common;
using DUNES.Shared.DTOs.Inventory;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Repositories.Inventory.Common.Transactions
{

    /// <summary>
    /// All common inventory transactions
    /// </summary>
    public class TransactionsCommonINVRepository : ITransactionsCommonINVRepository
    {


        private readonly AppDbContext _context;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>
        public TransactionsCommonINVRepository(AppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Create a new cons output call
        /// </summary>
        /// <param name="callInfo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> createConsOutPutCall(TzebB2bOutConsReqsInsertDto callInfo, CancellationToken ct)
        {
            TzebB2bOutConsReqs objdet = new TzebB2bOutConsReqs();


            objdet.TypeOfCallId = callInfo.TypeOfCallId;
            objdet.AdditionalInfo = callInfo.AdditionalInfo;
            objdet.TransactionCode = callInfo.TransactionCode;
            objdet.InProcess = callInfo.InProcess;
            objdet.Additional = callInfo.Additional;

            _context.TzebB2bOutConsReqs.Add(objdet);
            await _context.SaveChangesAsync(ct);

            return objdet.Id;



        }

        /// <summary>
        /// create a new inventory transction log and update inventory table 
        /// </summary>
        /// <param name="listItemDetail"></param>
        /// <param name="User"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> createInventoryTransactionLog(List<TzebB2bReplacementPartsInventoryLogDto> listItemDetail, string User, CancellationToken ct)
        {


            foreach (var logdata in listItemDetail)
            {
                {
                    var p1 = new SqlParameter("@Part_Definition_id", logdata.PartDefinitionId);
                    var p2 = new SqlParameter("@Inventory_Type_id_Source", logdata.InventoryTypeIdSource);
                    var p3 = new SqlParameter("@Inventory_Type_id_Dest", logdata.InventoryTypeIdDest);
                    var p4 = new SqlParameter("@Serial_No", logdata.SerialNo);
                    var p5 = new SqlParameter("@Qty", logdata.Qty);
                    var p6 = new SqlParameter("@Notes", logdata.Notes);
                    var p7 = new SqlParameter("@Repair_No", logdata.RepairNo);
                    var p8 = new SqlParameter("@User", User);
                    var p9 = new SqlParameter("@Manual", "");
                    var p10 = new SqlParameter("@isCE", false);

                    var result = await _context.TzebB2bReplacementPartsInventoryLog
                        .FromSqlRaw("EXEC _SPZEB_B2B_Insert_New_Replacement_Parts_Inventory_Log" +
                        " @Part_Definition_id,@Inventory_Type_id_Source,@Inventory_Type_id_Dest," +
                        " @Serial_No,@Qty,@Notes,@Repair_No,@User,@Manual,@isCE", p1, p2, p3, p4, p5,
                        p6, p7, p8, p9, p10)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ct);
                }


            }

            return true;
        }

        /// <summary>
        /// Leave a call ready to process
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> updateConsOutPutCallReadyToProcess(int id, CancellationToken ct)
        {
            var infocall = await _context.TzebB2bOutConsReqs.FirstOrDefaultAsync(x => x.Id == id);


            if (infocall == null)
            {
                return false;
            }
            else
            {
                infocall.FullXmlsent = null;
                infocall.AckReceived = false;
                infocall.ResponseXml = null;
                infocall.Result = null;
                infocall.Failure = null;
                infocall.SentTimestamp = null;
                infocall.DateTimeSent = null;
                infocall.InProcess = false;

                _context.TzebB2bOutConsReqs.Update(infocall);
                await _context.SaveChangesAsync();

                return true;
            }
        }
    }
}
