using MDValidator.App.Models;
using MDValidator.Domain.Configuration.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDValidator.App.Areas.Configuration.Models.ValidationRules
{
    public class HeadersViewModel : ViewModelBase
    {
        public HeadersViewModel()
        {
            this.Headers = new List<Header>();
        }
        public List<Header> Headers { get; set; }
    }
}
