using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represent the workflow app settings
    /// </summary>
    public class DocumentWorkflowAppSettings
    {
        /// <summary>
        /// Gets or sets the guid
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the internal name
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        /// Gets or sets the public name
        /// </summary>
        public string PublicName { get; set; }
    }
}
