using AutoMapper;
using DUNES.API.RepositoriesWMS.Masters.ClientCompanies;
using DUNES.API.RepositoriesWMS.Masters.Companies;
using DUNES.API.RepositoriesWMS.Masters.Countries;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.Companies
{

    /// <summary>
    /// Companies service
    /// </summary>
    public class CompaniesWMSAPIService : ICompaniesWMSAPIService
    {
        private readonly ICompaniesWMSAPIRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="repository"></param>
        ///  <param name="mapper"></param>


        public CompaniesWMSAPIService(ICompaniesWMSAPIRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// get all countries
        /// </summary>
        public async Task<ApiResponse<List<WMSCompaniesDTO>>> GetAllAsync(CancellationToken ct)
        {
            var data = await _repository.GetAllCompaniesInformation(ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSCompaniesDTO>>("No companies found.");

            var objlist = _mapper.Map<List<WMSCompaniesDTO>>(data);

            return ApiResponseFactory.Ok(objlist);
        }

        /// <summary>
        /// get all active countries
        /// </summary>
        public async Task<ApiResponse<List<WMSCompaniesDTO>>> GetActiveAsync(CancellationToken ct)
        {
           

            var data = await _repository.GetAllCompaniesInformation(ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSCompaniesDTO>>("No companies found.");

           var listactives = data.Where(x => x.Active == true).ToList();

            if (listactives == null || listactives.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSCompaniesDTO>>("No active companies found.");

            var objlist = _mapper.Map<List<WMSCompaniesDTO>>(listactives);

            return ApiResponseFactory.Ok(objlist);
        }

        /// <summary>
        /// get country by id
        /// </summary>
        public async Task<ApiResponse<WMSCompaniesDTO?>> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);

            if (entity is null)
                return ApiResponseFactory.NotFound<WMSCompaniesDTO?>($"Company with Id {id} was not found.");

            var objmap = _mapper.Map<WMSCompaniesDTO>(entity);

            return ApiResponseFactory.Ok(objmap);
        }

        /// <summary>
        /// create new country
        /// </summary>
        public async Task<ApiResponse<bool>> CreateAsync(WMSCompaniesDTO entity, CancellationToken ct)
        {
            // validar nombre duplicado
            var exists = await _repository.ExistsByNameAsync(entity.Name!, null, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                         error: "DUPLICATE_COMPANY_NAME",
                         message: $"There is already a company with the name '{entity.Name}'.",
                         statusCode: (int)HttpStatusCode.Conflict);
            }

            var objmap = _mapper.Map<DUNES.API.ModelsWMS.Masters.Company>(entity);

            await _repository.CreateAsync(objmap, ct);
            return ApiResponseFactory.Ok(true, "Company created successfully.");
        }

        /// <summary>
        /// update country information
        /// </summary>
        public async Task<ApiResponse<bool>> UpdateAsync(WMSCompaniesDTO entity, CancellationToken ct)
        {
            // validar nombre duplicado excluyendo el propio Id
            var exists = await _repository.ExistsByNameAsync(entity.Name!, entity.Id, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                         error: "DUPLICATE_COMPANY_NAME",
                         message: $"There is already a company with the name '{entity.Name}'.",
                         statusCode: (int)HttpStatusCode.Conflict);
            }

            var current = await _repository.GetByIdAsync(entity.Id, ct);
            if (current is null)
            {
                return ApiResponseFactory.NotFound<bool>($"Company with Id {entity.Id} was not found.");
            }

            // si quieres, puedes copiar solo campos editables en vez de reemplazar la entidad
            current.Name = entity.Name;
            current.Active = entity.Active;

            await _repository.UpdateAsync(current, ct);

            return ApiResponseFactory.Ok(true, "Company updated successfully.");
        }

        /// <summary>
        /// activate / deactivate country
        /// </summary>
        public async Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, CancellationToken ct)
        {
            var ok = await _repository.SetActiveAsync(id, isActive, ct);

            if (!ok)
                return ApiResponseFactory.NotFound<bool>($"Company with Id {id} was not found.");

            var msg = isActive
                ? "Company has been activated successfully."
                : "Company has been deactivated successfully.";

            return ApiResponseFactory.Ok(true, msg);
        }

        /// <summary>
        /// exist country name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct)
        {
            var exists = await _repository.ExistsByNameAsync(name, excludeId, ct);

            // aquí tienes dos enfoques posibles:

            // 1) Regresar "exists" tal cual (true = ya existe, false = no existe):
            return ApiResponseFactory.Ok(exists);

            // 2) O si lo quieres como "isAvailable" (true = nombre disponible):
            // return ApiResponseFactory.Ok(!exists);
        }
    }
}
