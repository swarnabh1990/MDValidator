using System;
using System.Collections.Generic;
using System.Text;

namespace MDValidator.Domain.Validation
{
    public class OfficialList
    {
        public string Name { get; set; }

        public List<OfficialListEntry> Entries { get; set; }
    }
}
