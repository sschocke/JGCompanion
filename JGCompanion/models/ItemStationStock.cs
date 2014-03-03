using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class ItemStationStock
    {
        public Int64 StationID { get; set; }
        public string Name { get; set; }
        public Int64 Qty { get; set; }
        public Int64 Price { get; set; }

        public ItemStationStock(XElement xml)
        {
            Int64 id, qty, price;

            XAttribute idAttribute = xml.Attribute("id");
            XAttribute qtyAttribute = xml.Attribute("qty");
            XAttribute priceAttribute = xml.Attribute("price");
            if (Int64.TryParse(idAttribute.Value, out id) == false)
            {
                throw new ArgumentException("Station ID not valid", "xml['id']");
            }
            if (Int64.TryParse(qtyAttribute.Value, out qty) == false)
            {
                throw new ArgumentException("Station Item Qty not valid", "xml['qty']");
            }
            if (Int64.TryParse(priceAttribute.Value, out price) == false)
            {
                throw new ArgumentException("Station Item Price not valid", "xml['price']");
            }

            this.StationID = id;
            this.Qty = qty;
            this.Price = price;
            this.Name = xml.Value;
        }
    }
}
