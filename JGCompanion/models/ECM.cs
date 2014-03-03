using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class ECM : Equipment
    {
        public Int16 SensorLevel { get; set; }
        public Int32 Power { get; set; }

        public ECM(XElement xml)
            :base(xml)
        {
            Int16 level;
            Int32 power;

            XElement stats = xml.Element("stats");
            XAttribute sensorLevelAttribute = stats.Attribute("sensor_level");
            XAttribute powerAttribute = stats.Attribute("power");

            if (Int16.TryParse(sensorLevelAttribute.Value, out level) == false)
            {
                throw new ArgumentException("ECM Sensor Level not valid", "xml['sensor_level']");
            }
            if (String.IsNullOrEmpty(powerAttribute.Value))
            {
                power = Int32.MinValue;
            }
            else if (Int32.TryParse(powerAttribute.Value, out power) == false)
            {
                throw new ArgumentException("ECM Power Usage not valid", "xml['power']");
            }

            this.SensorLevel = level;
            this.Power = power;
        }
    }
}
