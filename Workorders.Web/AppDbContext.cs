using System.Configuration;
using System.Data.Entity;
using Workorders.Data;
using System.ComponentModel.DataAnnotations.Schema;
using Workorders.Web.Helpers;

namespace Workorders.Web
{
    public class AppDbContext : DbContext
    {
        public DbSet<Workorder> Workorders { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<DepartmentSupervisorEmail> DepartmentSupervisorEmails { get; set; }
        public DbSet<AlertRecipient> AlertRecipients { get; set; }
        public DbSet<ReportRecipient> ReportRecipients { get; set; }

        public AppDbContext()
            : base(nameOrConnectionString: ConnectionStringName)
        {
            //disable initializer
            Database.SetInitializer<AppDbContext>(null);
        }

        public AppDbContext(string connectionStringName) : base(nameOrConnectionString: connectionStringName) { }

        public static string ConnectionStringName
        {
            get
            {
                if (ConfigurationManager.AppSettings["ConnectionStringName"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["ConnectionStringName"].ToString();
                }
                return "AppConnection";
            }
        }

        // Interrupt the in-memory model creation and configure with the Fluent API.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var conn = Database.Connection;
            var company = CompanyCookie.SelectedCompany;
            //Workorder
            //Maintenance.MaintWorkorder

            if (company == "DSN")
            {
                modelBuilder.Entity<Workorder>().Map(m => { m.ToTable("dbo.MaintWorkorder"); });
            }
            else
            {
                modelBuilder.Entity<Workorder>().Map(m => { m.ToTable("Maintenance.MaintWorkorder"); });
                modelBuilder.Entity<Workorder>().Property(m => m.PersonServed).HasColumnName("Consumer");
            }

            modelBuilder.Entity<Workorder>().Property(m => m.Id).HasColumnName("MaintWorkorderID");
            modelBuilder.Entity<Workorder>().Property(m => m.DepartmentId).HasColumnName("DepartmentID");
            modelBuilder.Entity<Workorder>().Property(m => m.Reported).HasColumnName("MaintWorkorderDate");
            modelBuilder.Entity<Workorder>().Property(m => m.Reporter).HasColumnName("SubmittingParty");
            modelBuilder.Entity<Workorder>().Property(m => m.Priority).HasColumnName("PriorityID");
            modelBuilder.Entity<Workorder>().Property(m => m.ConsumerId).HasColumnName("ConsumerID");
            modelBuilder.Entity<Workorder>().Property(m => m.Closed).HasColumnName("MaintWorkorderComplete");
            modelBuilder.Entity<Workorder>().Property(m => m.Details).HasColumnName("MaintWorkorderDetail");
            modelBuilder.Entity<Workorder>().Property(m => m.Closer).HasColumnName("CompletedBy");
            modelBuilder.Entity<Workorder>().Property(m => m.Open).HasColumnName("Open").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            //Department
            //Common.Departments
            modelBuilder.Entity<Department>().Map(d => { d.ToTable("Common.Departments"); });
            modelBuilder.Entity<Department>().Property(d => d.Id).HasColumnName("DepartmentID");
            modelBuilder.Entity<Department>().Property(d => d.Name).HasColumnName("DepartmentName");
            modelBuilder.Entity<Department>().Property(d => d.Comments).HasColumnName("DeptNotes");

            //Area
            //Common.AreasLookup
            modelBuilder.Entity<Area>().Map(a => { a.ToTable("Common.AreasLookup"); });
            modelBuilder.Entity<Area>().Property(a => a.Id).HasColumnName("AreaID");
            modelBuilder.Entity<Area>().Property(a => a.Name).HasColumnName("AreaName");
            modelBuilder.Entity<Area>().Property(a => a.Active).HasColumnName("AreaActive");

            //Consumer
            //Common.ConsumersLookup
            if (company == "DSN")
            {
                modelBuilder.Entity<Consumer>().Map(c => { c.ToTable("Common.ConsumersLookups"); });
            }
            else
            {
                modelBuilder.Entity<Consumer>().Map(c => { c.ToTable("Common.ConsumersLookup"); });
            }

            modelBuilder.Entity<Consumer>().Property(c => c.Id).HasColumnName("PartyId");
            modelBuilder.Entity<Consumer>().Property(c => c.Name).HasColumnName("ConsumerName");

            //DepartmentSupervisorEmail
            //Common.DepartmentSupervisorEmailLookup
            modelBuilder.Entity<DepartmentSupervisorEmail>().Map(c => { c.ToTable("Common.DepartmentSupervisorEmailLookup"); });

            //ReportRecipients
            //Maintenance.ReportRecipient
            modelBuilder.Entity<ReportRecipient>().Map(m => { m.ToTable("Maintenance.ReportRecipient"); });
            modelBuilder.Entity<ReportRecipient>().Property(a => a.Emails).HasColumnName("EmailAddresses");

            //AlertRecipients
            //Maintenance.AlertRecipient
            modelBuilder.Entity<AlertRecipient>().Map(m => { m.ToTable("Maintenance.AlertRecipient"); });
            modelBuilder.Entity<AlertRecipient>().Property(a => a.Emails).HasColumnName("EmailAddresses");


            base.OnModelCreating(modelBuilder);
        }
    }
}