using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Simplic.Framework.UI;
using Simplic.UI.MVC;
using Simplic.User;

namespace Simplic.FileStructure.Workflow.UI
{
    /// <summary>
    /// Represents the workflow organization unit assignment view model
    /// </summary>
    public class WorkflowOrganizationUnitAssignmentViewModel : ExtendableViewModel
    {
        private IUserService userService;
        private ObservableCollection<UserViewModel> allUsers = new ObservableCollection<UserViewModel>();
        private ObservableCollection<UserViewModel> assignedUsers = new ObservableCollection<UserViewModel>();
        private IWorkflowOrganizationUnitService workflowOrganizationUnitService;
        
        private string allUserSearchText = "";
        private string assignedUserSearchText = "";

        private CollectionViewSource userSource;

        /// <summary>
        /// Constructor to get all user and initialize the services
        /// </summary>
        public WorkflowOrganizationUnitAssignmentViewModel(WorkflowOrganizationUnitAssignment workflowOrganizationUnit, IViewModelBase parent)
        {
            Parent = parent;
            Model = workflowOrganizationUnit;
            userService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IUserService>();
            workflowOrganizationUnitService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IWorkflowOrganizationUnitService>();

            if (Parent is DocumentWorkflowConfigurationViewModel viewModel)
                Header = workflowOrganizationUnitService.Get(workflowOrganizationUnit.WorkflowOrganisationUnitId).DisplayName;

            //Todo:Expect the user that are already loaded in
            allUsers = new ObservableCollection<UserViewModel>((Parent as DocumentWorkflowConfigurationViewModel).Users.Select(x => new UserViewModel(x, new WorkflowOrganizationUnitUserAssignment(), this) { }));
            assignedUsers = new ObservableCollection<UserViewModel>(Model.Users.GetAsObservableCollection().Select(x => new UserViewModel(userService.GetById(x.UserId), x,this)));
            var list = workflowOrganizationUnit.Users.GetItems();
           
            //TODO: Improve because of performance issues, maybe?
            allUsers = new ObservableCollection<UserViewModel>(allUsers.Where(allUser => !assignedUsers.Any(assignedUser => assignedUser.Model.UserId == allUser.Model.UserId)) );

            userSource = new CollectionViewSource();
            userSource.Source = AllUsers;

            userSource.Filter += UserSource_Filter;
            userSource.SortDescriptions.Add(new System.ComponentModel.SortDescription { Direction = System.ComponentModel.ListSortDirection.Ascending, PropertyName = "Name" });

            CollectionNotifyPropertyChanged(assignedUsers, workflowOrganizationUnit.Users, (x) => x.Model);
            RaisePropertyChanged(nameof(AllUsers));
        }

        private void UserSource_Filter(object sender, FilterEventArgs e)
        {
            if (e.Item is UserViewModel user && user.Name != null)
                e.Accepted = user.Name.ToLower().Contains(allUserSearchText.ToLower());
        }

        /// <summary>
        /// Gets or sets the header which is basically user for the tab item header binding
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the workflow organization unit
        /// </summary>
        public WorkflowOrganizationUnitAssignment Model { get; private set; }

        /// <summary>
        /// Gets or sets the list of all user the system has in storage
        /// </summary>
        public ObservableCollection<UserViewModel> AllUsers { get => allUsers; set => allUsers = value; }

        public string AllUserSearchText 
        {
            get => allUserSearchText;
            set => PropertySetter(value, (x) =>
            {
                allUserSearchText = value; 
                userSource.View.Refresh();
            });
        }

        /// <summary>
        /// Gest or sets the assigned users that must be assigned to the organization unit 
        /// </summary>
        public ObservableCollection<UserViewModel> AssignedUsers { get => assignedUsers; set => assignedUsers = value; }

        public CollectionViewSource UserSource { get => userSource; set => userSource = value; }
    }
}
