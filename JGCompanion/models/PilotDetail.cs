using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Text;
using System.Xml.Linq;

namespace JGCompanion
{
    public enum Registry
    {
        ROOKIE = 1,
        CIVILIAN = 2,
        MILITARY = 3,
        PIRATE = 4,
        MERCENARY = 5
    }

    public class PilotDetail : Pilot
    {
        public bool Online { get; set; }
        public bool Docked { get; set; }
        public Registry Registration { get; set; }
        public string Squad { get; set; }
        public Int32 Rank { get; set; }
        public Int64 Experience { get; set; }
        public double DutyHours { get; set; }
        public Int64 Credits { get; set; }
        public Int16 RatingOct { get; set; }
        public Int16 RatingQuant { get; set; }
        public Int16 RatingSol { get; set; }
        public Int16 RatingAman { get; set; }
        public Int16 RatingHyp { get; set; }
        public Int32 KillsOct { get; set; }
        public Int32 KillsQuant { get; set; }
        public Int32 KillsSol { get; set; }
        public Int32 KillsConflux { get; set; }
        public Int64 BountyCollected { get; set; }
        public Int32 Deaths { get; set; }
        public Int32 Launches { get; set; }
        public Int32 Landings { get; set; }
        public Int32 Disconnects { get; set; }
        public Int32 MissilesFired { get; set; }
        public Int32 MissilesHit { get; set; }
        public Int32 ShotsFired { get; set; }
        public Int32 ShotsHit { get; set; }
        public Int32 MissionsFlown { get; set; }
        public Int32 MissionsCompleted { get; set; }
        public DateTime LastOnline { get; set; }

        public PilotDetail(XElement xml)
            : base()
        {
            Int64 lastOnline = 0;

            this.PilotID = Int64.Parse(xml.Element("id").Value);
            this.PilotName = xml.Element("callsign").Value;
            this.Faction = FromString(xml.Element("faction").Value);
            this.Online = Boolean.Parse(xml.Element("online").Value);
            this.Docked = Boolean.Parse(xml.Element("docked").Value);
            this.Registration = RegFromString(xml.Element("registration").Value);
            this.Squad = xml.Element("squad").Value;
            this.Rank = Int32.Parse(xml.Element("rank").Value);
            this.Experience = Int64.Parse(xml.Element("experience").Value);
            this.DutyHours = Double.Parse(xml.Element("dutyhours").Value);
            this.Credits = Int64.Parse(xml.Element("credits").Value);
            this.RatingOct = Int16.Parse(xml.Element("rating_oct").Value);
            this.RatingQuant = Int16.Parse(xml.Element("rating_quant").Value);
            this.RatingSol = Int16.Parse(xml.Element("rating_sol").Value);
            this.RatingAman = Int16.Parse(xml.Element("rating_aman").Value);
            this.RatingHyp = Int16.Parse(xml.Element("rating_hyp").Value);
            this.KillsOct = Int32.Parse(xml.Element("kills_oct").Value);
            this.KillsQuant = Int32.Parse(xml.Element("kills_quant").Value);
            this.KillsSol = Int32.Parse(xml.Element("kills_sol").Value);
            this.KillsConflux = Int32.Parse(xml.Element("kills_conflux").Value);
            this.BountyCollected = Int64.Parse(xml.Element("bounty_collected").Value);
            this.Deaths = Int32.Parse(xml.Element("deaths").Value);
            this.Launches = Int32.Parse(xml.Element("launches").Value);
            this.Landings = Int32.Parse(xml.Element("landings").Value);
            this.Disconnects = Int32.Parse(xml.Element("disconnects").Value);
            this.MissilesFired = Int32.Parse(xml.Element("missiles_fired").Value);
            this.MissilesHit = Int32.Parse(xml.Element("missiles_hit").Value);
            this.ShotsFired = Int32.Parse(xml.Element("shots_fired").Value);
            this.ShotsHit = Int32.Parse(xml.Element("shots_hit").Value);
            this.MissionsFlown = Int32.Parse(xml.Element("missions_flown").Value);
            this.MissionsCompleted = Int32.Parse(xml.Element("missions_completed").Value);
            this.LastOnline = new DateTime(1970, 1, 1);
            lastOnline = Int64.Parse(xml.Element("last_online").Value);
            this.LastOnline = this.LastOnline.AddSeconds(lastOnline);
            this.LastOnline = this.LastOnline.ToLocalTime();
        }

