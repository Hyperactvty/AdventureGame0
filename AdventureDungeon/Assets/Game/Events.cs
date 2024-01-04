using UnityEngine;

// namespace Unity.FPS.Game
// {
    // The Game Events used across the Game.
    // Anytime there is a need for a new event, it should be added here.

    public static class Events
    {
        public static ObjectiveUpdateEvent ObjectiveUpdateEvent = new ObjectiveUpdateEvent();
        public static LoadScreenEvent LoadScreenEvent = new LoadScreenEvent();
        public static RoomLoadScreenEvent RoomLoadScreenEvent = new RoomLoadScreenEvent();
        // public static TriggerRoomLockEvent TriggerRoomLockEvent = new TriggerRoomLockEvent();
        public static RoomClearEvent RoomClearEvent = new RoomClearEvent();
        public static LevelClearEvent LevelClearEvent = new LevelClearEvent();
        public static UserCompleteOrExitLevelEvent UserCompleteOrExitLevelEvent = new UserCompleteOrExitLevelEvent();
        public static SyncUserDataEvent SyncUserDataEvent = new SyncUserDataEvent();
        public static AllObjectivesCompletedEvent AllObjectivesCompletedEvent = new AllObjectivesCompletedEvent();
        public static GameOverEvent GameOverEvent = new GameOverEvent();
        public static PlayerDeathEvent PlayerDeathEvent = new PlayerDeathEvent();
        public static EnemyKillEvent EnemyKillEvent = new EnemyKillEvent();
        public static PickupEvent PickupEvent = new PickupEvent();
        public static AmmoPickupEvent AmmoPickupEvent = new AmmoPickupEvent();
        public static DamageEvent DamageEvent = new DamageEvent();
        public static DisplayMessageEvent DisplayMessageEvent = new DisplayMessageEvent();
        
        #region Player Stuff
        #endregion // Player Stuff
    }

    public class ObjectiveUpdateEvent : GameEvent
    {
        public Objective Objective;
        public string DescriptionText;
        public string CounterText;
        public bool IsComplete;
        public string NotificationText;
    }

    public class LoadScreenEvent : GameEvent
    {
        public bool Loading;
    }

    public class RoomLoadScreenEvent : GameEvent
    {
        public GameObject NextRoom;
        public GameObject BaseRoom;
        public PlayerCharacterController _pCC;
        // public DoorTrigger.ROOMDIRECTION roomDir;
    }

    public class RoomClearEvent : GameEvent
    {
        public bool EnableNextRooms;
        public float DamageDealt;
    }
    // Essentially when boss is slain
    public class LevelClearEvent : GameEvent
    {
        public float DamageDealt;
    }

    // When the user leaves the level
    public class UserCompleteOrExitLevelEvent : GameEvent
    {
        public PlayerCharacterController PCC;
        public string UserId;
        public double ExperienceGain;
        // Add more as needed. This may be used for syncing or pushing to DB
    }

    public class SyncUserDataEvent : GameEvent
    {
        public string UserId;
        public double Experience;
        // Add more as needed. This may be used for syncing or pushing to DB
        public float DamageDealt;
    }

    public class AllObjectivesCompletedEvent : GameEvent { }

    public class GameOverEvent : GameEvent
    {
        public bool Win;
    }

    public class PlayerDeathEvent : GameEvent { }

    public class EnemyKillEvent : GameEvent
    {
        public GameObject Enemy;
        public int RemainingEnemyCount;
    }

    public class PickupEvent : GameEvent // TODO: Powerup Event
    {
        public string UserId;
        public GameObject Pickup;
    }

    public class AmmoPickupEvent : GameEvent 
    {
        public WeaponController Weapon;
    }

    public class DamageEvent : GameEvent
    {
        public GameObject Sender;
        public float DamageValue;
    }

    public class DisplayMessageEvent : GameEvent
    {
        public string Message;
        public float DelayBeforeDisplay;
    }

    #region Player Stuff

    #endregion // Player Stuff
// }
