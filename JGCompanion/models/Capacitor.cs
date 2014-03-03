using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class Capacitor : Equipment
    {
        public Int32 Capacity { get; set; }
        public Double Efficiency { get; set; }

        public Capacitor(XElement xml)
            :base(xml)
        {
            Int32 capacity;
            Double efficiency;

            XElement stats = xml.Element("stats");
            XAttribute efficiencyAttribute = stats.Attribute("efficiency");
            XAttribute capacityAttribute = stats.Attribute("capacity");

            if (Int32.TryParse(capacityAttribute.Value, out capacity) == false)
            {
                throw new ArgumentException("Capacitor Capacity not valid", "xml['capacity']");
            }
            if (Double.TryParse(efficiencyAttribute.Value, out efficiency) == false)
            {
                throw new ArgumentException("Capacitor Efficiency not valid", "xml['efficiency']");
            }

            this.Capacity = capacity;
            this.Efficiency = efficiency;
        }
    }
}
