using System;
using System.Collections.Generic;
using System.Text;

namespace MDValidator.Domain.Configuration.ExtractionRules
{
    public class Header
    {
        public string ExtrId { get; set; }
        public string Table { get; set; }
        public int Hierarchy { get; set; }

        public DateTime DateTime { get; set; }
        public string User { get; set; }
    }
}
