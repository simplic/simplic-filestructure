using Simplic.Flow;
using System;

namespace Simplic.FileStructure.Workflow.Flow
{
    /// <summary>
    /// Handles the completed event in the simplic workflow system
    /// </summary>
    [ActionNodeDefinition(Name = nameof(OnDocumentWorkflowOperationCompleted), DisplayName = "Dms workflow completed", Category = "DMS",
        Tooltip = "Will be triggered, when a user completes a worklfow in the workflow system")]
    public class OnDocumentWorkflowOperationCompleted : EventNode
    {
        /// <summary>
        /// Set workflow op and execute next node
        /// </summary>
        public override bool Execute(IFlowRuntimeService runtime, DataPinScope scope)
        {
            try
            {
                var args = runtime.FlowEventArgs.Object as WorkflowOperation;
            
                scope.SetValue(OutPinWorkflowOperation, args);

                if (OutNodeSuccess != null)
                    runtime.EnqueueNode(OutNodeSuccess, scope);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogManagerInstance.Instance.Error("Error while executing workflow operation completed", ex);

                if (OutNodeFailed != null)
                    runtime.EnqueueNode(OutNodeFailed, scope);

                return true;
            }
        }

        /// <summary>
        /// Gets the event name `OnDocumentWorkflowOperationCompleted`
        /// </summary>
        public override string EventName => "OnDocumentWorkflowOperationCompleted";

        /// <summary>
        /// Gets enqueued when completing was successful.
        /// </summary>
        [FlowPinDefinition(Name = "OutNodeSuccess", DisplayName = "Out", PinDirection = PinDirection.Out)]
        public ActionNode OutNodeSuccess { get; set; }

        /// <summary>
        /// Gets enqueued when the completing failed.
        /// </summary>
        [FlowPinDefinition(Name = "OutNodeFailed", DisplayName = "Out", PinDirection = PinDirection.Out)]
        public ActionNode OutNodeFailed { get; set; }

        /// <summary>
        /// Gets or sets the output-pin that contains the workflow operation information
        /// </summary>
        [DataPinDefinition(
            Id = "8052d6d1-196b-4e4f-a2c3-bc2878c06566",
            ContainerType = DataPinContainerType.Single,
            DataType = typeof(WorkflowOperation),
            Direction = PinDirection.Out,
            Name = nameof(OutPinWorkflowOperation),
            DisplayName = "Workflow operation")]
        public DataPin OutPinWorkflowOperation { get; set; }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        public override string Name => nameof(OnDocumentWorkflowOperationCompleted);

        /// <summary>
        /// Gets the user friendly name of the node.
        /// </summary>
        public override string FriendlyName => nameof(OnDocumentWorkflowOperationCompleted);
    }
}
