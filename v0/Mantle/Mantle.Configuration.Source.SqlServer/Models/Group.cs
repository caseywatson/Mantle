using System;
using System.Collections.Generic;

namespace Mantle.Configuration.Source.SqlServer.Models
{
    public partial class Group
    {
        public Group()
        {
            this.Modules = new List<Module>();
        }

        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
    }
}
