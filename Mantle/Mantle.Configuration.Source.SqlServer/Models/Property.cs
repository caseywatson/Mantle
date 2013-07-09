using System;
using System.Collections.Generic;

namespace Mantle.Configuration.Source.SqlServer.Models
{
    public partial class Property
    {
        public Property()
        {
            this.PropertyValues = new List<PropertyValue>();
        }

        public int PropertyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<PropertyValue> PropertyValues { get; set; }
    }
}
