using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageGridGenerator : MonoBehaviour
{
    private Transform parentObject;
    public GameObject tilePrefab;
    private int maxBackpack = 40; //40 // Grab from `user`

    // Start is called before the first frame update
    void Start()
    {
        parentObject = this.gameObject.transform;//.GetChild(0);
        for (int tileCount = 1; tileCount < maxBackpack; tileCount++)
        {
          // GameObject.Find("InstanceStandardChest").GetChild(0).Instantiate();
          GameObject newTile = GameObject.Instantiate(tilePrefab);
          newTile.transform.SetParent(parentObject);
          newTile.transform.localScale = new Vector3(1,1,1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
