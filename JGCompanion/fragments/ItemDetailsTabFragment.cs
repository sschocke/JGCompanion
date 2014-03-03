using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.View;
using Android.Os;
using Android.Widget;
using Android.Content;

namespace JGCompanion
{
    public class ItemDetailsTabFragment : Fragment
    {
        public Item details;

        public ItemDetailsTabFragment(Item details)
        {
            this.details = details;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            if (details == null)
            {
                return inflater.Inflate(R.Layouts.ItemDetailsTabFragment_Blank_Layout, container, false);
            }

            View view;
            switch (details.ItemType)
            {
                case ItemType.COMMODITY:
                    view = inflater.Inflate(R.Layouts.ItemDetailsTabFragment_Commodity_Layout, container, false);
                    break;
                case ItemType.ECM:
                    view = inflater.Inflate(R.Layouts.ItemDetailsTabFragment_ECM_Layout, container, false);
                    break;
                case ItemType.SHIELD:
                    view = inflater.Inflate(R.Layouts.ItemDetailsTabFragment_Shield_Layout, container, false);
                    break;
                case ItemType.CAPACITOR:
                    view = inflater.Inflate(R.Layouts.ItemDetailsTabFragment_Capacitor_Layout, container, false);
                    break;
                case ItemType.ENGINE:
                    view = inflater.Inflate(R.Layouts.ItemDetailsTabFragment_Engine_Layout, container, false);
                    break;
                case ItemType.POWERPLANT:
                    view = inflater.Inflate(R.Layouts.ItemDetailsTabFragment_PowerPlant_Layout, container, false);
                    break;
                case ItemType.RADAR:
                    view = inflater.Inflate(R.Layouts.ItemDetailsTabFragment_Radar_Layout, container, false);
                    break;
                case ItemType.MODX:
                    view = inflater.Inflate(R.Layouts.ItemDetailsTabFragment_MODx_Layout, container, false);
                    break;
                case ItemType.GUN:
                    view = inflater.Inflate(R.Layouts.ItemDetailsTabFragment_Gun_Layout, container, false);
                    break;
                case ItemType.MISSILE:
                    view = inflater.Inflate(R.Layouts.ItemDetailsTabFragment_Missile_Layout, container, false);
                    break;
                default:
                    throw new Exception("Item Type not supported");
            }

            Refresh(view);
            return view;
        }

        private void Refresh(View view)
        {
            var description = view.FindViewById<TextView>(R.Ids.txtItemDetailDescription);
            description.Text = details.Description;
            var classification = view.FindViewById<TextView>(R.Ids.txtItemDetailClassification);
            classification.Text = details.Classification;
            switch (details.ItemType)
            {
                case ItemType.COMMODITY:
                    CommodityDetails(view, details);
                    break;
                case ItemType.ECM:
                    ECMDetails(view, details);
                    break;
                case ItemType.SHIELD:
                    ShieldDetails(view, details);
                    break;
                case ItemType.CAPACITOR:
                    CapacitorDetails(view, details);
                    break;
                case ItemType.ENGINE:
                    EngineDetails(view, details);
                    break;
                case ItemType.POWERPLANT:
                    PowerPlantDetails(view, details);
                    break;
                case ItemType.RADAR:
                    RadarDetails(view, details);
                    break;
                case ItemType.MODX:
                    MODxDetails(view, details);
                    break;
                case ItemType.GUN:
                    GunDetails(view, details);
                    break;
                case ItemType.MISSILE:
                    MissileDetails(view, details);
                    break;
            }
            var components = view.FindViewById<ListView>(R.Ids.listItemDetailComponents);
            var componentsHeading = view.FindViewById<TextView>(R.Ids.headingItemDetailComponents);
            if (details.Components.Count < 1)
            {
                components.Visibility = View.GONE;
                componentsHeading.Visibility = View.GONE;
            }
            else
            {
                components.Adapter = new ItemComponentsAdapter(this.Activity, details.Components, false);
                components.ItemClick += new EventHandler<ItemClickEventArgs>(components_ItemClick);
                MainActivity.setListViewHeightBasedOnChildren(components);
            }
        }

