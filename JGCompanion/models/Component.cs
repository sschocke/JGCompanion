using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class Component
    {
        public Int64 ItemID { get; set; }
        public string Name { get; set; }
        public Int64 Qty { get; set; }

        public Component(XElement xml)
        {
            Int64 id, qty = 0;

            XAttribute idAttribute = xml.Attribute("id");
            XAttribute qtyAttribute = xml.Attribute("qty");
            if (Int64.TryParse(idAttribute.Value, out id) == false)
            {
                throw new ArgumentException("Component ID not valid", "xml['id']");
            }
            if (qtyAttribute != null)
            {
                if (Int64.TryParse(qtyAttribute.Value, out qty) == false)
                {
                    throw new ArgumentException("Component Qty not valid", "xml['qty']");
                }
            }

            this.ItemID = id;
            this.Qty = qty;
            this.Name = xml.Value;
        }
    }
}
