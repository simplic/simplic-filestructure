﻿using Simplic.FileStructure.Workflow;
using System;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Represents a document path and connect it with a directory/file structure
    /// </summary>
    public class FileStructureDocumenPath
    {
        private string path;
        private DocumentWorkflowStateType documentWorkflowStateType = DocumentWorkflowStateType.InReview;

        /// <summary>
        /// Gets or set the entry id
        /// </summary>
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the directory guid
        /// </summary>
        public Guid DirectoryGuid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the file structure id
        /// </summary>
        public Guid FileStructureGuid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the document id
        /// </summary>
        public Guid DocumentGuid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hash in the storage
        /// </summary>
        public string StorageHash
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hash in the file structure
        /// </summary>
        public string FileStructureHash
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether this document path is the main/primary document path.
        /// Between a document and a file structure only one primary path possible
        /// </summary>
        public bool IsProtectedPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path as string
        /// </summary>
        public string Path
        {
            get => path;
            set
            {
                path = value;
                if (PreviousPath == null) // Only check for null, not for null and whitespace
                    PreviousPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the previous path
        /// </summary>
        public string PreviousPath
        {
            get;
            set;
        }

        public DocumentWorkflowStateType WorkflowState
        {
            get => documentWorkflowStateType;
            set
            {
                documentWorkflowStateType = value;
                if (PreviousPath == null) // Only check for null, not for null and whitespace
                    PreviousWorkflowState = value;
            }
        }

        /// <summary>
        /// Gets or sets the previous workflow state
        /// </summary>
        public DocumentWorkflowStateType? PreviousWorkflowState
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the workflow id
        /// </summary>
        public Guid? WorkflowId { get; set; }
    }
}
