using APIZEBRA.DTOs.B2B;
using APIZEBRA.Models;
using APIZEBRA.Models.B2b;
using APIZEBRA.Models.B2B;
using APIZEBRA.Models.Masters;
using Microsoft.EntityFrameworkCore;

namespace APIZEBRA.Data
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class using the specified options.
    /// </summary>
  
    public class AppDbContext: DbContext
    {

        /// <summary>
        /// Constructor that sets up the database context with the given options.
        /// </summary>
        /// <param name="options">Configuration settings for the DbContext.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        ///############################
        ///DTO Definition
        ///############################
        ///

        /// <summary>
        /// Show Area from a Repair Order.
        /// </summary>
        public DbSet<AreaNameDto> areaNamesDto { get; set; }

        /// <summary>
        /// Basic information about repair number ready to be received
        /// </summary>
        public DbSet<RepairReadyToReceiveDto> repairReadyToReceiveDto { get; set; }

        ///############################
        ///End DTO Definition
        ///############################
        ///

        /// <summary>
        /// Stores API error logs captured by Serilog.
        /// </summary>
        public DbSet<DbkMvcLogApi> DbkMvcLogApi { get; set; }
        /// <summary>
        /// Master table for B2B consignment call types.
        /// </summary>
        public virtual DbSet<TzebB2bConsignmentCallsType> TzebB2bConsignmentCallsType { get; set; }
        /// <summary>
        /// Master table for inventory types used in B2B processing.
        /// </summary>
        public virtual DbSet<TzebB2bInventoryType> TzebB2bInventoryType { get; set; }
        /// <summary>
        /// Master list of repair action codes used during repair processing.
        /// </summary>
        public virtual DbSet<TrepairActionsCodes> TrepairActionsCodes { get; set; }
        /// <summary>
        /// Master list of fault codes assigned to devices or components.
        /// </summary>
        public virtual DbSet<TzebFaultCodes> TzebFaultCodes { get; set; }
        /// <summary>
        /// Mapping of work codes to target definitions used in the repair workflow.
        /// </summary>
        public virtual DbSet<TzebWorkCodesTargets> TzebWorkCodesTargets { get; set; }
        /// <summary>
        /// Log of inbound request files processed for Zebra B2B integrations (RMA reception).
        /// </summary>
        public virtual DbSet<TzebInBoundRequestsFile> TzebInBoundRequestsFile { get; set; }
        /// <summary>
        /// Repair order header records containing general order information.
        /// </summary>
        public virtual DbSet<TorderRepairHdr> TorderRepairHdr { get; set; }
        /// <summary>
        /// Items associated with each repair order, including part-level details.
        /// </summary>
        public virtual DbSet<TorderRepairItems> TorderRepairItems { get; set; }
        /// <summary>
        /// Tracks serial numbers received for each repair item.
        /// </summary>
        public virtual DbSet<TorderRepairItemsSerialsReceiving> TorderRepairItemsSerialsReceiving { get; set; }
        /// <summary>
        /// Tracks serial numbers shipped out after repair completion.
        /// </summary>
        public virtual DbSet<TorderRepairItemsSerialsShipping> TorderRepairItemsSerialsShipping { get; set; }
        /// <summary>
        /// Stores core repair data including diagnostics and results.WIP process
        /// </summary>
        public virtual DbSet<Trepair> Trepair { get; set; }

        /// <summary>
        /// Save all area status for each repair
        /// </summary>
        public virtual DbSet<MvcChangeAreaLogs> MvcChangeAreaLogs { get; set; }

        /// <summary>
        /// Action repair logs
        /// </summary>
        public virtual DbSet<TrepairActionsLog> TrepairActionsLog { get; set; }

        /// <summary>
        /// B2B Calls from Peaktech to ZEBRA
        /// </summary>
        public virtual DbSet<TzebB2bOutBoundRequestsLog> TzebB2bOutBoundRequestsLog { get; set; }
        /// <summary>
        /// Orange label for replacement repairable partas
        /// </summary>
        public virtual DbSet<TzebB2bReplacedPartLabel> TzebB2bReplacedPartLabel { get; set; }

        /// <summary>
        /// Diagnostic repair codes
        /// </summary>
        public virtual DbSet<TzebRepairCodes> TzebRepairCodes { get; set; }
        /// <summary>
        /// Diagnostic repair codes parts used
        /// </summary>
        public virtual DbSet<TzebRepairCodesPartNo> TzebRepairCodesPartNo { get; set; }

        /// <summary>
        /// Assigning the technician to machine
        /// </summary>
        public virtual DbSet<UserMvcAssignments> UserMvcAssignments { get; set; }

        /// <summary>
        /// preflash process record
        /// </summary>
        public virtual DbSet<MvcRepairPreflash> MvcRepairPreflash { get; set; }

        /// <summary>
        /// Receiving process record
        /// </summary>
        public virtual DbSet<UserMvcReceiving> UserMvcReceiving { get; set; }

        /// <summary>
        /// Repair calls type
        /// </summary>
        public virtual DbSet<TzebB2bOutBoundRequestsTypeOfCalls> TzebB2bOutBoundRequestsTypeOfCalls { get; set; }
        /// <summary>
        /// Codes action repair
        /// </summary>
        public virtual DbSet<TzebWorkCodesActions> TzebWorkCodesActions { get; set; }

        /// <summary>
        /// ZEBRA hold release
        /// </summary>
        public virtual DbSet<TzebInBoundRequestsFileHoldsLog> TzebInBoundRequestsFileHoldsLog { get; set; }

        /// <summary>
        /// Technician information
        /// </summary>
        public virtual DbSet<Ttech> Ttech { get; set; }

        /// <summary>
        /// DBK Users
        /// </summary>
        public virtual DbSet<Tdbkusers> Tdbkusers { get; set; }

        /// <summary>
        /// List to Rapair ready for receive and pending for receive
        /// </summary>
        public virtual DbSet<TiewRepairStatusZebraBldgReceiving> TiewRepairStatusZebraBldgReceiving { get; set; }
        /// <summary>
        /// Configures the database model and relationships using the Fluent API.
        /// This method is called when the model for a derived context has been initialized,
        /// but before the model has been locked down and used to initialize the context.
        /// </summary>
        /// <param name="modelBuilder">Provides a simple API to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbkMvcLogApi>()
                .ToTable("dbk_mvc_logs_api");

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

            modelBuilder.Entity<TzebFaultCodes>(entity =>
            {
                entity.HasKey(e => e.FaultCode);

                entity.ToTable("_TZEB_FAULT_CODES");

                entity.Property(e => e.FaultCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Fault_Code");
                entity.Property(e => e.Categorization)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.DateInserted)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.FaultCodeDefinition)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Fault_Code_Definition");
                entity.Property(e => e.FaultCodeGroup)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Fault_Code_Group");
                entity.Property(e => e.FaultDesc)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Fault_Desc");
                entity.Property(e => e.ProductGroup)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("Product_Group");
                entity.Property(e => e.Show).HasDefaultValue(true);
            });

            modelBuilder.Entity<TzebB2bConsignmentCallsType>(entity =>
            {
                entity.ToTable("_TZEB_B2B_Consignment_Calls_Type");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Attr1).HasMaxLength(100);
                entity.Property(e => e.Attr2).HasMaxLength(100);
                entity.Property(e => e.Code).HasMaxLength(15);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.DocNumPrefix)
                    .HasMaxLength(10)
                    .HasColumnName("Doc_Num_Prefix");
                entity.Property(e => e.ManualReq).HasColumnName("Manual_Req");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<TzebB2bInventoryType>(entity =>
            {
                entity.ToTable("_TZEB_B2B_Inventory_Type");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Comments)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Name)
                    .HasMaxLength(25)
                    .IsUnicode(false);
                entity.Property(e => e.PreconInvDest)
                    .HasComment("IF NULL: INVENTORY IS NOT PART OF PRE-CONSUMPTION MODEL -- NOT NULL, INV ONLY CAN BE PRECONSUMED INTO THIS PRECON_INV_DEST VALUE")
                    .HasColumnName("PRECON_INV_DEST");
                entity.Property(e => e.ShipToLocation)
                    .HasMaxLength(25)
                    .IsUnicode(false);
                entity.Property(e => e.Usps).HasColumnName("USPS");
            });

            modelBuilder.Entity<TrepairActionsCodes>(entity =>
            {
                entity.HasKey(e => e.ActionId).HasFillFactor(90);

                entity.ToTable("_TRepair_Actions_Codes");

                entity.Property(e => e.ActionId)
                    .ValueGeneratedNever()
                    .HasColumnName("ActionID");
                entity.Property(e => e.ActionDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TzebWorkCodesTargets>(entity =>
            {
                entity.HasKey(e => e.WorkCodeTarget);

                entity.ToTable("_TZEB_WORK_CODES_TARGETS");

                entity.Property(e => e.WorkCodeTarget)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Work_Code_Target");
                entity.Property(e => e.ConsideredForBer)
                    .HasDefaultValue(false)
                    .HasColumnName("Considered_For_BER");
                entity.Property(e => e.Show).HasDefaultValue(true);
                entity.Property(e => e.WorkDescTarget)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Work_Desc_Target");
            });

            modelBuilder.Entity<TorderRepairHdr>(entity =>
            {
                entity.HasKey(e => e.RefNo)
                    .IsClustered(false)
                    .HasFillFactor(90);

                entity.ToTable("_TOrderRepair_Hdr", tb =>
                {
                    tb.HasTrigger("Address_Updated");
                    tb.HasTrigger("OrderHdr_INSERT_RECORD");
                    tb.HasTrigger("OrderHdr_RECORD_DELETED");
                    tb.HasTrigger("OrderHdr_UPDATE_RECORD");
                    tb.HasTrigger("WhoUpdateCloseDate");
                    tb.HasTrigger("_TOrderRepair_Hdr_RECORD_UPDATED");
                });

                entity.HasIndex(e => e.DateCreated, "IX__TOrderRepair_Hdr").HasFillFactor(90);

                entity.HasIndex(e => new { e.TcityId, e.TstateId }, "IX__TOrderRepair_Hdr_1");

                entity.HasIndex(e => e.RefNo, "IX__TOrderRepair_Hdr_2");

                entity.HasIndex(e => e.StopDate, "IX__TOrderRepair_Hdr_3");

                entity.HasIndex(e => new { e.CloseDate, e.RefNo, e.TcustNo, e.DateCreated }, "IX__TOrderRepair_Hdr_4");

                entity.HasIndex(e => new { e.RefNo, e.DateCreated }, "IX__TOrderRepair_Hdr_5");

                entity.HasIndex(e => new { e.RefNo, e.TcustNo, e.DateCreated }, "IX__TOrderRepair_Hdr_6");

                entity.HasIndex(e => e.PoRefNo, "IX__TOrderRepair_Hdr_7");

                entity.HasIndex(e => e.CloseDate, "IX__TOrderRepair_Hdr_Close_Date");

                entity.HasIndex(e => new { e.CanceledDate, e.StopDate, e.CloseDate }, "Ref_No_IDX");

                entity.HasIndex(e => e.CanceledDate, "TOrderRepair_Hdr_Canceled_Date_Index>");

                entity.HasIndex(e => e.TcustNo, "_TOrderRepair_Hdr10").HasFillFactor(90);

                entity.HasIndex(e => new { e.TcustNo, e.RefNo, e.DateCreated, e.CustRef, e.CustName, e.ShipToAddr, e.CanceledDate, e.CloseDate }, "_TOrderRepair_Hdr17").HasFillFactor(90);

                entity.HasIndex(e => new { e.RefNo, e.TcustNo }, "_TOrderRepair_Hdr2")
                    .IsClustered()
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.RefNo, e.TcustNo, e.CanceledDate, e.StopDate }, "_TOrderRepair_Hdr9").HasFillFactor(90);

                entity.HasIndex(e => e.CanceledDate, "_TOrderRepair_Hdr999").HasFillFactor(90);

                entity.HasIndex(e => new { e.DateCreated, e.RefNo, e.TcustNo }, "_dta_index__TOrderRepair_Hdr_7_1461684355__K2_K1_K3_4_6");

                entity.HasIndex(e => new { e.CanceledDate, e.CloseDate, e.TcustNo, e.RefNo }, "_dta_index__TOrderRepair_Hdr_8_1461684355__K17_K19_K3_K1_4_5_11");

                entity.HasIndex(e => new { e.StopDate, e.CanceledDate }, "_dta_index__TOrderRepair_Hdr_8_1461684355__K18_K17");

                entity.HasIndex(e => new { e.RefNo, e.CloseDate, e.StopDate, e.TcustNo }, "_dta_index__TOrderRepair_Hdr_8_1461684355__K1_K19_K18_K3_5_13_17_21_2203");

                entity.HasIndex(e => new { e.RefNo, e.TcustNo }, "_dta_index__TOrderRepair_Hdr_8_1461684355__K1_K3_8258");

                entity.Property(e => e.RefNo)
                    .ValueGeneratedNever()
                    .HasColumnName("Ref_No");
                entity.Property(e => e.Att)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.BinLocation)
                    .HasMaxLength(8)
                    .IsUnicode(false);
                entity.Property(e => e.CanceledDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Canceled_Date");
                entity.Property(e => e.CloseDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Close_Date");
                entity.Property(e => e.CourierId).HasColumnName("Courier_Id");
                entity.Property(e => e.CreatedOnline)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValue("N");
                entity.Property(e => e.CustName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Cust_Name");
                entity.Property(e => e.CustRef)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Cust_Ref");
                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Created");
                entity.Property(e => e.DateInserted)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.DateSaved)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Saved");
                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValue("Not Provided");
                entity.Property(e => e.EmailResponseDateTime).HasColumnType("smalldatetime");
                entity.Property(e => e.EmailResponseSource).HasMaxLength(50);
                entity.Property(e => e.EstimateRequired).HasColumnName("Estimate_Required");
                entity.Property(e => e.OriginId).HasColumnName("OriginID");
                entity.Property(e => e.PoRefNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PO_RefNo");
                entity.Property(e => e.ReceiverUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ReceivingCompName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ReceivingEndDate).HasColumnType("datetime");
                entity.Property(e => e.ReceivingStartDate).HasColumnType("datetime");
                entity.Property(e => e.ShipToAddr)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValue(" ")
                    .HasColumnName("Ship_To_Addr");
                entity.Property(e => e.ShipToAddr1)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValue(" ")
                    .HasColumnName("Ship_To_Addr1");
                entity.Property(e => e.ShipViaId).HasColumnName("ShipVia_Id");
                entity.Property(e => e.ShippingInstr)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.SonimCustType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SonimOperCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.StopDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Stop_Date");
                entity.Property(e => e.TcityId)
                    .HasDefaultValueSql("(' ')")
                    .HasColumnName("TCity_ID");
                entity.Property(e => e.TcustNo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("TCust_No");
                entity.Property(e => e.TechNotes)
                    .HasMaxLength(7000)
                    .IsUnicode(false)
                    .HasColumnName("Tech_Notes");
                entity.Property(e => e.TstateId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValue(" ")
                    .HasColumnName("TState_ID");
                entity.Property(e => e.TstatusId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("TStatusID");
                entity.Property(e => e.ZipCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValue(" ")
                    .HasColumnName("ZIP_Code");
            });

            modelBuilder.Entity<TorderRepairItems>(entity =>
            {
                entity.HasKey(e => new { e.RefNo, e.PartNo })
                    .IsClustered(false)
                    .HasFillFactor(90);

                entity.ToTable("_TOrderRepair_Items");

                entity.HasIndex(e => e.CompanyPartNo, "IX__TOrderRepair_Items");

                entity.HasIndex(e => new { e.RefNo, e.PartNo, e.CompanyPartNo }, "IX__TOrderRepair_Items_1");

                entity.HasIndex(e => new { e.CompanyPartNo, e.PartNo }, "IX__TOrderRepair_Items_2");

                entity.HasIndex(e => new { e.RefNo, e.CompanyPartNo }, "IX__TOrderRepair_Items_3");

                entity.HasIndex(e => e.RefNo, "_TOrderRepair_Items3")
                    .IsClustered()
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.PartNo, e.RefNo }, "_dta_index__TOrderRepair_Items_8_1165963230__K2_K1_3");

                entity.Property(e => e.RefNo).HasColumnName("Ref_No");
                entity.Property(e => e.PartNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Part_No");
                entity.Property(e => e.CompanyPartNo)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("Company_PartNo");

                entity.HasOne(d => d.RefNoNavigation).WithMany(p => p.TorderRepairItems)
                    .HasForeignKey(d => d.RefNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TOrderRepair_Items__TOrderRepair_Hdr");
            });

            modelBuilder.Entity<TorderRepairItemsSerialsReceiving>(entity =>
            {
                entity.HasKey(e => new { e.RefNo, e.PartNo, e.Id })
                    .HasName("PK__TorderRepair_ItemsSerials_Receiving_194")
                    .IsClustered(false)
                    .HasFillFactor(90);

                entity.ToTable("_TorderRepair_ItemsSerials_Receiving", tb =>
                {
                    tb.HasTrigger("Receiving_INSERT_RECORD_194");
                    tb.HasTrigger("Receiving_RECORD_DELETED_194");
                    tb.HasTrigger("Receiving_UPDATE_RECORD_194");
                    tb.HasTrigger("Update_Estimated_Agree_date_194");
                });

                entity.HasIndex(e => e.DateReceived, "Date_Received_IDX_194");

                entity.HasIndex(e => e.DateReceived, "Date_Received_Ref_No_Repair_No_TrackingNumber_Idx_194");

                entity.HasIndex(e => e.RepairNo, "IX__REPAIR_194").HasFillFactor(90);

                entity.HasIndex(e => e.DateReceived, "IX__TDATE_RCV_194").HasFillFactor(90);

                entity.HasIndex(e => new { e.RepairNo, e.PartNo, e.SerialReceived }, "IX__TorderRepair_ItemsSerials_Receiving_10_194");

                entity.HasIndex(e => new { e.RepairNo, e.TrackingNumber }, "IX__TorderRepair_ItemsSerials_Receiving_11_194");

                entity.HasIndex(e => e.EstimatedAgree, "IX__TorderRepair_ItemsSerials_Receiving_12_194");

                entity.HasIndex(e => new { e.RepairNo, e.PartNo, e.CourierId }, "IX__TorderRepair_ItemsSerials_Receiving_15_194");

                entity.HasIndex(e => new { e.RefNo, e.RepairNo, e.TstatusId, e.EstimatedAgree, e.SerialReceived, e.DateReceived }, "IX__TorderRepair_ItemsSerials_Receiving_16_194");

                entity.HasIndex(e => e.PartNo, "IX__TorderRepair_ItemsSerials_Receiving_194").HasFillFactor(90);

                entity.HasIndex(e => e.TstatusId, "IX__TorderRepair_ItemsSerials_Receiving_1_194").HasFillFactor(90);

                entity.HasIndex(e => new { e.PartNo, e.SerialReceived }, "IX__TorderRepair_ItemsSerials_Receiving_2_194");

                entity.HasIndex(e => new { e.RepairNo, e.PartNo, e.SerialReceived }, "IX__TorderRepair_ItemsSerials_Receiving_3_194");

                entity.HasIndex(e => new { e.RepairNo, e.PartNo }, "IX__TorderRepair_ItemsSerials_Receiving_4_194");

                entity.HasIndex(e => new { e.RefNo, e.RepairNo, e.SerialReceived }, "IX__TorderRepair_ItemsSerials_Receiving_7_194");

                entity.HasIndex(e => new { e.RefNo, e.RepairNo, e.TstatusId }, "IX__TorderRepair_ItemsSerials_Receiving_8_194");

                entity.HasIndex(e => new { e.RepairNo, e.SerialReceived }, "IX__TorderRepair_ItemsSerials_Receiving_9_194");

                entity.HasIndex(e => e.RefNo, "NonClusteredIndex-20181206-100954_194");

                entity.HasIndex(e => e.SparePoolId, "NonClusteredIndex-20191025-005759_194");

                entity.HasIndex(e => new { e.PartNo, e.RefNo, e.SerialReceived, e.Id }, "_TorderRepair_ItemsSerials_Receiving13_194").HasFillFactor(90);

                entity.HasIndex(e => new { e.RefNo, e.RepairNo }, "_TorderRepair_ItemsSerials_Receiving15_194")
                    .IsClustered()
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.RefNo, e.DateReceived, e.SerialReceived, e.RepairNo }, "_TorderRepair_ItemsSerials_Receiving16_194").HasFillFactor(90);

                entity.HasIndex(e => new { e.RefNo, e.DateReceived }, "_TorderRepair_ItemsSerials_Receiving17_194").HasFillFactor(90);

                entity.HasIndex(e => e.TstatusId, "_TorderRepair_ItemsSerials_Receiving_TStatusID_Index_194");

                entity.Property(e => e.RefNo).HasColumnName("Ref_No");
                entity.Property(e => e.PartNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Part_No");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ConfirmationDate).HasColumnType("datetime");
                entity.Property(e => e.CourierId).HasColumnName("Courier_ID");
                entity.Property(e => e.DateReceived)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Received");
                entity.Property(e => e.DateTimeReceived)
                    .HasColumnType("datetime")
                    .HasColumnName("DateTime_Received");
                entity.Property(e => e.EstimatedAgree)
                    .HasDefaultValue(false)
                    .HasColumnName("Estimated_Agree");
                entity.Property(e => e.EstimatedAgreeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Estimated_Agree_date");
                entity.Property(e => e.InWaranty).HasDefaultValue(false);
                entity.Property(e => e.IsObf)
                    .HasDefaultValue(false)
                    .HasColumnName("Is_OBF");
                entity.Property(e => e.KitNo).HasColumnName("Kit_No");
                entity.Property(e => e.ObfApprovedBy)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("OBF_ApprovedBy");
                entity.Property(e => e.ObfDateApproved)
                    .HasColumnType("datetime")
                    .HasColumnName("OBF_DateApproved");
                entity.Property(e => e.PriceEstimated).HasColumnName("Price_Estimated");
                entity.Property(e => e.ProjectName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.QtyReceived)
                    .HasDefaultValue(0)
                    .HasColumnName("Qty_Received");
                entity.Property(e => e.RepairLogPrinted)
                    .HasDefaultValue(false)
                    .HasColumnName("RepairLog_Printed");
                entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
                entity.Property(e => e.SerialInbound)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("")
                    .HasColumnName("SerialINBOUND");
                entity.Property(e => e.SerialReceived)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("")
                    .HasColumnName("SerialRECEIVED");
                entity.Property(e => e.SparePoolId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Spare_Pool_id");
                entity.Property(e => e.TechEvaluationDone).HasDefaultValue(false);
                entity.Property(e => e.TechNotes)
                    .HasMaxLength(7000)
                    .IsUnicode(false)
                    .HasDefaultValue("")
                    .HasColumnName("Tech_Notes");
                entity.Property(e => e.TrackingNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TstatusId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValue("")
                    .HasColumnName("TStatusID");
                entity.Property(e => e.TypeOfRepairId)
                    .HasDefaultValue(1)
                    .HasComment("Codifier: _TDBK_TypeOfRepair")
                    .HasColumnName("TypeOfRepair_ID");
            });

            modelBuilder.Entity<TorderRepairItemsSerialsShipping>(entity =>
            {
                entity.HasKey(e => new { e.RefNo, e.PartNo, e.Id }).HasFillFactor(90);

                entity.ToTable("_TorderRepair_ItemsSerials_Shipping", tb =>
                {
                    tb.HasTrigger("Shipping_INSERT_RECORD");
                    tb.HasTrigger("Shipping_RECORD_DELETED");
                    tb.HasTrigger("Shipping_UPDATE_RECORD");
                });

                entity.HasIndex(e => e.TstatusId, "<Name of Missing Index, sysname,>");

                entity.HasIndex(e => e.DateShip, "Date_Ship_IDX");

                entity.HasIndex(e => e.DateShip, "Date_Ship_IDX1");

                entity.HasIndex(e => new { e.DateShip, e.TrackingNumber }, "Date_Ship_TrackingNumber_IDX");

                entity.HasIndex(e => e.DateShip, "IX__TorderRepair_ItemsSerials_Shipping").HasFillFactor(90);

                entity.HasIndex(e => new { e.RefNo, e.RepairNo, e.DateShip }, "IX__TorderRepair_ItemsSerials_Shipping_1").HasFillFactor(90);

                entity.HasIndex(e => new { e.RepairNo, e.PartNo, e.SerialShip }, "IX__TorderRepair_ItemsSerials_Shipping_10").HasFillFactor(90);

                entity.HasIndex(e => new { e.RepairNo, e.SerialShip, e.DateShip }, "IX__TorderRepair_ItemsSerials_Shipping_11").HasFillFactor(90);

                entity.HasIndex(e => new { e.RefNo, e.PartNo }, "IX__TorderRepair_ItemsSerials_Shipping_2").HasFillFactor(90);

                entity.HasIndex(e => new { e.SerialShip, e.DateShip }, "IX__TorderRepair_ItemsSerials_Shipping_3").HasFillFactor(90);

                entity.HasIndex(e => new { e.RepairNo, e.PartNo }, "IX__TorderRepair_ItemsSerials_Shipping_4").HasFillFactor(90);

                entity.HasIndex(e => e.RepairNo, "IX__TorderRepair_ItemsSerials_Shipping_5").HasFillFactor(90);

                entity.HasIndex(e => new { e.RefNo, e.RepairNo }, "IX__TorderRepair_ItemsSerials_Shipping_6").HasFillFactor(90);

                entity.HasIndex(e => new { e.RefNo, e.RepairNo, e.TstatusId }, "IX__TorderRepair_ItemsSerials_Shipping_7").HasFillFactor(90);

                entity.HasIndex(e => new { e.RefNo, e.RepairNo, e.PartNo, e.DateShip, e.SerialShip, e.TstatusId }, "IX__TorderRepair_ItemsSerials_Shipping_8").HasFillFactor(90);

                entity.HasIndex(e => e.PartNo, "IX__TorderRepair_ItemsSerials_Shipping_9").HasFillFactor(90);

                entity.HasIndex(e => new { e.SerialShip, e.TstatusId }, "SerialSHIP_TStatusID_idx");

                entity.HasIndex(e => new { e.RefNo, e.PartNo, e.DateShip, e.RepairNo }, "_TorderRepair_ItemsSerials_Shipping27").HasFillFactor(90);

                entity.HasIndex(e => new { e.DateShip, e.TstatusId }, "_TorderRepair_ItemsSerials_Shipping_Index");

                entity.HasIndex(e => new { e.RefNo, e.DateShip, e.PartNo }, "_dta_index__TorderRepair_ItemsSerials_Shipp_8_453628709__K1_K3_K2_8").HasFillFactor(90);

                entity.HasIndex(e => new { e.RepairNo, e.RefNo }, "_dta_index__TorderRepair_ItemsSerials_Shipp_8_453628709__K6_K1_2894").HasFillFactor(90);

                entity.HasIndex(e => new { e.RepairNo, e.PartNo, e.RefNo, e.Id }, "_dta_index__TorderRepair_ItemsSerials_Shipp_8_453628709__K6_K2_K1_K10_3_4_5_7_8_11_12").HasFillFactor(90);

                entity.Property(e => e.RefNo).HasColumnName("Ref_No");
                entity.Property(e => e.PartNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Part_No");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DateShip)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Ship");
                entity.Property(e => e.DateTimeShip)
                    .HasColumnType("datetime")
                    .HasColumnName("DateTime_Ship");
                entity.Property(e => e.DateTrackingNumber).HasColumnType("datetime");
                entity.Property(e => e.DaysToPickupFromShipping).HasDefaultValue(0);
                entity.Property(e => e.NeedSoftwareLoading).HasDefaultValue(false);
                entity.Property(e => e.Qty).HasDefaultValue(0);
                entity.Property(e => e.QtyShip)
                    .HasDefaultValue(0)
                    .HasColumnName("Qty_Ship");
                entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
                entity.Property(e => e.SerialShip)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SerialSHIP");
                entity.Property(e => e.ShipViaId).HasColumnName("ShipVia_Id");
                entity.Property(e => e.ShippingGroupId).HasColumnName("Shipping_Group_ID");
                entity.Property(e => e.TrackingNumber)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.TstatusId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValue("")
                    .HasColumnName("TStatusID");
                entity.Property(e => e.UserName)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("User_Name");
            });

            modelBuilder.Entity<Trepair>(entity =>
            {
                entity.HasKey(e => e.RepairNo).HasFillFactor(90);

                entity.ToTable("_TRepair", tb =>
                {
                    tb.HasTrigger("UpdateOrderStatus");
                    tb.HasTrigger("Update_TDBK_Repair_Status");
                    tb.HasTrigger("_TRepair_INSERT_RECORD");
                    tb.HasTrigger("_TRepair_RECORD_DELETED");
                    tb.HasTrigger("_TRepair_UPDATE_RECORD");
                });

                entity.HasIndex(e => e.Division, "IX__Division").HasFillFactor(90);

                entity.HasIndex(e => e.QualityEmployeeNo, "IX__Quality_Employee_No").HasFillFactor(90);

                entity.HasIndex(e => e.RefNo, "IX__TRefNo").HasFillFactor(90);

                entity.HasIndex(e => new { e.SerialNo, e.DateRepaired, e.Division }, "IX__TRepair").HasFillFactor(90);

                entity.HasIndex(e => e.DateRepaired, "IX__TRepair_1").HasFillFactor(90);

                entity.HasIndex(e => e.DateClose, "IX__TRepair_10").HasFillFactor(90);

                entity.HasIndex(e => new { e.RepairNo, e.DateClose }, "IX__TRepair_11").HasFillFactor(90);

                entity.HasIndex(e => new { e.RepairNo, e.DateCreated }, "IX__TRepair_12").HasFillFactor(90);

                entity.HasIndex(e => new { e.RepairNo, e.RefNo, e.DateCreated }, "IX__TRepair_13").HasFillFactor(90);

                entity.HasIndex(e => e.TtechNo, "IX__TRepair_2").HasFillFactor(90);

                entity.HasIndex(e => e.PartNo, "IX__TRepair_3").HasFillFactor(90);

                entity.HasIndex(e => e.TrepairStateId, "IX__TRepair_4").HasFillFactor(90);

                entity.HasIndex(e => new { e.RepairNo, e.TtechNo }, "IX__TRepair_6").HasFillFactor(90);

                entity.HasIndex(e => e.DateCreated, "IX__TRepair_8").HasFillFactor(90);

                entity.HasIndex(e => new { e.Division, e.RepairNo, e.SerialNo, e.DateRepaired }, "_TRepair2").HasFillFactor(90);

                entity.HasIndex(e => new { e.Division, e.RepairNo, e.PartNo, e.DateCreated, e.DateRepaired, e.RepairTime, e.TrepairStateId }, "_TRepair5").HasFillFactor(90);

                entity.HasIndex(e => new { e.TrepairStateId, e.RepairNo }, "_dta_index__TRepair_5_1030346785__K15_K1_2_3_7_8_9_17_25_1973_4801_6980");

                entity.HasIndex(e => new { e.TrepairStateId, e.RepairNo }, "_dta_index__TRepair_5_1030346785__K15_K1_3982_3928");

                entity.HasIndex(e => e.RepairNo, "_dta_index__TRepair_5_1030346785__K1_15_4009");

                entity.HasIndex(e => new { e.CompanyDsc, e.RepairNo }, "_dta_index__TRepair_5_1030346785__K5_K1_15_3088");

                entity.Property(e => e.RepairNo)
                    .ValueGeneratedNever()
                    .HasColumnName("Repair_No");
                entity.Property(e => e.CompanyDsc)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Company_DSC");
                entity.Property(e => e.DateClose)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Close");
                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Created");
                entity.Property(e => e.DateCs)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_CS");
                entity.Property(e => e.DateLoading)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Loading");
                entity.Property(e => e.DateRepaired)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Repaired");
                entity.Property(e => e.DateTech)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Tech");
                entity.Property(e => e.DateWorkUnassigned)
                    .HasComment("Date supervisor removed work from technician")
                    .HasColumnType("datetime");
                entity.Property(e => e.DeviceConfigId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Device_Config_ID");
                entity.Property(e => e.Division)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.InvoiceNo).HasColumnName("Invoice_No");
                entity.Property(e => e.NoRechazos)
                    .HasDefaultValue(0)
                    .HasColumnName("No_Rechazos");
                entity.Property(e => e.PartNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Part_No");
                entity.Property(e => e.QualityEmployeeNo).HasColumnName("Quality_Employee_No");
                entity.Property(e => e.RealDateTimeCreated)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.RefNo).HasColumnName("Ref_No");
                entity.Property(e => e.RepairBoxNo)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasComment("In the case repair is put into a box or bin (ZEBRA)")
                    .HasColumnName("Repair_Box_No");
                entity.Property(e => e.RepairTime).HasColumnName("Repair_Time");
                entity.Property(e => e.SerialNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Serial_No");
                entity.Property(e => e.TrepairStateId).HasColumnName("TRepairState_ID");
                entity.Property(e => e.TtechNo).HasColumnName("TTech_No");
                entity.Property(e => e.WfpBin)
                    .HasMaxLength(50)
                    .HasColumnName("WFP_BIN");
            });

            modelBuilder.Entity<MvcRepairPreflash>(entity =>
            {
                entity.ToTable("mvcRepairPreflash");

                entity.Property(e => e.Dateprocess).HasColumnName("dateprocess");
                entity.Property(e => e.Datereceive).HasColumnName("datereceive");
                entity.Property(e => e.Repairid).HasColumnName("repairid");
                entity.Property(e => e.TechFingerprint)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0),0))")
                    .HasColumnName("techFingerprint");
                entity.Property(e => e.Techprevious).HasColumnName("techprevious");
                entity.Property(e => e.User).HasColumnName("user");
            });

            modelBuilder.Entity<UserMvcReceiving>(entity =>
            {
                entity.ToTable("userMvcReceiving");

                entity.Property(e => e.Repairno).HasColumnName("repairno");
                entity.Property(e => e.User)
                    .HasMaxLength(100)
                    .HasColumnName("user");
            });

            modelBuilder.Entity<MvcChangeAreaLogs>(entity =>
            {
                entity.Property(e => e.ActualArea).HasMaxLength(200);
                entity.Property(e => e.ChangeDate).HasColumnName("changeDate");
                entity.Property(e => e.Lastchange).HasColumnName("lastchange");
                entity.Property(e => e.Repairid).HasColumnName("repairid");
                entity.Property(e => e.UserChange)
                    .HasMaxLength(200)
                    .HasColumnName("userChange");
            });

            modelBuilder.Entity<TrepairActionsLog>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__TRepairs_Assigned_To_Technicians")
                    .HasFillFactor(90);

                entity.ToTable("_TRepair_Actions_Log", tb => tb.HasTrigger("_TRepair_Actions_Log_INSERT_RECORD"));

                entity.HasIndex(e => e.RepairNo, "IX__TRepair_Actions_Log").HasFillFactor(90);

                entity.HasIndex(e => e.ActionDate, "IX__TRepair_Actions_Log_1")
                    .IsDescending()
                    .HasFillFactor(90);

                entity.HasIndex(e => e.RepairNo, "_dta_index__TRepair_Actions_Log_5_216569497__K3_4_742");

                entity.HasIndex(e => new { e.RepairNo, e.ActionDate }, "_dta_index__TRepair_Actions_Log_5_216569497__K3_K5_1973_4801");

                entity.HasIndex(e => new { e.RepairNo, e.ActionDate, e.ActionId, e.Id }, "_dta_index__TRepair_Actions_Log_5_216569497__K3_K5_K4_K1_1623");

                entity.HasIndex(e => new { e.ActionId, e.Id, e.RepairNo, e.ActionDate }, "_dta_index__TRepair_Actions_Log_5_216569497__K4_K1_K3_K5_7646");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ActionDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.ActionId).HasColumnName("ActionID");
                entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
                entity.Property(e => e.TtechNo).HasColumnName("TTech_No");
            });

            modelBuilder.Entity<TzebB2bOutBoundRequestsLog>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__TZEB_B2B_OutBound_Request_Log");

                entity.ToTable("_TZEB_B2B_OutBound_Requests_Log", tb => tb.HasTrigger("AFTER_INSERT_RECORD"));

                entity.HasIndex(e => e.RepairNo, "IX__TZEB_B2B_OutBound_Requests_Log_Repair_No");

                entity.HasIndex(e => e.AckResult, "NonClusteredIndex-20191002-153537");

                entity.HasIndex(e => e.InProcess, "NonClusteredIndex-20191017-111002");

                entity.HasIndex(e => e.NotificationDateTime, "NonClusteredIndex-20191017-111104");

                entity.HasIndex(e => e.NotificationSent, "_TZEB_B2B_OutBound_Requests_Log_b2b");

                entity.Property(e => e.AckDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Ack_Date_Time");
                entity.Property(e => e.AckReceived).HasColumnName("Ack_Received");
                entity.Property(e => e.AckResult)
                    .HasMaxLength(50)
                    .HasColumnName("Ack_Result");
                entity.Property(e => e.AckXml).HasColumnName("Ack_XML");
                entity.Property(e => e.AdditionalInfo).HasColumnName("Additional_info");
                entity.Property(e => e.DateTimeInserted)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Time_Inserted");
                entity.Property(e => e.InProcess).HasColumnName("In_Process");
                entity.Property(e => e.NotificationDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Notification_Date_Time");
                entity.Property(e => e.NotificationSent).HasColumnName("Notification_Sent");
                entity.Property(e => e.NotificationXml).HasColumnName("Notification_XML");
                entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
                entity.Property(e => e.Srlnumber)
                    .HasMaxLength(255)
                    .HasColumnName("SRLNumber");
                entity.Property(e => e.SuccessCall)
                    .HasDefaultValue(true)
                    .HasColumnName("Success_Call");
                entity.Property(e => e.TypeOfCallId).HasColumnName("Type_Of_Call_Id");
            });

            modelBuilder.Entity<TzebB2bReplacedPartLabel>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__TZEB_ReplacedPartLabel");

                entity.ToTable("_TZEB_B2B_ReplacedPart_Label", tb => tb.HasTrigger("trgReplacedPartLabelDelete"));

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DateDcr1)
                    .HasColumnType("datetime")
                    .HasColumnName("DateDCR1");
                entity.Property(e => e.DateInserted)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.DateLabelPrinted).HasColumnType("datetime");
                entity.Property(e => e.PartNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Part_No");
                entity.Property(e => e.PartRunner)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Part_Runner");
                entity.Property(e => e.ProblemDesc)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Problem_Desc");
                entity.Property(e => e.RepId)
                    .HasComment("_TZEB_REPAIR_CODES_PART_NO - Rep_ID - To get the part associated to this one being removed")
                    .HasColumnName("Rep_ID");
                entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
                entity.Property(e => e.RtvType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RTV_TYPE");
                entity.Property(e => e.SerialNo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Serial_No");
                entity.Property(e => e.TtechNo).HasColumnName("TTech_No");
            });

            modelBuilder.Entity<TzebRepairCodes>(entity =>
            {
                entity.HasKey(e => e.RepId);

                entity.ToTable("_TZEB_REPAIR_CODES");

                entity.HasIndex(e => e.RepairNo, "RepID_TTechNo_DateAdd_J_IDX");

                entity.HasIndex(e => e.WorkCodeAction, "_TZEB_REPAIR_CODES_Work_Code_Action_Index");

                entity.Property(e => e.RepId).HasColumnName("Rep_ID");
                entity.Property(e => e.Ber)
                    .HasDefaultValue(false)
                    .HasColumnName("BER");
                entity.Property(e => e.DateAdded)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.DateSubmitted)
                    .HasComment("Date super tech click on SAVE AND EXIT, making request visible to part runners (if parts were requested)")
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Submitted");
                entity.Property(e => e.FaultCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Fault_Code");
                entity.Property(e => e.PrimaryFault)
                    .HasDefaultValue(true)
                    .HasComment("Primary_Fault = TRUE means this is the main fault for device, the best describing the problem selected by customer   ")
                    .HasColumnName("Primary_Fault");
                entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
                entity.Property(e => e.TtechNo).HasColumnName("TTech_No");
                entity.Property(e => e.WorkCodeAction)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Work_Code_Action");
                entity.Property(e => e.WorkCodeTarget)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Work_Code_Target");
            });

            modelBuilder.Entity<TzebRepairCodesPartNo>(entity =>
            {
                entity.HasKey(e => e.RepId).HasName("PK__TZEB_REPAIR_CODES_PART_NO_1");

                entity.ToTable("_TZEB_REPAIR_CODES_PART_NO");

                entity.HasIndex(e => e.PartNo, "<Name of Missing Index, sysname,>");

                entity.Property(e => e.RepId)
                    .ValueGeneratedNever()
                    .HasColumnName("Rep_ID");
                entity.Property(e => e.Cost).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.PartNo)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("Part_No");
            });

            modelBuilder.Entity<UserMvcAssignments>(entity =>
            {
                entity.Property(e => e.ActualArea).HasMaxLength(100);
                entity.Property(e => e.Assigneddate).HasColumnName("assigneddate");
                entity.Property(e => e.Bypass)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0),0))")
                    .HasColumnName("bypass");
                entity.Property(e => e.Bypassdate).HasColumnName("bypassdate");
                entity.Property(e => e.Duedate).HasColumnName("duedate");
                entity.Property(e => e.Repairid).HasColumnName("repairid");
                entity.Property(e => e.Sendalert)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0),0))")
                    .HasColumnName("sendalert");
                entity.Property(e => e.Userassigned)
                    .HasMaxLength(100)
                    .HasColumnName("userassigned");
                entity.Property(e => e.Userbypass)
                    .HasMaxLength(100)
                    .HasColumnName("userbypass");
                entity.Property(e => e.Userprocess)
                    .HasMaxLength(100)
                    .HasColumnName("userprocess");
            });

            modelBuilder.Entity<TzebInBoundRequestsFileHoldsLog>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__TZEB_InBound_Requests_File_Holds_Log_1");

                entity.ToTable("_TZEB_InBound_Requests_File_Holds_Log");

                entity.HasIndex(e => e.HoldType, "NonClusteredIndex-20191017-113506");

                entity.HasIndex(e => e.RowId, "NonClusteredIndex-20191017-113642");

                entity.HasIndex(e => e.DateReleased, "_TZEB_InBound_Requests_File_Holds_Log_Index");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DateOnHold)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("Date_On_Hold");
                entity.Property(e => e.DateReleased)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Released");
                entity.Property(e => e.HoldId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Hold_Id");
                entity.Property(e => e.HoldName)
                    .HasMaxLength(255)
                    .HasColumnName("Hold_Name");
                entity.Property(e => e.HoldType)
                    .HasMaxLength(255)
                    .HasColumnName("Hold_Type");
                entity.Property(e => e.RowId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ROW_ID");
            });

            modelBuilder.Entity<Ttech>(entity =>
            {
                entity.HasKey(e => e.TtechNo)
                    .IsClustered(false)
                    .HasFillFactor(90);

                entity.ToTable("_TTech");

                entity.HasIndex(e => e.TtechName, "IX__TTech");

                entity.HasIndex(e => e.Password, "IX__TTech_1");

                entity.HasIndex(e => e.Login, "IX__TTech_2");

                entity.HasIndex(e => e.IsTech, "IX__TTech_3");

                entity.Property(e => e.TtechNo)
                    .ValueGeneratedNever()
                    .HasColumnName("TTech_No");
                entity.Property(e => e.Active).HasDefaultValue(true);
                entity.Property(e => e.AdditionalTimeFromCurrDate)
                    .HasDefaultValue(0.0)
                    .HasColumnName("Additional_time_from _curr_date");
                entity.Property(e => e.Admin).HasDefaultValue(false);
                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.EmployeeCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.Engineer).HasDefaultValue(false);
                entity.Property(e => e.IsTech).HasColumnName("Is_Tech");
                entity.Property(e => e.Login)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RepParts).HasComment("Boolean: if Tec repairs parts (radio, cpu, etc) = 1; only Devices = 0 ");
                entity.Property(e => e.Shift)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("D: Day, N:Night");
                entity.Property(e => e.Supervisor).HasDefaultValue(false);
                entity.Property(e => e.TtechName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TTech_Name");
            });

            modelBuilder.Entity<Tdbkusers>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .IsClustered(false)
                    .HasFillFactor(90);

                entity.ToTable("TDBKUsers");

                entity.Property(e => e.Login)
                    .HasMaxLength(25)
                    .IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
                entity.Property(e => e.IsCustomerService)
                    .HasDefaultValue(false)
                    .HasColumnName("Is_Customer_Service");
                entity.Property(e => e.IsTechnicalSupport)
                    .HasDefaultValue(false)
                    .HasColumnName("Is_Technical_Support");
                entity.Property(e => e.IsZebraPartRunner).HasColumnName("Is_Zebra_PartRunner");
                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Password)
                    .HasMaxLength(25)
                    .IsUnicode(false);
                entity.Property(e => e.SupervisorPin)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.WorkingForCompany)
                    .HasMaxLength(100)
                    .HasDefaultValue("DBK")
                    .HasColumnName("Working_for_company");
            });

            modelBuilder.Entity<TzebWorkCodesActions>(entity =>
            {
                entity.HasKey(e => e.WorkCodeAction);

                entity.ToTable("_TZEB_WORK_CODES_ACTIONS");

                entity.Property(e => e.WorkCodeAction)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Work_Code_Action");
                entity.Property(e => e.RepairCodeDefinition)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Repair_Code_Definition");
                entity.Property(e => e.RequiresAssemblingArea)
                    .HasDefaultValue(true)
                    .HasColumnName("Requires_Assembling_Area");
                entity.Property(e => e.RequiresPartsReplaced).HasColumnName("Requires_Parts_Replaced");
                entity.Property(e => e.Show).HasDefaultValue(true);
                entity.Property(e => e.WorkDescAction)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Work_Desc_Action");
            });

            modelBuilder.Entity<TzebB2bOutBoundRequestsTypeOfCalls>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__TZEB_B2B_OutBound_Request_Type_Of_Calls");

                entity.ToTable("_TZEB_B2B_OutBound_Requests_Type_Of_Calls");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Code).HasMaxLength(5);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TiewRepairStatusZebraBldgReceiving>(entity =>
            {
                entity.ToTable("_TIEW_Repair_Status_Zebra_Bldg_Receiving");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Allrepairstate).HasColumnName("ALLREPAIRSTATE");
                entity.Property(e => e.AttHdr)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Att_HDR");
                entity.Property(e => e.BinLocation)
                    .HasMaxLength(8)
                    .IsUnicode(false);
                entity.Property(e => e.CanceledDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Canceled_Date");
                entity.Property(e => e.Chd).HasColumnName("CHD");
                entity.Property(e => e.CloseDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Close_Date");
                entity.Property(e => e.CompanyDsc)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Company_DSC");
                entity.Property(e => e.CompanyPartNo)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("Company_PartNo");
                entity.Property(e => e.CourierDsc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Courier_Dsc");
                entity.Property(e => e.CourierId).HasColumnName("Courier_ID");
                entity.Property(e => e.CurrInstructions)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Curr_instructions");
                entity.Property(e => e.CustName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Cust_Name");
                entity.Property(e => e.CustRef)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Cust_Ref");
                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Created");
                entity.Property(e => e.DateInserted)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.DateLoading)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Loading");
                entity.Property(e => e.DateReceived)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Received");
                entity.Property(e => e.DateRepaired)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Repaired");
                entity.Property(e => e.DateShip)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Ship");
                entity.Property(e => e.DateTech)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Tech");
                entity.Property(e => e.DaysLeftToArrive).HasColumnName("DaysLeft_To_Arrive");
                entity.Property(e => e.DbkPartDsc)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DBK_Part_DSC");
                entity.Property(e => e.Division)
                    .HasMaxLength(30)
                    .IsUnicode(false);
                entity.Property(e => e.EstimatedAgree).HasColumnName("Estimated_Agree");
                entity.Property(e => e.InBoundpt)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.InboundTracking)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("Inbound_Tracking");
                entity.Property(e => e.InboundTrackingNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Model)
                    .HasMaxLength(2)
                    .IsUnicode(false);
                entity.Property(e => e.NeedRepair).HasColumnName("Need_Repair");
                entity.Property(e => e.NeedSerialNo).HasColumnName("Need_SerialNo");
                entity.Property(e => e.OrderOnHoldDateAdded).HasColumnName("OrderOnHold_DateAdded");
                entity.Property(e => e.OrderOnHoldReason)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("OrderOnHold_Reason");
                entity.Property(e => e.OrderStateId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Order_State_ID");
                entity.Property(e => e.OrderZipCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Order_ZipCode");
                entity.Property(e => e.OutBoundpt)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PartDsc)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Part_DSC");
                entity.Property(e => e.PartNo)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("Part_No");
                entity.Property(e => e.PickupStatus).HasColumnName("Pickup_Status");
                entity.Property(e => e.QtyReceived).HasColumnName("Qty_Received");
                entity.Property(e => e.QtyToOrder).HasColumnName("Qty_To_Order");
                entity.Property(e => e.QtyToReceive).HasColumnName("Qty_To_Receive");
                entity.Property(e => e.QtyToShip).HasColumnName("Qty_To_Ship");
                entity.Property(e => e.RefNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Ref_No");
                entity.Property(e => e.RepairDateClose)
                    .HasColumnType("datetime")
                    .HasColumnName("Repair_Date_Close");
                entity.Property(e => e.RepairNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Repair_No");
                entity.Property(e => e.RepairNoTrepair).HasColumnName("Repair_No_TRepair");
                entity.Property(e => e.SerialInbound)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SerialINBOUND");
                entity.Property(e => e.SerialNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Serial_No");
                entity.Property(e => e.SerialReceived)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SerialRECEIVED");
                entity.Property(e => e.SerialShip)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SerialSHIP");
                entity.Property(e => e.ShipToAddr)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Ship_To_Addr");
                entity.Property(e => e.ShipToAddr1)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Ship_To_Addr1");
                entity.Property(e => e.ShippSwap)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SHIPP_SWAP");
                entity.Property(e => e.SparePoolId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Spare_Pool_id");
                entity.Property(e => e.StatusForCompany)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.StopDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Stop_Date");
                entity.Property(e => e.TcityDsc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TCity_Dsc");
                entity.Property(e => e.TcustNo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("TCust_No");
                entity.Property(e => e.TrackingNumber)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.TrepairStateDsc)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TRepairState_DSC");
                entity.Property(e => e.TrepairStateId).HasColumnName("TRepairState_ID");
                entity.Property(e => e.TstateId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TState_ID");
                entity.Property(e => e.TstatusId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("TStatusID");
                entity.Property(e => e.UnitId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UnitID");
                entity.Property(e => e.ZipCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ZIP_Code");
            });

        }
    }
}
