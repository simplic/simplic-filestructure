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
using Simplic.User;
using Telerik.Windows.Controls;
using Telerik.Windows.Core;
using Telerik.Windows.Data;
using Telerik.Windows.Persistence.Services;

namespace Simplic.FileStructure.Workflow.UI
{
    /// <summary>
    /// View model primarly used by DocumentWorkflowContext window
    /// </summary>
    public class DocumentWorkflowConfigurationViewModel : ExtendableViewModel, IWindowViewModel<DocumentWorkflowConfiguration>
    {
        #region [Fields]
        private ObservableCollection<string> stateProviders = new ObservableCollection<string>();
        private ObservableCollection<string> accessProviders = new ObservableCollection<string>();
        private ObservableCollection<SelectWorkflowOrganizationUnitViewModel> workflowOrganizationUnits;
        private IWorkflowOrganizationUnitService workflowOrganizationUnitService;
        private ICommand addTab;
        private ObservableCollection<WorkflowOrganizationUnitAssignmentViewModel> assignments = new ObservableCollection<WorkflowOrganizationUnitAssignmentViewModel>();
        private SelectWorkflowOrganizationUnitViewModel workflowOrganzitationUnit;
        private IList<User.User> users = new List<User.User>();
        private IUserService userService;
        private WorkflowOrganizationUnitAssignmentViewModel selectedAssignment;
        #endregion

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
            var stateProviderList = CommonServiceLocator.ServiceLocator.Current.GetAllInstances<IDocumentWorkflowStateProvider>();
            var accessProviderList = CommonServiceLocator.ServiceLocator.Current.GetAllInstances<IDocumentWorkflowAccessProvider>();
            userService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IUserService>();

            foreach (var provider in stateProviderList)
                stateProviders.Add(provider.Name);

            RaisePropertyChanged(nameof(StateProviders));

            foreach (var provider in accessProviderList)
                accessProviders.Add(provider.Name);

            RaisePropertyChanged(nameof(AccessProviders));

            workflowOrganizationUnits = new ObservableCollection<SelectWorkflowOrganizationUnitViewModel>(workflowOrganizationUnitService.GetAll().Select(x => new SelectWorkflowOrganizationUnitViewModel(x) { Parent = this }).ToList());
            assignments = new ObservableCollection<WorkflowOrganizationUnitAssignmentViewModel>(Model.OrganizationUnits.GetAsObservableCollection().Select(y => new WorkflowOrganizationUnitAssignmentViewModel(y, this) { }));
            CollectionNotifyPropertyChanged(assignments, model.OrganizationUnits, x => x.Model);
            addTab = new RelayCommand(o => Add());

            if (assignments.Any())
                selectedAssignment = assignments.FirstOrDefault();

        }

        /// <summary>
        /// Adds a new tab with  
        /// </summary>
        private void Add()
        {
            if (workflowOrganzitationUnit == null)
                return;

            //Assignment is not initialized
            if (assignments == null)
                assignments = new ObservableCollection<WorkflowOrganizationUnitAssignmentViewModel>();

            //Assignment has already the wou inside
            if (assignments.Any(x => x.Model.WorkflowOrganisationUnitId == workflowOrganzitationUnit.Guid))
                return;


            var workflow = new WorkflowOrganizationUnitAssignmentViewModel(new WorkflowOrganizationUnitAssignment { WorkflowOrganisationUnitId = workflowOrganzitationUnit.Guid }, this)
            {
                Header = workflowOrganzitationUnit?.Name,
            };

            if (string.IsNullOrWhiteSpace(workflow.Header))
                return;

            assignments.Add(workflow);
            selectedAssignment = workflow;

            RaisePropertyChanged(nameof(Tabs));
            RaisePropertyChanged(nameof(SelectedAssignment));
        }

        /// <summary>
        /// Saves the assignments
        /// </summary>
        public void PrepareSaving()
        {
            var workflowConfigurationGuid = Model.Guid;

            foreach (var tab in assignments)
            {
                var organizationUnitAssignment = new WorkflowOrganizationUnitAssignment
                {
                    WorkflowId = workflowConfigurationGuid,
                    WorkflowOrganisationUnitId = tab.Model.Guid
                };

                //Assigns and saves the user to a worklfow organization unit 
                foreach (var userViewModel in tab.AssignedUsers)
                {
                    var userAssignment = new WorkflowOrganizationUnitUserAssignment { };
                    userAssignment.UserId = (int)userViewModel.Id;
                    userAssignment.WorkflowOrganzitionAssignmentId = tab.Model.Guid;
                }
            }
        }

        /// <summary>
        /// Gets or sets the model
        /// </summary>
        public DocumentWorkflowConfiguration Model { get; set; }

        /// <summary>
        /// Gets a list of all users and filter them by their active state ( User -> IsActive )
        /// </summary>
        public IList<User.User> Users
        {
            get
            {
                if (users == null || !users.Any())
                    users = userService.GetAll().ToList();
                return users.Where(u => u.IsActive).ToList();
            }
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
        /// Gets the state providers 
        /// </summary>
        public ObservableCollection<string> StateProviders => stateProviders;

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
        /// Gets a list of access providers
        /// </summary>
        public ObservableCollection<string> AccessProviders => accessProviders;

        /// <summary>
        /// Gets or sets the selected access provider
        /// </summary>
        public string SelectedAccessProvider
        {
            get => Model.AccessProviderName;
            set => PropertySetter(value, newValue =>
            {
                Model.AccessProviderName = newValue;
            });
        }

        /// <summary>
        /// Gest or sets the workflow organization unit that is selected 
        /// </summary>
        public SelectWorkflowOrganizationUnitViewModel WorkflowOrganizationUnit { get => workflowOrganzitationUnit; set => workflowOrganzitationUnit = value; }

        /// <summary>
        /// Gets or sets the workflow organizations units that are available
        /// </summary>
        public ObservableCollection<SelectWorkflowOrganizationUnitViewModel> WorkflowOrganizationUnits
        {
            get => workflowOrganizationUnits;
            set => workflowOrganizationUnits = value;
        }

        /// <summary>
        /// Gets or sets the command to add a tab
        /// </summary>
        public ICommand AddTab { get => addTab; set => addTab = value; }

        /// <summary>
        /// Gets or sets a list of <see cref="WorkflowOrganizationUnitAssignmentViewModel"/> 
        /// </summary>
        public ObservableCollection<WorkflowOrganizationUnitAssignmentViewModel> Tabs { get => assignments; set => assignments = value; }

        /// <summary>
        /// Gets or sets the selected assignment
        /// </summary>
        public WorkflowOrganizationUnitAssignmentViewModel SelectedAssignment { get => selectedAssignment; set => selectedAssignment = value; }

        /// <summary>
        /// Removes the <see cref="WorkflowOrganizationUnitAssignmentViewModel"/> 
        /// </summary>
        /// <param name="workflowOrganizationUnitAssignmentViewModel"></param>
        public void RemoveItem(WorkflowOrganizationUnitAssignmentViewModel workflowOrganizationUnitAssignmentViewModel)
        {
            var result = LocalizedMessageBox.Show("dwc_caption", "dwc_sure_delete", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Information);
            if (result == System.Windows.MessageBoxResult.Yes)
                if (assignments.Contains(workflowOrganizationUnitAssignmentViewModel))
                    assignments.Remove(workflowOrganizationUnitAssignmentViewModel);
        }
    }

}
