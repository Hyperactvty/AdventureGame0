using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack 
{
  public string UserId { get; set; /* Maybe not set */ }
  public List<Equippable> Contents { get; set; }
}

public class ConsumableBackpack 
{
  public string UserId { get; set; /* Maybe not set */ }
  public List<Consumable> Contents { get; set; }
}

public class UserStorageBase : MonoBehaviour
{

    [Header("Backpack")] [Tooltip("The player's backpack")]
    public List<GameObject> PlayerBackpack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
