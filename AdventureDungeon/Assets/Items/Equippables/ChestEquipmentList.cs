// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // Temprty until DB is up
// public record /*class*/ ChestEquipment
// {
//   // private (string ItemId, string Name, string Description) _equipment = {
//   // private enum _equipment = {
//   //   Legendary_BlackHoodie
//   // };
//   // public string ItemId { get; set; }
//   // public string Name { get; set; }
//   // public string Description { get; set; }
//   // public EQUIPPABLESLOT EquippableSlot { get; set; }
//   // public ItemRarity Rarity { get; set; }
//   // public GEARSETS GearSet { get; set; }
//   // public Texture2D ItemSprite { get; set; }

//   // public ChestEquipmentList() {}

//   public string ItemId;
//   public string Name;
//   public string Description;
//   public EQUIPPABLESLOT EquippableSlot;
//   public ItemRarity Rarity;
//   public GEARSETS GearSet;
//   // public Texture2D ItemSprite;
// }

// public static class ChestEquipmentList
// {
//   public static List<Equippable> GetChestEquipment() => new() 
//   {
//     new("Mythic_Chest_Black_Hoodie", "Black Hoodie", "It has markings of Syndicate on it", EQUIPPABLESLOT.Chest.ToString(), ItemRarity.Mythic.ToString(), 
//     10, 8, 2,
//     new List<ItemStats> {
//       new(ItemStats.STATTYPES.Health, ItemStats.STATVALUES.Add, 500f)
//     }, GEARSETS.None.ToString(), null)
//   };

//   // public static List<Equippable> GetChestEquipment()
//   // {
//   //   List<Equippable> r = new List<Equippable>();
//   //   r.Add(new Equippable("Mythic_Chest_Black_Hoodie", "Black Hoodie", "It has markings of Syndicate on it", EQUIPPABLESLOT.Chest, ItemRarity.Mythic, new List<ItemStats> {
//   //     new(ItemStats.STATTYPES.Health, ItemStats.STATVALUES.Add, 500f)
//   //   }, GEARSETS.None, null));

//   //   return r;
//   // }
// }