
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Services.DisciplinaryCases;
using CDFStaffManagement.Services.Employee;
using CDFStaffManagement.Services.PayslipDetails;
using CDFStaffManagement.Services.PositionCodeDetails;
using CDFStaffManagement.Services.EmployeeObject;
using CDFStaffManagement.Services.LeaveManagement;
using CDFStaffManagement.Services.NapsaConfiguration;
using CDFStaffManagement.Services.Parameters;
using CDFStaffManagement.Services.Parameters.Interfaces;
using CDFStaffManagement.Services.Parameters.TerminationReason;
using CDFStaffManagement.Services.PayrollDefinitions;
using CDFStaffManagement.Services.PayrollRun;
using CDFStaffManagement.Services.Uploads;
using CDFStaffManagement.Services.UserAccount;
using CDFStaffManagement.Utilities;
using CDFStaffManagement.Services.Timesheet;

namespace MyPayroll
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnectionStr = Configuration.GetConnectionString("DBConnection");
            services.AddDbContext<MyPayrollContext>(options => options.UseSqlServer(dbConnectionStr));
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<IUserManagerRepository, UserManager>();
            services.AddScoped<ICustomLogger, LogsModel>();
            services.AddScoped<IParameterRepository, ParametersRepository>();
            services.AddScoped<IPayslipDefinitionRepository, PayrollDefinitionsRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IPayslipDetailRepository, PayslipDetailRepository>();
            services.AddScoped<INapsaConfigurationRepository, NapsaConfigurationService>();
            services.AddScoped<IEmployeeObjectRepository, EmployeeObjectRepository>();
            services.AddScoped<IEmployeeRemunerationService, EmployeeRemunerationService>();
            services.AddScoped<IPositionCodeDetailsService, PositionCodeCodeDetailsService>();
            services.AddScoped<IPromotionAndTerminationService, PromotionAndTerminationService>();
            services.AddScoped<ITerminationReasonService, TerminationReasonService>();
            services.AddScoped<IPayrollRunService, PayrollRunService>();
            services.AddScoped<IEarningAndDeductionLinesUploadService, EarningAndDeductionLinesUploadService>();
            services.AddScoped<IBanksDetailsService, BanksDetailsService>();
            services.AddScoped<IQualificationDocumentsService, QualificationDocumentsService>();
            services.AddScoped<ITimesheetService, TimesheetService>();
            services.AddScoped<IDisciplinaryCaseService, DisciplinaryCaseService>();
            services.AddScoped<ILeaveManagementService, LeaveManagementService>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddHttpContextAccessor();
            services.AddScoped<DataProtectionSettings>();
            services.AddAuthorization();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=UserAccount}/{action=Login}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
