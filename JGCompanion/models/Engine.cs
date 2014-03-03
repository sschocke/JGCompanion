using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class Engine : Equipment
    {
        public Int32 MaxThrust { get; set; }
        public Double Efficiency { get; set; }

        public Engine(XElement xml)
            :base(xml)
        {
            Int32 maxThrust;
            Double efficiency;

            XElement stats = xml.Element("stats");
            XAttribute efficiencyAttribute = stats.Attribute("efficiency");
            XAttribute maxThrustAttribute = stats.Attribute("max_thrust");

            if (Int32.TryParse(maxThrustAttribute.Value, out maxThrust) == false)
            {
                throw new ArgumentException("Engine Max Thrust not valid", "xml['max_thrust']");
            }
            if (Double.TryParse(efficiencyAttribute.Value, out efficiency) == false)
            {
                throw new ArgumentException("Engine Efficiency not valid", "xml['efficiency']");
            }

            this.MaxThrust = maxThrust;
            this.Efficiency = efficiency;
        }
    }
}
