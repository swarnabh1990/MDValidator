using MDValidator.App.Areas.Configuration.Models.ReadingRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDValidator.App.Services.Configuration.ReadingRules
{
    public interface IReadingRulesService
    {
        HeadersViewModel ProcessHeaderFile(string fileName);

        ItemsViewModel ProcessItemsFile(string fileName);

        bool CheckHeaderItemsConsistency(List<string> headerIds, List<string> itemIds);
    }
}
