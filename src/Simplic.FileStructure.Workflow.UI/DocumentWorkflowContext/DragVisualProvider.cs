using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;

namespace Simplic.FileStructure.Workflow.UI.DocumentWorkflowContext
{
    public class DragVisualProvider : DependencyObject, IDragVisualProvider
	{
		public DataTemplate DraggedItemTemplate
		{
			get
			{
				return (DataTemplate)GetValue(DraggedItemTemplateProperty);
			}
			set
			{
				SetValue(DraggedItemTemplateProperty, value);
			}
		}

		public static readonly DependencyProperty DraggedItemTemplateProperty =
		DependencyProperty.Register("DraggedItemTemplate", typeof(DataTemplate), typeof(DragVisualProvider), new PropertyMetadata(null));

		public FrameworkElement CreateDragVisual(DragVisualProviderState state)
		{
			var visual = new DragVisual();

			var theme = StyleManager.GetTheme(state.Host);
			if (theme != null)
			{
				StyleManager.SetTheme(visual, theme);
			}

			visual.Content = state.DraggedItems.OfType<object>().FirstOrDefault();
			visual.ContentTemplate = this.DraggedItemTemplate;

			return visual;
		}

		public Point GetDragVisualOffset(DragVisualProviderState state)
		{
			return state.RelativeStartPoint;
		}

		public bool UseDefaultCursors { get; set; }
	}
}
}
