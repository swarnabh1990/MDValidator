using MDValidator.App.Models;
using MDValidator.Domain.Configuration.LaneMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDValidator.App.Areas.Configuration.Models.LaneMaster
{
    public class LaneMasterViewModel : ViewModelBase
    {
        public LaneMasterViewModel() : base()
        {
            LaneMaster = new List<LaneMasterRecord>();
        }

        public List<LaneMasterRecord> LaneMaster { get; set; }
    }
}
