using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Simplic.FileStructure.Model;

namespace Simplic.FileStructure.UI
{
    public class WorkflowVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Check if the value is a directory view model and if the directory type of workflow
        /// </summary>
        /// <param name="value"> expected value</param>
        /// <param name="targetType"> not necessary</param>
        /// <param name="parameter">not necessary</param>
        /// <param name="culture">not necessary</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is DirectoryViewModel viewModel)
                flag = viewModel.DirectoryType.DirectoryFunction == DirectoryFunctionType.Workflow;

            return (flag ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
