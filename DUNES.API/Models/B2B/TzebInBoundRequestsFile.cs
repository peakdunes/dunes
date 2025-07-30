using System;
using System.Collections.Generic;

namespace DUNES.API.Models.B2b;

public partial class TzebInBoundRequestsFile
{
    public string RowId { get; set; } = null!;

    public string? RepairCenter { get; set; }

    public string? ServiceRequestNumber { get; set; }

    public string? RmaStatus { get; set; }

    public string? ServiceRequestLineNumber { get; set; }

    public string? SerialNumber { get; set; }

    public string? ServiceRequestLineStatus { get; set; }

    public string? ServiceRequestJobType { get; set; }

    public string? PartNumber { get; set; }

    public string? ContractCode { get; set; }

    public string? ContractCodeDescription { get; set; }

    public string? BatteryManagement { get; set; }

    public string? BatteryManagementType { get; set; }

    public string? BatteryExchange { get; set; }

    public string? ComprehensiveContractPlus { get; set; }

    public string? AccessoriesCovered { get; set; }

    public string? ContractCoverageHours { get; set; }

    public string? ContractApplicationLoading { get; set; }

    public string? ManagedConfComiss { get; set; }

    public string? ExpressReplacement { get; set; }

    public string? SpecialShipping { get; set; }

    public string? ComprehensiveDamageCover { get; set; }

    public string? SiteCode { get; set; }

    public string? SiteName { get; set; }

    public string? ShipToCity { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? Address3 { get; set; }

    public string? Address4 { get; set; }

    public string? ShipToState { get; set; }

    public string? ShipToPostalCode { get; set; }

    public string? ShipToContactPhone { get; set; }

    public string? ShipToContactName { get; set; }

    public string? ShipToCountry { get; set; }

    public string? CustomerReference { get; set; }

    public string? CustomerReference2 { get; set; }

    public string? CustomerReference3 { get; set; }

    public string? ProblemCode { get; set; }

    public string? ResponseDate { get; set; }

    public string? Carrier { get; set; }

    public string? CarrierCustomerAccount { get; set; }

    public string? CarrierShipmentMethod { get; set; }

    /// <summary>
    /// &apos;Cancelled&apos; then close the repair order in the DBK system; &apos;Return Unrepaired&apos; then return the unit back to customer - TRepairState_Id = 3; &apos;Scrap&apos; then scrap the unit (ABUSED); &apos;Receive Only&apos; repair the unit and send to the sparepool. No shipment against this order; &apos;Ship Only&apos; ship a good  unit from the sparepool; no defective unit is expected from customer
    /// </summary>
    public string? InstructionType { get; set; }

    public string? SoftwareUpgrades { get; set; }

    public string? SparePoolId { get; set; }

    public string? LikeModelReplacement { get; set; }

    public string? SameSerialRequired { get; set; }

    public string? ReturnScrap { get; set; }

    public string? ManufactureDate { get; set; }

    public string? RepeatReturn { get; set; }

    public string? PreServiceRequestNum { get; set; }

    public string? PreServiceRequestLineNum { get; set; }

    public string? ProductEoslDate { get; set; }

    public string? SpecialProject { get; set; }

    public string? QualityCreteria { get; set; }

    public string? ConsolidatedShipment { get; set; }

    public string? LegacyJobCode { get; set; }

    public string? WarrantyEndDate { get; set; }

    public string? OpenDate { get; set; }

    public string? CompatibleModelList { get; set; }

    public string? PreviousSrShipDate { get; set; }

    public string? PreviousSrPrimaryFault { get; set; }

    public string? RepairResponseDate { get; set; }

    public string? RabRequired { get; set; }

    public string? SaturdayDelivery { get; set; }

    public string? ReturnAirwayBillAccountNum { get; set; }

    public string? ReturnCustomerPrePrinted { get; set; }

    public string? ShipToContactEmail { get; set; }

    public string? ReturnShippmentMethod { get; set; }

    public string? ReturnShipmentCarrier { get; set; }

    public int? RefNo { get; set; }

    public int? RepairNo { get; set; }

    public string? ErrorMsg { get; set; }

    public DateTime? DateInserted { get; set; }

    public DateTime? DateUpdated { get; set; }

    public DateTime? DateClosed { get; set; }

    public int? OrderItemNo { get; set; }

    public string? AdditionalProblemDescription { get; set; }

    public string? CustomerNo { get; set; }

    public string? CustomerName { get; set; }

    public DateTime? DateCancelled { get; set; }

    public int? RefNoShp { get; set; }

    public int? RepairNoShp { get; set; }
}
