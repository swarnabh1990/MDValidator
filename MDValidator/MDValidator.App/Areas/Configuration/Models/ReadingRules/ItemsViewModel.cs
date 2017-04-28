using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDValidator.App.Models;
using MDValidator.Domain.Configuration.ReadingRules;

namespace MDValidator.App.Areas.Configuration.Models.ReadingRules
{
    public class ItemsViewModel : ViewModelBase
    {
        public ItemsViewModel()
        {
            this.Items = new List<Item>();
        }

        public List<Item> Items { get; set; }
    }
}
