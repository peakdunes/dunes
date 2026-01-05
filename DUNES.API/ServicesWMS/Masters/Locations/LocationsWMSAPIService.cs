using AutoMapper;
using DUNES.API.RepositoriesWMS.Masters.Countries;
using DUNES.API.RepositoriesWMS.Masters.Locations;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.Locations
{
    /// <summary>
    /// locations service implementation
    /// </summary>

    public class LocationsWMSAPIService : ILocationsWMSAPIService
    {

        private readonly ILocationsWMSAPIRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="repository"></param>
        ///  <param name="mapper"></param>


        public LocationsWMSAPIService(ILocationsWMSAPIRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// add new location
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<bool>> CreateAsync(WMSCountriesDTO entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// search location by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get all active locations
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<List<WMSCountriesDTO>>> GetActiveAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// get all locations
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<List<WMSCountriesDTO>>> GetAllAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get location by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<WMSCountriesDTO?>> GetByIdAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// active no active location
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// update location
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<bool>> UpdateAsync(WMSCountriesDTO entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
