using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemRarity { Common, Uncommon, Rare, Epic, Legendary, Mythic, Limited }

public enum GEARSETS { None, GlitchSeries_2023 } 

public class Consumable //Item 
{
  public string ItemId { get; set; /* Maybe not set */ }
  public string Name { get; set; }
  public string Description { get; set; }
  public ItemRarity Rarity { get; set; }
  // public ITEMTYPE ItemType { get; set; }

  // public bool Stackable { get; set; }
  public int Quantity { get; set; }
}

public class Gem
{
  public string ItemId { get; set; /* Maybe not set */ }
  public string Name { get; set; }
  public string Description { get; set; }
  public ItemRarity Rarity { get; set; }
  public bool Primordial { get; set; }

}



public class ItemBase : MonoBehaviour
{

  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
