﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Rendering.Service
{
    public class RenderingService : IRenderingService
    {
        // HTML template to render. must contain {iconClasses}, {content}
        private string htmlTemplate = "<!DOCTYPE html><html><head> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"/> <style>{iconClasses} ul, #myUL{list-style-type: none;}#myUL{margin: 0; padding: 0;}.caret{cursor: pointer; -webkit-user-select: none; /* Safari 3.1+ */ -moz-user-select: none; /* Firefox 2+ */ -ms-user-select: none; /* IE 10+ */ user-select: none;}.caret::before{content: \"\25B6\"; color: black; display: inline-block; margin-right: 6px;}.caret-down::before{-ms-transform: rotate(90deg); /* IE 9 */ -webkit-transform: rotate(90deg); /* Safari */' transform: rotate(90deg);}.nested{display: none;}.active{display: block;}</style></head><body>{content}<script>var toggler=document.getElementsByClassName(\"caret\"); var i; for (i=0; i < toggler.length; i++){toggler[i].addEventListener(\"click\", function(){this.parentElement.querySelector(\".nested\").classList.toggle(\"active\"); this.classList.toggle(\"caret-down\");});}</script></body></html>";
        private IDictionary<string, string> icons = new Dictionary<string, string>();

        public string Render(FileStructure fileStructure)
        {
            var sb = new StringBuilder();

            sb.Append("<ul id=\"myUL\">");
            sb.Append(WalkItems(fileStructure.Directories));
            sb.Append("</ul>");

            htmlTemplate.Replace("{iconClasses}", GetIconClasses());
            var html = htmlTemplate.Replace("{content}", sb.ToString());

            return htmlTemplate.Replace("{content}", sb.ToString());
        }

        private string GetIconClasses()
        {
            var sb = new StringBuilder();
            foreach (var item in icons)
            {
                sb.Append($".{item.Key}");
                sb.Append("{cursor: pointer;-webkit-user-select: none; /* Safari 3.1+ */-moz-user-select: none; /* Firefox 2+ */-ms-user-select: none; /* IE 10+ */user-select: none;}");
                sb.Append($".{item.Key}::before");
                sb.Append("{ content: \"" + item.Value + "\";color: black;display: inline - block;margin - right: 6px;}");
            }

            return sb.ToString();
        }

        private string WalkItems(IList<Directory> directories)
        {
            var sb = new StringBuilder();

            foreach (var item in directories)
            {
                var iconClass = GetIcon(item.DirectoryTypeId);

                sb.Append("<li>");

                if (HasChildren(directories, item.Id))
                {
                    sb.Append($"<span class=\"caret {iconClass}\">");
                    sb.Append(item.Name);
                    sb.Append("</span>");

                    sb.Append("<ul class=\"nested\">");

                    var children = WalkItems(directories.Where(x => x.Parent.Id == item.Id).ToList());
                    if (children.Length > 0)
                    {
                        sb.Append(children.ToString());
                    }

                    sb.Append("</ul>");
                }
                else
                {
                    sb.Append($"<span class=\"{iconClass}\">");
                    sb.Append(item.Name);
                    sb.Append("</span>");
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
            return "\\25B6";
        }

        private bool HasChildren(IList<Directory> directories, Guid id)
        {
            return directories.Any(x => x.Parent.Id == id);
        }
    }
}
