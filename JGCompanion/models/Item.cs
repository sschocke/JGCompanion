using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Android.Content;

namespace JGCompanion
{
    public enum ItemType
    {
        COMMODITY = 1,
        CAPACITOR = 2,
        ECM = 3,
        ENGINE = 4,
        GUN = 5,
        MISSILE = 6,
        MODX = 7,
        POWERPLANT = 8,
        RADAR = 9,
        SHIELD = 10
    }

    public abstract class Item
    {
        public Int64 ItemID { get; set; }
        public string Name { get; set; }
        public string Classification { get; set; }
        public ItemType ItemType { get; set; }
        public string Description { get; set; }
        public List<Component> Components { get; private set; }
        public List<ItemStationStock> StationStocks { get; private set; }
        public List<ItemProducer> Producers { get; private set; }

        private static Dictionary<string, int> icons;

        static Item()
        {
            icons = new Dictionary<string, int>();
        }

        public static int GetIconId(string name, Context context)
        {
            if (icons.ContainsKey(name) == true)
            {
                return icons[name];
            }

            int id = context.GetResources().GetIdentifier(name, "drawable", context.GetPackageName());
            if (id == 0)
            {
                return 0;
            }

            icons.Add(name, id);

            return id;
        }

        public Item(XElement xml)
        {
            Int64 id;
            Int32 typeID;

            XAttribute idAttribute = xml.Attribute("id");
            XAttribute nameAttribute = xml.Attribute("name");
            XAttribute classAttribute = xml.Attribute("class");
            XAttribute typeAttribute = xml.Attribute("type");

            if (Int64.TryParse(idAttribute.Value, out id) == false)
            {
                throw new ArgumentException("Item ID not valid", "xml['id']");
            }
            if (Int32.TryParse(typeAttribute.Value, out typeID) == false)
            {
                throw new ArgumentException("Item Type not valid", "xml['type']");
            }

            this.ItemID = id;
            this.Name = nameAttribute.Value;
            this.Classification = classAttribute.Value;
            this.ItemType = (ItemType)typeID;
            this.Components = new List<Component>();
            this.StationStocks = new List<ItemStationStock>();
            this.Producers = new List<ItemProducer>();
        }

        protected void ReadComponents(XElement xml)
        {
            if (xml == null)
            {
                return;
            }

            this.Components.Clear();
            IEnumerable<XElement> components = xml.Elements("component");
            foreach (XElement comp in components)
            {
                Component entry = new Component(comp);
                this.Components.Add(entry);
            }
        }
        protected void ReadStationStocks(XElement xml)
        {
            if (xml == null)
            {
                return;
            }

            this.StationStocks.Clear();
            IEnumerable<XElement> stations = xml.Elements("station");
            foreach (XElement station in stations)
            {
                ItemStationStock entry = new ItemStationStock(station);
                this.StationStocks.Add(entry);
            }
        }

        protected void ReadProducers(XElement xml)
        {
            if (xml == null)
            {
                return;
            }

            this.Producers.Clear();
            IEnumerable<XElement> stations = xml.Elements("station");
            foreach (XElement station in stations)
            {
                ItemProducer entry = new ItemProducer(station);
                this.Producers.Add(entry);
            }
        }

        public static Item FromDetailXML(XElement xml)
        {
            Int32 type;
            XAttribute typeAttribute = xml.Attribute("type");
            if (Int32.TryParse(typeAttribute.Value, out type) == false)
            {
                throw new ArgumentException("Item Type not valid", "xml['type']");
            }

            switch ((ItemType)type)
            {
                case ItemType.COMMODITY:
                    return new Commodity(xml);
                case ItemType.ECM:
                    return new ECM(xml);
                case ItemType.SHIELD:
                    return new Shield(xml);
                case ItemType.CAPACITOR:
                    return new Capacitor(xml);
                case ItemType.ENGINE:
                    return new Engine(xml);
                case ItemType.POWERPLANT:
                    return new PowerPlant(xml);
                case ItemType.RADAR:
                    return new Radar(xml);
                case ItemType.MODX:
                    return new MODx(xml);
                case ItemType.GUN:
                    return new Gun(xml);
                case ItemType.MISSILE:
                    return new Missile(xml);
            }

            return null;
        }

        public string ResourceName
        {
            get
            {
                return this.Name.ToLower().Replace(' ', '_').Replace(".", "").Replace("-", "");
            }
        }
    }
}