        public Int32 TotalPilotKills
        {
            get { return KillsOct + KillsQuant + KillsSol; }
        }
        public double BountyPerKill
        {
            get
            {
                if (TotalPilotKills < 1)
                {
                    return 0;
                }

                return (double)BountyCollected / (double)TotalPilotKills;
            }
        }
        public double KillRatio
        {
            get
            {
                if (Deaths == 0)
                {
                    if (TotalPilotKills == 0)
                    {
                        return 0.0;
                    }

                    return 100.0;
                }

                return (double)TotalPilotKills / (double)Deaths * 100.0;
            }
        }
        public double GunAccuracy
        {
            get
            {
                if (ShotsFired == 0)
                {
                    return 0.0;
                }

                return (double)ShotsHit / (double)ShotsFired * 100.0;
            }
        }
        public double MissileAccuracy
        {
            get
            {
                if (MissilesFired == 0)
                {
                    return 0.0;
                }

                return (double)MissilesHit / (double)MissilesFired * 100.0;
            }
        }
        public double InsuranceRating
        {
            get
            {
                if (MissionsFlown == 0)
                {
                    return 100.0;
                }

                return (double)MissionsCompleted / (double)MissionsFlown * 100.0;
            }
        }
        public Int64 ExperienceNextRank
        {
            get 
            {
                if (Rank == 50) return 0;

                long totalExpNextRank = rankRequirement[this.Rank + 1];

                return totalExpNextRank - this.Experience;
            }
        }

        private static Dictionary<Int32, Int64> rankRequirement;

        static PilotDetail()
        {
            rankRequirement = new Dictionary<Int32, Int64>();
            rankRequirement.Add(0, 0);
            rankRequirement.Add(1, 2000);
            rankRequirement.Add(2, 4000);
            rankRequirement.Add(3, 8800);
            rankRequirement.Add(4, 15700);
            rankRequirement.Add(5, 25000);
            rankRequirement.Add(6, 37000);
            rankRequirement.Add(7, 51600);
            rankRequirement.Add(8, 69000);
            rankRequirement.Add(9, 89300);
            rankRequirement.Add(10, 112600);
            rankRequirement.Add(11, 140000);
            rankRequirement.Add(12, 169000);
            rankRequirement.Add(13, 202000);
            rankRequirement.Add(14, 240000);
            rankRequirement.Add(15, 280000);
            rankRequirement.Add(16, 325000);
            rankRequirement.Add(17, 372000);
            rankRequirement.Add(18, 424000);
            rankRequirement.Add(19, 480000);
            rankRequirement.Add(20, 540000);
            rankRequirement.Add(21, 605000);
            rankRequirement.Add(22, 675000);
            rankRequirement.Add(23, 750000);
            rankRequirement.Add(24, 828000);
            rankRequirement.Add(25, 911000);
            rankRequirement.Add(26, 1000000);
            rankRequirement.Add(27, 1100000);
            rankRequirement.Add(28, 1200000);
            rankRequirement.Add(29, 1300000);
            rankRequirement.Add(30, 1407000);
            rankRequirement.Add(31, 1520000);
            rankRequirement.Add(32, 1640000);
            rankRequirement.Add(33, 1770000);
            rankRequirement.Add(34, 1910000);
            rankRequirement.Add(35, 2065000);
            rankRequirement.Add(36, 2230000);
            rankRequirement.Add(37, 2400000);
            rankRequirement.Add(38, 2650000);
            rankRequirement.Add(39, 3000000);
            rankRequirement.Add(40, 3300000);
            rankRequirement.Add(41, 3700000);
            rankRequirement.Add(42, 4200000);
            rankRequirement.Add(43, 4700000);
            rankRequirement.Add(44, 5300000);
            rankRequirement.Add(45, 5900000);
            rankRequirement.Add(46, 6500000);
            rankRequirement.Add(47, 7500000);
            rankRequirement.Add(48, 8500000);
            rankRequirement.Add(49, 9500000);
            rankRequirement.Add(50, 12000000);
        }

        public static Registry RegFromString(string reg)
        {
            switch (reg)
            {
                case "TRI Rookie":
                    return Registry.ROOKIE;
                case "TRI Civilian":
                    return Registry.CIVILIAN;
                case "Military":
                    return Registry.MILITARY;
                case "Pirate":
                    return Registry.PIRATE;
                case "Mercenary":
                    return Registry.MERCENARY;
                default:
                    throw new ArgumentOutOfRangeException("reg", "Not a valid Registration string!!");
            }
        }

        public static string RegString(Registry registration)
        {
            switch (registration)
            {
                case Registry.ROOKIE:
                    return "TRI Rookie";
                case Registry.CIVILIAN:
                    return "TRI Civilian";
                case Registry.MILITARY:
                    return "Military";
                case Registry.PIRATE:
                    return "Pirate";
                case Registry.MERCENARY:
                    return "Mercenary";
                default:
                    throw new ArgumentOutOfRangeException("registration", "Not a valid TRI Registration status");
            }
        }