        void components_ItemClick(object sender, ItemClickEventArgs e)
        {
            var listview = sender as ListView;
            var adapter = listview.Adapter as ItemComponentsAdapter;
            var t = adapter.GetItem(e.Position);

            var itemDetails = new Intent(this.Activity, typeof(ItemDetailActivity));
            itemDetails.PutExtra("itemID", t.ItemID);
            itemDetails.PutExtra("itemName", t.Name);
            StartActivity(itemDetails);
        }

        private void EquipDetails(View view, Item details)
        {
            Equipment equip = details as Equipment;
            if (equip == null)
                return;

            var level = view.FindViewById<TextView>(R.Ids.txtItemDetailTechLevel);
            level.Text = equip.TechLevel.ToString();
            var manufacturer = view.FindViewById<TextView>(R.Ids.txtItemDetailManufacturer);
            manufacturer.Text = equip.Manufacturer;
            var mass = view.FindViewById<TextView>(R.Ids.txtItemDetailMass);
            mass.Text = equip.Mass.ToString("N0");
            var size = view.FindViewById<TextView>(R.Ids.txtItemDetailSize);
            size.Text = equip.Size.ToString("N0");
        }

        private void ECMDetails(View view, Item details)
        {
            ECM ecm = details as ECM;
            if (ecm == null)
                return;

            EquipDetails(view, ecm);
            var sensorLevel = view.FindViewById<TextView>(R.Ids.txtItemDetailSensorLevel);
            sensorLevel.Text = ecm.SensorLevel.ToString();
            var power = view.FindViewById<TextView>(R.Ids.txtItemDetailPowerUsage);
            if (ecm.Power > 0)
            {
                power.Text = (ecm.Power / 1000.0).ToString("N1") + " K";
            }
            else
            {
                power.Text = "Varies";
            }
        }
        private void ShieldDetails(View view, Item details)
        {
            Shield shield = details as Shield;
            if (shield == null)
                return;

            EquipDetails(view, shield);
            var efficiency = view.FindViewById<TextView>(R.Ids.txtItemDetailEfficiency);
            efficiency.Text = shield.Efficiency.ToString();
            var maxRegen = view.FindViewById<TextView>(R.Ids.txtItemDetailMaxRegenRate);
            maxRegen.Text = (shield.MaxRegenRate / 1000.0).ToString("N1") + " K";
            var maxDeflect = view.FindViewById<TextView>(R.Ids.txtItemDetailMaxDeflect);
            maxDeflect.Text = (shield.MaxDeflect / 1000.0).ToString("N1") + " K";
            var baseRate = view.FindViewById<TextView>(R.Ids.txtItemDetailBaseRate);
            baseRate.Text = shield.BaseRate.ToString();
        }
        private void CapacitorDetails(View view, Item details)
        {
            Capacitor cap = details as Capacitor;
            if (cap == null)
                return;

            EquipDetails(view, cap);
            var efficiency = view.FindViewById<TextView>(R.Ids.txtItemDetailEfficiency);
            efficiency.Text = cap.Efficiency.ToString();
            var capacity = view.FindViewById<TextView>(R.Ids.txtItemDetailCapacity);
            capacity.Text = (cap.Capacity / 1000.0).ToString("N1") + " K";
        }
        private void EngineDetails(View view, Item details)
        {
            Engine engine = details as Engine;
            if (engine == null)
                return;

            EquipDetails(view, engine);
            var efficiency = view.FindViewById<TextView>(R.Ids.txtItemDetailEfficiency);
            efficiency.Text = engine.Efficiency.ToString();
            var maxThrust = view.FindViewById<TextView>(R.Ids.txtItemDetailMaxThrust);
            maxThrust.Text = (engine.MaxThrust / 1000.0).ToString("N1") + " K";
        }
        private void PowerPlantDetails(View view, Item details)
        {
            PowerPlant powerplant = details as PowerPlant;
            if (powerplant == null)
                return;

            EquipDetails(view, powerplant);
            var output = view.FindViewById<TextView>(R.Ids.txtItemDetailOutput);
            output.Text = (powerplant.Output / 1000.0).ToString("N1") + " K";
        }
        private void RadarDetails(View view, Item details)
        {
            Radar ecm = details as Radar;
            if (ecm == null)
                return;

            EquipDetails(view, ecm);
            var sensorLevel = view.FindViewById<TextView>(R.Ids.txtItemDetailSensorLevel);
            sensorLevel.Text = ecm.SensorLevel.ToString();
            var power = view.FindViewById<TextView>(R.Ids.txtItemDetailPowerUsage);
            power.Text = (ecm.Power / 1000.0).ToString("N1") + " K";
            var maxRange = view.FindViewById<TextView>(R.Ids.txtItemDetailMaxRange);
            maxRange.Text = ecm.MaxRange.ToString("N0");
        }
        private void MODxDetails(View view, Item details)
        {
            MODx modx = details as MODx;
            if (modx == null)
                return;

            EquipDetails(view, modx);
        }
        private void GunDetails(View view, Item details)
        {
            Gun gun = details as Gun;
            if (gun == null)
                return;

            EquipDetails(view, gun);
            var delay = view.FindViewById<TextView>(R.Ids.txtItemDetailDelay);
            delay.Text = gun.Delay.ToString();
            var damage = view.FindViewById<TextView>(R.Ids.txtItemDetailDamage);
            damage.Text = (gun.Damage / 1000.0).ToString("N1") + " K";
            var energyUse = view.FindViewById<TextView>(R.Ids.txtItemDetailEnergyUse);
            energyUse.Text = (gun.EnergyUse / 1000.0).ToString("N1") + " K";
            var ammo = view.FindViewById<TextView>(R.Ids.txtItemDetailAmmo);
            ammo.Text = gun.Ammo.ToString();
            var velocity = view.FindViewById<TextView>(R.Ids.txtItemDetailVelocity);
            velocity.Text = gun.Velocity.ToString("N0");
            var life = view.FindViewById<TextView>(R.Ids.txtItemDetailLife);
            life.Text = gun.Life.ToString("N0");
            var range = view.FindViewById<TextView>(R.Ids.txtItemDetailRange);
            range.Text = gun.Range.ToString("N0");
        }
        private void MissileDetails(View view, Item details)
        {
            Missile missile = details as Missile;
            if (missile == null)
                return;

            EquipDetails(view, missile);
            var drag = view.FindViewById<TextView>(R.Ids.txtItemDetailDrag);
            drag.Text = missile.Drag.ToString();
            var damage = view.FindViewById<TextView>(R.Ids.txtItemDetailDamage);
            damage.Text = (missile.Damage / 1000.0).ToString("N1") + " K";
            var thrust = view.FindViewById<TextView>(R.Ids.txtItemDetailThrust);
            thrust.Text = missile.Thrust.ToString("N0");
            var maxPitch = view.FindViewById<TextView>(R.Ids.txtItemDetailMaxPitch);
            maxPitch.Text = missile.MaxPitch.ToString("N0");
            var maxYaw = view.FindViewById<TextView>(R.Ids.txtItemDetailMaxYaw);
            maxYaw.Text = missile.MaxYaw.ToString("N0");
            var life = view.FindViewById<TextView>(R.Ids.txtItemDetailLife);
            life.Text = missile.Life.ToString("N0");
        }

        private void CommodityDetails(View view, Item details)
        {
            Commodity commod = details as Commodity;
            if (commod == null)
                return;

            var level = view.FindViewById<TextView>(R.Ids.txtItemDetailTechLevel);
            level.Text = commod.TechLevel.ToString();
            var abbr = view.FindViewById<TextView>(R.Ids.txtItemDetailAbbr);
            abbr.Text = commod.Abbr;
            var mass = view.FindViewById<TextView>(R.Ids.txtItemDetailMass);
            mass.Text = commod.Mass.ToString("N0");
            var melt = view.FindViewById<TextView>(R.Ids.txtItemDetailMeltingPoint);
            if (Double.IsNaN(commod.MeltingPoint) == true)
            {
                melt.Text = "N/A";
            }
            else
            {
                melt.Text = commod.MeltingPoint.ToString("N0");
            }
            var grav = view.FindViewById<TextView>(R.Ids.txtItemDetailGraviticSig);
            if (Double.IsNaN(commod.GraviticSig) == true)
            {
                grav.Text = "N/A";
            }
            else
            {
                grav.Text = commod.GraviticSig.ToString("N1");
            }
        }
    }
}
