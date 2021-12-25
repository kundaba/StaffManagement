using System.Linq;
using CDFStaffManagement.Model.EntityModels;
using Microsoft.EntityFrameworkCore;
using MyPayroll;
using CDFStaffManagement.Services.Parameters;
using CDFStaffManagement.Services.StoredProcedures;

namespace CDFStaffManagement.Model.DBContext
{
    public class MyPayrollContext : DbContext
    {
        public MyPayrollContext()
        {
        }

        public MyPayrollContext(DbContextOptions<MyPayrollContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bank> Bank { get; set; } = null!;
        public virtual DbSet<BankBranch> BankBranch { get; set; } = null!;

        public virtual DbSet<Company> Company { get; set; } = null!;
        public virtual DbSet<CountryNames> CountryNames { get; set; } = null!;
        public virtual DbSet<Department> Department { get; set; } = null!;
        public virtual DbSet<Employee> Employee { get; set; } = null!;

        public virtual DbSet<EmployeeHistory> EmployeeHistory { get; set; } = null!;
        public virtual DbSet<EmployeeStatus> EmployeeStatus { get; set; } = null!;
        public virtual DbSet<Entity> Entity { get; set; } = null!;
        public virtual DbSet<ErrorLog> ErrorLog { get; set; } = null!;

        public virtual DbSet<Gender> Gender { get; set; } = null!;
        public virtual DbSet<IdnumberType> IdnumberType { get; set; } = null!;

        public virtual DbSet<IncreaseHistory> IncreaseHistory { get; set; } = null!;

        public virtual DbSet<IncreaseReason> IncreaseReason { get; set; } = null!;
        public virtual DbSet<JobGeneral> JobGeneral { get; set; } = null!;
        public virtual DbSet<JobGrade> JobGrade { get; set; } = null!;
        public virtual DbSet<JobTitle> JobTitle { get; set; } = null!;
        public virtual DbSet<MaritalStatus> MaritalStatus { get; set; } = null!;
        public virtual DbSet<NatureOfContract> NatureOfContract { get; set; } = null!;
        public virtual DbSet<PayrollDeductionDef> PayrollDeductionDef { get; set; } = null!;
        public virtual DbSet<PayrollEarningDef> PayrollEarningDef { get; set; } = null!;
        public virtual DbSet<PayslipDefinition> PayslipDefinition { get; set; } = null!;
        public virtual DbSet<PayslipDetail> PayslipDetail { get; set; } = null!;

        public virtual DbSet<PayslipDetailArchive> PayslipDetailArchive { get; set; } = null!;
        public virtual DbSet<StatusDescription> StatusDescription { get; set; } = null!;
        public virtual DbSet<TaxTableDefinition> TaxTableDefinition { get; set; } = null!;

        public virtual DbSet<TitleDescription> TitleDescription { get; set; } = null!;
        public virtual DbSet<TerminationReason> TerminationReason { get; set; } = null!;
        public virtual DbSet<EmployeeRemuneration> EmployeeRemuneration { get; set; } = null!;
        public virtual DbSet<UserAuditLogs> UserAuditLogs { get; set; } = null!;
        public virtual DbSet<UserDetail> UserDetail { get; set; } = null!;

        public virtual DbSet<UserMenu> UserMenu { get; set; } = null!;

        public virtual DbSet<UserMenuMapping> UserMenuMapping { get; set; } = null!;
        public virtual DbSet<UserPasswordResets> UserPasswordResets { get; set; } = null!;

        public virtual DbSet<UserRoles> UserRoles { get; set; } = null!;

        public virtual DbSet<UserStatus> UserStatus { get; set; } = null!;
        public virtual DbSet<EmployeeDetail> EmployeeDetail { get; set; } = null!;
        public virtual DbSet<NapsaConfiguration> NapsaConfiguration { get; set; } = null!;
        public DbSet<UserList> GetUserList { get; set; } = null!;

        public DbSet<PositionDetails> PositionDetails { get; set; } = null!;

        public DbSet<PositionCodesView> PositionCodesView { get; set; } = null!;

        public DbSet<PromotionHistory> PromotionHistory { get; set; } = null!;
        public virtual DbSet<EmployeeBankDetails> EmployeeBankDetails { get; set; } = null!;
        public virtual DbSet<EmployeeBankDetailsView> EmployeeBankDetailsView { get; set; } = null!;
        
        public virtual DbSet<NhimaConfiguration> NhimaConfiguration { get; set; } = null!;
        public virtual DbSet<EmployeeQualifications> EmployeeQualifications { get; set; } = null!;
        public virtual DbSet<EmployeeQualificationsView> EmployeeQualificationsView { get; set; } = null!;
        
        public virtual DbSet<PayrollRunDetailsView> PayrollRunDetailsView { get; set; } = null!;
        public virtual DbSet<EmployeeTimeSheet> EmployeeTimeSheet { get; set; } = null!;
        public virtual DbSet<DisciplinaryCases> DisciplinaryCases { get; set; } = null!;
        
        public virtual DbSet<LeaveTypes> LeaveTypes { get; set; } = null!;
        
        public virtual DbSet<LeaveDetail> LeaveDetail { get; set; } = null!;
        
        public virtual DbSet<LeaveEntitlementView> LeaveEntitlementView { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.BankId).HasColumnName("BankID");

                entity.Property(e => e.BankName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BankBranch>(entity =>
            {
                entity.HasKey(e => e.BranchId);

                entity.Property(e => e.BranchId).HasColumnName("BranchID");

                entity.Property(e => e.BankId).HasColumnName("BankID");

                entity.Property(e => e.BranchCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BranchName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p!.BankBranch)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankBranch_Bank");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CompanyName)
                    .HasColumnName("CompanyName")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CountryNames>(entity =>
            {
                entity.HasIndex(e => e.CountryCode)
                    .HasName("IX_CountryNames")
                    .IsUnique();

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CountryName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.DepartmentCode)
                    .HasName("IX_Department")
                    .IsUnique();

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.DepartmentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LongDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.EmployeeCode)
                    .HasName("IX_Employee")
                    .IsUnique();

                entity.HasIndex(e => e.EntityId)
                    .HasName("IX_Employee_1")
                    .IsUnique();

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.DateEngaged).HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.EmployeeCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeStatusId).HasColumnName("EmployeeStatusID");

                entity.Property(e => e.EntityId).HasColumnName("EntityID");

                entity.Property(e => e.JobGeneralId).HasColumnName("JobGeneralID");

                entity.Property(e => e.JobGradeId).HasColumnName("JobGradeID");

                entity.Property(e => e.JobTitleId).HasColumnName("JobTitleID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LeaveStartDate).HasColumnType("datetime");

                entity.Property(e => e.NatureOfContractId).HasColumnName("NatureOfContractID");

                entity.Property(e => e.ReportToEmployeeId).HasColumnName("ReportToEmployeeID");

                entity.Property(e => e.TerminationDate).HasColumnType("datetime");

                entity.Property(e => e.TerminationReasonId).HasColumnName("TerminationReasonID");

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p!.Employee)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Employee_Department");

                entity.HasOne(d => d.EmployeeStatus)
                    .WithMany(p => p!.Employee)
                    .HasForeignKey(d => d.EmployeeStatusId)
                    .HasConstraintName("FK_Employee_EmployeeStatus");

                entity.HasOne(d => d.Entity)
                    .WithOne(p => p!.Employee!)
                    .HasForeignKey<Employee>(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Entity");

                entity.HasOne(d => d.JobGeneral)
                    .WithMany(p => p!.Employee)
                    .HasForeignKey(d => d.JobGeneralId)
                    .HasConstraintName("FK_Employee_JobGeneral");

                entity.HasOne(d => d.JobGrade)
                    .WithMany(p => p!.Employee)
                    .HasForeignKey(d => d.JobGradeId)
                    .HasConstraintName("FK_Employee_JobGrade");

                entity.HasOne(d => d.JobTitle)
                    .WithMany(p => p!.Employee)
                    .HasForeignKey(d => d.JobTitleId)
                    .HasConstraintName("FK_Employee_JobTitle");

                entity.HasOne(d => d.NatureOfContract)
                    .WithMany(p => p!.Employee)
                    .HasForeignKey(d => d.NatureOfContractId)
                    .HasConstraintName("FK_Employee_NatureOfContract");
            });

            modelBuilder.Entity<EmployeeHistory>(entity =>
            {
                entity.HasKey(e => e.EmployeeHistoryId);
            });

            modelBuilder.Entity<EmployeeStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.HasIndex(e => e.StatusCode)
                    .HasName("IX_EmployeeStatus")
                    .IsUnique();

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.StatusDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Entity>(entity =>
            {
                entity.HasKey(e => e.EntityCode);

                entity.Property(e => e.BankBranchId).HasColumnName("BankBranchID");

                entity.Property(e => e.BankId).HasColumnName("BankID");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CellNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CountryOfBirthId).HasColumnName("CountryOfBirthID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeStatusId).HasColumnName("EmployeeStatusID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Idnumber)
                    .IsRequired()
                    .HasColumnName("IDNumber")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdnumberType).HasColumnName("IDNumberType");

                entity.Property(e => e.LastChanged).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaidenName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaritalStatusId).HasColumnName("MaritalStatusID");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhysicalAddress).IsUnicode(false);

