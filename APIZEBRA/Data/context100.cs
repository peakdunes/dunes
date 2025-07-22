using System;
using System.Collections.Generic;
using APIZEBRA.Models.B2b;
using Microsoft.EntityFrameworkCore;

namespace APIZEBRA.Data;

public partial class context100 : DbContext
{
    public context100()
    {
    }

    public context100(DbContextOptions<context100> options)
        : base(options)
    {
    }

    public virtual DbSet<TzebInBoundRequestsFile> TzebInBoundRequestsFile { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebInBoundRequestsFile>(entity =>
        {
            entity.HasKey(e => e.RowId).HasName("PK__TZEB_InBound_Requests_File_1");

            entity.ToTable("_TZEB_InBound_Requests_File", tb =>
                {
                    tb.HasTrigger("RECORD_DELETED");
                    tb.HasTrigger("RECORD_INSERTED");
                    tb.HasTrigger("RECORD_UPDATED");
                });

            entity.HasIndex(e => e.ServiceRequestLineNumber, "<Name of Missing Index, sysname,>");

            entity.HasIndex(e => new { e.RefNo, e.RepairNo }, "IX__TZEB_InBound_Requests_File");

            entity.HasIndex(e => e.RowId, "IX__TZEB_InBound_Requests_File_ROWID").IsUnique();

            entity.HasIndex(e => e.RefNo, "IX__TZEB_InBound_Requests_File_Ref_No");

            entity.HasIndex(e => e.RepairNo, "Instuction_Ref_No_J_IDX");

            entity.HasIndex(e => e.SerialNumber, "NonClusteredIndex-20181206-100432");

            entity.HasIndex(e => e.RowId, "NonClusteredIndex-20191017-114551");

            entity.HasIndex(e => e.RepairNoShp, "Ref_No_SHP_IDX");

            entity.HasIndex(e => e.RefNoShp, "Response_Date_IDX");

            entity.HasIndex(e => e.RepairNo, "_TZEB_InBound_Requests_File_Index");

            entity.HasIndex(e => e.RefNo, "_TZEB_InBound_Requests_File_b2b-index");

            entity.HasIndex(e => e.RefNo, "_TZEB_InBound_Requests_File_index-b2b");

            entity.Property(e => e.RowId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROW_ID");
            entity.Property(e => e.AccessoriesCovered)
                .HasMaxLength(255)
                .HasColumnName("Accessories_Covered");
            entity.Property(e => e.AdditionalProblemDescription)
                .HasMaxLength(255)
                .HasColumnName("Additional_Problem_Description");
            entity.Property(e => e.Address1).HasMaxLength(255);
            entity.Property(e => e.Address2).HasMaxLength(255);
            entity.Property(e => e.Address3).HasMaxLength(255);
            entity.Property(e => e.Address4).HasMaxLength(255);
            entity.Property(e => e.BatteryExchange)
                .HasMaxLength(255)
                .HasColumnName("Battery_Exchange");
            entity.Property(e => e.BatteryManagement)
                .HasMaxLength(255)
                .HasColumnName("Battery_Management");
            entity.Property(e => e.BatteryManagementType)
                .HasMaxLength(255)
                .HasColumnName("Battery_Management_Type");
            entity.Property(e => e.Carrier).HasMaxLength(255);
            entity.Property(e => e.CarrierCustomerAccount)
                .HasMaxLength(255)
                .HasColumnName("Carrier_Customer_Account");
            entity.Property(e => e.CarrierShipmentMethod)
                .HasMaxLength(255)
                .HasColumnName("Carrier_Shipment_Method");
            entity.Property(e => e.CompatibleModelList)
                .HasMaxLength(255)
                .HasColumnName("Compatible_Model_List");
            entity.Property(e => e.ComprehensiveContractPlus)
                .HasMaxLength(255)
                .HasColumnName("Comprehensive_Contract_Plus");
            entity.Property(e => e.ComprehensiveDamageCover)
                .HasMaxLength(255)
                .HasColumnName("Comprehensive_Damage_Cover");
            entity.Property(e => e.ConsolidatedShipment)
                .HasMaxLength(255)
                .HasColumnName("Consolidated_Shipment");
            entity.Property(e => e.ContractApplicationLoading)
                .HasMaxLength(255)
                .HasColumnName("Contract_Application_Loading");
            entity.Property(e => e.ContractCode)
                .HasMaxLength(255)
                .HasColumnName("Contract_Code");
            entity.Property(e => e.ContractCodeDescription)
                .HasMaxLength(255)
                .HasColumnName("Contract_Code_Description");
            entity.Property(e => e.ContractCoverageHours)
                .HasMaxLength(255)
                .HasColumnName("Contract_Coverage_Hours");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(255)
                .HasColumnName("Customer_Name");
            entity.Property(e => e.CustomerNo)
                .HasMaxLength(255)
                .HasColumnName("Customer_No");
            entity.Property(e => e.CustomerReference)
                .HasMaxLength(255)
                .HasColumnName("Customer_Reference");
            entity.Property(e => e.CustomerReference2)
                .HasMaxLength(255)
                .HasColumnName("Customer_Reference2");
            entity.Property(e => e.CustomerReference3)
                .HasMaxLength(255)
                .HasColumnName("Customer_Reference3");
            entity.Property(e => e.DateCancelled)
                .HasColumnType("datetime")
                .HasColumnName("Date_Cancelled");
            entity.Property(e => e.DateClosed).HasColumnType("datetime");
            entity.Property(e => e.DateInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            entity.Property(e => e.ErrorMsg).HasMaxLength(255);
            entity.Property(e => e.ExpressReplacement)
                .HasMaxLength(255)
                .HasColumnName("Express_Replacement");
            entity.Property(e => e.InstructionType)
                .HasMaxLength(255)
                .HasComment("'Cancelled' then close the repair order in the DBK system; 'Return Unrepaired' then return the unit back to customer - TRepairState_Id = 3; 'Scrap' then scrap the unit (ABUSED); 'Receive Only' repair the unit and send to the sparepool. No shipment against this order; 'Ship Only' ship a good  unit from the sparepool; no defective unit is expected from customer")
                .HasColumnName("Instruction_Type");
            entity.Property(e => e.LegacyJobCode)
                .HasMaxLength(255)
                .HasColumnName("Legacy Job Code");
            entity.Property(e => e.LikeModelReplacement)
                .HasMaxLength(255)
                .HasColumnName("Like_Model_Replacement");
            entity.Property(e => e.ManagedConfComiss)
                .HasMaxLength(255)
                .HasColumnName("Managed_Conf_Comiss");
            entity.Property(e => e.ManufactureDate)
                .HasMaxLength(255)
                .HasColumnName("Manufacture_Date");
            entity.Property(e => e.OpenDate)
                .HasMaxLength(255)
                .HasColumnName("Open_Date");
            entity.Property(e => e.OrderItemNo)
                .HasComputedColumnSql("(CONVERT([int],substring([Service_Request_Line_Number],charindex('-',[Service_Request_Line_Number])+(1),len([Service_Request_Line_Number])),(0)))", false)
                .HasColumnName("Order_Item_No");
            entity.Property(e => e.PartNumber)
                .HasMaxLength(255)
                .HasColumnName("Part_Number");
            entity.Property(e => e.PreServiceRequestLineNum)
                .HasMaxLength(255)
                .HasColumnName("Pre_Service_Request_Line_Num");
            entity.Property(e => e.PreServiceRequestNum)
                .HasMaxLength(255)
                .HasColumnName("Pre_Service_Request_Num");
            entity.Property(e => e.PreviousSrPrimaryFault)
                .HasMaxLength(255)
                .HasColumnName("Previous_SR_PrimaryFault");
            entity.Property(e => e.PreviousSrShipDate)
                .HasMaxLength(255)
                .HasColumnName("Previous_SR_ShipDate");
            entity.Property(e => e.ProblemCode)
                .HasMaxLength(255)
                .HasColumnName("Problem_Code");
            entity.Property(e => e.ProductEoslDate)
                .HasMaxLength(255)
                .HasColumnName("Product_EOSL_Date");
            entity.Property(e => e.QualityCreteria)
                .HasMaxLength(255)
                .HasColumnName("Quality Creteria");
            entity.Property(e => e.RabRequired)
                .HasMaxLength(255)
                .HasColumnName("RAB_Required");
            entity.Property(e => e.RefNo).HasColumnName("Ref_No");
            entity.Property(e => e.RefNoShp).HasColumnName("Ref_No_SHP");
            entity.Property(e => e.RepairCenter)
                .HasMaxLength(255)
                .HasColumnName("Repair Center");
            entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
            entity.Property(e => e.RepairNoShp).HasColumnName("Repair_No_SHP");
            entity.Property(e => e.RepairResponseDate)
                .HasMaxLength(255)
                .HasColumnName("Repair_Response_Date");
            entity.Property(e => e.RepeatReturn)
                .HasMaxLength(255)
                .HasColumnName("Repeat_Return");
            entity.Property(e => e.ResponseDate)
                .HasMaxLength(255)
                .HasColumnName("Response_Date");
            entity.Property(e => e.ReturnAirwayBillAccountNum)
                .HasMaxLength(255)
                .HasColumnName("Return_Airway_Bill_Account_Num");
            entity.Property(e => e.ReturnCustomerPrePrinted)
                .HasMaxLength(255)
                .HasColumnName("Return_Customer_PrePrinted");
            entity.Property(e => e.ReturnScrap)
                .HasMaxLength(255)
                .HasColumnName("Return_Scrap");
            entity.Property(e => e.ReturnShipmentCarrier)
                .HasMaxLength(255)
                .HasColumnName("Return_Shipment_Carrier");
            entity.Property(e => e.ReturnShippmentMethod)
                .HasMaxLength(255)
                .HasColumnName("Return_Shippment_Method");
            entity.Property(e => e.RmaStatus)
                .HasMaxLength(255)
                .HasColumnName("RMA_Status");
            entity.Property(e => e.SameSerialRequired)
                .HasMaxLength(255)
                .HasColumnName("Same_Serial_Required");
            entity.Property(e => e.SaturdayDelivery)
                .HasMaxLength(255)
                .HasColumnName("Saturday_Delivery");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(255)
                .HasColumnName("Serial_Number");
            entity.Property(e => e.ServiceRequestJobType)
                .HasMaxLength(255)
                .HasColumnName("Service_Request_Job_Type");
            entity.Property(e => e.ServiceRequestLineNumber)
                .HasMaxLength(255)
                .HasColumnName("Service_Request_Line_Number");
            entity.Property(e => e.ServiceRequestLineStatus)
                .HasMaxLength(255)
                .HasColumnName("Service_Request_Line_Status");
            entity.Property(e => e.ServiceRequestNumber)
                .HasMaxLength(255)
                .HasColumnName("Service_Request_Number");
            entity.Property(e => e.ShipToCity)
                .HasMaxLength(255)
                .HasColumnName("Ship_to_City");
            entity.Property(e => e.ShipToContactEmail)
                .HasMaxLength(255)
                .HasColumnName("Ship_To_Contact_Email");
            entity.Property(e => e.ShipToContactName)
                .HasMaxLength(255)
                .HasColumnName("Ship_To_ContactName");
            entity.Property(e => e.ShipToContactPhone)
                .HasMaxLength(255)
                .HasColumnName("Ship_To_Contact_Phone");
            entity.Property(e => e.ShipToCountry)
                .HasMaxLength(255)
                .HasColumnName("Ship_To_Country");
            entity.Property(e => e.ShipToPostalCode)
                .HasMaxLength(255)
                .HasColumnName("Ship_To_Postal_Code");
            entity.Property(e => e.ShipToState)
                .HasMaxLength(255)
                .HasColumnName("Ship_To_State");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .HasColumnName("Site_Code");
            entity.Property(e => e.SiteName)
                .HasMaxLength(255)
                .HasColumnName("Site_Name");
            entity.Property(e => e.SoftwareUpgrades)
                .HasMaxLength(255)
                .HasColumnName("Software_upgrades");
            entity.Property(e => e.SparePoolId)
                .HasMaxLength(255)
                .HasColumnName("Spare_Pool_ID");
            entity.Property(e => e.SpecialProject)
                .HasMaxLength(255)
                .HasColumnName("Special_Project");
            entity.Property(e => e.SpecialShipping)
                .HasMaxLength(255)
                .HasColumnName("Special_Shipping");
            entity.Property(e => e.WarrantyEndDate)
                .HasMaxLength(255)
                .HasColumnName("Warranty End Date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
