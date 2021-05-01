using Simplic.Document;
using Simplic.Framework.DBUI;
using Simplic.Localization;
using Simplic.Session;
using System;
using System.Collections.Generic;
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
            Checkout(parameter);

            if (parameter.SelectedRows.Count == 0)
            {
                return GridInvokeMethodResult.NoGridRefresh();
            }

            var itemBox = (AsyncGridItemBox)ItemBoxManager.GetItemBoxFromDB("IB_Document_Workflow_User");
            itemBox.SetPlaceholder("WorkflowId", parameter.GetSelectedRowsAsDataRow().FirstOrDefault()["WorkflowId"].ToString());
            itemBox.ShowDialog();
            int targetUserId = 0;
            Guid? workflowOrganzisationId = null;

            if (itemBox.SelectedItem == null)
                return new GridInvokeMethodResult { RefreshGrid = false };

            if ((string)itemBox.GetSelectedItemCell("Type") == "Benutzer")
                targetUserId = (int)itemBox.GetSelectedItemCell("Ident");
            else
                workflowOrganzisationId = (Guid)itemBox.GetSelectedItemCell("Guid");

            var comment = new Framework.Extension.InstanceDataCommentModel
            {
                UserGroupVisibility = Visibility.Hidden,
                UserId = sessionService.CurrentSession.UserId,
                InstanceDataGuid = Guid.NewGuid(),
                StackGuid = Guid.Parse("12c9b95b-bd33-4fa0-9ca1-05e11122018c") // TODO: Remove magic string
            };

            var commentWindow = new Framework.Extension.NewCommentWindow(comment);
            commentWindow.ShowDialog();

            foreach (var row in parameter.GetSelectedRowsAsDataRow())
            {
                var documentId = (Guid)row["Guid"];
                var documentPathId = (Guid)row["DocumentPathId"];
                // The grid needs a the column workflow id 
                var workflowId = (Guid)fileStructureDocumentPathService.Get(documentPathId).WorkflowId;

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

                if (itemBox.GetSelectedItemCell("Type").ToString() == "Gruppe")
                {
                    workflowOperation.OperationType = WorkflowOperationType.WorkflowOrganizationUnit;
                    workflowOperation.WorkflowOrganizationId = workflowOrganzisationId;
                }
                try
                {
                    workflowOperationService.ForwardTo(workflowOperation);
                }
                catch (DocumentWorkflowException ex)
                {
                    Log.LogManagerInstance.Instance.Error("Could not forward document in workflow", ex);

                    // TODO: Add localization
                    MessageBox.Show("Workflow für Zielbenutzer nicht gefunden.", "Workflow nicht gefunden", MessageBoxButton.OK, MessageBoxImage.Information);
                    return GridInvokeMethodResult.NoGridRefresh();
                }

                if (!string.IsNullOrWhiteSpace(comment.Comment))
                {
                    comment.InstanceDataGuid = documentId;
                    comment.CommentId = Guid.NewGuid();

                    Framework.Extension.InstanceDataComment.Singleton.Create(comment);
                }
            }

            return new GridInvokeMethodResult { RefreshGrid = true };


        }


        /// <summary>
        /// Forwards a copy to the user that will be shown in the itembox
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static GridInvokeMethodResult ForwardCopyTo(GridFunctionParameter parameter)
        {
            Checkout(parameter);

            Guid? workflowOrganizationId = null;
            int targetUserId = 0;

            if (parameter.SelectedRows.Count == 0)
            {
                return GridInvokeMethodResult.NoGridRefresh();
            }

            var itemBox = (AsyncGridItemBox)ItemBoxManager.GetItemBoxFromDB("IB_Document_Workflow_User");
            itemBox.SetPlaceholder("WorkflowId", parameter.GetSelectedRowsAsDataRow().FirstOrDefault()["WorkflowId"].ToString());
            itemBox.ShowDialog();

            if (itemBox.SelectedItem == null)
                return new GridInvokeMethodResult { RefreshGrid = false };



            var comment = new Framework.Extension.InstanceDataCommentModel
            {
                UserGroupVisibility = Visibility.Hidden,
                UserId = sessionService.CurrentSession.UserId,
                InstanceDataGuid = Guid.NewGuid(),
                StackGuid = Guid.Parse("12c9b95b-bd33-4fa0-9ca1-05e11122018c") // TODO: Remove magic string
            };

            var commentWindow = new Framework.Extension.NewCommentWindow(comment);
            commentWindow.ShowDialog();

            if (itemBox.GetSelectedItemCell("Type").ToString() == "Benutzer")
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
                if (itemBox.GetSelectedItemCell("Type").ToString() == "Gruppe")
                {
                    workflowOperation.OperationType = WorkflowOperationType.WorkflowOrganizationUnit;
                    workflowOperation.WorkflowOrganizationId = workflowOrganizationId;
                }

                try
                {
                    workflowOperationService.ForwardCopyTo(workflowOperation);
                }
                catch (DocumentWorkflowException ex)
                {
                    Log.LogManagerInstance.Instance.Error("Could not forward document in workflow", ex);

                    // TODO: Add localization
                    MessageBox.Show("Workflow für Zielbenutzer nicht gefunden.", "Workflow nicht gefunden", MessageBoxButton.OK, MessageBoxImage.Information);
                    return GridInvokeMethodResult.NoGridRefresh();
                }

                if (!string.IsNullOrWhiteSpace(comment.Comment))
                {
                    comment.InstanceDataGuid = documentId;
                    comment.CommentId = Guid.NewGuid();

                    Framework.Extension.InstanceDataComment.Singleton.Create(comment);
                }
            }

            return new GridInvokeMethodResult { RefreshGrid = true };
        }

        private static AsyncItemBox ShowWorkflowUser()
        {
            var itembox = ItemBoxManager.GetItemBoxFromDB("IB_Document_Workflow_User");
            return itembox;
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

        #endregion
    }
}
