using System;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Simplic.Framework.UI;
using Simplic.Studio.UI;
using Simplic.UI.MVC;
using Telerik.Windows.Controls;

namespace Simplic.FileStructure.Workflow.UI
{
    public class WorkflowOrganizationUnitViewModel : ExtendableViewModel, IWindowViewModel<WorkflowOrganizationUnit>
    {


        public void Initialize(WorkflowOrganizationUnit model)
        {

            if (model == null)
            {
                model = new WorkflowOrganizationUnit();
            }
            Model = model;
        }

       
        /// <summary>
        /// Gets or sets the internal name
        /// </summary>
        public string InternalName
		{
			get => Model.InternalName;
			set => PropertySetter(value, newValue =>
			{
				Model.InternalName = newValue;
			});
		}

		/// <summary>
		/// Gets or sets the display name
		/// </summary>
		public string DisplayName
		{
			get => Model.DisplayName;
			set => PropertySetter(value, newValue =>
			{
				Model.DisplayName = newValue;
			});
		}

		/// <summary>
		/// Gets or sets the model 
		/// </summary>
        public WorkflowOrganizationUnit Model { get; set; }


    }
}
