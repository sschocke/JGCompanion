using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public class PilotStats
    {
        public class PilotStat : Pilot
        {
            public Int64 Value { get; set; }

            public PilotStat(XElement xml)
                : base(xml)
            {
                Int64 value;

                XAttribute valueAttribute = xml.Attribute("value");
                if (Int64.TryParse(valueAttribute.Value, out value) == false)
                {
                    throw new ArgumentException("Pilot Stat Value not valid", "xml['value']");
                }

                this.Value = value;
            }
        }

        public PilotStat[] FluxKills { get; set; }
        public PilotStat[] PilotKills { get; set; }
        public PilotStat[] Deaths { get; set; }
        public PilotStat[] Credits { get; set; }
        public PilotStat[] MissionsCompleted { get; set; }
        public PilotStat[] DutyHours { get; set; }
        public PilotStat[] Experience { get; set; }
    }
}
