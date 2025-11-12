using AutoMapper;
using DUNES.API.Data;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WebService;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Repositories.WebService.Queries
{
    /// <summary>
    /// web service Inteface
    /// </summary>
    public class CommonQueryWebServiceRepository : ICommonQueryWebServiceRepository
    {

        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public CommonQueryWebServiceRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        /// <summary>
        /// get daily summmary
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<MvcWebServiceDailySummaryDto?> GetCurrentDailyAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get daily summary by range
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<List<MvcWebServiceDailySummaryDto>> GetDailyRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken ct)
        {



            throw new NotImplementedException();
        }
        /// <summary>
        ///  Get all calls log per day - hour
        /// </summary>
        /// <param name="DateRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<MvcWebServiceHourlySummaryDto>> GetHourlyTransactions(DateTime DateRequest, CancellationToken ct)
        {

            var entities = await _context.MvcWebServiceHourlySummary
                .AsNoTracking()
                .Where(x => x.Year == DateRequest.Year
                         && x.Month == DateRequest.Month
                         && x.Day == DateRequest.Day)
                .OrderBy(x => x.Hour)
                .ToListAsync(ct);

            var dtos = _mapper.Map<List<MvcWebServiceHourlySummaryDto>>(entities);


            return dtos;


        }
    }
}
