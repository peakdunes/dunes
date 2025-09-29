using AutoMapper;
using DUNES.API.Models.Inventory;
using DUNES.API.Models.Masters;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.Masters;

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

        }

    }
}
