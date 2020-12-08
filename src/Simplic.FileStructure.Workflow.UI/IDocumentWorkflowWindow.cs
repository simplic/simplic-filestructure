using Simplic.FileStructure.Workflow;
using Simplic.FileStructure.Workflow.UI.ViewModel;
using Simplic.Framework.UI.Info;
using Simplic.Studio.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.UI
{
    public interface IDocumentWorkflowAppSettingsWindow : IWindow<Guid, DocumentWorkflowAppSettings, DocumentWorkflowAppSettingsViewModel>
    {
        /// <summary>
        /// Gets or sets the actual db intern page instance 
        /// </summary>
        DBInternPage DBInternPage { get; set; }
    }
}
