using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.WiewModels.Inventory;
using DUNES.UI.WiewModels;
namespace DUNES.UI.WiewModels
{

    /// <summary>
    /// All ASN Information
    /// </summary>
    public class AsnCompanyClientsWm
    {
       
      public ASNWm asdDto { get; set; }

      public  List<WMSClientCompaniesDto> listcompanyclients {  get; set; }

    }



}
