using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyController> Enemies { get; private set; }
    public List<EnemyController> Bosses { get; private set; }
    public List<EnemyController> AllHostiles { get; private set; }
    public List<PlayerCharacterController> Players { get; private set; }
    public int NumberOfEnemiesTotal { get; private set; }
    public int NumberOfEnemiesRemaining => Enemies.Count;

    void Awake()
    {
        Enemies = new List<EnemyController>();
        Bosses = new List<EnemyController>();
        AllHostiles = new List<EnemyController>();
        Players = new List<PlayerCharacterController>();
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        Enemies.Add(enemy);
        AllHostiles.Add(enemy);

        NumberOfEnemiesTotal++;
    }

    public void UnregisterEnemy(EnemyController enemyKilled)
    {
        int enemiesRemainingNotification = NumberOfEnemiesRemaining - 1;

        EnemyKillEvent evt = Events.EnemyKillEvent;
        evt.Enemy = enemyKilled.gameObject;
        evt.RemainingEnemyCount = enemiesRemainingNotification;
        EventManager.Broadcast(evt);

        // removes the enemy from the list, so that we can keep track of how many are left on the map
        Enemies.Remove(enemyKilled);
        AllHostiles.Remove(enemyKilled);

        // If all enemies have been eliminated, trigger the `Event.RoomClearEvent` event
        if(Enemies.Count == 0) {
          Debug.Log("[EnemyManager] Triggering `RoomClearEvent`...");
          RoomClearEvent rcevt = Events.RoomClearEvent;
          rcevt.EnableNextRooms = true;
          EventManager.Broadcast(rcevt);
        }

        // if(Bosses.Count == 0) {
        //   Debug.Log("[EnemyManager] Triggering `LevelClearEvent`...");
        //   LevelClearEvent lcevt = Events.LevelClearEvent;
        //   EventManager.Broadcast(lcevt);
        // }
    }

    public void RegisterBoss(EnemyController boss)
    {
        Bosses.Add(boss);
        AllHostiles.Add(boss);
    }

    public void UnregisterBoss(EnemyController bossKilled)
    {

        // EnemyKillEvent evt = Events.EnemyKillEvent;
        // evt.Enemy = enemyKilled.gameObject;
        // evt.RemainingEnemyCount = enemiesRemainingNotification;
        // EventManager.Broadcast(evt);

        // removes the enemy from the list, so that we can keep track of how many are left on the map
        Bosses.Remove(bossKilled);
        AllHostiles.Remove(bossKilled);

        if(Bosses.Count == 0) {
          Debug.Log("[EnemyManager] Triggering `LevelClearEvent`...");
          LevelClearEvent lcevt = Events.LevelClearEvent;
          EventManager.Broadcast(lcevt);
        }
    }

    public void RegisterPlayer(PlayerCharacterController player)
    {
        Players.Add(player);
    }

    public void UnregisterPlayer(PlayerCharacterController playerKilled)
    {
        // removes the player from the list, so that we can keep track of how many are left on the map
        Players.Remove(playerKilled);
    }
}
