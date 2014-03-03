using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class StationInventories
    {
        public class InventoryItem
        {
            public Int64 ItemID { get; set; }
            public string Name { get; set; }
            public Int64 Qty { get; set; }
            public Int64 Price { get; set; }

            public InventoryItem(XElement xml)
            {
                Int64 id, qty, price;

                XAttribute idAttribute = xml.Attribute("id");
                XAttribute qtyAttribute = xml.Attribute("qty");
                XAttribute priceAttribute = xml.Attribute("price");
                if (Int64.TryParse(idAttribute.Value, out id) == false)
                {
                    throw new ArgumentException("Inventory Item ID not valid", "xml['id']");
                }
                if (Int64.TryParse(qtyAttribute.Value, out qty) == false)
                {
                    throw new ArgumentException("Inventory Item Qty not valid", "xml['qty']");
                }
                if (Int64.TryParse(priceAttribute.Value, out price) == false)
                {
                    throw new ArgumentException("Inventory Item Price not valid", "xml['price']");
                }

                this.ItemID = id;
                this.Name = xml.Attribute("name").Value;
                this.Qty = qty;
                this.Price = price;
            }
        }
        public class StationInventory
        {
            public Int64 StationID { get; set; }
            public string Name { get; set; }
            public Factions Faction { get; set; }
            public List<InventoryItem> Inventory { get; private set; }

            public StationInventory(XElement xml)
            {
                Int64 id;

                XAttribute idAttribute = xml.Attribute("id");
                if (Int64.TryParse(idAttribute.Value, out id) == false)
                {
                    throw new ArgumentException("Station ID not valid", "xml['id']");
                }

                this.StationID = id;
                this.Name = xml.Attribute("name").Value;
                this.Faction = Pilot.FromString(xml.Attribute("faction").Value);
                this.Inventory = new List<InventoryItem>();
                var items = xml.Elements("item");
                foreach (XElement item in items)
                {
                    InventoryItem entry = new InventoryItem(item);
                    this.Inventory.Add(entry);
                }
            }
        }

        public List<StationInventory> Stations { get; private set; }

        public StationInventories(XElement xml)
        {
            this.Stations = new List<StationInventory>();
            var stations = xml.Elements("station");
            foreach(XElement station in stations)
            {
                StationInventory entry = new StationInventory(station);
                this.Stations.Add(entry);
            }
        }
    }
}
