using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class Missile : Equipment
    {
        public Int16 MaxPitch { get; set; }
        public Int16 MaxYaw { get; set; }
        public Int32 Thrust { get; set; }
        public Int32 Damage { get; set; }
        public Double Drag { get; set; }
        public Int32 Life { get; set; }

        public Missile(XElement xml)
            :base(xml)
        {
            Int16 maxPitch, maxYaw;
            Int32 thrust, damage;
            Int32 life;
            Double drag;

            XElement stats = xml.Element("stats");
            XAttribute dragAttribute = stats.Attribute("drag");
            XAttribute thrustAttribute = stats.Attribute("thrust");
            XAttribute damageAttribute = stats.Attribute("damage");
            XAttribute maxPitchAttribute = stats.Attribute("max_pitch");
            XAttribute maxYawAttribute = stats.Attribute("max_yaw");
            XAttribute lifeAttribute = stats.Attribute("life");

            if (Int16.TryParse(maxPitchAttribute.Value, out maxPitch) == false)
            {
                throw new ArgumentException("Missile Max Pitch not valid", "xml['max_pitch']");
            }
            if (Int16.TryParse(maxYawAttribute.Value, out maxYaw) == false)
            {
                throw new ArgumentException("Missile Max Yaw not valid", "xml['max_yaw']");
            }
            if (Int32.TryParse(thrustAttribute.Value, out thrust) == false)
            {
                throw new ArgumentException("Missile Thrust not valid", "xml['thrust']");
            }
            if (Int32.TryParse(damageAttribute.Value, out damage) == false)
            {
                throw new ArgumentException("Missile Damage not valid", "xml['damage']");
            }
            if (Int32.TryParse(lifeAttribute.Value, out life) == false)
            {
                throw new ArgumentException("Missile Life not valid", "xml['life']");
            }
            if (Double.TryParse(dragAttribute.Value, out drag) == false)
            {
                throw new ArgumentException("Missile Drag not valid", "xml['drag']");
            }

            this.MaxPitch = maxPitch;
            this.MaxYaw = maxYaw;
            this.Damage = damage;
            this.Thrust = thrust;
            this.Drag = drag;
            this.Life = life;
        }
    }
}
