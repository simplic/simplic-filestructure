using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Simplic.Framework.UI;
using Simplic.Studio.UI;
using Simplic.UI.MVC;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using Telerik.Windows.Persistence.Services;
using Unity;

namespace Simplic.FileStructure.Workflow.UI
{
	/// <summary>
	/// View model primarly used by DocumentWorkflowContext window
	/// </summary>
	public class DocumentWorkflowConfigurationViewModel : ExtendableViewModel, IWindowViewModel<DocumentWorkflowConfiguration>
	{
		#region [Fields]
		private ObservableCollection<string> stateProviders = new ObservableCollection<string>();
		private ObservableCollection<WorkflowOrganizationUnit> workflowOrganizationUnits;
		private IWorkflowOrganizationUnitService workflowOrganizationUnitService;
		private ICommand addTab;
		private ObservableCollection<UserViewModel> tabs;
		private WorkflowOrganizationUnit workflowOrganzitationUnit;
		#endregion

		/// <summary>
		/// Gets or sets the model
		/// </summary>
		public DocumentWorkflowConfiguration Model { get; set; }


		private void Add()
		{
			if (tabs == null)
				tabs = new ObservableCollection<UserViewModel>();

			tabs.Add(new UserViewModel
			{
				Header = workflowOrganzitationUnit.DisplayName
			});
			RaisePropertyChanged(nameof(Tabs));
		}


		/// <summary>
		/// Initialize the to pass the model 
		/// </summary>
		/// <param name="model"></param>
		public void Initialize(DocumentWorkflowConfiguration model)
		{
			this.workflowOrganizationUnitService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IWorkflowOrganizationUnitService>();
			if (model == null)
			{
				model = new DocumentWorkflowConfiguration();
			}
			Model = model;

			var providerList = CommonServiceLocator.ServiceLocator.Current.GetAllInstances<IDocumentWorkflowStateProvider>();

			foreach (var provider in providerList)
				stateProviders.Add(provider.Name);

			RaisePropertyChanged(nameof(StateProviders));
			workflowOrganizationUnits = new ObservableCollection<WorkflowOrganizationUnit>(workflowOrganizationUnitService.GetAll());
			

			addTab = new RelayCommand(o => Add());
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
		/// Gets or sets the state providers 
		/// </summary>
		public ObservableCollection<string> StateProviders
		{
			get => stateProviders;

		}

		/// <summary>
		/// Gets or sets the selected state provider
		/// </summary>
		public string SelectedStateProvider
		{
			get => Model.StateProviderName;
			set => PropertySetter(value, newValue =>
			{
				Model.StateProviderName = newValue;
			});
		}

		/// <summary>
		/// Gest or sets the workflow organization unit that is selected 
		/// </summary>
		public WorkflowOrganizationUnit WorkflowOrganizationUnit { get => workflowOrganzitationUnit; set => workflowOrganzitationUnit = value; }

		/// <summary>
		/// Gets or sets the workflow organizations units that are available
		/// </summary>
		public ObservableCollection<WorkflowOrganizationUnit> WorkflowOrganizationUnits
		{
			get => workflowOrganizationUnits;
			set => workflowOrganizationUnits = value;
		}

		/// <summary>
		/// Gets or sets the command to add a tab
		/// </summary>
		public ICommand AddTab { get => addTab; set => addTab = value; }

		public ObservableCollection<UserViewModel> Tabs { get => tabs; set => tabs = value; }

	}

}
