using System;
using System.Runtime.Serialization;

namespace Simplic.FileStructure.Workflow
{
    public class DocumentWorkflowException : Exception
    {
        public DocumentWorkflowException()
        {
        }

        public DocumentWorkflowException(string message) : base(message)
        {
        }

        public DocumentWorkflowException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DocumentWorkflowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
