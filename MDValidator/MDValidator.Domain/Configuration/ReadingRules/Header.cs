using System;
using System.Collections.Generic;
using System.Text;

namespace MDValidator.Domain.Configuration.ReadingRules
{
    public class Header
    {
        public string ReadId { get; set; }
        public string Table { get; set; }
        public int Hierarchy { get; set; }

        public DateTime DateTime { get; set; }
        public string User { get; set; }
    }
}
