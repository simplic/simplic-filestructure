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


        #region [ContextMenuStuff]
        /// <summary>
        /// Forwards the document to all user who installed the Workflow
        /// </summary>
        /// <param name="parameter">Grid parameter</param>
        /// <returns>Grid invoke result, to control grid refresh</returns>
        public static GridInvokeMethodResult ForwardTo(GridFunctionParameter parameter)
        {
            var itemBox = ItemBoxManager.GetItemBoxFromDB("IB_Document_Workflow_User");
            itemBox.ShowDialog();

            if (itemBox.SelectedItem == null)
                return new GridInvokeMethodResult { RefreshGrid = false };

            var targetUserId = (int)itemBox.GetSelectedItemCell("Ident");

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
                    Guid = Guid.NewGuid()
                };

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

        public static GridInvokeMethodResult ForwardCopyTo(GridFunctionParameter parameter)
        {

            var itemBox = ShowWorkflowUser();

            itemBox.ShowDialog();

            if (itemBox.SelectedItem == null)
                return new GridInvokeMethodResult { RefreshGrid = false };

            var targetUserId = (int)itemBox.GetSelectedItemCell("Ident");

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

        public static GridInvokeMethodResult Complete(GridFunctionParameter parameter)
        {

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
        #endregion
    }
}
