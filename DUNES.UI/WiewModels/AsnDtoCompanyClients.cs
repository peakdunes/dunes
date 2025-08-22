using DUNES.Shared.DTOs.Inventory;

namespace DUNES.UI.WiewModels
{

    /// <summary>
    /// All ASN Information
    /// </summary>
    public class AsnDtoCompanyClients
    {
       
      public  ASNDto asdDto { get; set; }

      public  List<WMSClientCompanies> listcompanyclients {  get; set; }

    }
}
