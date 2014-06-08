using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mantle.Configuration.Source.SqlServer.Models.Mapping
{
    public class ModuleMap : EntityTypeConfiguration<Module>
    {
        public ModuleMap()
        {
            // Primary Key
            this.HasKey(t => t.ModuleId);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Description)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Module");
            this.Property(t => t.ModuleId).HasColumnName("ModuleId");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");

            // Relationships
            this.HasMany(t => t.PropertyValues)
                .WithMany(t => t.Modules)
                .Map(m =>
                    {
                        m.ToTable("ModuleProperty");
                        m.MapLeftKey("ModuleId");
                        m.MapRightKey("ValueId");
                    });

            this.HasRequired(t => t.Group)
                .WithMany(t => t.Modules)
                .HasForeignKey(d => d.GroupId);

        }
    }
}
