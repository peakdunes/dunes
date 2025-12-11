using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.TemporalModels;
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

        public List<WMSClientCompaniesDTO> listcompanyclients { get; set; } = new();

        public PickProcessCallsReadDto? CallsRead { get; set; }

        public WMSTransactionTm? wmstransactions { get; set; }

        public List<TzebB2bReplacementPartsInventoryLogDto>? listinvtran { get; set; }

    }



}
