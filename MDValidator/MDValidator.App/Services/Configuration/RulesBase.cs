using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDValidator.App.Services.Configuration
{
    public class RulesBase
    {
        public bool CheckHeaderItemsConsistency(List<string> headerIds, List<string> itemIds)
        {
            foreach (var header in headerIds)
            {
                if (!itemIds.Contains(header))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