        public static string RankMatrix(Factions faction, int rank)
        {
            switch (faction)
            {
                case Factions.QUANTAR:
                    return quantarRankMatrix(rank);
                case Factions.SOLRAIN:
                    return solrainRankMatrix(rank);
                case Factions.OCTAVIUS:
                    return octavianRankMatrix(rank);
                default:
                    throw new ArgumentOutOfRangeException("faction", "Not a valid faction!");
            }
        }
        private static string quantarRankMatrix(int rank)
        {
            switch (rank)
            {
                case 0: return "Recruit [" + rank.ToString() + "]";
                case 1: return "Follower [" + rank.ToString() + "]";
                case 2: return "Believer [" + rank.ToString() + "]";
                case 3: return "Devotee [" + rank.ToString() + "]";
                case 4: return "Initiate [" + rank.ToString() + "]";
                case 5: return "Brahmin [" + rank.ToString() + "]";
                case 6: return "Disciple [" + rank.ToString() + "]";
                case 7: return "Deacon [" + rank.ToString() + "]";
                case 8: return "Harbinger [" + rank.ToString() + "]";
                case 9: return "Envoy [" + rank.ToString() + "]";
                case 10: return "Khoury [" + rank.ToString() + "]";
                case 11: return "Samirosta [" + rank.ToString() + "]";
                case 12: return "Concentist [" + rank.ToString() + "]";
                case 13: return "Morhista [" + rank.ToString() + "]";
                case 14: return "Phontista [" + rank.ToString() + "]";
                case 15: return "Culurista [" + rank.ToString() + "]";
                case 16: return "Audotist [" + rank.ToString() + "]";
                case 17: return "Flaourist [" + rank.ToString() + "]";
                case 18: return "Tatist [" + rank.ToString() + "]";
                case 19: return "Odroutist [" + rank.ToString() + "]";
                case 20: return "Knight [" + rank.ToString() + "]";
                case 21: return "Kundali [" + rank.ToString() + "]";
                case 22: return "Mentali [" + rank.ToString() + "]";
                case 23: return "Kintali [" + rank.ToString() + "]";
                case 24: return "Vasudeva [" + rank.ToString() + "]";
                case 25: return "Kuntasha [" + rank.ToString() + "]";
                case 26: return "Mentasha [" + rank.ToString() + "]";
                case 27: return "Kintasha [" + rank.ToString() + "]";
                case 28: return "Crusader [" + rank.ToString() + "]";
                case 29: return "Mahavashi [" + rank.ToString() + "]";
                case 30: return "Visionary [" + rank.ToString() + "]";
                case 31: return "Enlightened [" + rank.ToString() + "]";
                case 32: return "Paddashi [" + rank.ToString() + "]";
                case 33: return "Tribune [" + rank.ToString() + "]";
                case 34: return "Nasih [" + rank.ToString() + "]";
                case 35: return "Consul [" + rank.ToString() + "]";
                case 36: return "Tariq [" + rank.ToString() + "]";
                case 37: return "Enigma [" + rank.ToString() + "]";
                case 38: return "Ambador [" + rank.ToString() + "]";
                case 39: return "Guru [" + rank.ToString() + "]";
                case 40: return "Sakyan [" + rank.ToString() + "]";
                case 41: return "Fiqarun [" + rank.ToString() + "]";
                case 42: return "Ghazin [" + rank.ToString() + "]";
                case 43: return "Rafikan [" + rank.ToString() + "]";
                case 44: return "Hatiman [" + rank.ToString() + "]";
                case 45: return "Rahanan [" + rank.ToString() + "]";
                case 46: return "Asuran [" + rank.ToString() + "]";
                case 47: return "Shivan [" + rank.ToString() + "]";
                case 48: return "Hamalzan [" + rank.ToString() + "]";
                case 49: return "Quantar Alterion [" + rank.ToString() + "]";
                case 50: return "Quantar Optimus [" + rank.ToString() + "]";
                default:
                    return "UNKNOWN [" + rank.ToString() + "]";
            }
        }
        private static string solrainRankMatrix(int rank)
        {
            switch (rank)
            {
                case 0: return "Recruit [" + rank.ToString() + "]";
                case 1: return "Porter [" + rank.ToString() + "]";
                case 2: return "Apprentice [" + rank.ToString() + "]";
                case 3: return "Associate [" + rank.ToString() + "]";
                case 4: return "First Associate [" + rank.ToString() + "]";
                case 5: return "Master Associate [" + rank.ToString() + "]";
                case 6: return "Senior Associate [" + rank.ToString() + "]";
                case 7: return "Agent [" + rank.ToString() + "]";
                case 8: return "Senior Agent [" + rank.ToString() + "]";
                case 9: return "Lead Agent [" + rank.ToString() + "]";
                case 10: return "Provisioner [" + rank.ToString() + "]";
                case 11: return "First Provisioner [" + rank.ToString() + "]";
                case 12: return "Master Provisioner [" + rank.ToString() + "]";
                case 13: return "Senior Provisioner [" + rank.ToString() + "]";
                case 14: return "Cambist [" + rank.ToString() + "]";
                case 15: return "Master Cambist [" + rank.ToString() + "]";
                case 16: return "Senior Cambist [" + rank.ToString() + "]";
                case 17: return "Contractor [" + rank.ToString() + "]";
                case 18: return "Senior Contractor [" + rank.ToString() + "]";
                case 19: return "Lead Contractor [" + rank.ToString() + "]";
                case 20: return "Merchant [" + rank.ToString() + "]";
                case 21: return "First Merchant [" + rank.ToString() + "]";
                case 22: return "Master Merchant [" + rank.ToString() + "]";
                case 23: return "Senior Merchant [" + rank.ToString() + "]";
                case 24: return "Magnate [" + rank.ToString() + "]";
                case 25: return "Master Magnate [" + rank.ToString() + "]";
                case 26: return "Senior Magnate [" + rank.ToString() + "]";
                case 27: return "Specialist [" + rank.ToString() + "]";
                case 28: return "Master Specialist [" + rank.ToString() + "]";
                case 29: return "Arbiter [" + rank.ToString() + "]";
                case 30: return "Junior Executive [" + rank.ToString() + "]";
                case 31: return "Coordinator [" + rank.ToString() + "]";
                case 32: return "Chief Coordinator [" + rank.ToString() + "]";
                case 33: return "Regulator [" + rank.ToString() + "]";
                case 34: return "Senior Regulator [" + rank.ToString() + "]";
                case 35: return "Chief Regulator [" + rank.ToString() + "]";
                case 36: return "Vice President [" + rank.ToString() + "]";
                case 37: return "President [" + rank.ToString() + "]";
                case 38: return "Directorate [" + rank.ToString() + "]";
                case 39: return "Chief Executive [" + rank.ToString() + "]";
                case 40: return "Financier [" + rank.ToString() + "]";
                case 41: return "Master Financier [" + rank.ToString() + "]";
                case 42: return "Senior Financier [" + rank.ToString() + "]";
                case 43: return "Tycoon [" + rank.ToString() + "]";
                case 44: return "Master Tycoon [" + rank.ToString() + "]";
                case 45: return "Senior Tycoon [" + rank.ToString() + "]";
                case 46: return "Duke [" + rank.ToString() + "]";
                case 47: return "Baronet [" + rank.ToString() + "]";
                case 48: return "Baron [" + rank.ToString() + "]";
                case 49: return "Solrain Alterion [" + rank.ToString() + "]";
                case 50: return "Solrain Optimus [" + rank.ToString() + "]";
                default:
                    return "UNKNOWN [" + rank.ToString() + "]";
            }
        }
        private static string octavianRankMatrix(int rank)
        {
            switch (rank)
            {
                case 0: return "Recruit [" + rank.ToString() + "]";
                case 1: return "Private [" + rank.ToString() + "]";
                case 2: return "Specialist [" + rank.ToString() + "]";
                case 3: return "Corporal [" + rank.ToString() + "]";
                case 4: return "Sergeant [" + rank.ToString() + "]";
                case 5: return "Staff Sergeant [" + rank.ToString() + "]";
                case 6: return "Master Sergeant [" + rank.ToString() + "]";
                case 7: return "First Sergeant [" + rank.ToString() + "]";
                case 8: return "Sergeant Major [" + rank.ToString() + "]";
                case 9: return "Senior Sergeant [" + rank.ToString() + "]";
                case 10: return "Lancer [" + rank.ToString() + "]";
                case 11: return "First Lancer [" + rank.ToString() + "]";
                case 12: return "Master Lancer [" + rank.ToString() + "]";
                case 13: return "Senior Lancer [" + rank.ToString() + "]";
                case 14: return "Marksman [" + rank.ToString() + "]";
                case 15: return "First Marksman [" + rank.ToString() + "]";
                case 16: return "Master Marksman [" + rank.ToString() + "]";
                case 17: return "Senior Marksman [" + rank.ToString() + "]";
                case 18: return "Gunner [" + rank.ToString() + "]";
                case 19: return "Master Gunner [" + rank.ToString() + "]";
                case 20: return "Commander [" + rank.ToString() + "]";
                case 21: return "First Commander [" + rank.ToString() + "]";
                case 22: return "Master Commander [" + rank.ToString() + "]";
                case 23: return "Senior Commander [" + rank.ToString() + "]";
                case 24: return "Brigadier [" + rank.ToString() + "]";
                case 25: return "First Brigadier [" + rank.ToString() + "]";
                case 26: return "Master Brigadier [" + rank.ToString() + "]";
                case 27: return "Senior Brigadier [" + rank.ToString() + "]";
                case 28: return "Brigadier General [" + rank.ToString() + "]";
                case 29: return "General [" + rank.ToString() + "]";
                case 30: return "Mentor [" + rank.ToString() + "]";
                case 31: return "First Mentor [" + rank.ToString() + "]";
                case 32: return "Senior Mentor [" + rank.ToString() + "]";
                case 33: return "Preceptor [" + rank.ToString() + "]";
                case 34: return "First Preceptor [" + rank.ToString() + "]";
                case 35: return "High Preceptor [" + rank.ToString() + "]";
                case 36: return "Phaeton [" + rank.ToString() + "]";
                case 37: return "First Phaeton [" + rank.ToString() + "]";
                case 38: return "High Phaeton [" + rank.ToString() + "]";
                case 39: return "Empirius [" + rank.ToString() + "]";
                case 40: return "Praetorius [" + rank.ToString() + "]";
                case 41: return "First Praetorius [" + rank.ToString() + "]";
                case 42: return "High Praetorius [" + rank.ToString() + "]";
                case 43: return "Ardean [" + rank.ToString() + "]";
                case 44: return "First Ardean [" + rank.ToString() + "]";
                case 45: return "Master Ardean [" + rank.ToString() + "]";
                case 46: return "Vendictor [" + rank.ToString() + "]";
                case 47: return "First Vendictor [" + rank.ToString() + "]";
                case 48: return "High Vendictor [" + rank.ToString() + "]";
                case 49: return "Octavius Alterion [" + rank.ToString() + "]";
                case 50: return "Octavius Optimus [" + rank.ToString() + "]";
                default:
                    return "UNKNOWN";
            }
        }

