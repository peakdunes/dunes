using AutoMapper;
using DUNES.API.Models.Inventory.ASN;
using DUNES.API.Models.Inventory.Common;
using DUNES.API.Models.Inventory.PickProcess;
using DUNES.API.Models.Masters;
using DUNES.API.Models.WebService;
using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WebService;
using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.Profiles
{
    /// <summary>
    /// Mapper profile
    /// </summary>
    public class WmsProfile: Profile
    {
        /// <summary>
        /// contructor
        /// </summary>
        public WmsProfile()
        {
            CreateMap<WmsCompanyclient, WmsCompanyclientDto>().ReverseMap();
            CreateMap<TdivisionCompany, TdivisionCompanyDto>().ReverseMap();
            CreateMap<TzebB2bMasterPartDefinition,TzebB2bMasterPartDefinitionDto>().ReverseMap();

            CreateMap<TzebB2bInbConsReqs,TzebB2bInbConsReqsDto>().ReverseMap();

            CreateMap <TzebB2bInventoryType, TzebB2bInventoryTypeDto>().ReverseMap();

            //ASN MAP

            CreateMap<TzebB2bAsnOutHdrDetItemInbConsReqs, ASNHdrDto>().ReverseMap();

            CreateMap<TzebB2bAsnLineItemTblItemInbConsReqs, ASNItemDetailDto>().ReverseMap();

            CreateMap<TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog, TzebB2bIrReceiptOutHdrDetItemInbConsReqsLogDto>().ReverseMap();

            CreateMap<TzebB2bIrReceiptLineItemTblItemInbConsReqsLog, TzebB2bIrReceiptLineItemTblItemInbConsReqsLogDto>().ReverseMap();


            CreateMap<MvcWebServiceHourlySummary, MvcWebServiceHourlySummaryDto>().ReverseMap();

            CreateMap<TzebB2bPSoWoHdrTblItemInbConsReqsLog, PickProcessHdrDto>().ReverseMap();

            CreateMap<MvcGeneralParameters, MvcGeneralParametersDto>().ReverseMap();

            CreateMap<Countries , WMSCountriesDTO>().ReverseMap();

            CreateMap<StatesCountries, WMSStatesCountriesDTO>().ReverseMap();

            CreateMap<StatesCountries, WMSStatesCountriesReadDTO>().ReverseMap();

            CreateMap<Cities, WMSCitiesDTO>().ReverseMap();

            CreateMap<Cities, WMSCitiesReadDTO>().ReverseMap();

            CreateMap<CompanyClient, WmsCompanyclientDto>().ReverseMap();

            CreateMap < CompanyClient, WMSClientCompaniesReadDTO>().ReverseMap();

            CreateMap<CompanyClientDivision, WMSCompanyClientDivisionDTO>().ReverseMap();


            CreateMap<CompaniesContract, WMSCompaniesContractDTO>().ReverseMap();

            CreateMap<CompaniesContract, WMSCompaniesContractReadDTO>().ReverseMap();

            CreateMap<Company, WMSCompaniesDTO>().ReverseMap();

            CreateMap<Locations, WMSLocationsDTO > ().ReverseMap();


        }


    }
}
