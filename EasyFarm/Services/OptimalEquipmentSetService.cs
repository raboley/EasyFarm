using EasyFarm.Classes;
using EasyFarm.Context;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Services
{
    public class OptimalEquipmentSetService
    {
        public EquipmentSet GetOptimalEquipment(IGameContext context)
        {
            return new EquipmentSet();
        }

        public List<EquipmentSet> GetAllEquipmentSets(string jsonFileFullPath)
        {
            using (StreamReader r = new StreamReader(jsonFileFullPath))
            {
                string json = r.ReadToEnd();
                List<OptimalSetJson> items = JsonConvert.DeserializeObject<List<OptimalSetJson>>(json);
            
                List<EquipmentSet> equipmentSets = AdaptJsonToEquipmentSet(items);
                return equipmentSets;
            }
        }

        private List<EquipmentSet> AdaptJsonToEquipmentSet(List<OptimalSetJson> listFromJson)
        {
            if(listFromJson == null)
            {
                throw new ArgumentNullException("listFromJson", "listFromJson is null.");
            }

            List<EquipmentSet> equipmentSets = new List<EquipmentSet>();

            foreach (OptimalSetJson equipmentSet in listFromJson)
            {
                EquipmentSet set = new EquipmentSet();

                set.Level = equipmentSet.Level;
                if (equipmentSet.Main == null)
                {
                    set.Main = new EquipableItem(MemoryAPI.EquipSlots.Main);
                }
                else
                {
                    set.Main = new EquipableItem(equipmentSet.Main, MemoryAPI.EquipSlots.Main);
                }

                if (equipmentSet.Sub == null)
                {
                    set.Sub = new EquipableItem(MemoryAPI.EquipSlots.Sub);
                }
                else
                {
                    set.Sub = new EquipableItem(equipmentSet.Sub, MemoryAPI.EquipSlots.Sub);
                }

                if (equipmentSet.Range == null)
                {
                    set.Range = new EquipableItem(MemoryAPI.EquipSlots.Range);
                }
                else
                {
                    set.Range = new EquipableItem(equipmentSet.Range, MemoryAPI.EquipSlots.Range);
                }

                if (equipmentSet.Ammo == null)
                {
                    set.Ammo = new EquipableItem(MemoryAPI.EquipSlots.Ammo);
                }
                else
                {
                    set.Ammo = new EquipableItem(equipmentSet.Ammo, MemoryAPI.EquipSlots.Ammo);
                }

                if (equipmentSet.Head == null)
                {
                    set.Head = new EquipableItem(MemoryAPI.EquipSlots.Head);
                }
                else
                {
                    set.Head = new EquipableItem(equipmentSet.Head, MemoryAPI.EquipSlots.Head);
                }

                if (equipmentSet.Neck == null)
                {
                    set.Neck = new EquipableItem(MemoryAPI.EquipSlots.Neck);
                }
                else
                {
                    set.Neck = new EquipableItem(equipmentSet.Neck, MemoryAPI.EquipSlots.Neck);
                }

                if (equipmentSet.Body == null)
                {
                    set.Body = new EquipableItem(MemoryAPI.EquipSlots.Body);
                }
                else
                {
                    set.Body = new EquipableItem(equipmentSet.Body, MemoryAPI.EquipSlots.Body);
                }

                if (equipmentSet.Hands == null)
                {
                    set.Hands = new EquipableItem(MemoryAPI.EquipSlots.Hands);
                }
                else
                {
                    set.Hands = new EquipableItem(equipmentSet.Hands, MemoryAPI.EquipSlots.Hands);
                }

                if (equipmentSet.Waist == null)
                {
                    set.Waist = new EquipableItem(MemoryAPI.EquipSlots.Waist);
                }
                else
                {
                    set.Waist = new EquipableItem(equipmentSet.Waist, MemoryAPI.EquipSlots.Waist);
                }

                if (equipmentSet.Back == null)
                {
                    set.Back = new EquipableItem(MemoryAPI.EquipSlots.Back);
                }
                else
                {
                    set.Back = new EquipableItem(equipmentSet.Back, MemoryAPI.EquipSlots.Back);
                }

                if (equipmentSet.Legs == null)
                {
                    set.Legs = new EquipableItem(MemoryAPI.EquipSlots.Legs);
                }
                else
                {
                    set.Legs = new EquipableItem(equipmentSet.Legs, MemoryAPI.EquipSlots.Legs);
                }

                if (equipmentSet.Feet == null)
                {
                    set.Feet = new EquipableItem(MemoryAPI.EquipSlots.Feet);
                }
                else
                {
                    set.Feet = new EquipableItem(equipmentSet.Feet, MemoryAPI.EquipSlots.Feet);
                }

                if (equipmentSet.Ring1 == null)
                {
                    set.Ring1 = new EquipableItem(MemoryAPI.EquipSlots.Ring1);
                }
                else
                {
                    set.Ring1 = new EquipableItem(equipmentSet.Ring1, MemoryAPI.EquipSlots.Ring1);
                }

                if (equipmentSet.Ring2 == null)
                {
                    set.Ring2 = new EquipableItem(MemoryAPI.EquipSlots.Ring2);
                }
                else
                {
                    set.Ring2 = new EquipableItem(equipmentSet.Ring2, MemoryAPI.EquipSlots.Ring2);
                }

                if (equipmentSet.Ear1 == null)
                {
                    set.Ear1 = new EquipableItem(MemoryAPI.EquipSlots.Ear1);
                }
                else
                {
                    set.Ear1 = new EquipableItem(equipmentSet.Ear1, MemoryAPI.EquipSlots.Ear1);
                }

                if (equipmentSet.Ear2 == null)
                {
                    set.Ear2 = new EquipableItem(MemoryAPI.EquipSlots.Ear2);
                }
                else
                {
                    set.Ear2 = new EquipableItem(equipmentSet.Ear2, MemoryAPI.EquipSlots.Ear2);
                }

                equipmentSets.Add(set);
            }
            
            return equipmentSets;
        }
    }

    public class OptimalSetJson
    {
        public int Level { get; set; }
        public string Main { get; set; }
        public string Sub { get; set; }
        public string Range { get; set; }
        public string Ammo { get; set; }
        public string Head { get; set; }
        public string Neck { get; set; }
        public string Body { get; set; }
        public string Hands { get; set; }
        public string Waist { get; set; }
        public string Back { get; set; }
        public string Legs { get; set; }
        public string Feet { get; set; }
        public string Ring1 { get; set; }
        public string Ring2 { get; set; }
        public string Ear1 { get; set; }
        public string Ear2 { get; set; }
    }
}
