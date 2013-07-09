using System;
using System.Collections.Generic;

namespace Mantle.Configuration.Source.SqlServer.Models
{
    public partial class PropertyValue
    {
        public PropertyValue()
        {
            this.Modules = new List<Module>();
        }

        public int ValueId { get; set; }
        public int PropertyId { get; set; }
        public string Value { get; set; }
        public virtual Property Property { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
    }
}
