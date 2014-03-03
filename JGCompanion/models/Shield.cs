using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class Shield : Equipment
    {
        public Int16 BaseRate { get; set; }
        public Int32 MaxDeflect { get; set; }
        public Int32 MaxRegenRate { get; set; }
        public Double Efficiency { get; set; }

        public Shield(XElement xml)
            :base(xml)
        {
            Int16 baseRate;
            Int32 maxRegen, maxDeflect;
            Double efficiency;

            XElement stats = xml.Element("stats");
            XAttribute efficiencyAttribute = stats.Attribute("efficiency");
            XAttribute maxDeflectAttribute = stats.Attribute("max_deflect");
            XAttribute maxRegenAttribute = stats.Attribute("max_regenrate");
            XAttribute baseRateAttribute = stats.Attribute("base_rate");

            if (Int16.TryParse(baseRateAttribute.Value, out baseRate) == false)
            {
                throw new ArgumentException("Shield Base Rate not valid", "xml['base_rate']");
            }
            if (Int32.TryParse(maxDeflectAttribute.Value, out maxDeflect) == false)
            {
                throw new ArgumentException("Shield Max Deflect not valid", "xml['max_deflect']");
            }
            if (Int32.TryParse(maxRegenAttribute.Value, out maxRegen) == false)
            {
                throw new ArgumentException("Shield Max Regen Rate not valid", "xml['max_regenrate']");
            }
            if (Double.TryParse(efficiencyAttribute.Value, out efficiency) == false)
            {
                throw new ArgumentException("Shield Efficiency not valid", "xml['efficiency']");
            }

            this.BaseRate = baseRate;
            this.MaxDeflect = maxDeflect;
            this.MaxRegenRate = maxRegen;
            this.Efficiency = efficiency;
        }
    }
}
