using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartSpawnpoint : MonoBehaviour
{

    public Transform levelStartPoint;
    public GameObject playerAsset;
    GameObject gameManager;

    public GameObject respawnPrefab;
    public GameObject[] respawns;

    // Start is called before the first frame update
    void Start()
    {
      // Code for checkpoint system
        // if (respawns == null)
        //     respawns = GameObject.FindGameObjectsWithTag("Respawn");

        // foreach (GameObject respawn in respawns)
        // {
        //     Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Loads player to spawn point
    public void DoPlayerLevelLoad()
    {
      GameObject locateUGC = GameObject.FindWithTag("UserGameController");
      // Check if tag is `Spawnpoint`
      if (locateUGC.ToString().IndexOf("GameManager") != -1) 
      {
        // gameManager = GameObject.Find("GameManager");
        string uid = locateUGC.GetComponent<UserBase>().CurrentUser.UserId;
        GameObject tempPlayerRef = GameObject.FindWithTag("Player"); // temp until able to load multiple players
        tempPlayerRef.name = uid;
        return;
        GameObject spnPlr = GameObject.Instantiate(playerAsset, levelStartPoint.position, levelStartPoint.rotation, null);
        spnPlr.name = uid;
        spnPlr.GetComponent<PlayerStatsBase>().UserId = uid;
        Debug.Log(uid);
      }
        
    }

    public void DoPlayerLoad()
    {
        // Check if tag is `Checkpoint`
        
    }
}
