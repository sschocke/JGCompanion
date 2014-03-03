using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public enum Factions
    {
        SOLRAIN = 2,
        QUANTAR = 3,
        OCTAVIUS = 1,
        AMANANTH = 4,
        HYPERIAL = 5,
        UNREGULATED = 6
    }

    public class Pilot
    {
        public Int64 PilotID { get; set; }
        public string PilotName { get; set; }
        public Factions Faction { get; set; }

        public Pilot(XElement xml)
        {
            Int64 id;

            XAttribute idAttribute = xml.Attribute("id");
            XAttribute callsignAttribute = xml.Attribute("callsign");
            XAttribute factionAttribute = xml.Attribute("faction");

            if (Int64.TryParse(idAttribute.Value, out id) == false)
            {
                throw new ArgumentException("Pilot ID not valid", "xml['id']");
            }

            this.PilotID = id;
            this.PilotName = callsignAttribute.Value; ;
            this.Faction = FromString(factionAttribute.Value);
        }

        protected Pilot()
        {
        }

        public static Factions FromString(string faction)
        {
            switch (faction.ToLower())
            {
                case "quantar":
                    return Factions.QUANTAR;
                case "solrain":
                    return Factions.SOLRAIN;
                case "octavius":
                    return Factions.OCTAVIUS;
                case "amananth":
                    return Factions.AMANANTH;
                case "hyperial":
                    return Factions.HYPERIAL;
                case "unregulated":
                    return Factions.UNREGULATED;
                default:
                    throw new ArgumentOutOfRangeException("faction", "Not a valid faction!!");
            }
        }
    }
}
