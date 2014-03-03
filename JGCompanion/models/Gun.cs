using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class Gun : Equipment
    {
        public Int16 Ammo { get; set; }
        public Int32 EnergyUse { get; set; }
        public Int32 Damage { get; set; }
        public Double Delay { get; set; }
        public Int32 Velocity { get; set; }
        public Int32 Life { get; set; }
        public Int32 Range { get; set; }

        public Gun(XElement xml)
            :base(xml)
        {
            Int16 ammo;
            Int32 energyUse, damage;
            Int32 velocity, life, range;
            Double delay;

            XElement stats = xml.Element("stats");
            XAttribute delayAttribute = stats.Attribute("delay");
            XAttribute energyUseAttribute = stats.Attribute("energy_use");
            XAttribute damageAttribute = stats.Attribute("damage");
            XAttribute ammoAttribute = stats.Attribute("ammo");
            XAttribute velocityAttribute = stats.Attribute("velocity");
            XAttribute lifeAttribute = stats.Attribute("life");
            XAttribute rangeAttribute = stats.Attribute("range");

            if (Int16.TryParse(ammoAttribute.Value, out ammo) == false)
            {
                throw new ArgumentException("Gun Ammo not valid", "xml['ammo']");
            }
            if (Int32.TryParse(energyUseAttribute.Value, out energyUse) == false)
            {
                throw new ArgumentException("Gun Energy Yse not valid", "xml['energy_use']");
            }
            if (Int32.TryParse(damageAttribute.Value, out damage) == false)
            {
                throw new ArgumentException("Gun Damage not valid", "xml['damage']");
            }
            if (Int32.TryParse(velocityAttribute.Value, out velocity) == false)
            {
                throw new ArgumentException("Gun Velocity not valid", "xml['velocity']");
            }
            if (Int32.TryParse(lifeAttribute.Value, out life) == false)
            {
                throw new ArgumentException("Gun Life not valid", "xml['life']");
            }
            if (Int32.TryParse(rangeAttribute.Value, out range) == false)
            {
                throw new ArgumentException("Gun Range not valid", "xml['range']");
            }
            if (Double.TryParse(delayAttribute.Value, out delay) == false)
            {
                throw new ArgumentException("Gun Delay not valid", "xml['delay']");
            }

            this.Ammo = ammo;
            this.Damage = damage;
            this.EnergyUse = energyUse;
            this.Delay = delay;
            this.Velocity = velocity;
            this.Life = life;
            this.Range = range;
        }
    }
}
