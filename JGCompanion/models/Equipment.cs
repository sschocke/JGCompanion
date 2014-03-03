using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public abstract class Equipment : Item
    {
        public string Manufacturer { get; set; }
        public Int16 TechLevel { get; set; }
        public Int16 Size { get; set; }
        public Int32 Mass { get; set; }

        public Equipment(XElement xml)
            :base(xml)
        {
            Int16 level, size;
            Int32 mass;

            XElement stats = xml.Element("stats");
            XAttribute techLevelAttribute = stats.Attribute("tech_level");
            XAttribute massAttribute = stats.Attribute("mass");
            XAttribute sizeAttribute = stats.Attribute("size");
            XAttribute manufacturerAttribute = stats.Attribute("manufacturer");

            if (Int16.TryParse(techLevelAttribute.Value, out level) == false)
            {
                throw new ArgumentException("Equipment Tech Level not valid", "xml['tech_level']");
            }
            if (Int16.TryParse(sizeAttribute.Value, out size) == false)
            {
                throw new ArgumentException("Equipment Size not valid", "xml['size']");
            }
            if (Int32.TryParse(massAttribute.Value, out mass) == false)
            {
                throw new ArgumentException("Equipment Mass not valid", "xml['mass']");
            }

            this.TechLevel = level;
            this.Mass = mass;
            this.Size = size;
            this.Manufacturer = manufacturerAttribute.Value;

            XElement components = xml.Element("components");
            XElement inventory = xml.Element("inventory");
            XElement producers = xml.Element("producers");
            this.Description = xml.Element("description").Value;
            base.ReadComponents(components);
            base.ReadStationStocks(inventory);
            base.ReadProducers(producers);
        }
    }
}
