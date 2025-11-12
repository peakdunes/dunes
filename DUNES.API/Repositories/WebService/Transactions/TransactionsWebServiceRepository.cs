
using AutoMapper;
using DUNES.API.Data;
using DUNES.API.Models.WebService;
using DUNES.Shared.DTOs.WebService;

using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Repositories.WebService.Transactions
{
    /// <summary>
    /// web service log transactions (insert summary in web service  log tables
    /// for all RMA web service calls
    /// </summary>
    public class TransactionsWebServiceRepository : ITransactionsWebServiceRepository
    {


        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public TransactionsWebServiceRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// create or update daily record
        /// </summary>
        /// <param name="dateRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task UpsertDailyFromHourlyAsync(DateTime dateRequest, CancellationToken ct)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// create or update hourly record
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpsertHourlyAsync(MvcWebServiceHourlySummaryDto dto, CancellationToken ct)
        {

            var infodata = await _context.MvcWebServiceHourlySummary.FirstOrDefaultAsync(
                x => x.Year == dto.Year && x.Month == dto.Month && x.Day == dto.Day && x.Hour == dto.Hour, ct);

            if (infodata == null)
            {

                MvcWebServiceHourlySummary objinsert = new MvcWebServiceHourlySummary();

                objinsert.Year = dto.Year;
                objinsert.Month = dto.Month;
                objinsert.Day = dto.Day;
                objinsert.Hour = dto.Hour;
                objinsert.TotalCalls = dto.TotalCalls;
                objinsert.TotalErrors = dto.TotalErrors;
                objinsert.Source = dto.Source ?? Environment.MachineName;
                objinsert.ErrorRate = dto.ErrorRate;
                objinsert.LastUpdatedUtc = DateTime.Now;

                _context.MvcWebServiceHourlySummary.Add(objinsert);
            }
            else
            {
                infodata.TotalCalls = dto.TotalCalls;
                infodata.TotalErrors = dto.TotalErrors;
                infodata.Source = dto.Source ?? Environment.MachineName;
                infodata.ErrorRate = dto.ErrorRate;

                _context.MvcWebServiceHourlySummary.Update(infodata);
            }

            await _context.SaveChangesAsync(ct);

            return true;

        }

       
    }
}
