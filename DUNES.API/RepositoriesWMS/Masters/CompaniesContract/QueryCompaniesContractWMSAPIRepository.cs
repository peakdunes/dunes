using AutoMapper;
using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.RepositoriesWMS.Masters.CompaniesContract
{

    /// <summary>
    /// common query company client contracts
    /// </summary>
    public class QueryCompaniesContractWMSAPIRepository : IQueryCompaniesContractWMSAPIRepository
    {


        private readonly appWmsDbContext _context;
       

        /// <summary>
        /// contructor (DI)
        /// </summary>
        /// <param name="context"></param>
        public QueryCompaniesContractWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
            
        }
        /// <summary>
        /// Get all client contract information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<WMSCompaniesContractReadDTO>> GetAllClientCompaniesContractInformationAsync(CancellationToken ct)
        {
            var listcontracts = await _context.CompaniesContract
                .Include(x => x.CompanyNavegation)
                .Include(x => x.CompanyClientNavegation)
                .AsNoTracking()
                .Select(c => new WMSCompaniesContractReadDTO
                {
                    Id = c.Id,
                    CompanyId = c.CompanyId,
                    CompanyClientId = c.CompanyClientId,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    IsActive = c.IsActive,
                    ContractCode = c.ContractCode,
                    ContactName = c.ContactName,
                    ContactEmail = c.ContactEmail,
                    ContactPhone = c.ContactPhone,
                    Notes = c.Notes,
                    ItemCatalogMode = c.ItemCatalogMode,
                    companyname = c.CompanyNavegation.Name,
                    companyclientname = c.CompanyClientNavegation.Name
                }).ToListAsync(ct);


            return listcontracts;
        }
        /// <summary>
        /// Get all client contract information by company
        /// </summary>
        /// <param name="companyclientid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<WMSCompaniesContractReadDTO>> GetAllClientCompaniesContractInformationByCompanyClientIdAsync(int companyclientid, CancellationToken ct)
        {
            var listcontracts = await _context.CompaniesContract.Where(x => x.CompanyClientId == companyclientid)
             .Include(x => x.CompanyNavegation)
             .Include(x => x.CompanyClientNavegation)
             .AsNoTracking()
             .Select(c => new WMSCompaniesContractReadDTO
             {
                 Id = c.Id,
                 CompanyId = c.CompanyId,
                 CompanyClientId = c.CompanyClientId,
                 StartDate = c.StartDate,
                 EndDate = c.EndDate,
                 IsActive = c.IsActive,
                 ContractCode = c.ContractCode,
                 ContactName = c.ContactName,
                 ContactEmail = c.ContactEmail,
                 ContactPhone = c.ContactPhone,
                 Notes = c.Notes,
                 ItemCatalogMode = c.ItemCatalogMode,
                 companyname = c.CompanyNavegation.Name,
                 companyclientname = c.CompanyClientNavegation.Name
             }).ToListAsync(ct);



            return listcontracts;
        }
        /// <summary>
        /// Get all client contract information by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WMSCompaniesContractReadDTO?> GetClientCompanyContractInformationByContractIdAsync(int Id, CancellationToken ct)
        {
            var dto = await _context.CompaniesContract
                  .AsNoTracking()
                  .Where(x => x.Id == Id)
                  .Select(c => new WMSCompaniesContractReadDTO
                  {
                      Id = c.Id,
                      CompanyId = c.CompanyId,
                      CompanyClientId = c.CompanyClientId,
                      StartDate = c.StartDate,
                      EndDate = c.EndDate,
                      IsActive = c.IsActive,
                      ContractCode = c.ContractCode,
                      ContactName = c.ContactName,
                      ContactEmail = c.ContactEmail,
                      ContactPhone = c.ContactPhone,
                      Notes = c.Notes,
                      ItemCatalogMode = c.ItemCatalogMode,
                      companyname = c.CompanyNavegation.Name != null ? c.CompanyNavegation.Name:"",
                      companyclientname = c.CompanyClientNavegation.Name != null ? c.CompanyClientNavegation.Name : "",
                  })
                  .FirstOrDefaultAsync(ct);

            return dto;
        }
        ///// <summary>
        ///// Get all client contract information by contract number
        ///// </summary>
        ///// <param name="companyclientid"></param>
        ///// <param name="contractcode"></param>
        ///// <param name="ct"></param>
        ///// <returns></returns>
        ///// <exception cref="NotImplementedException"></exception>
        //public async Task<WMSCompaniesContractReadDTO?> GetClientCompanyContractInformationByContractIdAsync(int companyclientid, string contractcode, CancellationToken ct)
        //{
        //    var dto = await _context.CompaniesContract
        //     .AsNoTracking()
        //     .Where(x => x.CompanyClientId == companyclientid && x.ContractCode!.ToUpper().Trim() == contractcode.ToUpper().Trim())
        //     .Select(c => new WMSCompaniesContractReadDTO
        //     {
        //         Id = c.Id,
        //         CompanyId = c.CompanyId,
        //         CompanyClientId = c.CompanyClientId,
        //         StartDate = c.StartDate,
        //         EndDate = c.EndDate,
        //         IsActive = c.IsActive,
        //         ContractCode = c.ContractCode,
        //         ContactName = c.ContactName,
        //         ContactEmail = c.ContactEmail,
        //         ContactPhone = c.ContactPhone,
        //         Notes = c.Notes,
        //         ItemCatalogMode = c.ItemCatalogMode,
        //         companyname = c.CompanyNavegation.Name != null ? c.CompanyNavegation.Name : "",
        //         companyclientname = c.CompanyClientNavegation.Name != null ? c.CompanyClientNavegation.Name : "",
        //     })
        //     .FirstOrDefaultAsync(ct);

        //    return dto;
        //}

        /// <summary>
        /// Get all company contract  information
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public async Task<List<WMSCompaniesContractReadDTO>> GetAllClientCompaniesContractInformationByCompanyIdAsync(int companyid, CancellationToken ct)
        {
            var listcontracts = await _context.CompaniesContract.Where(x => x.CompanyId == companyid)
             .Include(x => x.CompanyNavegation)
             .Include(x => x.CompanyClientNavegation)
             .AsNoTracking()
             .Select(c => new WMSCompaniesContractReadDTO
             {
                 Id = c.Id,
                 CompanyId = c.CompanyId,
                 CompanyClientId = c.CompanyClientId,
                 StartDate = c.StartDate,
                 EndDate = c.EndDate,
                 IsActive = c.IsActive,
                 ContractCode = c.ContractCode,
                 ContactName = c.ContactName,
                 ContactEmail = c.ContactEmail,
                 ContactPhone = c.ContactPhone,
                 Notes = c.Notes,
                 ItemCatalogMode = c.ItemCatalogMode,
                 companyname = c.CompanyNavegation.Name,
                 companyclientname = c.CompanyClientNavegation.Name
             }).ToListAsync(ct);



            return listcontracts;
        }
        /// <summary>
        /// get contract information by company client id and contract number
        /// </summary>
        /// <param name="companyclientid"></param>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<WMSCompaniesContractReadDTO?> GetClientCompanyInformationContractByNumberAndCompanyIdAsync(int companyclientid, string contractcode, CancellationToken ct)
        {
            var dto = await _context.CompaniesContract
          .AsNoTracking()
          .Where(x => x.CompanyClientId == companyclientid && x.ContractCode!.ToUpper().Trim() == contractcode.ToUpper().Trim())
          .Select(c => new WMSCompaniesContractReadDTO
          {
              Id = c.Id,
              CompanyId = c.CompanyId,
              CompanyClientId = c.CompanyClientId,
              StartDate = c.StartDate,
              EndDate = c.EndDate,
              IsActive = c.IsActive,
              ContractCode = c.ContractCode,
              ContactName = c.ContactName,
              ContactEmail = c.ContactEmail,
              ContactPhone = c.ContactPhone,
              Notes = c.Notes,
              ItemCatalogMode = c.ItemCatalogMode,
              companyname = c.CompanyNavegation.Name != null ? c.CompanyNavegation.Name : "",
              companyclientname = c.CompanyClientNavegation.Name != null ? c.CompanyClientNavegation.Name : "",
          })
          .FirstOrDefaultAsync(ct);

            return dto;
        }

        /// <summary>
        /// get contract information by number
        /// </summary>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<WMSCompaniesContractReadDTO?> GetClientCompanyInformationContractByNumberAsync(string contractcode, CancellationToken ct)
        {
            var dto = await _context.CompaniesContract
            .AsNoTracking()
            .Where(x =>  x.ContractCode!.ToUpper().Trim() == contractcode.ToUpper().Trim())
            .Select(c => new WMSCompaniesContractReadDTO
            {
                Id = c.Id,
                CompanyId = c.CompanyId,
                CompanyClientId = c.CompanyClientId,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                IsActive = c.IsActive,
                ContractCode = c.ContractCode,
                ContactName = c.ContactName,
                ContactEmail = c.ContactEmail,
                ContactPhone = c.ContactPhone,
                Notes = c.Notes,
                ItemCatalogMode = c.ItemCatalogMode,
                companyname = c.CompanyNavegation.Name != null ? c.CompanyNavegation.Name : "",
                companyclientname = c.CompanyClientNavegation.Name != null ? c.CompanyClientNavegation.Name : "",
            })
            .FirstOrDefaultAsync(ct);

            return dto;
        }
    }
}
