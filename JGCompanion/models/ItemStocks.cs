using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class ItemStocks
    {
        public class StockItem : Item
        {
            public Int64 Qty { get; set; }

            public StockItem(XElement xml)
                : base(xml)
            {
                Int64 qty;

                XAttribute qtyAttribute = xml.Attribute("qty");
                if (Int64.TryParse(qtyAttribute.Value, out qty) == false)
                {
                    throw new ArgumentException("Stock Item Qty not valid", "xml['qty']");
                }

                this.Qty = qty;
            }
        }

        public List<StockItem> Items { get; private set; }

        public ItemStocks(XElement xml)
        {
            this.Items = new List<StockItem>();
            var items = xml.Elements("item").ToArray();
            foreach (XElement item in items)
            {
                StockItem entry = new StockItem(item);
                this.Items.Add(entry);
            }
        }
    }
}
