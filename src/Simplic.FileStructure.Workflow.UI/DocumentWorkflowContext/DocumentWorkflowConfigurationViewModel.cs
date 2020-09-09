using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Framework.UI;
using Simplic.Studio.UI;
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
		#endregion

		/// <summary>
		/// Gets or sets the model
		/// </summary>
		public DocumentWorkflowConfiguration Model { get; set; }


		/// <summary>
		/// Initialize the to pass the model 
		/// </summary>
		/// <param name="model"></param>
		public void Initialize(DocumentWorkflowConfiguration model)
		{
			if (model == null)
			{
				model = new DocumentWorkflowConfiguration();
			}
			Model = model;

			var providerList = CommonServiceLocator.ServiceLocator.Current.GetAllInstances<IDocumentWorkflowStateProvider>();

			foreach (var provider in providerList)
				stateProviders.Add(provider.Name);

			RaisePropertyChanged(nameof(StateProviders));
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


	}

}
