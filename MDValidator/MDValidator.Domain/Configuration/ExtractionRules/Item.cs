using System;
using System.Collections.Generic;
using System.Text;

namespace MDValidator.Domain.Configuration.ExtractionRules
{
    public class Item
    {
        public string ExtrId { get; set; }
        public int Step { get; set; }
        public string Control { get; set; }
        public string Table { get; set; }
        public string Field { get; set; }
        public string Command { get; set; }
        public string ParamTable { get; set; }
        public string ParamField { get; set; }
        public string Parameter { get; set; }
        public string Connector { get; set; }

        public DateTime DateTime { get; set; }
        public string User { get; set; }
    }
}
