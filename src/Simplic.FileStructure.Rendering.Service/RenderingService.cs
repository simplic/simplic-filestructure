using Simplic.FileStructure.Service;
using Simplic.Icon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Rendering.Service
{
    /// <summary>
    /// Simplic html rendering service
    /// </summary>
    public class RenderingService : IRenderingService
    {
        private readonly IFieldTypeService FieldTypeService;
        private readonly IDirectoryTypeService directoryTypeService;
        private readonly IIconService iconService;

        // HTML template to render. must contain {content}
        private string htmlTemplate = "<!DOCTYPE html><html><head> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"/> <style>body { font-family: Verdana;} ul, #myUL{list-style-type: none;}#myUL{margin: 0; padding: 0;}.caret{cursor: pointer; -webkit-user-select: none; /* Safari 3.1+ */ -moz-user-select: none; /* Firefox 2+ */ -ms-user-select: none; /* IE 10+ */ user-select: none;}.caret::before{content: \"\\25B6\"; color: black; display: inline-block; margin-right: 6px;}.caret-down::before{-ms-transform: rotate(90deg); /* IE 9 */ -webkit-transform: rotate(90deg); /* Safari */ transform: rotate(90deg);}.nested{display: none;}.active{display: block;}</style></head><body>{content}<script>var toggler=document.getElementsByClassName(\"caret\"); var i; for (i=0; i < toggler.length; i++){toggler[i].addEventListener(\"click\", function(){this.parentElement.querySelector(\".nested\").classList.toggle(\"active\"); this.classList.toggle(\"caret-down\");});}</script></body></html>";
        private IDictionary<Guid, string> icons;

        /// <summary>
        /// Initialize service
        /// </summary>
        public RenderingService(IDirectoryTypeService directoryTypeService, IIconService iconService)
        {
            this.directoryTypeService = directoryTypeService;
            this.iconService = iconService;

            icons = new Dictionary<Guid, string>();
            GetIcons();
        }

        /// <summary>
        /// Render file structure to html
        /// </summary>
        /// <param name="fileStructure">Filestructure instance</param>
        /// <returns>Html result</returns>
        public string Render(FileStructure fileStructure)
        {                        
            var sb = new StringBuilder();
            sb.Append($"<h1>{System.Web.HttpUtility.HtmlEncode(fileStructure.Name)}</h1>");
            sb.Append($"<br />");
            sb.Append($"<br />");

            sb.Append("<ul id=\"myUL\">");
            sb.Append(WalkItems(fileStructure, fileStructure.Directories.Where(x => x.Parent == null).ToList()));
            sb.Append("</ul>");
            
            var html = htmlTemplate.Replace("{content}", sb.ToString());

            return htmlTemplate.Replace("{content}", sb.ToString());
        }

        private void GetIcons()
        {
            icons.Clear();

            var directoryTypes = directoryTypeService.GetAll().ToList();
            foreach (var item in directoryTypes)
            {                
                var icon = iconService.GetById(item.IconId);
                if (icon == null) continue;

                var base64 = Convert.ToBase64String(icon);                

                var img = $"<img width=\"16\" height=\"16\" src=\"data:image/png;base64,{base64}\" />";

                icons[item.Id] = img;
            }
        }

        private string WalkItems(FileStructure fileStructure, IList<Directory> directories)
        {
            var sb = new StringBuilder();

            foreach (var item in directories)
            {
                var iconImg = GetIcon(item.DirectoryTypeId);

                sb.Append("<li>");

                if (HasChildren(fileStructure.Directories, item.Id))
                {
                    sb.Append($"<span class=\"caret\">{iconImg} {System.Web.HttpUtility.HtmlEncode(item.Name)}</span>");                    
                    sb.Append("<ul class=\"nested\">");

                    var children = WalkItems(fileStructure, fileStructure.Directories.Where(x => x.Parent?.Id == item.Id).ToList());
                    if (children.Length > 0)
                    {
                        sb.Append(children.ToString());
                    }

                    sb.Append("</ul>");
                }
                else
                {
                    sb.Append($"{iconImg} {item.Name}");
                }

                sb.Append("</li>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a BASE 64 encoded icon 
        /// </summary>
        /// <param name="directoryTypeId"></param>
        /// <returns></returns>
        private string GetIcon(Guid directoryTypeId)
        {
            if (icons.ContainsKey(directoryTypeId))
            {
                return icons[directoryTypeId];
            }

            return "";
        }

        private bool HasChildren(IList<Directory> directories, Guid id)
        {
            return directories.Any(x => x.Parent?.Id == id);
        }
    }
}
