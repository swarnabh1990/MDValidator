using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDValidator.App.Models
{
    public class FileUploadControlViewModel
    {
        public FileUploadControlViewModel()
        {
            this.UploadBtnText = "Upload File";
        }

        public FileUploadControlViewModel(string uploadBtnText, string area = "", string controller = "", string action = "")
        {
            this.UploadBtnText = uploadBtnText;
            this.Area = area;
            this.Controller = controller;
            this.Action = action;
        }

        public string UploadBtnText { get; set; }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }
    }
}
