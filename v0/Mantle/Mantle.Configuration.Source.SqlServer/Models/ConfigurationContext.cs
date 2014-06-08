using System.Data.Entity;
using Mantle.Configuration.Source.SqlServer.Models.Mapping;

namespace Mantle.Configuration.Source.SqlServer.Models
{
    public class ConfigurationContext : DbContext
    {
        static ConfigurationContext()
        {
            Database.SetInitializer<ConfigurationContext>(null);
        }

        public ConfigurationContext(string connectionStringOrName)
            : base(connectionStringOrName)
        {
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyValue> PropertyValues { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GroupMap());
            modelBuilder.Configurations.Add(new ModuleMap());
            modelBuilder.Configurations.Add(new PropertyMap());
            modelBuilder.Configurations.Add(new PropertyValueMap());
        }
    }
}