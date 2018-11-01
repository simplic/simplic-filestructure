using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.UI.Converter
{
    /// <summary>
    /// Grid model converter
    /// </summary>
    public class GridDocumentPathConverter
    {
        /// <summary>
        /// Convert data
        /// </summary>
        /// <param name="guids">Guids to convert</param>
        /// <returns>Instance of <see cref="FileStructureDocumenPath"/> if exists</returns>
        public static FileStructureDocumenPath Convert(IList<object> guids)
        {
            if (guids.Count == 0)
                return null;

            if (!(guids[0] is Guid))
                return null;

            var service = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureDocumentPathService>();
            var path = service.Get((Guid)guids[0]);

            return path;
        }
    }
}