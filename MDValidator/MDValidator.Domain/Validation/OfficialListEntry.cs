using System;
using System.Collections.Generic;

namespace MDValidator.Domain.Validation
{
    public class OfficialListEntry : IEquatable<OfficialListEntry>
    {
        public string Lane { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public string MTART { get; set; }

        public string MATNR { get; set; }


        public bool SatisfiesCompleteness
        {
            get
            {
                if (string.IsNullOrEmpty(Lane) || string.IsNullOrWhiteSpace(Lane) ||
                    string.IsNullOrEmpty(Origin) || string.IsNullOrWhiteSpace(Origin) ||
                    string.IsNullOrEmpty(Destination) || string.IsNullOrWhiteSpace(Destination) ||
                    string.IsNullOrEmpty(MTART) || string.IsNullOrWhiteSpace(MTART) ||
                    string.IsNullOrEmpty(MATNR) || string.IsNullOrWhiteSpace(MATNR))
                {
                    return false;
                }

                return true;
            }
        }

        public bool Equals(OfficialListEntry other)
        {
            if (this.Lane.Equals(other.Lane) && this.Origin.Equals(other.Origin) &&
                this.Destination.Equals(other.Destination) && this.MATNR.Equals(other.MATNR))
            {
                return true;
            }

            return false;
        }
    }
}