                entity.Property(e => e.SecondName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TitleId).HasColumnName("TitleID");

                entity.Property(e => e.WorkAddress).IsUnicode(false);

                entity.Property(e => e.WorkNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.CountryOfBirth)
                    .WithMany(p => p!.Entity)
                    .HasForeignKey(d => d.CountryOfBirthId)
                    .HasConstraintName("FK_Entity_CountryNames");

                entity.HasOne(d => d.EmployeeStatus)
                    .WithMany(p => p!.Entity)
                    .HasForeignKey(d => d.EmployeeStatusId)
                    .HasConstraintName("FK_Entity_EmployeeStatus");

                entity.HasOne(d => d.IdnumberTypeNavigation)
                    .WithMany(p => p!.Entity)
                    .HasForeignKey(d => d.IdnumberType)
                    .HasConstraintName("FK_Entity_IDNumberType");

                entity.HasOne(d => d.MaritalStatus)
                    .WithMany(p => p!.Entity)
                    .HasForeignKey(d => d.MaritalStatusId)
                    .HasConstraintName("FK_Entity_MaritalStatus");

                entity.HasOne(d => d.Title)
                    .WithMany(p => p!.Entity)
                    .HasForeignKey(d => d.TitleId)
                    .HasConstraintName("FK_Entity_TitleDescription");
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateLogged).HasColumnType("datetime");

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasIndex(e => e.Code)
                    .HasName("IX_Gender")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.LongDescription)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IdnumberType>(entity =>
            {
                entity.ToTable("IDNumberType");

                entity.HasIndex(e => e.Idcode)
                    .HasName("IX_IDNumberType")
                    .IsUnique();

                entity.Property(e => e.IdnumberTypeId).HasColumnName("IDNumberTypeID");

                entity.Property(e => e.Idcode)
                    .HasColumnName("IDCode")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LongDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IncreaseHistory>(entity =>
            {
                entity.Property(e => e.IncreaseHistoryId).HasColumnName("IncreaseHistoryID");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.IncreaseAmount).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.IncreaseAppliedOn)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IncreasePercentage).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.IncreaseProcessedDate).HasColumnType("datetime");

                entity.Property(e => e.IncreaseReasonTypeId).HasColumnName("IncreaseReasonTypeID");

                entity.Property(e => e.JobGradeId).HasColumnName("JobGradeID");

                entity.Property(e => e.JobTitleId).HasColumnName("JobTitleID");

                entity.Property(e => e.LastChanged).HasColumnType("datetime");

                entity.Property(e => e.NewAnnualSalary).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.NewMonthlySalary).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.NewRatePerDay).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.NewRatePerHour).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.PreviousAnnualSalary).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.PreviousMonthlySalary).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.PreviousRatePerDay).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.PreviousRatePerHour).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.ProcessedByUserId).HasColumnName("ProcessedByUserID");
            });

            modelBuilder.Entity<IncreaseReason>(entity =>
            {
                entity.HasIndex(e => e.Code)
                    .HasName("IX_IncreaseReason")
                    .IsUnique();

                entity.Property(e => e.IncreaseReasonId).HasColumnName("IncreaseReasonID");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastChanged).HasColumnType("datetime");

                entity.Property(e => e.LongDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ShortDescription)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<JobGeneral>(entity =>
            {
                entity.Property(e => e.JobGeneralId).HasColumnName("JobGeneralID");

                entity.Property(e => e.LongDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<JobGrade>(entity =>
            {
                entity.HasIndex(e => e.JobGradeCode)
                    .HasName("IX_JobGrade")
                    .IsUnique();

                entity.Property(e => e.JobGradeId).HasColumnName("JobGradeID");

                entity.Property(e => e.JobGradeCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.JobGradeDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<JobTitle>(entity =>
            {
                entity.Property(e => e.JobTitleId).HasColumnName("JobTitleID");

                entity.Property(e => e.Jobcode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MaritalStatus>(entity =>
            {
                entity.Property(e => e.LongDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NatureOfContract>(entity =>
            {
                entity.HasIndex(e => e.ContractTypeCode)
                    .HasName("IX_NatureOfContract")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractTypeCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ContractTypeDecsription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PayrollDeductionDef>(entity =>
            {
                entity.HasKey(e => e.DefId);

                entity.Property(e => e.DefId).HasColumnName("DefID");

                entity.Property(e => e.DeductionCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DeductionDecsription)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PayrollEarningDef>(entity =>
            {
                entity.HasKey(e => e.DefId);

                entity.Property(e => e.DefId).HasColumnName("DefID");

                entity.Property(e => e.EarningLineCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EarningLineDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PayslipDetail>(entity =>
            {
                entity.HasKey(e => e.PayslipId);

                entity.Property(e => e.PayslipId).HasColumnName("PayslipID");

                entity.Property(e => e.DeductionAmount).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.DeductionDefId).HasColumnName("DeductionDefId");

                entity.Property(e => e.EarningAmount).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.EarningDefId).HasColumnName("EarningDefId");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeId");

                entity.Property(e => e.PayPeriod).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<PayslipDetailArchive>(entity =>
            {
                entity.HasKey(e => e.PayslipArchiveId);

                entity.Property(e => e.PayslipArchiveId).HasColumnName("PayslipArchiveID");

                entity.Property(e => e.DeductionAmount).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.DeductionDefId).HasColumnName("DeductionDefID");

                entity.Property(e => e.EarningAmount).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.EarningDefId).HasColumnName("EarningDefID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmpoyeeID");

                entity.Property(e => e.ExportDate).HasColumnType("datetime");

                entity.Property(e => e.ExportedByUserId).HasColumnName("ExportedByUserID");

                entity.Property(e => e.PayPeriod).HasColumnType("datetime");
            });

            modelBuilder.Entity<StatusDescription>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK_GeneralStatusDescription");

                entity.HasIndex(e => e.StatusId)
                    .HasName("IX_GeneralStatusDescription")
                    .IsUnique();

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.StausDescription)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TaxTableDefinition>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BandDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LowerLimit).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UperLimit).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<TitleDescription>(entity =>
            {
                entity.HasKey(e => e.TitleId);

                entity.Property(e => e.TitleDescription1)
                    .HasColumnName("TitleDescription")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserAuditLogs>(entity =>
            {
                entity.HasKey(e => e.AuditId);

                entity.Property(e => e.AuditId).HasColumnName("AuditID");

                entity.Property(e => e.Action).IsUnicode(false);

                entity.Property(e => e.ActionDate).HasColumnType("datetime");

                entity.Property(e => e.ActionType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .IsUnicode(false);

                entity.Property(e => e.NewValue)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OldValue)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.EmployeId)
                    .HasName("IX_UserDetail")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("IX_UserDetail_1")
                    .IsUnique();

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogon).HasColumnType("datetime");

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProfileStatusNavigation)
                    .WithMany(p => p!.UserDetail)
                    .HasForeignKey(d => d.ProfileStatus)
                    .HasConstraintName("FK_UserDetail_UserStatus");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p!.UserDetail)
                    .HasForeignKey(d => d.UserRoleId)
                    .HasConstraintName("FK_UserDetail_UserRoles");
            });

            modelBuilder.Entity<UserMenu>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MenuDescription).IsUnicode(false);
            });

            modelBuilder.Entity<UserMenuMapping>(entity =>
            {
                entity.Property(e => e.UserMenuMappingId).HasColumnName("UserMenuMappingID");

                entity.Property(e => e.UserMenuId).HasColumnName("UserMenuID");

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

                entity.HasOne(d => d.UserMenu)
                    .WithMany(p => p!.UserMenuMapping)
                    .HasForeignKey(d => d.UserMenuId)
                    .HasConstraintName("FK_UserMenuMapping_UserMenu");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p!.UserMenuMapping)
                    .HasForeignKey(d => d.UserRoleId)
                    .HasConstraintName("FK_UserMenuMapping_UserRoles");
            });

            modelBuilder.Entity<UserPasswordResets>(entity =>
            {
                entity.HasKey(e => e.ResetId);

                entity.HasIndex(e => e.ResetId)
                    .HasName("IX_UserPasswordResets_1");

                entity.Property(e => e.ResetId).HasColumnName("ResetID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ResetDate).HasColumnType("datetime");

                entity.Property(e => e.ResetToken).IsUnicode(false);

                entity.Property(e => e.TokenStatusId).HasColumnName("TokenStatusID");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p!.UserPasswordResets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserPasswordResets_UserDetail");
            });

            modelBuilder.Entity<EmployeeBankDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<EmployeeBankDetailsView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("EmployeeBankDetailsView");

                entity.Property(e => e.AccountName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BankName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BranchName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasKey(e => e.UserRoleId);

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

                entity.Property(e => e.RoleDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.StatusDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmployeeRemuneration>(entity =>
            {
                entity.HasKey(e => e.RemunerationId);
            });  
            
            modelBuilder.Entity<PositionDetails>(entity =>
            {
                entity.HasKey(e => e.PositionCodeId);
            });  
            
            modelBuilder.Entity<PositionCodesView>(entity =>
            {
                entity.HasNoKey();
            });   
            
            modelBuilder.Entity<NapsaConfiguration>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            
            modelBuilder.Entity<TerminationReason>(entity =>
            {
                entity.HasKey(e => e.TerminationReasonId);
            });
            
            modelBuilder.Entity<PromotionHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
            });   
            
            modelBuilder.Entity<NhimaConfiguration>(entity =>
            {
                entity.HasKey(e => e.Id);
            }); 
            
            modelBuilder.Entity<EmployeeQualifications>(entity =>
            {
                entity.HasKey(e => e.FileId);
            });  
            
            modelBuilder.Entity<EmployeeTimeSheet>(entity =>
            {
                entity.HasKey(e => e.Id);
            });  
            
            modelBuilder.Entity<EmployeeQualificationsView>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<DisciplinaryCases>(entity =>
            {
                entity.HasKey(e=>e.CaseId);
            });
            modelBuilder.Entity<PayrollRunDetailsView>(entity =>
            {
                entity.HasNoKey();
            }); 
            
            modelBuilder.Entity<LeaveTypes>(entity =>
            {
                entity.HasKey(e=>e.Id);
            });   
            
            modelBuilder.Entity<LeaveDetail>(entity =>
            {
                entity.HasKey(e=>e.Id);
            }); 
            
            modelBuilder.Entity<LeaveEntitlementView>(entity =>
            {
                entity.HasNoKey();
            });
            //modelBuilder.Entity<EmployeeDetail>().ToView("EmployeeDetail");
            modelBuilder.Entity<EmployeeDetail>(entity => {
                entity.HasKey(e => e.EmployeeId);
                entity.ToTable("EmployeeDetail");
            });
        }

        
        public DbSet<PayrollEarningLine> PayrollEarningLine { get; set; } = null!;

        public DbSet<Services.Employee.Dto.Employee> Employee_1 { get; set; } = null!;

        public DbSet<TerminationReasons> TerminationReasons { get; set; } = null!;
    }
}
