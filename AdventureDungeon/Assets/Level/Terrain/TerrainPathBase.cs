using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPathBase : MonoBehaviour
{
  
    void Awake()
    {
        /* Checks to see if there are any contact points */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision other)
    {
        //Check to see if the tag on the collider is equal to Enemy
        // if (other.tag == "LevelPreGen_Path")
        if(other.collider.tag=="LevelPreGen_Path")
        {
            Debug.Log($"[{gameObject.name} @TerrainPathBase] Contact with {other}");
        }
    }
}
