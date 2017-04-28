using System;
using System.Collections.Generic;
using System.Text;

namespace MDValidator.Domain.Configuration.ValidationRules
{
    public class Header
    {
        public string ValId { get; set; }
        public string ValTxt { get; set; }
        public string ReadId { get; set; }
        public string ExtrID { get; set; }
        public string Routine { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string MTART { get; set; }
        public string Leg { get; set; }
        public string Site { get; set; }
        public string SysId { get; set; }

        public DateTime DateTime { get; set; }
        public string User { get; set; }
    }
}
