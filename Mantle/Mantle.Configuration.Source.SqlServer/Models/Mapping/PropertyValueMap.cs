using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mantle.Configuration.Source.SqlServer.Models.Mapping
{
    public class PropertyValueMap : EntityTypeConfiguration<PropertyValue>
    {
        public PropertyValueMap()
        {
            // Primary Key
            this.HasKey(t => t.ValueId);

            // Properties
            this.Property(t => t.Value)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("PropertyValue");
            this.Property(t => t.ValueId).HasColumnName("ValueId");
            this.Property(t => t.PropertyId).HasColumnName("PropertyId");
            this.Property(t => t.Value).HasColumnName("Value");

            // Relationships
            this.HasRequired(t => t.Property)
                .WithMany(t => t.PropertyValues)
                .HasForeignKey(d => d.PropertyId);

        }
    }
}
