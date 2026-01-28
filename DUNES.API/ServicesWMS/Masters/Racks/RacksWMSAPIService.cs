using AutoMapper;
using DUNES.API.RepositoriesWMS.Masters.Companies;
using DUNES.API.RepositoriesWMS.Masters.Locations;
using DUNES.API.RepositoriesWMS.Masters.Racks;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using NuGet.Protocol.Core.Types;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DUNES.API.ServicesWMS.Masters.Racks
{

    /// <summary>
    /// Racks Service
    /// </summary>
    public class RacksWMSAPIService : IRacksWMSAPIService
    {


        private readonly IRacksWMSAPIRepository _repository;
        private readonly ICompaniesWMSAPIRepository _companyRepository;
        private readonly ILocationsWMSAPIRepository _locationRepository;

        private readonly IMapper _mapper;
        /// <summary>
        /// contructor (DI)
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        ///   /// <param name="companyRepository"></param>
        /// <param name="locationRepository"></param>
        public RacksWMSAPIService(IRacksWMSAPIRepository repository, IMapper mapper,
             ICompaniesWMSAPIRepository companyRepository, ILocationsWMSAPIRepository locationRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _companyRepository = companyRepository;
            _locationRepository = locationRepository;
        }

        /// <summary>
        /// add new rack
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> CreateAsync(WMSRacksDTO entity, CancellationToken ct)
        {
            if (entity.Idcompany <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>("Company is required");
            }

            var ExistCompany = await _companyRepository.GetByIdAsync(entity.Idcompany, ct);

            if (ExistCompany == null)
            {

                return ApiResponseFactory.BadRequest<bool>($"Company {entity.Idcompany} don't exist");
            }

            if (entity.LocationsId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>("Location is required");
            }

            if (!ExistCompany.Active)
            {
                return ApiResponseFactory.BadRequest<bool>($"Company {entity.IdcompanyNavigation.Name} is not active");
            }

            var ExistLocation = await _locationRepository.GetByIdAsync(ExistCompany.Id, entity.LocationsId,ct);

            if (ExistLocation == null)
            {
                return ApiResponseFactory.BadRequest<bool>($"Location {entity.LocationsId} don't exist");
            }

            if (!ExistLocation.Active)
            {
                return ApiResponseFactory.BadRequest<bool>($"Location {entity.Locations.Name} is not active");
            }


            if (string.IsNullOrEmpty(entity.Name))
            {
                return ApiResponseFactory.BadRequest<bool>("Rack Name is required");
            }


            // validar nombre duplicado
            var exists = await _repository.ExistsByNameAsync(entity.Idcompany, entity.LocationsId, entity.Name!, null, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                         error: "DUPLICATE_RACK_NAME",
                         message: $"There is already a rack with the name '{entity.Name}'.",
                         statusCode: (int)HttpStatusCode.Conflict);
            }

            var objmap = _mapper.Map<DUNES.API.ModelsWMS.Masters.Racks>(entity);

            await _repository.CreateAsync(objmap, ct);
            return ApiResponseFactory.Ok(true, "Rack created successfully.");
          
        }
        /// <summary>
        /// Check if exist a rack with the same number
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> ExistsByNameAsync(int companyId, int locationId, string name, int? excludeId, CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>("Company is required");
            }

            if (string.IsNullOrEmpty(name))
            {
                return ApiResponseFactory.BadRequest<bool>("Rack Name is required");
            }
            var exists = await _repository.ExistsByNameAsync(companyId, locationId, name!, null, ct);
           
            
            return ApiResponseFactory.Ok(exists, "");
        }
        /// <summary>
        /// get all active racks by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSRacksDTO>>> GetActiveAsync(int companyId, int locationId, CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<List<WMSRacksDTO>>("Company is required");
            }

            var data = await _repository.GetActiveAsync(companyId, locationId, ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSRacksDTO>>("No active racks found.");

            var objlist = _mapper.Map<List<WMSRacksDTO>>(data);

            return ApiResponseFactory.Ok(objlist);

        }


        /// <summary>
        /// get all racks by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSRacksDTO>>> GetAllAsync(int companyId, int locationId, CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<List<WMSRacksDTO>>("Company is required");
            }

            var data = await _repository.GetAllAsync(companyId, locationId, ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSRacksDTO>>("No racks found.");

            var objlist = _mapper.Map<List<WMSRacksDTO>>(data);

            return ApiResponseFactory.Ok(objlist);
        }

        /// <summary>
        /// get a rack by id and company id
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<WMSRacksDTO>> GetByIdAsync(int companyId, int locationId, int id, CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSRacksDTO>("Company is required");
            }

            if (id <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSRacksDTO>("Rack is required");
            }

            var entity = await _repository.GetByIdAsync(companyId, locationId, id, ct);

            if (entity is null)
                return ApiResponseFactory.NotFound<WMSRacksDTO>($"Rack with Id {id} was not found.");

            var objmap = _mapper.Map<WMSRacksDTO>(entity);

            return ApiResponseFactory.Ok(objmap);
        }

        /// <summary>
        /// set active / no activa a rack
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> SetActiveAsync(int companyId, int locationId, int id, bool isActive, CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>("Company is required");
            }

            if (id <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>("Rack is required");
            }

            var ok = await _repository.SetActiveAsync(companyId, locationId, id, isActive, ct);

            if (!ok)
                return ApiResponseFactory.NotFound<bool>($"Rack with Id {id} was not found.");

            var msg = isActive
                ? "Rack has been activated successfully."
                : "Rack has been deactivated successfully.";

            return ApiResponseFactory.Ok(true, msg);


        }


        /// <summary>
        /// update rack information
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> UpdateAsync(WMSRacksDTO entity, CancellationToken ct)
        {
            if (entity.Idcompany <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>("Company is required");
            }

            var ExistCompany = await _companyRepository.GetByIdAsync(entity.Idcompany, ct);

            if (ExistCompany == null)
            {

                return ApiResponseFactory.BadRequest<bool>($"Company {entity.Idcompany} don't exist");
            }

            if (entity.LocationsId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>("Location is required");
            }

            if (!ExistCompany.Active)
            {
                return ApiResponseFactory.BadRequest<bool>($"Company {entity.IdcompanyNavigation.Name} is not active");
            }

            var ExistLocation = await _locationRepository.GetByIdAsync(ExistCompany.Id, entity.LocationsId, ct);

            if (ExistLocation == null)
            {
                return ApiResponseFactory.BadRequest<bool>($"Location {entity.LocationsId} don't exist");
            }

            if (!ExistLocation.Active)
            {
                return ApiResponseFactory.BadRequest<bool>($"Location {entity.Locations.Name} is not active");
            }


            if (string.IsNullOrEmpty(entity.Name))
            {
                return ApiResponseFactory.BadRequest<bool>("Rack Name is required");
            }

            // validar nombre duplicado excluyendo el propio Id
            var exists = await _repository.ExistsByNameAsync(entity.Idcompany, entity.LocationsId, entity.Name!, entity.Id, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                         error: "DUPLICATE_RACK_NAME",
                         message: $"There is already a rack with the name '{entity.Name}'.",
                         statusCode: (int)HttpStatusCode.Conflict);
            }

            var current = await _repository.GetByIdAsync(entity.Idcompany, entity.LocationsId, entity.Id, ct);
            if (current is null)
            {
                return ApiResponseFactory.NotFound<bool>($"Rack with Id {entity.Id} was not found.");
            }

           
            current.Name = entity.Name;
            current.Active = entity.Active;

            await _repository.UpdateAsync(current, ct);

            return ApiResponseFactory.Ok(true, "Rack updated successfully.");
        }
    }
}
