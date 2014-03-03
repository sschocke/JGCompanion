using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class Radar : Equipment
    {
        public Int16 SensorLevel { get; set; }
        public Int32 Power { get; set; }
        public Int32 MaxRange { get; set; }

        public Radar(XElement xml)
            :base(xml)
        {
            Int16 level;
            Int32 power, maxRange;

            XElement stats = xml.Element("stats");
            XAttribute sensorLevelAttribute = stats.Attribute("sensor_level");
            XAttribute powerAttribute = stats.Attribute("power");
            XAttribute maxRangeAttribute = stats.Attribute("max_range");

            if (Int16.TryParse(sensorLevelAttribute.Value, out level) == false)
            {
                throw new ArgumentException("Radar Sensor Level not valid", "xml['sensor_level']");
            }
            if (Int32.TryParse(powerAttribute.Value, out power) == false)
            {
                throw new ArgumentException("Radar Power Usage not valid", "xml['power']");
            }
            if (Int32.TryParse(maxRangeAttribute.Value, out maxRange) == false)
            {
                throw new ArgumentException("Radar Max Range not valid", "xml['max_range']");
            }

            this.SensorLevel = level;
            this.Power = power;
            this.MaxRange = maxRange;
        }
    }
}
