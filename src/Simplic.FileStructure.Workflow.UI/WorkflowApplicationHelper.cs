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

                var workflowOperation = new WorkflowOperation
                {
                    DocumentId = documentId,
                    DocumentPath = documentPathId,
                    UserId = sessionService.CurrentSession.UserId,
                    TargetUserId = targetUserId,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now,
                    ActionName = "forward",
                    InternalWorkflowName = itemBox.GetSelectedItemCell("InternalName").ToString(),
                    Guid = Guid.NewGuid()                    
                };

                workflowOperationService.ForwardTo(workflowOperation);

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

                var workflowOperation = new WorkflowOperation
                {
                    DocumentId = documentId,
                    DocumentPath = documentPathId,
                    UserId = sessionService.CurrentSession.UserId,
                    TargetUserId = targetUserId,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now,
                    ActionName = "forward",
                    InternalWorkflowName = itemBox.GetSelectedItemCell("InternalName").ToString(),
                    Guid = Guid.NewGuid()
                };

                workflowOperationService.ForwardCopyTo(workflowOperation);


                if (!string.IsNullOrWhiteSpace(comment.Comment))
                {
                    comment.InstanceDataGuid = documentId;
                    comment.CommentId = Guid.NewGuid();

                    Framework.Extension.InstanceDataComment.Singleton.Create(comment);
                }
            }

            return new GridInvokeMethodResult { RefreshGrid = true };
        }
        public static GridInvokeMethodResult Complete(GridFunctionParameter parameter)
        {

            foreach (var row in parameter.GetSelectedRowsAsDataRow())
            {
                var documentId = (Guid)row["Guid"];
                var documentPathId = (Guid)row["DocumentPathId"];

                var workflowOperation = new WorkflowOperation
                {
                    DocumentId = documentId,
                    DocumentPath = documentPathId,
                    UserId = sessionService.CurrentSession.UserId,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now,
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
