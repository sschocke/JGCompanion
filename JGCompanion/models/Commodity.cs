using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class Commodity : Item
    {
        public string Abbr { get; set; }
        public Int16 TechLevel { get; set; }
        public Int32 Mass { get; set; }
        public double GraviticSig { get; set; }
        public double MeltingPoint { get; set; }

        public Commodity(XElement xml)
            : base(xml)
        {
            Int16 level;
            Int32 mass;
            double grav, melt;

            XElement stats = xml.Element("stats");
            XAttribute techLevelAttribute = stats.Attribute("tech_level");
            XAttribute massAttribute = stats.Attribute("mass");
            XAttribute gravAttribute = stats.Attribute("gravitic_sig");
            XAttribute meltAttribute = stats.Attribute("melting_point");
            XAttribute abbrAttribute = stats.Attribute("abbr");

            if (Int16.TryParse(techLevelAttribute.Value, out level) == false)
            {
                throw new ArgumentException("Commodity Tech Level not valid", "xml['tech_level']");
            }
            if (Int32.TryParse(massAttribute.Value, out mass) == false)
            {
                throw new ArgumentException("Commodity Mass not valid", "xml['mass']");
            }
            if (String.IsNullOrEmpty(gravAttribute.Value))
            {
                grav = Double.NaN;
            }
            else if (Double.TryParse(gravAttribute.Value, out grav) == false)
            {
                throw new ArgumentException("Commodity Gravitic Signature not valid", "xml['gravitic_sig']");
            }
            if (String.IsNullOrEmpty(meltAttribute.Value))
            {
                melt = Double.NaN;
            }
            else if (Double.TryParse(meltAttribute.Value, out melt) == false)
            {
                throw new ArgumentException("Commodity Melting Point not valid", "xml['melting_point']");
            }

            this.Abbr = abbrAttribute.Value;
            this.TechLevel = level;
            this.Mass = mass;
            this.GraviticSig = grav;
            this.MeltingPoint = melt;

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
