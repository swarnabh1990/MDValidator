using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDValidator.App.Models
{
    public abstract class ViewModelBase
    {
        public ViewModelBase()
        {
            this.Errors = new List<string>();
        }

        public List<string> Errors { get; set; }
    }
}
