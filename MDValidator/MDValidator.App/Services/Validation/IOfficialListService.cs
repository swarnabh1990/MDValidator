using MDValidator.App.Areas.Validation.Models.Validation;
using MDValidator.Domain.Configuration.LaneMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDValidator.App.Services.Validation
{
    public interface IOfficialListService
    {
        OfficialListViewModel ProcessFile(string fileName, List<LaneMasterRecord> laneMaster);
    }
}
