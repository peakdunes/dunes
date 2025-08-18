using System;
using System.Collections.Generic;
using DUNES.API.Models.Inventory;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context100 : DbContext
{
    public context100()
    {
    }

    public context100(DbContextOptions<context100> options)
        : base(options)
    {
    }

    public virtual DbSet<TzebB2bAsnLineItemTblItemInbConsReqsLog> TzebB2bAsnLineItemTblItemInbConsReqsLog { get; set; }

    public virtual DbSet<TzebB2bAsnOutHdrDetItemInbConsReqs> TzebB2bAsnOutHdrDetItemInbConsReqs { get; set; }

    public virtual DbSet<TzebB2bPSoLineItemTblItemInbConsReqsLog> TzebB2bPSoLineItemTblItemInbConsReqsLog { get; set; }

    public virtual DbSet<TzebB2bPSoWoHdrTblItemInbConsReqsLog> TzebB2bPSoWoHdrTblItemInbConsReqsLog { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=ZebraUAT;TrustServerCertificate=true;persist security info=True;user id=radeonuat;password=Mrhojt53");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebB2bAsnLineItemTblItemInbConsReqsLog>(entity =>
        {
            entity.ToTable("_TZEB_B2B_ASN_LINE_ITEM_TBL_ITEM_Inb_Cons_Reqs_Log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AsnLineItemTblItemId).HasColumnName("ASN_LINE_ITEM_TBL_ITEM_ID");
            entity.Property(e => e.AsnOutHdrDetItemId).HasColumnName("ASN_OUT_HDR_DET_ITEM_ID");
            entity.Property(e => e.Attribute1)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE1");
            entity.Property(e => e.Attribute10)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE10");
            entity.Property(e => e.Attribute11)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE11");
            entity.Property(e => e.Attribute12)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE12");
            entity.Property(e => e.Attribute13)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE13");
            entity.Property(e => e.Attribute14)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE14");
            entity.Property(e => e.Attribute15)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE15");
            entity.Property(e => e.Attribute2)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE2");
            entity.Property(e => e.Attribute3)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE3");
            entity.Property(e => e.Attribute4)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE4");
            entity.Property(e => e.Attribute5)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE5");
            entity.Property(e => e.Attribute6)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE6");
            entity.Property(e => e.Attribute7)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE7");
            entity.Property(e => e.Attribute8)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE8");
            entity.Property(e => e.Attribute9)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE9");
            entity.Property(e => e.Comments)
                .HasMaxLength(4000)
                .HasColumnName("COMMENTS");
            entity.Property(e => e.ContainerNum)
                .HasMaxLength(30)
                .HasColumnName("CONTAINER_NUM");
            entity.Property(e => e.DateCompleted).HasColumnName("DATE_COMPLETED");
            entity.Property(e => e.DateTimeInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Inserted");
            entity.Property(e => e.HazardClass)
                .HasMaxLength(40)
                .HasColumnName("HAZARD_CLASS");
            entity.Property(e => e.InventoryItemId).HasColumnName("INVENTORY_ITEM_ID");
            entity.Property(e => e.ItemDescription)
                .HasMaxLength(240)
                .HasColumnName("ITEM_DESCRIPTION");
            entity.Property(e => e.ItemNumber)
                .HasMaxLength(40)
                .HasColumnName("ITEM_NUMBER");
            entity.Property(e => e.LineNum).HasColumnName("LINE_NUM");
            entity.Property(e => e.Locator)
                .HasMaxLength(204)
                .HasColumnName("LOCATOR");
            entity.Property(e => e.LpnInfo)
                .HasMaxLength(100)
                .HasColumnName("LPN_INFO");
            entity.Property(e => e.OrderLineId).HasColumnName("ORDER_LINE_ID");
            entity.Property(e => e.OrderNum).HasColumnName("ORDER_NUM");
            entity.Property(e => e.OrderType)
                .HasMaxLength(30)
                .HasColumnName("ORDER_TYPE");
            entity.Property(e => e.OrganizationCode)
                .HasMaxLength(3)
                .HasColumnName("ORGANIZATION_CODE");
            entity.Property(e => e.PoShipmentNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("PO_SHIPMENT_NUMBER");
            entity.Property(e => e.QuantityReceived)
                .HasDefaultValue(0f)
                .HasColumnName("QUANTITY_RECEIVED");
            entity.Property(e => e.QuantityShipped).HasColumnName("QUANTITY_SHIPPED");
            entity.Property(e => e.ReasonCode)
                .HasMaxLength(50)
                .HasColumnName("REASON_CODE");
            entity.Property(e => e.ReceiptRouting)
                .HasMaxLength(80)
                .HasColumnName("RECEIPT_ROUTING");
            entity.Property(e => e.ReconfiguredAssembly)
                .HasMaxLength(40)
                .HasColumnName("RECONFIGURED_ASSEMBLY");
            entity.Property(e => e.ShipmentLineId).HasColumnName("SHIPMENT_LINE_ID");
            entity.Property(e => e.ShipmentLineStatusCode)
                .HasMaxLength(25)
                .HasColumnName("SHIPMENT_LINE_STATUS_CODE");
            entity.Property(e => e.SubinventoryCode)
                .HasMaxLength(10)
                .HasColumnName("SUBINVENTORY_CODE");
            entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
            entity.Property(e => e.TransactionQuantity).HasColumnName("TRANSACTION_QUANTITY");
            entity.Property(e => e.TruckNum)
                .HasMaxLength(35)
                .HasColumnName("TRUCK_NUM");
            entity.Property(e => e.UnNumber)
                .HasMaxLength(25)
                .HasColumnName("UN_NUMBER");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(25)
                .HasColumnName("UNIT_OF_MEASURE");
            entity.Property(e => e.WipJobName)
                .HasMaxLength(240)
                .HasColumnName("WIP_JOB_NAME");
        });

        modelBuilder.Entity<TzebB2bAsnOutHdrDetItemInbConsReqs>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TZEB_B2B_Inbound_ASN_Consignment_Requests");

            entity.ToTable("_TZEB_B2B_ASN_OUT_HDR_DET_ITEM_Inb_Cons_Reqs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attribute1)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE1");
            entity.Property(e => e.Attribute10)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE10");
            entity.Property(e => e.Attribute2)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE2");
            entity.Property(e => e.Attribute3)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE3");
            entity.Property(e => e.Attribute4)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE4");
            entity.Property(e => e.Attribute5)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE5");
            entity.Property(e => e.Attribute6)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE6");
            entity.Property(e => e.Attribute7)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE7");
            entity.Property(e => e.Attribute8)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE8");
            entity.Property(e => e.Attribute9)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE9");
            entity.Property(e => e.BatchId)
                .HasMaxLength(150)
                .HasColumnName("BATCH_ID");
            entity.Property(e => e.BillOfLading)
                .HasMaxLength(25)
                .HasColumnName("BILL_OF_LADING");
            entity.Property(e => e.CarrierMethod)
                .HasMaxLength(2)
                .HasColumnName("CARRIER_METHOD");
            entity.Property(e => e.Comments)
                .HasMaxLength(4000)
                .HasColumnName("COMMENTS");
            entity.Property(e => e.ConsignRequestId).HasColumnName("Consign_RequestID");
            entity.Property(e => e.DateTimeInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Inserted");
            entity.Property(e => e.DateTimeUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Updated");
            entity.Property(e => e.ExpectedReceiptDate).HasColumnName("EXPECTED_RECEIPT_DATE");
            entity.Property(e => e.FreightCarrierCode)
                .HasMaxLength(25)
                .HasColumnName("FREIGHT_CARRIER_CODE");
            entity.Property(e => e.NumOfContainers).HasColumnName("NUM_OF_CONTAINERS");
            entity.Property(e => e.Operation)
                .HasMaxLength(30)
                .HasColumnName("OPERATION");
            entity.Property(e => e.OrgSystemId3pl)
                .HasMaxLength(150)
                .HasColumnName("ORG_SYSTEM_ID_3PL");
            entity.Property(e => e.OrganizationCode)
                .HasMaxLength(20)
                .HasColumnName("ORGANIZATION_CODE");
            entity.Property(e => e.PackingSlip)
                .HasMaxLength(25)
                .HasColumnName("PACKING_SLIP");
            entity.Property(e => e.ReceiptSourceCode)
                .HasMaxLength(25)
                .HasColumnName("RECEIPT_SOURCE_CODE");
            entity.Property(e => e.ShipToLocationId).HasColumnName("SHIP_TO_LOCATION_ID");
            entity.Property(e => e.ShipmentHeaderId).HasColumnName("SHIPMENT_HEADER_ID");
            entity.Property(e => e.ShipmentNum)
                .HasMaxLength(30)
                .HasColumnName("SHIPMENT_NUM");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(30)
                .HasColumnName("TRANSACTION_TYPE");
            entity.Property(e => e.VendorId).HasColumnName("VENDOR_ID");
            entity.Property(e => e.VendorName)
                .HasMaxLength(240)
                .HasColumnName("VENDOR_NAME");
            entity.Property(e => e.VendorSiteCode)
                .HasMaxLength(15)
                .HasColumnName("VENDOR_SITE_CODE");
            entity.Property(e => e.WaybillAirbillNum)
                .HasMaxLength(20)
                .HasColumnName("WAYBILL_AIRBILL_NUM");
        });

        modelBuilder.Entity<TzebB2bPSoLineItemTblItemInbConsReqsLog>(entity =>
        {
            entity.ToTable("_TZEB_B2B_P_SO_LINE_ITEM_TBL_ITEM_Inb_Cons_Reqs_Log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AltLangItemDescription)
                .HasMaxLength(140)
                .HasColumnName("ALT_LANG_ITEM_DESCRIPTION");
            entity.Property(e => e.Attribute1)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE1");
            entity.Property(e => e.Attribute10)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE10");
            entity.Property(e => e.Attribute11)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE11");
            entity.Property(e => e.Attribute12)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE12");
            entity.Property(e => e.Attribute13)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE13");
            entity.Property(e => e.Attribute14)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE14");
            entity.Property(e => e.Attribute15)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE15");
            entity.Property(e => e.Attribute2)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE2");
            entity.Property(e => e.Attribute3)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE3");
            entity.Property(e => e.Attribute4)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE4");
            entity.Property(e => e.Attribute5)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE5");
            entity.Property(e => e.Attribute6)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE6");
            entity.Property(e => e.Attribute7)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE7");
            entity.Property(e => e.Attribute8)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE8");
            entity.Property(e => e.Attribute9)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE9");
            entity.Property(e => e.BlindPaperwork)
                .HasMaxLength(240)
                .HasColumnName("BLIND_PAPERWORK");
            entity.Property(e => e.Class)
                .HasMaxLength(150)
                .HasColumnName("CLASS");
            entity.Property(e => e.ConversionRate).HasColumnName("CONVERSION_RATE");
            entity.Property(e => e.CooCAttribute1)
                .HasMaxLength(30)
                .HasColumnName("COO_C_ATTRIBUTE1");
            entity.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
            entity.Property(e => e.CreationDate).HasColumnName("CREATION_DATE");
            entity.Property(e => e.CustomerPartNumber)
                .HasMaxLength(400)
                .HasColumnName("CUSTOMER_PART_NUMBER");
            entity.Property(e => e.CustomerPoNumber)
                .HasMaxLength(50)
                .HasColumnName("CUSTOMER_PO_NUMBER");
            entity.Property(e => e.CustomerSuppliedPaperwork)
                .HasMaxLength(240)
                .HasColumnName("CUSTOMER_SUPPLIED_PAPERWORK");
            entity.Property(e => e.DateScheduled).HasColumnName("DATE_SCHEDULED");
            entity.Property(e => e.DateTimeInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Inserted");
            entity.Property(e => e.DateTimeProcessed)
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Processed");
            entity.Property(e => e.DeliveryDetailId).HasColumnName("DELIVERY_DETAIL_ID");
            entity.Property(e => e.EndUserPoNumber)
                .HasMaxLength(240)
                .HasColumnName("END_USER_PO_NUMBER");
            entity.Property(e => e.ErrorCode)
                .HasMaxLength(400)
                .HasColumnName("ERROR_CODE");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(4000)
                .HasColumnName("ERROR_MESSAGE");
            entity.Property(e => e.FairMarketValue)
                .HasMaxLength(240)
                .HasColumnName("FAIR_MARKET_VALUE");
            entity.Property(e => e.FinalDestination)
                .HasMaxLength(240)
                .HasColumnName("FINAL_DESTINATION");
            entity.Property(e => e.Frm3plAttribute1)
                .HasMaxLength(150)
                .HasColumnName("FRM_3PL_ATTRIBUTE1");
            entity.Property(e => e.Frm3plAttribute2)
                .HasMaxLength(150)
                .HasColumnName("FRM_3PL_ATTRIBUTE2");
            entity.Property(e => e.Frm3plAttribute3)
                .HasMaxLength(150)
                .HasColumnName("FRM_3PL_ATTRIBUTE3");
            entity.Property(e => e.Frm3plAttribute4)
                .HasMaxLength(150)
                .HasColumnName("FRM_3PL_ATTRIBUTE4");
            entity.Property(e => e.Frm3plAttribute5)
                .HasMaxLength(150)
                .HasColumnName("FRM_3PL_ATTRIBUTE5");
            entity.Property(e => e.Frm3plAttribute6)
                .HasMaxLength(150)
                .HasColumnName("FRM_3PL_ATTRIBUTE6");
            entity.Property(e => e.Frm3plAttribute7)
                .HasMaxLength(150)
                .HasColumnName("FRM_3PL_ATTRIBUTE7");
            entity.Property(e => e.Frm3plAttribute8)
                .HasMaxLength(150)
                .HasColumnName("FRM_3PL_ATTRIBUTE8");
            entity.Property(e => e.Frm3plLocator)
                .HasMaxLength(50)
                .HasColumnName("FRM_3PL_LOCATOR");
            entity.Property(e => e.Frm3plLocatorStatus)
                .HasMaxLength(50)
                .HasColumnName("FRM_3PL_LOCATOR_STATUS");
            entity.Property(e => e.Frm3plSubinv)
                .HasMaxLength(50)
                .HasColumnName("FRM_3PL_SUBINV");
            entity.Property(e => e.Haz)
                .HasMaxLength(150)
                .HasColumnName("HAZ");
            entity.Property(e => e.HazardClass)
                .HasMaxLength(25)
                .HasColumnName("HAZARD_CLASS");
            entity.Property(e => e.HtsCode)
                .HasMaxLength(240)
                .HasColumnName("HTS_CODE");
            entity.Property(e => e.HtsDescription)
                .HasMaxLength(240)
                .HasColumnName("HTS_DESCRIPTION");
            entity.Property(e => e.Incoterms)
                .HasMaxLength(240)
                .HasColumnName("INCOTERMS");
            entity.Property(e => e.InventoryItemId).HasColumnName("INVENTORY_ITEM_ID");
            entity.Property(e => e.ItemDescription)
                .HasMaxLength(240)
                .HasColumnName("ITEM_DESCRIPTION");
            entity.Property(e => e.ItemNumber)
                .HasMaxLength(40)
                .HasColumnName("ITEM_NUMBER");
            entity.Property(e => e.LastUpdateDate).HasColumnName("LAST_UPDATE_DATE");
            entity.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY");
            entity.Property(e => e.LineId).HasColumnName("LINE_ID");
            entity.Property(e => e.MoveOrderLineId).HasColumnName("MOVE_ORDER_LINE_ID");
            entity.Property(e => e.MoveOrderNumber)
                .HasMaxLength(30)
                .HasColumnName("MOVE_ORDER_NUMBER");
            entity.Property(e => e.Nfmc)
                .HasMaxLength(150)
                .HasColumnName("NFMC");
            entity.Property(e => e.OrderLineNumber)
                .HasMaxLength(12)
                .HasColumnName("ORDER_LINE_NUMBER");
            entity.Property(e => e.OrderQuantity).HasColumnName("ORDER_QUANTITY");
            entity.Property(e => e.OrderingUom)
                .HasMaxLength(25)
                .HasColumnName("ORDERING_UOM");
            entity.Property(e => e.PSoWoHdrTblItemId).HasColumnName("P_SO_WO_HDR_TBL_ITEM_id");
            entity.Property(e => e.PickLpn)
                .HasMaxLength(10)
                .HasColumnName("Pick_LPN");
            entity.Property(e => e.PortOfDestination)
                .HasMaxLength(240)
                .HasColumnName("PORT_OF_DESTINATION");
            entity.Property(e => e.PortOfLoading)
                .HasMaxLength(240)
                .HasColumnName("PORT_OF_LOADING");
            entity.Property(e => e.ProcessStatus)
                .HasMaxLength(120)
                .HasColumnName("PROCESS_STATUS");
            entity.Property(e => e.PtoCompUnitQty).HasColumnName("PTO_COMP_UNIT_QTY");
            entity.Property(e => e.PtoParentItem)
                .HasMaxLength(40)
                .HasColumnName("PTO_PARENT_ITEM");
            entity.Property(e => e.QtyOnHand).HasColumnName("Qty_On_Hand");
            entity.Property(e => e.RequestedQuantity).HasColumnName("REQUESTED_QUANTITY");
            entity.Property(e => e.RequestedQuantityUom)
                .HasMaxLength(25)
                .HasColumnName("REQUESTED_QUANTITY_UOM");
            entity.Property(e => e.Requestor)
                .HasMaxLength(400)
                .HasColumnName("REQUESTOR");
            entity.Property(e => e.SalesOrderDate).HasColumnName("SALES_ORDER_DATE");
            entity.Property(e => e.SalesOrderNumber).HasColumnName("SALES_ORDER_NUMBER");
            entity.Property(e => e.ShipToContact)
                .HasMaxLength(400)
                .HasColumnName("SHIP_TO_CONTACT");
            entity.Property(e => e.Sub)
                .HasMaxLength(150)
                .HasColumnName("SUB");
            entity.Property(e => e.TopModelLineId).HasColumnName("TOP_MODEL_LINE_ID");
            entity.Property(e => e.TransactionTempId).HasColumnName("TRANSACTION_TEMP_ID");
            entity.Property(e => e.UnCode)
                .HasMaxLength(25)
                .HasColumnName("UN_CODE");
            entity.Property(e => e.WipJobName)
                .HasMaxLength(240)
                .HasColumnName("WIP_JOB_NAME");
        });

        modelBuilder.Entity<TzebB2bPSoWoHdrTblItemInbConsReqsLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TZEB_B2B_P_SO_WO_HDR_TBL_ITEM_Consignment_Requests_Log");

            entity.ToTable("_TZEB_B2B_P_SO_WO_HDR_TBL_ITEM_Inb_Cons_Reqs_Log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Addressee)
                .HasMaxLength(240)
                .HasColumnName("ADDRESSEE");
            entity.Property(e => e.Attribute1)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE1");
            entity.Property(e => e.Attribute10)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE10");
            entity.Property(e => e.Attribute2)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE2");
            entity.Property(e => e.Attribute3)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE3");
            entity.Property(e => e.Attribute4)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE4");
            entity.Property(e => e.Attribute5)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE5");
            entity.Property(e => e.Attribute6)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE6");
            entity.Property(e => e.Attribute7)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE7");
            entity.Property(e => e.Attribute8)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE8");
            entity.Property(e => e.Attribute9)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE9");
            entity.Property(e => e.BatchId).HasColumnName("BATCH_ID");
            entity.Property(e => e.BillToAddress1)
                .HasMaxLength(240)
                .HasColumnName("BILL_TO_ADDRESS1");
            entity.Property(e => e.BillToAddress2)
                .HasMaxLength(240)
                .HasColumnName("BILL_TO_ADDRESS2");
            entity.Property(e => e.BillToAddress3)
                .HasMaxLength(240)
                .HasColumnName("BILL_TO_ADDRESS3");
            entity.Property(e => e.BillToAddress4)
                .HasMaxLength(240)
                .HasColumnName("BILL_TO_ADDRESS4");
            entity.Property(e => e.BillToCity)
                .HasMaxLength(60)
                .HasColumnName("BILL_TO_CITY");
            entity.Property(e => e.BillToCountry)
                .HasMaxLength(60)
                .HasColumnName("BILL_TO_COUNTRY");
            entity.Property(e => e.BillToCounty)
                .HasMaxLength(60)
                .HasColumnName("BILL_TO_COUNTY");
            entity.Property(e => e.BillToPostalCode)
                .HasMaxLength(60)
                .HasColumnName("BILL_TO_POSTAL_CODE");
            entity.Property(e => e.BillToProvince)
                .HasMaxLength(60)
                .HasColumnName("BILL_TO_PROVINCE");
            entity.Property(e => e.BillToState)
                .HasMaxLength(60)
                .HasColumnName("BILL_TO_STATE");
            entity.Property(e => e.Carrier)
                .HasMaxLength(360)
                .HasColumnName("CARRIER");
            entity.Property(e => e.ConsignRequestId).HasColumnName("Consign_RequestID");
            entity.Property(e => e.ConsigneeName)
                .HasMaxLength(240)
                .HasColumnName("CONSIGNEE_NAME");
            entity.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
            entity.Property(e => e.CreationDate).HasColumnName("CREATION_DATE");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(240)
                .HasColumnName("CUSTOMER_NAME");
            entity.Property(e => e.CustomerNumber)
                .HasMaxLength(400)
                .HasColumnName("CUSTOMER_NUMBER");
            entity.Property(e => e.DateTimeConfirmed)
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Confirmed");
            entity.Property(e => e.DateTimeError)
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Error");
            entity.Property(e => e.DateTimeInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Inserted");
            entity.Property(e => e.DateTimeOnlineOrders)
                .HasColumnType("datetime")
                .HasColumnName("DateTime_OnlineOrders");
            entity.Property(e => e.DateTimeOnlineOrdersError)
                .HasColumnType("datetime")
                .HasColumnName("DateTime_OnlineOrdersError");
            entity.Property(e => e.DateTimeProcessed)
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Processed");
            entity.Property(e => e.DeliveryId).HasColumnName("DELIVERY_ID");
            entity.Property(e => e.ErrorCode)
                .HasMaxLength(400)
                .HasColumnName("ERROR_CODE");
            entity.Property(e => e.ErrorMesagge)
                .HasMaxLength(4000)
                .HasColumnName("ERROR_MESAGGE");
            entity.Property(e => e.FinalDestination)
                .HasMaxLength(400)
                .HasColumnName("FINAL_DESTINATION");
            entity.Property(e => e.FreightAccountNumber)
                .HasMaxLength(150)
                .HasColumnName("FREIGHT_ACCOUNT_NUMBER");
            entity.Property(e => e.FreightTerms)
                .HasMaxLength(30)
                .HasColumnName("FREIGHT_TERMS");
            entity.Property(e => e.HeaderId).HasColumnName("HEADER_ID");
            entity.Property(e => e.IncoTerms)
                .HasMaxLength(400)
                .HasColumnName("INCO_TERMS");
            entity.Property(e => e.LastUpdateDate).HasColumnName("LAST_UPDATE_DATE");
            entity.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY");
            entity.Property(e => e.ModeOfTransport)
                .HasMaxLength(30)
                .HasColumnName("MODE_OF_TRANSPORT");
            entity.Property(e => e.Operation)
                .HasMaxLength(30)
                .HasColumnName("OPERATION");
            entity.Property(e => e.OrgSystemId3pl)
                .HasMaxLength(150)
                .HasColumnName("ORG_SYSTEM_ID_3PL");
            entity.Property(e => e.OutConsReqsId).HasColumnName("Out_Cons_Reqs_Id");
            entity.Property(e => e.PackSlipNumber)
                .HasMaxLength(30)
                .HasColumnName("PACK_SLIP_NUMBER");
            entity.Property(e => e.PortOfDestination)
                .HasMaxLength(400)
                .HasColumnName("PORT_OF_DESTINATION");
            entity.Property(e => e.PortOfLoading)
                .HasMaxLength(400)
                .HasColumnName("PORT_OF_LOADING");
            entity.Property(e => e.ProcessStatus)
                .HasMaxLength(120)
                .HasColumnName("PROCESS_STATUS");
            entity.Property(e => e.ServiceLevel)
                .HasMaxLength(30)
                .HasColumnName("SERVICE_LEVEL");
            entity.Property(e => e.ShipDateTimeConfirmed)
                .HasColumnType("datetime")
                .HasColumnName("Ship_DateTime_Confirmed");
            entity.Property(e => e.ShipDateTimeError)
                .HasColumnType("datetime")
                .HasColumnName("Ship_DateTime_Error");
            entity.Property(e => e.ShipErrorMsg).HasColumnName("Ship_ErrorMsg");
            entity.Property(e => e.ShipMethod)
                .HasMaxLength(240)
                .HasColumnName("SHIP_METHOD");
            entity.Property(e => e.ShipOutConsReqsId).HasColumnName("Ship_Out_Cons_Reqs_Id");
            entity.Property(e => e.ShipToAddress1)
                .HasMaxLength(240)
                .HasColumnName("SHIP_TO_ADDRESS1");
            entity.Property(e => e.ShipToAddress2)
                .HasMaxLength(240)
                .HasColumnName("SHIP_TO_ADDRESS2");
            entity.Property(e => e.ShipToAddress3)
                .HasMaxLength(240)
                .HasColumnName("SHIP_TO_ADDRESS3");
            entity.Property(e => e.ShipToAddress4)
                .HasMaxLength(240)
                .HasColumnName("SHIP_TO_ADDRESS4");
            entity.Property(e => e.ShipToCity)
                .HasMaxLength(60)
                .HasColumnName("SHIP_TO_CITY");
            entity.Property(e => e.ShipToCountry)
                .HasMaxLength(60)
                .HasColumnName("SHIP_TO_COUNTRY");
            entity.Property(e => e.ShipToCounty)
                .HasMaxLength(60)
                .HasColumnName("SHIP_TO_COUNTY");
            entity.Property(e => e.ShipToPostalCode)
                .HasMaxLength(60)
                .HasColumnName("SHIP_TO_POSTAL_CODE");
            entity.Property(e => e.ShipToProvince)
                .HasMaxLength(60)
                .HasColumnName("SHIP_TO_PROVINCE");
            entity.Property(e => e.ShipToState)
                .HasMaxLength(60)
                .HasColumnName("SHIP_TO_STATE");
            entity.Property(e => e.ShippingPoint)
                .HasMaxLength(400)
                .HasColumnName("SHIPPING_POINT");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(30)
                .HasColumnName("TRANSACTION_TYPE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
