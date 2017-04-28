using System;
using System.Collections.Generic;
using System.Text;

namespace MDValidator.Domain.Configuration.ValidationRules
{
    public class Item
    {
        public string ValId { get; set; }
        public int Step { get; set; }
        public string Type { get; set; }
        public string Table { get; set; }
        public string Field { get; set; }
        public string Command { get; set; }
        public string ParamTable { get; set; }
        public string ParamField { get; set; }
        public string Parameter { get; set; }
        public string Connector { get; set; }
        public int PreReq { get; set; }

        public DateTime DateTime { get; set; }
        public string User { get; set; }
    }
}
