using MDValidator.App.Areas.Configuration.Models.ExtractionRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDValidator.App.Services.Configuration.ExtractionRules
{
    public interface IExtractionRulesService
    {
        HeadersViewModel ProcessHeaderFile(string fileName);

        ItemsViewModel ProcessItemsFile(string fileName);

        bool CheckHeaderItemsConsistency(List<string> headerIds, List<string> itemIds);
    }
}
