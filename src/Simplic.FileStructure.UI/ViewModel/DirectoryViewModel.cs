using Simplic.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Directory viewmodel
    /// </summary>
    public class DirectoryViewModel : ExtendableViewModel
    {
        private Directory model;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="model"></param>
        public DirectoryViewModel(Directory model)
        {
            this.model = model;
        }

        /// <summary>
        /// Gets the directory model
        /// </summary>
        public Directory Model
        {
            get
            {
                return model;
            }
        }

        /// <summary>
        /// Gets or sets the directory name
        /// </summary>
        public string Name
        {
            get
            {
                return model.Name;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.Name = newValue; });
            }
        }
    }
}
