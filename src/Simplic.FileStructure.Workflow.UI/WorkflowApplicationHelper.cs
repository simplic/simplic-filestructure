using Simplic.Configuration;
using Simplic.Document;
using Simplic.Framework.DBUI;
using Simplic.Localization;
using Simplic.Session;
using Simplic.Studio.UI;
using Simplic.UI.Control;
using Simplic.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simplic.FileStructure.Workflow.UI
{
    public class WorkflowApplicationHelper
    {
        private static IFileStructureService fileStructureService;
        private static IFileStructureDocumentPathService fileStructureDocumentPathService;
        private static ILocalizationService localizationService;
        private static IWorkflowOperationService workflowOperationService;
        private static ISessionService sessionService;
        private static IDocumentWorkflowOrganizationUnitAssignmentService documentWorkflowOrganizationUnitAssignmentService;
        private static IUserService userService;
        private static IConfigurationService configurationService;
        private static int forwardConfig;


        /// <summary>
        /// Initialize helper
        /// </summary>
        static WorkflowApplicationHelper()
        {
            fileStructureService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureService>();
            localizationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
            workflowOperationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IWorkflowOperationService>();
            fileStructureDocumentPathService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureDocumentPathService>();
            sessionService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ISessionService>();
            documentWorkflowOrganizationUnitAssignmentService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDocumentWorkflowOrganizationUnitAssignmentService>();
            userService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IUserService>();
            configurationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IConfigurationService>();
            forwardConfig = configurationService.GetValue<int>("ForwardOption", "Filestructure", "");
        }

        /// <summary>
        /// Opens a window to create a workflow and assign settings to it
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static GridInvokeMethodResult NewWorkflow(GridFunctionParameter parameter)
        {
            return new GridInvokeMethodResult { RefreshGrid = true };
        }

        /// <summary>
        /// Edits the workflow
        /// </summary>
        /// <param name="parameter">the </param>
        /// <returns></returns>
        public static GridInvokeMethodResult EditWorkflow(GridFunctionParameter parameter)
        {
            return new GridInvokeMethodResult { RefreshGrid = true };
        }

        /// <summary>
        /// Opens a window to create a workflow organization unit and assign settings to it
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static GridInvokeMethodResult NewWorkflowOrganizaitonUnit(GridFunctionParameter parameter)
        {
            return new GridInvokeMethodResult { RefreshGrid = true };
        }

        /// <summary>
        /// Edits the workflow organization unit
        /// </summary>
        /// <param name="parameter">the </param>
        /// <returns></returns>
        public static GridInvokeMethodResult EditWorkflowOrganizaitonUnit(GridFunctionParameter parameter)
        {
            return new GridInvokeMethodResult { RefreshGrid = true };
        }


        #region [ContextMenuStuff]
        /// <summary>
        /// Forwards the document to all user who installed the Workflow
        /// </summary>
        /// <param name="parameter">Grid parameter</param>
        /// <returns>Grid invoke result, to control grid refresh</returns>
        public static GridInvokeMethodResult ForwardTo(GridFunctionParameter parameter)
        {
            IList<WorkflowOperation> workflowOperationList;
            if (forwardConfig == 1) 
                workflowOperationList = WorkflowOperationsItemBoxGet(parameter);
            
            else
                workflowOperationList = WorkflowOperationsGet(parameter);

            if (workflowOperationList == null)
                return GridInvokeMethodResult.NoGridRefresh();

            foreach (var workflowOperation in workflowOperationList)
            {
                try
                {
                    workflowOperationService.ForwardTo(workflowOperation);
                }
                catch (DocumentWorkflowException ex)
                {
                    Log.LogManagerInstance.Instance.Error("Could not forward document in workflow", ex);

                    MessageBox.Show("filestructure_forward_error", "filestructure_forward_error_head", MessageBoxButton.OK, MessageBoxImage.Information);
                    return GridInvokeMethodResult.NoGridRefresh();
                }
            }
            return new GridInvokeMethodResult { RefreshGrid = true };
        }

        /// <summary>
        /// Forwards a copy to the user that will be shown in the itembox or multicolumncombobox.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static GridInvokeMethodResult ForwardCopyTo(GridFunctionParameter parameter)
        {
            IList<WorkflowOperation> workflowOperationList;
            if (forwardConfig == 1)
                workflowOperationList = WorkflowOperationsItemBoxGet(parameter);

            else
                workflowOperationList = WorkflowOperationsGet(parameter);

            if (workflowOperationList == null)
                return GridInvokeMethodResult.NoGridRefresh();

            foreach (var workflowOperation in workflowOperationList)
            {
                try
                {
                    workflowOperationService.ForwardCopyTo(workflowOperation);
                }
                catch (DocumentWorkflowException ex)
                {
                    Log.LogManagerInstance.Instance.Error("Could not forward document in workflow", ex);

                    // TODO: Add localization
                    LocalizedMessageBox.Show("textkey", "captionKey", MessageBoxButton.OK, MessageBoxImage.Information);
                    MessageBox.Show("filestructure_forward_error", "filestructure_forward_error_head", MessageBoxButton.OK, MessageBoxImage.Information);
                    return GridInvokeMethodResult.NoGridRefresh();
                }
            }
            return new GridInvokeMethodResult { RefreshGrid = true };
        }

        private static IList<WorkflowOperation> WorkflowOperationsGet(GridFunctionParameter parameter)
        {
            IList<WorkflowOperation> workflowOperations = new List<WorkflowOperation>();
            

            if (parameter.SelectedRows.Count == 0)
                return null;

            Dictionary<string, string> dictParams = new Dictionary<string, string>();
            dictParams.Add("[WorkflowId]", parameter.GetSelectedRowsAsDataRow().FirstOrDefault()["WorkflowId"].ToString());

            var win = new ForwardWindow(dictParams);
            win.ShowDialog();

            var windowDataContext = win.DataContext;
            ObservableCollection<IMultiSelectionComboBoxItem> itemList = null;
            var commentText = "";

            if (!win.IsSave)
                return null;

            if (windowDataContext is ForwardViewModel forwardViewModel)
            {
                itemList = forwardViewModel.MultiItemboxItems;
                commentText = forwardViewModel.CommentText;
            }

            if (itemList == null)
                return null;

            foreach (var item in itemList)
            {
                var user = userService.GetByGuid((Guid)item.Id);

                int targetUserId = 0;
                Guid? workflowOrganzisationId = null;

                if (user != null)
                    targetUserId = user.Ident;

                else
                    workflowOrganzisationId = (Guid?)item.Id;

                var comment = new Framework.Extension.InstanceDataCommentModel
                {
                    Comment = commentText,
                    UserGroupVisibility = Visibility.Hidden,
                    UserId = sessionService.CurrentSession.UserId,
                    InstanceDataGuid = Guid.NewGuid(),
                    StackGuid = (Guid)parameter.GridView.Configuration.SelectedStackId
                };

                foreach (var row in parameter.GetSelectedRowsAsDataRow())
                {
                    var documentId = (Guid)row["Guid"];
                    var documentPathId = (Guid)row["DocumentPathId"];
                    // The grid needs a the column workflow id 
                    var workflowId = (Guid)fileStructureDocumentPathService.Get(documentPathId).WorkflowId;

                    if (workflowId == null)
                        continue;

                    var workflowOperation = new WorkflowOperation
                    {
                        DocumentId = documentId,
                        DocumentPath = documentPathId,
                        UserId = sessionService.CurrentSession.UserId,
                        TargetUserId = targetUserId,
                        CreateDateTime = DateTime.Now,
                        UpdateDateTime = DateTime.Now,
                        ActionName = "forward",
                        WorkflowId = workflowId,
                        Guid = Guid.NewGuid(),
                    };

                    if (user == null)
                    {
                        workflowOperation.OperationType = WorkflowOperationType.WorkflowOrganizationUnit;
                        workflowOperation.WorkflowOrganizationId = workflowOrganzisationId;
                    }

                    workflowOperations.Add(workflowOperation);

                    if (!string.IsNullOrWhiteSpace(comment.Comment))
                    {
                        comment.InstanceDataGuid = documentId;
                        comment.CommentId = Guid.NewGuid();

                        Framework.Extension.InstanceDataComment.Singleton.Create(comment);
                    }
                }
            }
            Checkout(parameter);
            return workflowOperations;
        }

        private static IList<WorkflowOperation> WorkflowOperationsItemBoxGet(GridFunctionParameter parameter)
        {
            IList<WorkflowOperation> workflowOperations = new List<WorkflowOperation>();
            

            Guid? workflowOrganizationId = null;
            int targetUserId = 0;

            if (parameter.SelectedRows.Count == 0)
                return null;

            var itemBox = (AsyncGridItemBox)ItemBoxManager.GetItemBoxFromDB("IB_Document_Workflow_User");
            itemBox.SetPlaceholder("WorkflowId", parameter.GetSelectedRowsAsDataRow().FirstOrDefault()["WorkflowId"].ToString());
            itemBox.ShowDialog();

            if (itemBox.SelectedItem == null)
                return null;

            var comment = new Framework.Extension.InstanceDataCommentModel
            {
                UserGroupVisibility = Visibility.Hidden,
                UserId = sessionService.CurrentSession.UserId,
                InstanceDataGuid = Guid.NewGuid(),
                StackGuid = (Guid)parameter.GridView.Configuration.SelectedStackId
            };

            var commentWindow = new Framework.Extension.NewCommentWindow(comment);
            commentWindow.ShowDialog();

            if (itemBox.GetSelectedItemCell("InternalType").ToString() == "User")
                targetUserId = (int)itemBox.GetSelectedItemCell("Ident");
            else
                workflowOrganizationId = (Guid)itemBox.GetSelectedItemCell("Guid");

            foreach (var row in parameter.GetSelectedRowsAsDataRow())
            {
                var documentId = (Guid)row["Guid"];
                var documentPathId = (Guid)row["DocumentPathId"];
                var workflowId = (Guid)row["WorkflowId"];

                var workflowOperation = new WorkflowOperation
                {
                    DocumentId = documentId,
                    DocumentPath = documentPathId,
                    UserId = sessionService.CurrentSession.UserId,
                    TargetUserId = targetUserId,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now,
                    ActionName = "forward",
                    WorkflowId = workflowId,
                    Guid = Guid.NewGuid()
                };
                if (itemBox.GetSelectedItemCell("InternalType").ToString() == "Group")
                {
                    workflowOperation.OperationType = WorkflowOperationType.WorkflowOrganizationUnit;
                    workflowOperation.WorkflowOrganizationId = workflowOrganizationId;
                }
                workflowOperations.Add(workflowOperation);

                if (!string.IsNullOrWhiteSpace(comment.Comment))
                {
                    comment.InstanceDataGuid = documentId;
                    comment.CommentId = Guid.NewGuid();

                    Framework.Extension.InstanceDataComment.Singleton.Create(comment);
                }
            }
            Checkout(parameter);
            return workflowOperations;
        }

        /// <summary>
        /// Sets the state to complete 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static GridInvokeMethodResult Complete(GridFunctionParameter parameter)
        {
            Checkout(parameter);

            foreach (var row in parameter.GetSelectedRowsAsDataRow())
            {
                var documentId = (Guid)row["Guid"];
                var documentPathId = (Guid)row["DocumentPathId"];
                var workflowId = (Guid)row["WorkflowId"];

                var workflowOperation = new WorkflowOperation
                {
                    DocumentId = documentId,
                    DocumentPath = documentPathId,
                    UserId = sessionService.CurrentSession.UserId,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now,
                    WorkflowId = workflowId,
                    ActionName = "completed",
                    Guid = Guid.NewGuid()
                };

                workflowOperationService.Complete(workflowOperation);
            }

            return new GridInvokeMethodResult { RefreshGrid = true };
        }

        /// <summary>
        /// Sets the state to complete 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static GridInvokeMethodResult Release(GridFunctionParameter parameter)
        {
            Checkout(parameter);

            foreach (var row in parameter.GetSelectedRowsAsDataRow())
            {
                var documentId = (Guid)row["Guid"];
                var documentPathId = (Guid)row["DocumentPathId"];
                var workflowId = (Guid)row["WorkflowId"];

                var workflowOperation = new WorkflowOperation
                {
                    DocumentId = documentId,
                    DocumentPath = documentPathId,
                    UserId = sessionService.CurrentSession.UserId,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now,
                    WorkflowId = workflowId,
                    ActionName = "released",
                    Guid = Guid.NewGuid()
                };

                workflowOperationService.Release(workflowOperation);
            }

            return new GridInvokeMethodResult { RefreshGrid = true };
        }

        /// <summary>
        /// Shows  the tracking for the parameter which is a document
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static GridInvokeMethodResult ShowTracking(GridFunctionParameter parameter)
        {
            foreach (var row in parameter.GetSelectedRowsAsDataRow())
            {
                var documentId = (Guid)row["Guid"];
                var ib = (AsyncGridItemBox)ItemBoxManager.GetItemBoxFromDB($"IB_Document_Workflow_Tracking");
                ib.SetPlaceholder("DocumentId", documentId.ToString());
                ib.ShowDialog();
            }

            return new GridInvokeMethodResult { RefreshGrid = true };
        }

        /// <summary>
        /// Tries to checkout a document if required
        /// </summary>
        /// <param name="parameter">Grid parameter</param>
        private static void Checkout(GridFunctionParameter parameter)
        {
            foreach (var row in parameter.GetSelectedRowsAsDataRow())
            {
                if (Guid.TryParse(row["OrganizationId"]?.ToString(), out Guid organizationUnitId))
                {
                    var documentId = (Guid)row["Guid"];

                    // The grid needs a the column workflow id 
                    var workflowId = (Guid)row["WorkflowId"];
                    var directoryId = (Guid)row["DirectoryId"];

                    var workflowOperation = new WorkflowOperation
                    {
                        DocumentId = documentId,
                        UserId = sessionService.CurrentSession.UserId,
                        TargetUserId = sessionService.CurrentSession.UserId,
                        CreateDateTime = DateTime.Now,
                        UpdateDateTime = DateTime.Now,
                        ActionName = "forward",
                        WorkflowId = workflowId,
                        WorkflowOrganizationId = organizationUnitId,
                        DirectoryId = directoryId,
                        Guid = Guid.NewGuid()
                    };

                    var documentPath = workflowOperationService.DocumentCheckout(workflowOperation);
                    row["DocumentPathId"] = documentPath;
                }
            }
        }

        /// <summary>
        /// Checks the document out for the <see cref="WorkflowOrganizationUnit"/> and puts it in the user path
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static GridInvokeMethodResult DocumentCheckout(GridFunctionParameter parameter)
        {
            Checkout(parameter);

            return new GridInvokeMethodResult { RefreshGrid = true };
        }

        /// <summary>
        /// Forwards the document into the substitute folder
        /// </summary>
        /// <param name="gridFunctionParameter"></param>
        /// <returns></returns>
        public static GridInvokeMethodResult ForwardToSubstitute(GridFunctionParameter gridFunctionParameter)
        {
            var documentId = Guid.Empty;
            var pathService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureDocumentPathService>();

            foreach (var documentRow in gridFunctionParameter.GetSelectedRowsAsDataRow())
            {
                documentId = (Guid)documentRow["Guid"];

                if (documentId.Equals(Guid.Empty))
                    continue;

                var workflowId = (Guid)documentRow["WorkflowId"];
                var targetUserId = (int)documentRow["UserId"];
                var sessionService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ISessionService>();
                var currentUserId = sessionService.CurrentSession.UserId;
                var paths = pathService.GetByDocumentId(documentId);
                var pathId = paths.FirstOrDefault(x => x.WorkflowState != DocumentWorkflowStateType.Completed).Id;
                if (pathId == null)
                    continue;

                //The User is authorized to take the document
                var workflowOperationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IWorkflowOperationService>();

                workflowOperationService.ForwardTo(new WorkflowOperation
                {
                    DocumentId = documentId,
                    UserId = currentUserId,
                    TargetUserId = currentUserId,
                    ActionName = "Forward",
                    WorkflowId = workflowId,
                    DocumentPath = pathId,
                    OperationType = WorkflowOperationType.User,
                });
            }

            return new GridInvokeMethodResult()
            {
                RefreshGrid = true,
            };
        }

        #endregion
    }
}
