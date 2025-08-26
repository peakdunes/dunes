using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{

    /// <summary>
    /// ALL information required for ASN receiving process
    /// </summary>
    public class ASNProcessInformationDto
    {
        
        public List<WMSBinsDto> listbines {  get; set; }
        public List<WMSConceptsDto> listconcepts { get; set; } 
        public List<WMSInputTransactionsDto> listinputtransactions { get; set; }
        public List<InventoryTypeDto> listinventorytype { get; set; }
        public List<WMSInventoryTypeDto> listwmsinventorytype { get; set; }
        public List<itemstatusDto> listitemstatus { get; set; }

    }
     

    //public class WMSConceptsDto
    //{

    //    public int Id { get; set; }

    //    public string Name { get; set; }
    //}


    //public class WMSInputTransactionsDto
    //{

    //    public int Id { get; set; }

    //    public string Name { get; set; }
    //}

    //public class WMSBinsDto
    //{

    //    public int Id { get; set; }

    //    public string TagName { get; set; }
    //}


    //public class InventoryTypeDto
    //{

    //    public int Id { get; set; }

    //    public string Name { get; set; }
    //}

    //public class WMSInventoryTypeDto
    //{

    //    public int Id { get; set; }

    //    public string Name { get; set; }
    //}


    //public class itemstatusDto
    //{

    //    public int Id { get; set; }

    //    public string Name { get; set; }
    //}
}