        public static ISpanned PolRating(int pol)
        {
            if (pol >= 125)
            {
                return Html.FromHtml("<font color='#99ffff'>Worshipped</font>(" + pol.ToString() + ")");
            }
            else if (pol >= 116)
            {
                return Html.FromHtml("<font color='#33eeee'>Exalted</font>(" + pol.ToString() + ")");
            }
            else if (pol >= 101)
            {
                return Html.FromHtml("<font color='#33cccc'>Honored</font>(" + pol.ToString() + ")");
            }
            else if (pol >= 81)
            {
                return Html.FromHtml("<font color='#33bbbb'>Unfailing</font>(" + pol.ToString() + ")");
            }
            else if (pol >= 61)
            {
                return Html.FromHtml("<font color='#44aaaa'>Devoted</font>(" + pol.ToString() + ")");
            }
            else if (pol >= 41)
            {
                return Html.FromHtml("<font color='#449999'>Dedicated</font>(" + pol.ToString() + ")");
            }
            else if (pol >= 21)
            {
                return Html.FromHtml("<font color='#448888'>Dependable</font>(" + pol.ToString() + ")");
            }
            else if (pol >= 1)
            {
                return Html.FromHtml("<font color='#447777'>Legitimate</font>(" + pol.ToString() + ")");
            }
            else if (pol >= 0)
            {
                return Html.FromHtml("<font color='#999999'>Neutral</font>(" + pol.ToString() + ")");
            }
            else if (pol >= -1)
            {
                return Html.FromHtml("<font color='#bb8888'>Resistant</font>(" + pol.ToString() + ")");
            }
            else if (pol >= -20)
            {
                return Html.FromHtml("<font color='#bb6666'>Defiant</font>(" + pol.ToString() + ")");
            }
            else if (pol >= -40)
            {
                return Html.FromHtml("<font color='#cc4444'>Hostile</font>(" + pol.ToString() + ")");
            }
            else if (pol >= -60)
            {
                return Html.FromHtml("<font color='#cc2222'>Notorious</font>(" + pol.ToString() + ")");
            }
            else
            {
                return Html.FromHtml("<font color='#dd0000'>Feared</font>(" + pol.ToString() + ")");
            }
        }
    }
}
