using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Framework.UI;
using Simplic.UI.MVC;

namespace Simplic.FileStructure.Workflow.UI
{
    /// <summary>
    /// View model for the user
    /// </summary>
    public class UserViewModel : ExtendableViewModel, IEquatable<UserViewModel>
    {
        private readonly User.User user;

        /// <summary>
        /// Constructor to pass the user
        /// </summary>
        /// <param name="user"></param>
        public UserViewModel(User.User user, WorkflowOrganizationUnitUserAssignment workflowOrganizationUnitUserAssignment, IViewModelBase parent)
        {
            this.Parent = parent;
            Model = workflowOrganizationUnitUserAssignment;
            this.user = user;
            if (Model.WorkflowOrganzitionAssignmentId == Guid.Empty)
                Initialize();
        }


        private void Initialize()
        {
            if (Parent is WorkflowOrganizationUnitAssignmentViewModel viewModel)
            {
                Model.WorkflowOrganzitionAssignmentId = viewModel.Model.Guid;
                Model.UserId = user.Ident;
            }
        }

        public bool Equals(UserViewModel other)
        {
            if (other is null)
                return false;

            return this.Model.UserId == other.Model.UserId;
        }

        public WorkflowOrganizationUnitUserAssignment Model { get; private set; }

        public User.User User => user;

        /// <summary>
        /// Gets the id
        /// </summary>
        public long Id => user.Ident;

        /// <summary>
        /// Gets the name
        /// </summary>
        public string Name => user.UserName;
    }
}
