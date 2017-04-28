using MDValidator.App.Models;
using MDValidator.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDValidator.App.Areas.Validation.Models.Validation
{
    public class OfficialListViewModel : ViewModelBase
    {
        public OfficialListViewModel() : base()
        {
            this.OfficialList = new OfficialList();
            this.OfficialList.Entries = new List<OfficialListEntry>();
        }

        public OfficialList OfficialList { get; set; }
    }
}
