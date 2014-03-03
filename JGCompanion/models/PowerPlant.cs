using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class PowerPlant : Equipment
    {
        public Int32 Output { get; set; }

        public PowerPlant(XElement xml)
            :base(xml)
        {
            Int32 output;

            XElement stats = xml.Element("stats");
            XAttribute outputAttribute = stats.Attribute("energy_output");

            if (Int32.TryParse(outputAttribute.Value, out output) == false)
            {
                throw new ArgumentException("Power Plant Output not valid", "xml['energy_output']");
            }

            this.Output = output;
        }
    }
}
