using MDValidator.App.Areas.Configuration.Models.LaneMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDValidator.App.Services.Configuration.LaneMaster
{
    public interface ILaneMasterService
    {
        LaneMasterViewModel ProcessFile(string fileName);
    }
}
