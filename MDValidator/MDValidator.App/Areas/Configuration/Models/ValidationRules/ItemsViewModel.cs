using MDValidator.App.Models;
using MDValidator.Domain.Configuration.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDValidator.App.Areas.Configuration.Models.ValidationRules
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
