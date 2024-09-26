using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LILI_TMS.Temp_Models
{
    public partial class dbTicketManagementContext : DbContext
    {
        public dbTicketManagementContext()
        {
        }

        public dbTicketManagementContext(DbContextOptions<dbTicketManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<MenuMaster> MenuMasters { get; set; }
        public virtual DbSet<TblAction> TblActions { get; set; }
        public virtual DbSet<TblActionType> TblActionTypes { get; set; }
        public virtual DbSet<TblArea> TblAreas { get; set; }
        public virtual DbSet<TblAttachmentType> TblAttachmentTypes { get; set; }
        public virtual DbSet<TblBusinessSetupInfo> TblBusinessSetupInfos { get; set; }
        public virtual DbSet<TblComplainDepartmentApproval> TblComplainDepartmentApprovals { get; set; }
        public virtual DbSet<TblComplainDepartmentApprovalSmsdetail> TblComplainDepartmentApprovalSmsdetails { get; set; }
        public virtual DbSet<TblComplainTicket> TblComplainTickets { get; set; }
        public virtual DbSet<TblComplainTicketApproverSmsdetail> TblComplainTicketApproverSmsdetails { get; set; }
        public virtual DbSet<TblComplainTicketImageDetail> TblComplainTicketImageDetails { get; set; }
        public virtual DbSet<TblComplainTicketMachineDetail> TblComplainTicketMachineDetails { get; set; }
        public virtual DbSet<TblComplainType> TblComplainTypes { get; set; }
        public virtual DbSet<TblDepartment> TblDepartments { get; set; }
        public virtual DbSet<TblDepartmentType> TblDepartmentTypes { get; set; }
        public virtual DbSet<TblDesignation> TblDesignations { get; set; }
        public virtual DbSet<TblEmployeeSetup> TblEmployeeSetups { get; set; }
        public virtual DbSet<TblMachineSetup> TblMachineSetups { get; set; }
        public virtual DbSet<TblMenu> TblMenus { get; set; }
        public virtual DbSet<TblMenuList> TblMenuLists { get; set; }
        public virtual DbSet<TblPart> TblParts { get; set; }
        public virtual DbSet<TblRegion> TblRegions { get; set; }
        public virtual DbSet<TblServiceDepartmentTicketAssignment> TblServiceDepartmentTicketAssignments { get; set; }
        public virtual DbSet<TblSeverityLevel> TblSeverityLevels { get; set; }
        public virtual DbSet<TblStatus> TblStatuses { get; set; }
        public virtual DbSet<TblTicketAssigneeInfo> TblTicketAssigneeInfos { get; set; }
        public virtual DbSet<TblTicketAssigneeInfoPartsDetail> TblTicketAssigneeInfoPartsDetails { get; set; }
        public virtual DbSet<TblTicketAssigneeInfoSmsdetail> TblTicketAssigneeInfoSmsdetails { get; set; }
        public virtual DbSet<TblUserWiseBusinessAndPlantCode> TblUserWiseBusinessAndPlantCodes { get; set; }
        public virtual DbSet<TblUserWiseEmployeeMapping> TblUserWiseEmployeeMappings { get; set; }
        public virtual DbSet<ViewBom> ViewBoms { get; set; }
        public virtual DbSet<ViewBomdetail> ViewBomdetails { get; set; }
        public virtual DbSet<ViewIssueQuantity> ViewIssueQuantities { get; set; }
        public virtual DbSet<ViewMaterial> ViewMaterials { get; set; }
        public virtual DbSet<ViewProduct> ViewProducts { get; set; }
        public virtual DbSet<VwMaterialReport> VwMaterialReports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= 192.168.100.60;Database=dbMaintenanceTicketManagement;user id=sa;password=dataport;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DisplayUser).HasMaxLength(256);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.LoginProvider)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.ProviderKey)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.LoginProvider)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<MenuMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MenuMaster");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MenuFileName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MenuId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("MenuID");

                entity.Property(e => e.MenuName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MenuUrl)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("MenuURL");

                entity.Property(e => e.ParentMenuId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Parent_MenuID");

                entity.Property(e => e.UseYn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("USE_YN")
                    .IsFixedLength();

                entity.Property(e => e.UserRoll)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("User_Roll");
            });

            modelBuilder.Entity<TblAction>(entity =>
            {
                entity.ToTable("tblAction");

                entity.HasIndex(e => e.ActionCode, "IX_tblAction")
                    .IsUnique();

                entity.Property(e => e.ActionCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ActionName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblActionType>(entity =>
            {
                entity.ToTable("tblActionType");

                entity.HasIndex(e => e.ActionTypeCode, "IX_tblActionType")
                    .IsUnique();

                entity.Property(e => e.ActionTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ActionTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblArea>(entity =>
            {
                entity.ToTable("tblArea");

                entity.HasIndex(e => e.AreaCode, "IX_tblArea")
                    .IsUnique();

                entity.Property(e => e.AreaCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AreaName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblAttachmentType>(entity =>
            {
                entity.ToTable("tblAttachmentType");

                entity.HasIndex(e => e.AttachmentTypeCode, "IX_tblAttachmentType")
                    .IsUnique();

                entity.Property(e => e.AttachmentTypeCode)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.AttachmentTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblBusinessSetupInfo>(entity =>
            {
                entity.ToTable("tblBusinessSetupInfo");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");
            });

            modelBuilder.Entity<TblComplainDepartmentApproval>(entity =>
            {
                entity.ToTable("tblComplainDepartmentApproval");

                entity.HasIndex(e => e.ApprovalNo, "IX_tblComplainDepartmentApproval")
                    .IsUnique();

                entity.Property(e => e.ApprovalDate).HasColumnType("datetime");

                entity.Property(e => e.ApprovalNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ApproverCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ApproverComments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.PlantCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceDepartmentCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StatusCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TicketNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.ApproverCodeNavigation)
                    .WithMany(p => p.TblComplainDepartmentApprovals)
                    .HasPrincipalKey(p => p.EmployeeCode)
                    .HasForeignKey(d => d.ApproverCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComplainDepartmentApproval_tblEmployeeSetup");

                entity.HasOne(d => d.ServiceDepartmentCodeNavigation)
                    .WithMany(p => p.TblComplainDepartmentApprovals)
                    .HasPrincipalKey(p => p.DepartmentCode)
                    .HasForeignKey(d => d.ServiceDepartmentCode)
                    .HasConstraintName("FK_tblComplainDepartmentApproval_tblDepartment");

                entity.HasOne(d => d.TicketNoNavigation)
                    .WithMany(p => p.TblComplainDepartmentApprovals)
                    .HasPrincipalKey(p => p.TicketNo)
                    .HasForeignKey(d => d.TicketNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComplainDepartmentApproval_tblComplainTicket");
            });

            modelBuilder.Entity<TblComplainDepartmentApprovalSmsdetail>(entity =>
            {
                entity.ToTable("tblComplainDepartmentApprovalSMSDetails");

                entity.Property(e => e.ApprovalNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ApproverCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsSmsreceiver).HasColumnName("IsSMSReceiver");

                entity.Property(e => e.ReceiveDate).HasColumnType("datetime");

                entity.Property(e => e.ReplyMessage)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SentDate).HasColumnType("datetime");

                entity.Property(e => e.Smsmessage)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("SMSMessage")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Smsto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SMSTo");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.TicketNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.ApprovalNoNavigation)
                    .WithMany(p => p.TblComplainDepartmentApprovalSmsdetails)
                    .HasPrincipalKey(p => p.ApprovalNo)
                    .HasForeignKey(d => d.ApprovalNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComplainDepartmentApprovalDetails_tblComplainDepartmentApproval");

                entity.HasOne(d => d.TicketNoNavigation)
                    .WithMany(p => p.TblComplainDepartmentApprovalSmsdetails)
                    .HasPrincipalKey(p => p.TicketNo)
                    .HasForeignKey(d => d.TicketNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComplainDepartmentApprovalSMSDetails_tblComplainTicket");
            });

            modelBuilder.Entity<TblComplainTicket>(entity =>
            {
                entity.ToTable("tblComplainTicket");

                entity.HasIndex(e => e.TicketNo, "IX_tblComplainTicketInfo")
                    .IsUnique();

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ComplainDetails)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ComplainTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.PlantCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SeverityLevelCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StatusCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TicketDate).HasColumnType("datetime");

                entity.Property(e => e.TicketNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.SeverityLevelCodeNavigation)
                    .WithMany(p => p.TblComplainTickets)
                    .HasPrincipalKey(p => p.SeverityLevelCode)
                    .HasForeignKey(d => d.SeverityLevelCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComplainTicket_tblSeverityLevel");

                entity.HasOne(d => d.StatusCodeNavigation)
                    .WithMany(p => p.TblComplainTickets)
                    .HasPrincipalKey(p => p.StatusCode)
                    .HasForeignKey(d => d.StatusCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComplainTicket_tblStatus");
            });

            modelBuilder.Entity<TblComplainTicketApproverSmsdetail>(entity =>
            {
                entity.ToTable("tblComplainTicketApproverSMSDetail");

                entity.Property(e => e.ApproverCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsSmsreceiver).HasColumnName("IsSMSReceiver");

                entity.Property(e => e.ReceiveDate).HasColumnType("datetime");

                entity.Property(e => e.ReplyMessage)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SentDate).HasColumnType("datetime");

                entity.Property(e => e.Smsmessage)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("SMSMessage")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Smsto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SMSTo");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.TicketNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Try).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ApproverCodeNavigation)
                    .WithMany(p => p.TblComplainTicketApproverSmsdetails)
                    .HasPrincipalKey(p => p.EmployeeCode)
                    .HasForeignKey(d => d.ApproverCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComplainTicketApproverDetail_tblEmployeeSetup");

                entity.HasOne(d => d.TicketNoNavigation)
                    .WithMany(p => p.TblComplainTicketApproverSmsdetails)
                    .HasPrincipalKey(p => p.TicketNo)
                    .HasForeignKey(d => d.TicketNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComplainTicketApproverDetail_tblComplainTicket");
            });

            modelBuilder.Entity<TblComplainTicketImageDetail>(entity =>
            {
                entity.ToTable("tblComplainTicketImageDetail");

                entity.Property(e => e.DocumentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.FileName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.Location)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.OriginalFileName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TicketNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.TicketNoNavigation)
                    .WithMany(p => p.TblComplainTicketImageDetails)
                    .HasPrincipalKey(p => p.TicketNo)
                    .HasForeignKey(d => d.TicketNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComplainTicketImageDetail_tblComplainTicket");
            });

            modelBuilder.Entity<TblComplainTicketMachineDetail>(entity =>
            {
                entity.ToTable("tblComplainTicketMachineDetail");

                entity.Property(e => e.MachineCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TicketNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.MachineCodeNavigation)
                    .WithMany(p => p.TblComplainTicketMachineDetails)
                    .HasPrincipalKey(p => p.MachineCode)
                    .HasForeignKey(d => d.MachineCode)
                    .HasConstraintName("FK_tblComplainTicketMachineDetail_tblMachineSetup");

                entity.HasOne(d => d.TicketNoNavigation)
                    .WithMany(p => p.TblComplainTicketMachineDetails)
                    .HasPrincipalKey(p => p.TicketNo)
                    .HasForeignKey(d => d.TicketNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComplainTicketMachineDetail_tblComplainTicket");
            });

            modelBuilder.Entity<TblComplainType>(entity =>
            {
                entity.ToTable("tblComplainType");

                entity.HasIndex(e => e.ComplainTypeCode, "IX_tblComplainType")
                    .IsUnique();

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ComplainTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ComplainTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.PlantCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DepartmentCodeNavigation)
                    .WithMany(p => p.TblComplainTypes)
                    .HasPrincipalKey(p => p.DepartmentCode)
                    .HasForeignKey(d => d.DepartmentCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComplainType_tblDepartment");
            });

            modelBuilder.Entity<TblDepartment>(entity =>
            {
                entity.ToTable("tblDepartment");

                entity.HasIndex(e => e.DepartmentCode, "IX_tblDepartment")
                    .IsUnique();

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.PlantCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.TypeCodeNavigation)
                    .WithMany(p => p.TblDepartments)
                    .HasPrincipalKey(p => p.TypeCode)
                    .HasForeignKey(d => d.TypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDepartment_tblDepartment");
            });

            modelBuilder.Entity<TblDepartmentType>(entity =>
            {
                entity.ToTable("tblDepartmentType");

                entity.HasIndex(e => e.TypeCode, "IX_tblDepartmentType")
                    .IsUnique();

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PlantCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TypeName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblDesignation>(entity =>
            {
                entity.ToTable("tblDesignation");

                entity.HasIndex(e => e.DesignationCode, "IX_tblDesignation")
                    .IsUnique();

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DesignationCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DesignationName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.PlantCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DepartmentCodeNavigation)
                    .WithMany(p => p.TblDesignations)
                    .HasPrincipalKey(p => p.DepartmentCode)
                    .HasForeignKey(d => d.DepartmentCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDesignation_tblDesignation");
            });

            modelBuilder.Entity<TblEmployeeSetup>(entity =>
            {
                entity.ToTable("tblEmployeeSetup");

                entity.HasIndex(e => e.EmployeeCode, "IX_tblEmployeeSetup")
                    .IsUnique();

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ContactNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DesignationCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.EmployeeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.IsSmsreceiver).HasColumnName("IsSMSReceiver");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.PlantCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DepartmentCodeNavigation)
                    .WithMany(p => p.TblEmployeeSetups)
                    .HasPrincipalKey(p => p.DepartmentCode)
                    .HasForeignKey(d => d.DepartmentCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEmployeeSetup_tblDepartment");

                entity.HasOne(d => d.DesignationCodeNavigation)
                    .WithMany(p => p.TblEmployeeSetups)
                    .HasPrincipalKey(p => p.DesignationCode)
                    .HasForeignKey(d => d.DesignationCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEmployeeSetup_tblDesignation");
            });

            modelBuilder.Entity<TblMachineSetup>(entity =>
            {
                entity.ToTable("tblMachineSetup");

                entity.HasIndex(e => e.MachineCode, "IX_tblMachineSetup")
                    .IsUnique();

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.InstallationDate).HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.MachineCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MachineDetails)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.MachineName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModelNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OriginCountry)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlantCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DepartmentCodeNavigation)
                    .WithMany(p => p.TblMachineSetups)
                    .HasPrincipalKey(p => p.DepartmentCode)
                    .HasForeignKey(d => d.DepartmentCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMachineSetup_tblDepartment");
            });

            modelBuilder.Entity<TblMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblMenu");

                entity.Property(e => e.ContentName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Href)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("href");

                entity.Property(e => e.IconClass)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMenuList>(entity =>
            {
                entity.ToTable("tblMenuList");

                entity.Property(e => e.ActionName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ControllerName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModuleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ParentMenuName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblPart>(entity =>
            {
                entity.ToTable("tblParts");

                entity.Property(e => e.PartsCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PartsName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Unit)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblRegion>(entity =>
            {
                entity.ToTable("tblRegion");

                entity.HasIndex(e => e.RegionCode, "IX_tblRegion")
                    .IsUnique();

                entity.Property(e => e.RegionCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RegionName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblServiceDepartmentTicketAssignment>(entity =>
            {
                entity.ToTable("tblServiceDepartmentTicketAssignment");

                entity.HasIndex(e => e.AssignNo, "IX_tblServiceDepartmentTicketAssignment")
                    .IsUnique();

                entity.Property(e => e.AssignByCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AssignDate).HasColumnType("datetime");

                entity.Property(e => e.AssignNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AssignToCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AssignerComments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.PlantCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceDepartmentCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StatusCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TicketNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.AssignByCodeNavigation)
                    .WithMany(p => p.TblServiceDepartmentTicketAssignmentAssignByCodeNavigations)
                    .HasPrincipalKey(p => p.EmployeeCode)
                    .HasForeignKey(d => d.AssignByCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblServiceDepartmentTicketAssignment_tblEmployeeSetup");

                entity.HasOne(d => d.AssignToCodeNavigation)
                    .WithMany(p => p.TblServiceDepartmentTicketAssignmentAssignToCodeNavigations)
                    .HasPrincipalKey(p => p.EmployeeCode)
                    .HasForeignKey(d => d.AssignToCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblServiceDepartmentTicketAssignment_tblEmployeeSetup1");

                entity.HasOne(d => d.ServiceDepartmentCodeNavigation)
                    .WithMany(p => p.TblServiceDepartmentTicketAssignments)
                    .HasPrincipalKey(p => p.DepartmentCode)
                    .HasForeignKey(d => d.ServiceDepartmentCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblServiceDepartmentTicketAssignment_tblDepartment");

                entity.HasOne(d => d.TicketNoNavigation)
                    .WithMany(p => p.TblServiceDepartmentTicketAssignments)
                    .HasPrincipalKey(p => p.TicketNo)
                    .HasForeignKey(d => d.TicketNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblServiceDepartmentTicketAssignment_tblComplainTicket");
            });

            modelBuilder.Entity<TblSeverityLevel>(entity =>
            {
                entity.ToTable("tblSeverityLevel");

                entity.HasIndex(e => e.SeverityLevelCode, "IX_tblSeverityLevel")
                    .IsUnique();

                entity.Property(e => e.SeverityLevelCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SeverityLevelName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblStatus>(entity =>
            {
                entity.ToTable("tblStatus");

                entity.HasIndex(e => e.StatusCode, "IX_tblStatus")
                    .IsUnique();

                entity.Property(e => e.StatusCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblTicketAssigneeInfo>(entity =>
            {
                entity.ToTable("tblTicketAssigneeInfo");

                entity.HasIndex(e => e.JobNo, "IX_tblTicketAssigneeInfo")
                    .IsUnique();

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.JobDate).HasColumnType("datetime");

                entity.Property(e => e.JobExecutedByCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.JobNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PlantCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceDepartmentCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StatusCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TicketNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.JobExecutedByCodeNavigation)
                    .WithMany(p => p.TblTicketAssigneeInfos)
                    .HasPrincipalKey(p => p.EmployeeCode)
                    .HasForeignKey(d => d.JobExecutedByCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTicketAssigneeInfo_tblEmployeeSetup");

                entity.HasOne(d => d.ServiceDepartmentCodeNavigation)
                    .WithMany(p => p.TblTicketAssigneeInfos)
                    .HasPrincipalKey(p => p.DepartmentCode)
                    .HasForeignKey(d => d.ServiceDepartmentCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTicketAssigneeInfo_tblDepartment");

                entity.HasOne(d => d.TicketNoNavigation)
                    .WithMany(p => p.TblTicketAssigneeInfos)
                    .HasPrincipalKey(p => p.TicketNo)
                    .HasForeignKey(d => d.TicketNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTicketAssigneeInfo_tblComplainTicket");
            });

            modelBuilder.Entity<TblTicketAssigneeInfoPartsDetail>(entity =>
            {
                entity.ToTable("tblTicketAssigneeInfoPartsDetail");

                entity.Property(e => e.JobNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PartsCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Quantity).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.TicketNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.JobNoNavigation)
                    .WithMany(p => p.TblTicketAssigneeInfoPartsDetails)
                    .HasPrincipalKey(p => p.JobNo)
                    .HasForeignKey(d => d.JobNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTicketAssigneeInfoPartsDetail_tblTicketAssigneeInfo");

                entity.HasOne(d => d.TicketNoNavigation)
                    .WithMany(p => p.TblTicketAssigneeInfoPartsDetails)
                    .HasPrincipalKey(p => p.TicketNo)
                    .HasForeignKey(d => d.TicketNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTicketAssigneeInfoPartsDetail_tblComplainTicket");
            });

            modelBuilder.Entity<TblTicketAssigneeInfoSmsdetail>(entity =>
            {
                entity.ToTable("tblTicketAssigneeInfoSMSDetail");

                entity.Property(e => e.ApproverCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsSmsreceiver).HasColumnName("IsSMSReceiver");

                entity.Property(e => e.JobNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ReceiveDate).HasColumnType("datetime");

                entity.Property(e => e.ReplyMessage)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SentDate).HasColumnType("datetime");

                entity.Property(e => e.Smsmessage)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("SMSMessage")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Smsto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SMSTo");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.TicketNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.ApproverCodeNavigation)
                    .WithMany(p => p.TblTicketAssigneeInfoSmsdetails)
                    .HasPrincipalKey(p => p.EmployeeCode)
                    .HasForeignKey(d => d.ApproverCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTicketAssigneeInfoSMSDetail_tblEmployeeSetup");

                entity.HasOne(d => d.JobNoNavigation)
                    .WithMany(p => p.TblTicketAssigneeInfoSmsdetails)
                    .HasPrincipalKey(p => p.JobNo)
                    .HasForeignKey(d => d.JobNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTicketAssigneeInfoSMSDetail_tblTicketAssigneeInfo");

                entity.HasOne(d => d.TicketNoNavigation)
                    .WithMany(p => p.TblTicketAssigneeInfoSmsdetails)
                    .HasPrincipalKey(p => p.TicketNo)
                    .HasForeignKey(d => d.TicketNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTicketAssigneeInfoSMSDetail_tblComplainTicket");
            });

            modelBuilder.Entity<TblUserWiseBusinessAndPlantCode>(entity =>
            {
                entity.ToTable("tblUserWiseBusinessAndPlantCode");

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PlantCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<TblUserWiseEmployeeMapping>(entity =>
            {
                entity.ToTable("tblUserWiseEmployeeMapping");

                entity.Property(e => e.EmployeeCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<ViewBom>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_BOM");

                entity.Property(e => e.AltUnitBatchSize).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.AltUnitStandardOutput).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.AlternativeBom).HasColumnName("AlternativeBOM");

                entity.Property(e => e.AlternativeUnit)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BatchSize).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Bomtext)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BOMText");

                entity.Property(e => e.Bomtype)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BOMType");

                entity.Property(e => e.BomusageTypeId).HasColumnName("BOMUsageTypeId");

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ConversionValue).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StandardOutput).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValidFrom).HasColumnType("datetime");

                entity.Property(e => e.ValidTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<ViewBomdetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_BOMDetail");

                entity.Property(e => e.Bomid).HasColumnName("BOMId");

                entity.Property(e => e.CostPerProductUnit).HasColumnType("numeric(24, 8)");

                entity.Property(e => e.LossPercentage).HasColumnType("numeric(24, 2)");

                entity.Property(e => e.LossQuantity).HasColumnType("numeric(24, 8)");

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PerBatchQuantityExcludingLoss).HasColumnType("numeric(24, 8)");

                entity.Property(e => e.QuantityPerBatch).HasColumnType("numeric(24, 8)");

                entity.Property(e => e.Tolerance).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<ViewIssueQuantity>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_IssueQuantity");

                entity.Property(e => e.IssueQuantityDc)
                    .HasColumnType("numeric(38, 8)")
                    .HasColumnName("Issue_Quantity_DC");

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ViewMaterial>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_Material");

                entity.Property(e => e.BaseUnit)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaterialName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SubBusinessCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ViewProduct>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_Product");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.BrandCode)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.BusiSumGroupCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Business)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Carton).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.DiscountStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.DistDiscount).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.EffectedDate).HasColumnType("datetime");

                entity.Property(e => e.GroupCode)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Mrp)
                    .HasColumnType("numeric(18, 4)")
                    .HasColumnName("MRP");

                entity.Property(e => e.PackSize)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Pccc)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("PCCC");

                entity.Property(e => e.PrincipalCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCategory)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName1)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.RatePerCarton).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ReportGroupCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Show)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Smscode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("SMSCODE");

                entity.Property(e => e.Smsorder)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SMSOrder");

                entity.Property(e => e.StorageType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SubBusinessCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UnitPrice).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.Vat)
                    .HasColumnType("numeric(18, 4)")
                    .HasColumnName("VAT");
            });

            modelBuilder.Entity<VwMaterialReport>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Material_Report");

                entity.Property(e => e.BaseUnit)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MaterialCategory)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaterialCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaterialName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaterialType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
