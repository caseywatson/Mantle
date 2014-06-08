using System;
using System.Collections.Generic;

namespace Mantle.Configuration.Source.SqlServer.Models
{
    public partial class Module
    {
        public Module()
        {
            this.PropertyValues = new List<PropertyValue>();
        }

        public int ModuleId { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<PropertyValue> PropertyValues { get; set; }
    }
}
