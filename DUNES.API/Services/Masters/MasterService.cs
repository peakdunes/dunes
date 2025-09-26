using AutoMapper;
using DUNES.API.Repositories.Masters;
using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;

namespace DUNES.API.Services.Masters
{
    /// <summary>
    /// Service for all master tables
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    public class MasterService<T, TDto> : IMasterService<T, TDto>
     where T : class
     where TDto : class
    {
        private readonly IMasterRepository<T> _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public MasterService(IMasterRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        /// <summary>
        /// get all data for this table
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<IEnumerable<TDto>>> GetAllAsync(CancellationToken ct)
        {
            var result = await _repository.GetAllAsync(ct);

            var dtos = _mapper.Map<IEnumerable<TDto>>(result);

            return ApiResponseFactory.Ok(dtos);
        }
        /// <summary>
        /// get one register for this id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<TDto>> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);

            if (entity == null)
            {
                return ApiResponseFactory.NotFound<TDto>("Information not found");
            }
            else
            {
                var dtos = _mapper.Map<TDto>(entity);

                return ApiResponseFactory.Ok(dtos);
            }

        }
        /// <summary>
        /// add record
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<TDto>> AddAsync(TDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<T>(dto);

            // Guardar en la base
            var createdEntity = await _repository.AddAsync(entity, ct);

            // Map Entidad → DTO
            var createdDto = _mapper.Map<TDto>(createdEntity);

            // Retornar respuesta
            return ApiResponseFactory.Created(createdDto);
        }
        /// <summary>
        /// update a record
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<TDto>> UpdateAsync(TDto dto, CancellationToken ct)
        {
            // Mapear DTO → Entidad
            var entity = _mapper.Map<T>(dto);

            // Actualizar en DB
            var updatedEntity = await _repository.UpdateAsync(entity, ct);

            if (updatedEntity == null)
                return ApiResponseFactory.NotFound<TDto>("Record not updated");

            // Mapear Entidad → DTO
            var updatedDto = _mapper.Map<TDto>(updatedEntity);

            return ApiResponseFactory.Ok(updatedDto, "Update successful");
        }
        /// <summary>
        ///  delete a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> DeleteByIdAsync(int id, CancellationToken ct)
        {
            var deleted = await _repository.DeleteByIdAsync(id, ct);
            return deleted
                ? ApiResponseFactory.Ok(true, "Record deleted")
                : ApiResponseFactory.NotFound<bool>("Record not found");
        }
        /// <summary>
        /// Get all table records for a string field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<TDto>> SearchByFieldAsync(string fieldName, string value, CancellationToken ct)
        {
            var results = await _repository.SearchByFieldAsync(fieldName, value, ct);

            if (results == null)
                return ApiResponseFactory.NotFound<TDto>(
                    $"No records found for {fieldName} containing '{value}'"
                );

            var dtos = _mapper.Map<TDto>(results);

            return ApiResponseFactory.Ok(dtos, "Search completed successfully");
        }

       
    }
}
