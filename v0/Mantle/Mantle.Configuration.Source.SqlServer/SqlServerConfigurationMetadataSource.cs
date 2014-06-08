using System;
using System.Collections.Generic;
using System.Linq;
using Mantle.Configuration.Source.SqlServer.Models;

namespace Mantle.Configuration.Source.SqlServer
{
    public class SqlServerConfigurationMetadataSource : IConfigurationMetadataSource
    {
        private readonly ConfigurationContext context;

        public SqlServerConfigurationMetadataSource(ConfigurationContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            this.context = context;
        }

        public IEnumerable<IConfigurationMetadata> LoadConfiguration(string groupName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentException("Group name is required.", "groupName");

            return context.Modules
                .Where(m => (m.Group.Name == groupName))
                .ToArray()
                .Select(m => m.ToModuleConfiguration());
        }
    }
}