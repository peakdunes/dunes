using APIZEBRA.Models.B2b;
using APIZEBRA.Models.B2B;
using APIZEBRA.ModelsWMS.Masters;
using System.ComponentModel.DataAnnotations;

namespace APIZEBRA.DTOs.B2B
{
    /// <summary>
    /// Show all information about a Repair in our system
    /// </summary>
    public class CheckRepairInformationDto
    {
        /// <summary>
        /// actions realized for this repair
        /// </summary>
        public List<TrepairActionsLog>? actionlist { get; set; }
        /// <summary>
        /// technician assigned for this repair
        /// </summary>
        public UserMvcAssignments? assigninfo { get; set; }

        /// <summary>
        /// RMA Info file
        /// </summary>
        public TzebInBoundRequestsFile? infofile { get; set; }
        /// <summary>
        /// Movement through the different process area
        /// </summary>
        public List<MvcChangeAreaLogs>? changeareaslist { get; set; }
        /// <summary>
        /// B2B Calls Peak to ZEBRA
        /// </summary>
        public List<TzebB2bOutBoundRequestsLog>? calllist { get; set; }
        /// <summary>
        /// Fault codes diagnostic
        /// </summary>
        public List<TzebRepairCodes>? listcodes { get; set; }
        /// <summary>
        /// Fautl codes diagnostic parts
        /// </summary>
        public List<TzebRepairCodesPartNo>? faultlistparts { get; set; }
        /// <summary>
        /// Orange label for replacement or repairable parts
        /// </summary>
        public List<TzebB2bReplacedPartLabel>? labelslist { get; set; }
        /// <summary>
        /// Shipping information
        /// </summary>
        public List<TorderRepairItemsSerialsShipping>? listshipping { get; set; }
        /// <summary>
        /// WMS inventory movement for this repair
        /// </summary>
        public List<InventorytransactionDetail>? listinventorydetail { get; set; }
        /// <summary>
        /// ZEBRA holds 
        /// </summary>
        public List<TzebInBoundRequestsFileHoldsLog>? listhold { get; set; }
        /// <summary>
        /// WIP information (repair and box number)
        /// </summary>
        public Trepair? infotrepair { get; set; }
        /// <summary>
        /// Reciving process information
        /// </summary>
                
        public UserMvcReceiving? inforeceiving { get; set; }
        /// <summary>
        /// Preflash process information
        /// </summary>
        public MvcRepairPreflash? infopreflash { get; set; }

        /// <summary>
        /// action Name
        /// </summary>
        public string? username { get; set; }
      

    }
}
