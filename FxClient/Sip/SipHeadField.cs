using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Sip
{
    public class SipHeadField
    {
        private string name = string.Empty;
        private string value = string.Empty;
        public SipHeadField()
        {
        }

        public SipHeadField(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public SipHeadField(string name)
            : this(name, string.Empty)
        { 
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this.name, this.value);
        }

    }
}
