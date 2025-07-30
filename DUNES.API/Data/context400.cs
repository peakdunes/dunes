using System;
using System.Collections.Generic;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context400 : DbContext
{
    public context400()
    {
    }

    public context400(DbContextOptions<context400> options)
        : base(options)
    {
    }

    public virtual DbSet<Bines> Bines { get; set; }

    public virtual DbSet<Cities> Cities { get; set; }

    public virtual DbSet<Company> Company { get; set; }

    public virtual DbSet<Countries> Countries { get; set; }

    public virtual DbSet<Generalparameters> Generalparameters { get; set; }

    public virtual DbSet<InventoryTypes> InventoryTypes { get; set; }

    public virtual DbSet<Inventorycategories> Inventorycategories { get; set; }

    public virtual DbSet<Inventorydetail> Inventorydetail { get; set; }

    public virtual DbSet<Inventorymovement> Inventorymovement { get; set; }

    public virtual DbSet<InventorytransactionDetail> InventorytransactionDetail { get; set; }

    public virtual DbSet<InventorytransactionHdr> InventorytransactionHdr { get; set; }

    public virtual DbSet<Itemstatus> Itemstatus { get; set; }

    public virtual DbSet<Locations> Locations { get; set; }

    public virtual DbSet<Racks> Racks { get; set; }

    public virtual DbSet<StatesCountries> StatesCountries { get; set; }

    public virtual DbSet<Transactionconcepts> Transactionconcepts { get; set; }

    public virtual DbSet<Transactiontypes> Transactiontypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBKWMS;TrustServerCertificate=true;persist security info=True;user id=radeonuat;password=Mrhojt53");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bines>(entity =>
        {
            entity.ToTable("bines");

            entity.HasIndex(e => e.Idcompany, "IX_bines_Idcompany");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Idcompanyclient).HasMaxLength(200);
            entity.Property(e => e.IncludeInConsumption).HasColumnName("include_in_consumption");
            entity.Property(e => e.Observations).HasMaxLength(1000);
            entity.Property(e => e.TagName).HasMaxLength(100);

            entity.HasOne(d => d.IdcompanyNavigation).WithMany(p => p.Bines).HasForeignKey(d => d.Idcompany);
        });

        modelBuilder.Entity<Cities>(entity =>
        {
            entity.ToTable("cities");

            entity.HasIndex(e => e.Idcountry, "IX_cities_Idcountry");

            entity.HasIndex(e => e.Idstate, "IX_cities_Idstate");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");

            entity.HasOne(d => d.IdcountryNavigation).WithMany(p => p.Cities).HasForeignKey(d => d.Idcountry);

            entity.HasOne(d => d.IdstateNavigation).WithMany(p => p.Cities).HasForeignKey(d => d.Idstate);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("company");

            entity.HasIndex(e => e.Idcity, "IX_company_Idcity");

            entity.HasIndex(e => e.Idcountry, "IX_company_Idcountry");

            entity.HasIndex(e => e.Idstate, "IX_company_Idstate");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(100)
                .HasColumnName("companyId");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(100)
                .HasColumnName("phone");
            entity.Property(e => e.Website)
                .HasMaxLength(200)
                .HasColumnName("website");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(20)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.IdcityNavigation).WithMany(p => p.Company).HasForeignKey(d => d.Idcity);

            entity.HasOne(d => d.IdcountryNavigation).WithMany(p => p.Company)
                .HasForeignKey(d => d.Idcountry)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdstateNavigation).WithMany(p => p.Company)
                .HasForeignKey(d => d.Idstate)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Countries>(entity =>
        {
            entity.ToTable("countries");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Sigla)
                .HasMaxLength(5)
                .HasColumnName("sigla");
        });

        modelBuilder.Entity<Generalparameters>(entity =>
        {
            entity.ToTable("generalparameters");

            entity.Property(e => e.ParameterId).HasColumnName("parameterId");
            entity.Property(e => e.Parametername)
                .HasMaxLength(200)
                .HasColumnName("parametername");
            entity.Property(e => e.Parametervalue)
                .HasMaxLength(500)
                .HasColumnName("parametervalue");
        });

        modelBuilder.Entity<InventoryTypes>(entity =>
        {
            entity.ToTable("inventoryTypes");

            entity.HasIndex(e => e.Idcompany, "IX_inventoryTypes_Idcompany");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Idcompanyclient).HasMaxLength(200);
            entity.Property(e => e.Itemstatusid).HasColumnName("itemstatusid");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Observations).HasMaxLength(1000);
            entity.Property(e => e.Updateitemstatus).HasColumnName("updateitemstatus");
            entity.Property(e => e.Zebrainvassociated).HasColumnName("zebrainvassociated");

            entity.HasOne(d => d.IdcompanyNavigation).WithMany(p => p.InventoryTypes).HasForeignKey(d => d.Idcompany);
        });

        modelBuilder.Entity<Inventorycategories>(entity =>
        {
            entity.ToTable("inventorycategories");

            entity.HasIndex(e => e.Idcompany, "IX_inventorycategories_Idcompany");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Idcompanyclient).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Observations).HasMaxLength(1000);

            entity.HasOne(d => d.IdcompanyNavigation).WithMany(p => p.Inventorycategories).HasForeignKey(d => d.Idcompany);
        });

        modelBuilder.Entity<Inventorydetail>(entity =>
        {
            entity.ToTable("inventorydetail");

            entity.HasIndex(e => e.Idbin, "IX_inventorydetail_Idbin");

            entity.HasIndex(e => e.Idcompany, "IX_inventorydetail_Idcompany");

            entity.HasIndex(e => e.Idlocation, "IX_inventorydetail_Idlocation");

            entity.HasIndex(e => e.Idrack, "IX_inventorydetail_Idrack");

            entity.HasIndex(e => e.Idstatus, "IX_inventorydetail_Idstatus");

            entity.HasIndex(e => e.Idtype, "IX_inventorydetail_Idtype");

            entity.Property(e => e.Idcompanyclient).HasMaxLength(200);
            entity.Property(e => e.Iddivision).HasMaxLength(200);
            entity.Property(e => e.Iditem).HasMaxLength(200);
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Serialid)
                .HasMaxLength(200)
                .HasColumnName("serialid");
            entity.Property(e => e.TotalQty).HasColumnName("totalQty");

            entity.HasOne(d => d.IdbinNavigation).WithMany(p => p.Inventorydetail)
                .HasForeignKey(d => d.Idbin)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdcompanyNavigation).WithMany(p => p.Inventorydetail).HasForeignKey(d => d.Idcompany);

            entity.HasOne(d => d.IdlocationNavigation).WithMany(p => p.Inventorydetail)
                .HasForeignKey(d => d.Idlocation)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdrackNavigation).WithMany(p => p.Inventorydetail)
                .HasForeignKey(d => d.Idrack)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdstatusNavigation).WithMany(p => p.Inventorydetail)
                .HasForeignKey(d => d.Idstatus)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdtypeNavigation).WithMany(p => p.Inventorydetail)
                .HasForeignKey(d => d.Idtype)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Inventorymovement>(entity =>
        {
            entity.ToTable("inventorymovement");

            entity.HasIndex(e => e.Idbin, "IX_inventorymovement_Idbin");

            entity.HasIndex(e => e.Idcompany, "IX_inventorymovement_Idcompany");

            entity.HasIndex(e => e.Idlocation, "IX_inventorymovement_Idlocation");

            entity.HasIndex(e => e.Idrack, "IX_inventorymovement_Idrack");

            entity.HasIndex(e => e.Idstatus, "IX_inventorymovement_Idstatus");

            entity.HasIndex(e => e.IdtransactionHead, "IX_inventorymovement_IdtransactionHead");

            entity.HasIndex(e => e.Idtransactionconcept, "IX_inventorymovement_Idtransactionconcept");

            entity.HasIndex(e => e.Idtransactiontype, "IX_inventorymovement_Idtransactiontype");

            entity.HasIndex(e => e.Idtype, "IX_inventorymovement_Idtype");

            entity.HasIndex(e => e.IdtransactionDetail, "IX_inventorymovement_idtransactionDetail");

            entity.Property(e => e.Createdby)
                .HasMaxLength(450)
                .HasColumnName("createdby");
            entity.Property(e => e.Datecreated).HasColumnName("datecreated");
            entity.Property(e => e.Idcompanyclient).HasMaxLength(200);
            entity.Property(e => e.Iddivision).HasMaxLength(200);
            entity.Property(e => e.Iditem).HasMaxLength(200);
            entity.Property(e => e.IdtransactionDetail).HasColumnName("idtransactionDetail");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Qtybalance).HasColumnName("qtybalance");
            entity.Property(e => e.Qtyinput).HasColumnName("qtyinput");
            entity.Property(e => e.Qtyoutput).HasColumnName("qtyoutput");
            entity.Property(e => e.Serialid)
                .HasMaxLength(200)
                .HasColumnName("serialid");

            entity.HasOne(d => d.IdbinNavigation).WithMany(p => p.Inventorymovement).HasForeignKey(d => d.Idbin);

            entity.HasOne(d => d.IdcompanyNavigation).WithMany(p => p.Inventorymovement)
                .HasForeignKey(d => d.Idcompany)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdlocationNavigation).WithMany(p => p.Inventorymovement)
                .HasForeignKey(d => d.Idlocation)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdrackNavigation).WithMany(p => p.Inventorymovement)
                .HasForeignKey(d => d.Idrack)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdstatusNavigation).WithMany(p => p.Inventorymovement)
                .HasForeignKey(d => d.Idstatus)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdtransactionDetailNavigation).WithMany(p => p.Inventorymovement)
                .HasForeignKey(d => d.IdtransactionDetail)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdtransactionHeadNavigation).WithMany(p => p.Inventorymovement)
                .HasForeignKey(d => d.IdtransactionHead)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdtransactionconceptNavigation).WithMany(p => p.Inventorymovement)
                .HasForeignKey(d => d.Idtransactionconcept)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdtransactiontypeNavigation).WithMany(p => p.Inventorymovement)
                .HasForeignKey(d => d.Idtransactiontype)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdtypeNavigation).WithMany(p => p.Inventorymovement)
                .HasForeignKey(d => d.Idtype)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<InventorytransactionDetail>(entity =>
        {
            entity.ToTable("inventorytransactionDetail");

            entity.HasIndex(e => e.Idbin, "IX_inventorytransactionDetail_Idbin");

            entity.HasIndex(e => e.Idcompany, "IX_inventorytransactionDetail_Idcompany");

            entity.HasIndex(e => e.Idenctransaction, "IX_inventorytransactionDetail_Idenctransaction");

            entity.HasIndex(e => e.Idlocation, "IX_inventorytransactionDetail_Idlocation");

            entity.HasIndex(e => e.Idrack, "IX_inventorytransactionDetail_Idrack");

            entity.HasIndex(e => e.Idstatus, "IX_inventorytransactionDetail_Idstatus");

            entity.HasIndex(e => e.Idtype, "IX_inventorytransactionDetail_Idtype");

            entity.HasIndex(e => e.Idtypetransaction, "IX_inventorytransactionDetail_Idtypetransaction");

            entity.Property(e => e.Idcompanyclient).HasMaxLength(200);
            entity.Property(e => e.Iddivision).HasMaxLength(200);
            entity.Property(e => e.Iditem).HasMaxLength(200);
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Serialid)
                .HasMaxLength(200)
                .HasColumnName("serialid");
            entity.Property(e => e.TotalQty).HasColumnName("totalQty");

            entity.HasOne(d => d.IdbinNavigation).WithMany(p => p.InventorytransactionDetail)
                .HasForeignKey(d => d.Idbin)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdcompanyNavigation).WithMany(p => p.InventorytransactionDetail)
                .HasForeignKey(d => d.Idcompany)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdenctransactionNavigation).WithMany(p => p.InventorytransactionDetail)
                .HasForeignKey(d => d.Idenctransaction)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdlocationNavigation).WithMany(p => p.InventorytransactionDetail)
                .HasForeignKey(d => d.Idlocation)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdrackNavigation).WithMany(p => p.InventorytransactionDetail)
                .HasForeignKey(d => d.Idrack)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdstatusNavigation).WithMany(p => p.InventorytransactionDetail)
                .HasForeignKey(d => d.Idstatus)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdtypeNavigation).WithMany(p => p.InventorytransactionDetail)
                .HasForeignKey(d => d.Idtype)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdtypetransactionNavigation).WithMany(p => p.InventorytransactionDetail)
                .HasForeignKey(d => d.Idtypetransaction)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<InventorytransactionHdr>(entity =>
        {
            entity.ToTable("inventorytransactionHdr");

            entity.HasIndex(e => e.Idcompany, "IX_inventorytransactionHdr_Idcompany");

            entity.HasIndex(e => e.Idtransactionconcept, "IX_inventorytransactionHdr_Idtransactionconcept");

            entity.Property(e => e.Datecreated).HasColumnName("datecreated");
            entity.Property(e => e.Dateprocessed).HasColumnName("dateprocessed");
            entity.Property(e => e.Documentreference)
                .HasMaxLength(200)
                .HasColumnName("documentreference");
            entity.Property(e => e.IdUser).HasMaxLength(450);
            entity.Property(e => e.IdUserprocess).HasMaxLength(450);
            entity.Property(e => e.Idcompanyclient).HasMaxLength(200);
            entity.Property(e => e.Iddivision).HasMaxLength(200);
            entity.Property(e => e.Observations)
                .HasMaxLength(1000)
                .HasColumnName("observations");
            entity.Property(e => e.Processed).HasColumnName("processed");

            entity.HasOne(d => d.IdcompanyNavigation).WithMany(p => p.InventorytransactionHdr).HasForeignKey(d => d.Idcompany);

            entity.HasOne(d => d.IdtransactionconceptNavigation).WithMany(p => p.InventorytransactionHdr)
                .HasForeignKey(d => d.Idtransactionconcept)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Itemstatus>(entity =>
        {
            entity.ToTable("itemstatus");

            entity.HasIndex(e => e.Idcompany, "IX_itemstatus_Idcompany");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Idcompanyclient).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Observations).HasMaxLength(1000);

            entity.HasOne(d => d.IdcompanyNavigation).WithMany(p => p.Itemstatus).HasForeignKey(d => d.Idcompany);
        });

        modelBuilder.Entity<Locations>(entity =>
        {
            entity.ToTable("locations");

            entity.HasIndex(e => e.Idcity, "IX_locations_Idcity");

            entity.HasIndex(e => e.Idcompany, "IX_locations_Idcompany");

            entity.HasIndex(e => e.Idcountry, "IX_locations_Idcountry");

            entity.HasIndex(e => e.Idstate, "IX_locations_Idstate");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Namedbk)
                .HasMaxLength(200)
                .HasColumnName("namedbk");
            entity.Property(e => e.Phone)
                .HasMaxLength(100)
                .HasColumnName("phone");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(20)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.IdcityNavigation).WithMany(p => p.Locations).HasForeignKey(d => d.Idcity);

            entity.HasOne(d => d.IdcompanyNavigation).WithMany(p => p.Locations)
                .HasForeignKey(d => d.Idcompany)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdcountryNavigation).WithMany(p => p.Locations)
                .HasForeignKey(d => d.Idcountry)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdstateNavigation).WithMany(p => p.Locations)
                .HasForeignKey(d => d.Idstate)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Racks>(entity =>
        {
            entity.ToTable("racks");

            entity.HasIndex(e => e.Idcompany, "IX_racks_Idcompany");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Idcompanyclient).HasMaxLength(200);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.IdcompanyNavigation).WithMany(p => p.Racks).HasForeignKey(d => d.Idcompany);
        });

        modelBuilder.Entity<StatesCountries>(entity =>
        {
            entity.ToTable("statesCountries");

            entity.HasIndex(e => e.Idcountry, "IX_statesCountries_Idcountry");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Sigla)
                .HasMaxLength(5)
                .HasColumnName("sigla");

            entity.HasOne(d => d.IdcountryNavigation).WithMany(p => p.StatesCountries)
                .HasForeignKey(d => d.Idcountry)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Transactionconcepts>(entity =>
        {
            entity.ToTable("transactionconcepts");

            entity.HasIndex(e => e.Idcompany, "IX_transactionconcepts_Idcompany");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.CallType).HasColumnName("callType");
            entity.Property(e => e.CreateZebraCall).HasColumnName("createZebraCall");
            entity.Property(e => e.CreateZebraInvTran).HasColumnName("createZebraInvTran");
            entity.Property(e => e.Idcompanyclient).HasMaxLength(200);
            entity.Property(e => e.IsInternal).HasColumnName("isInternal");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.ZebraInventoryAssociated).HasColumnName("zebraInventoryAssociated");

            entity.HasOne(d => d.IdcompanyNavigation).WithMany(p => p.Transactionconcepts).HasForeignKey(d => d.Idcompany);
        });

        modelBuilder.Entity<Transactiontypes>(entity =>
        {
            entity.ToTable("transactiontypes");

            entity.HasIndex(e => e.Idcompany, "IX_transactiontypes_Idcompany");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Idcompanyclient).HasMaxLength(200);
            entity.Property(e => e.Isinput).HasColumnName("isinput");
            entity.Property(e => e.Isoutput).HasColumnName("isoutput");
            entity.Property(e => e.Ispreconsumption).HasColumnName("ispreconsumption");
            entity.Property(e => e.Match)
                .HasMaxLength(1)
                .HasColumnName("match");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.IdcompanyNavigation).WithMany(p => p.Transactiontypes).HasForeignKey(d => d.Idcompany);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
