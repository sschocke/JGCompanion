using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class ItemProducer
    {
        public Int64 StationID { get; set; }
        public string Name { get; set; }
        public List<Component> Components { get; private set; }

        public ItemProducer(XElement xml)
        {
            Int64 id;

            XAttribute idAttribute = xml.Attribute("id");
            XElement nameElement = xml.Element("name");
            if (Int64.TryParse(idAttribute.Value, out id) == false)
            {
                throw new ArgumentException("Station ID not valid", "xml['id']");
            }

            this.StationID = id;
            this.Name = nameElement.Value;
            this.Components = new List<Component>();

            XElement holder = xml.Element("components");
            if (holder != null)
            {
                IEnumerable<XElement> components = holder.Elements("component");
                foreach (XElement comp in components)
                {
                    Component entry = new Component(comp);
                    this.Components.Add(entry);
                }
            }
        }
    }
}
