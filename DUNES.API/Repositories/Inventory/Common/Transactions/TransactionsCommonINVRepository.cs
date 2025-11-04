using DUNES.API.Data;
using DUNES.API.Models.Inventory.Common;
using DUNES.Shared.DTOs.Inventory;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
                    var p1 = new SqlParameter("@Part_Definition_id", SqlDbType.Int) { Value = logdata.PartDefinitionId };
                    var p2 = new SqlParameter("@Inventory_Type_id_Source", SqlDbType.Int) { Value = logdata.InventoryTypeIdSource };
                    var p3 = new SqlParameter("@Inventory_Type_id_Dest", SqlDbType.Int) { Value = logdata.InventoryTypeIdDest };

                    var p4 = new SqlParameter("@Serial_No", SqlDbType.NVarChar, 50) { Value = (object?)logdata.SerialNo ?? DBNull.Value };
                    var p5 = new SqlParameter("@Qty", SqlDbType.Int) { Value = logdata.Qty };
                    var p6 = new SqlParameter("@Notes", SqlDbType.NVarChar, 250) { Value = (object?)logdata.Notes ?? DBNull.Value };

                    // <-- ESTE es el que te falla si viene null: mándalo como DBNull.Value
                    var p7 = new SqlParameter("@Repair_No", SqlDbType.Int) { Value = (object?)logdata.RepairNo ?? DBNull.Value };

                    var p8 = new SqlParameter("@User", SqlDbType.NVarChar, 10) { Value = User ?? (object)DBNull.Value };

                    // SP espera bit, no string:
                    var p9 = new SqlParameter("@Manual", SqlDbType.Bit) { Value = false };   // o logdata.Manual
                    var p10 = new SqlParameter("@isCE", SqlDbType.Bit) { Value = false };

                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC dbo._SPZEB_B2B_Insert_New_Replacement_Parts_Inventory_Log " +
                        "@Part_Definition_id,@Inventory_Type_id_Source,@Inventory_Type_id_Dest," +
                        "@Serial_No,@Qty,@Notes,@Repair_No,@User,@Manual,@isCE",
                        parameters: new[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 },
                        cancellationToken: ct);


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
