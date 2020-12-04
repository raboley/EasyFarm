using System;
using System.Collections.Generic;
using System.Linq;
using EliteMMO.API;
using MemoryAPI;
using static EliteMMO.API.EliteAPI;

namespace FinalFantasyXI.Classes
{

    public class Inventory : IInventory
    {
        private IMemoryAPI _api;

        public Inventory(IMemoryAPI api)
        {
            _api = api;
        }


        
        public bool InventoryIsFull(int InventoryContainerId = 0)
        {
            if (_api.Inventory.GetContainerCount(InventoryContainerId) == _api.Inventory.GetContainerMaxCount(InventoryContainerId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HaveItemInInventoryContainer(string itemPattern, int InventoryContainerId = 0)
        {
            var itemInInventory = GetMatchingItemsFromContainer(itemPattern);
            if (itemInInventory.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
        
        public List<IItem> GetMatchingItemsFromContainer(string itemPattern, int InventoryContainerId = 0)
        {
            List<IItem> items = GetItemsFromContainer();
            return items.Where(item => item.Name[0].ToLower().Contains(itemPattern.ToLower())).ToList();
        }

        public List<IItem> GetItemsFromContainer(int InventoryContainerId = 0)
        {
            List<IItem> items = new List<IItem>();
            var maxCount = _api.Inventory.GetContainerMaxCount(InventoryContainerId);
            for (uint i = 0; i < maxCount; i++)
            {
                var hasItem = _api.Inventory.GetContainerItem(InventoryContainerId, (int)i);
                if (hasItem.Id == 0)
                {
                    continue;
                }
                var item = _api.Resource.GetItem(hasItem.Id);
                items.Add(item);
            }

            return items;
        }

        public int GetCountOfItemsInContainer(string itemPattern, int InventoryContainerId = 0)
        {
            int count = 0;

            List<IItem> items = new List<IItem>();
            var maxCount = _api.Inventory.GetContainerMaxCount(InventoryContainerId);
            for (uint i = 0; i < maxCount; i++)
            {
                var hasItem = _api.Inventory.GetContainerItem(InventoryContainerId, (int)i);
                if (hasItem.Id == 0)
                {
                    continue;
                }
                
                var item = _api.Resource.GetItem(hasItem.Id);
                var itemName = item.Name[0];
                if (itemName.ToLower().Contains(itemPattern.ToLower()))
                {
                    count += (int)hasItem.Count;
                }
            }

            return count;
        }

        public EquipmentItem GetEquipmentItemFromItem(IItem item)
        {
           var equipmentItem = new EquipmentItem();

           equipmentItem.Name = item.Name[0];
           

           foreach (var equipSlot in  Enum.GetNames(typeof(EquipSlots)).Cast<string>())
           {
               EliteMMO.API.EquipmentSlotMask eliteEnum = (EquipmentSlotMask) Enum.Parse(typeof(EliteMMO.API.EquipmentSlotMask), equipSlot);
               if (CanSlotEquipItem(eliteEnum, item))
                   equipmentItem.EquipAbleInSlots.Add((EquipSlots) Enum.Parse(typeof(EquipSlots), equipSlot));
           }

           foreach (var job in Enum.GetNames(typeof(Job)).Cast<string>())
           {
               EliteMMO.API.JobMask eliteEnum = GetJobMaskFromJobEnum(job);
                if (CanJobEquipItem(eliteEnum, item))
                    equipmentItem.EquipAbleByJobs.Add((Job) Enum.Parse(typeof(Job), job));
           }


           return equipmentItem;
        }

        private static JobMask GetJobMaskFromJobEnum(string job)
        {
            var jobEnum = (Job) Enum.Parse(typeof(Job), job);
            int jobInt = (int) jobEnum;
            var jobMaskName = Enum.GetName(typeof(JobToEliteApiJobMap), jobInt);
            return (JobMask) Enum.Parse(typeof(EliteMMO.API.JobMask), jobMaskName ?? throw new Exception("Can't find matching Jobmask for job: " + job));
        }

        enum JobToEliteApiJobMap
        {
            None = 0,
            WAR = 1,
            MNK = 2,
            WHM = 3,
            BLM = 4, // 0x00000010
            RDM = 5, // 0x00000020
            THF = 6, // 0x00000040
            PLD = 7, // 0x00000080
            DRK = 8, // 0x00000100
            BST = 9, // 0x00000200
            BRD = 10, // 0x00000400
            RNG = 11, // 0x00000800
            SAM = 12, // 0x00001000
            NIN = 13, // 0x00002000
            DRG = 14, // 0x00004000
            SMN = 15, // 0x00008000
            BLU = 16, // 0x00010000
            COR = 17, // 0x00020000
            PUP = 18, // 0x00040000
            DNC = 19, // 0x00080000
            SCH = 20, // 0x00100000
            GEO = 21, // 0x00200000
            RUN = 22, // 0x00400000
        }
        

        // Helper methods /////////////////////////////////////////////////////////////

        private static bool CanJobEquipItem(byte Job, EliteMMO.API.EliteAPI.IItem Item)
        {
            return CanJobEquipItem((uint)Math.Pow(2, Job), Item);
        }

        private static bool CanJobEquipItem(EliteMMO.API.JobMask Job, EliteMMO.API.EliteAPI.IItem Item)
        {
            return CanJobEquipItem((uint)Job, Item);
        }

        private static bool CanJobEquipItem(uint JobMask, EliteMMO.API.EliteAPI.IItem Item)
        {
            return ((Item.Slots & JobMask) != 0);
        }

        private static bool CanSlotEquipItem(byte Slot, EliteMMO.API.EliteAPI.IItem Item)
        {
            return CanSlotEquipItem((uint)Math.Pow(2, Slot), Item);
        }

        public static bool CanSlotEquipItem(EliteMMO.API.EquipmentSlotMask Slot, EliteMMO.API.EliteAPI.IItem Item)
        {
            return CanSlotEquipItem((uint)Slot, Item);
        }

        private static bool CanSlotEquipItem(uint SlotMask, EliteMMO.API.EliteAPI.IItem Item)
        {
            return ((Item.Slots & SlotMask) != 0);
        }
        // Helper methods /////////////////////////////////////////////////////////////
    }

    public class EquipmentItem
    {
        public List<EquipSlots> EquipAbleInSlots { get; set; } = new List<EquipSlots>();
        public List<Job> EquipAbleByJobs { get; set; } = new List<Job>();
        public string Name { get; set; }
    }
}
