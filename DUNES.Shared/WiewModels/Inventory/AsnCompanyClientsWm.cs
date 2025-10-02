using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.WiewModels.Inventory;
using DUNES.UI.WiewModels;
namespace DUNES.UI.WiewModels
{

    /// <summary>
    /// All ASN Information
    /// </summary>
    public class AsnCompanyClientsWm
    {

        public ASNWm asdDto { get; set; } = new();

        public List<WMSClientCompaniesDto> listcompanyclients { get; set; } = new();

        public PickProcessCallsReadDto? CallsRead { get; set; }

    }



}
