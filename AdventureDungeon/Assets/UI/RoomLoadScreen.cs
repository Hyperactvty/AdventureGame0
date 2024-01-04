using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class MyGUO : GraphUpdateObject {
    public Vector3 offset = Vector3.up;
    public override void Apply (GraphNode node) {
        // Keep the base functionality
        base.Apply(node);
        // The position of a node is an Int3, so we need to cast the offset
        node.position += (Int3)offset;
    }
}

public class RoomLoadScreen : MonoBehaviour
{

    private IEnumerator coroutine_RoomLoad;
    private AstarPath pathfinding;
    // private Pathfinding.GridGraph pathfinding_graph; //.center


    void Awake()
    {
        EventManager.AddListener<RoomLoadScreenEvent>(OnRoomLoadScreen);
        pathfinding = (AstarPath)FindObjectOfType(typeof(AstarPath));

        Debug.Log($"[LevelBase] Graphs > {pathfinding.graphs[0]}");

    }

    // void Start()
    // {
    //   coroutine_RoomLoad = RoomLoadCoroutine();
    // }

    void OnRoomLoadScreen(RoomLoadScreenEvent evt) => DoRoomLoadScreen(evt);

    void DoRoomLoadScreen(RoomLoadScreenEvent evt)
    {
      if(evt.NextRoom==null) { return; }
      coroutine_RoomLoad = RoomLoadCoroutine(evt);
      StartCoroutine(coroutine_RoomLoad);
    }

    IEnumerator RoomLoadCoroutine(RoomLoadScreenEvent evt) {
      gameObject.GetComponent<CanvasGroup>().alpha=1;
      WaitForSeconds wait = new WaitForSeconds(0.5f);
      yield return wait; 

      evt.NextRoom.gameObject.SetActive(true);
      var rB = evt.NextRoom.GetComponent<RoomBase>();
      // rB.AssignDoorTriggers();

      // pathfinding.gameObject.transform.position = evt.NextRoom.gameObject.transform.position;
      // var graph = pathfinding.data.pointGraph;
      // var m = Matrix4x4.TRS ((pathfinding.gameObject.transform.position-evt.NextRoom.gameObject.transform.position), Quaternion.identity, Vector3.one);
      // graph.RelocateNodes (m);

        

      Bounds bounds = rB.roomFloor.bounds;
      Debug.Log($"BOUNDS : {bounds}");

      // This holds all graph data
      AstarData data = AstarPath.active.astarData;
      // This creates a Grid Graph
      GridGraph gg = data.AddGraph(typeof(GridGraph)) as GridGraph;
      // Setup a grid graph with some values
      
      gg.collision.mask=LayerMask.GetMask("LevelEnviroment");;
      gg.width = (int)bounds.size.x;
      gg.depth = (int)bounds.size.y;
      gg.nodeSize = 1;
      gg.center = bounds.center;
      gg.is2D = true;
      gg.collision.use2D=true;
      gg.collision.diameter=1.2f;
      // Updates internal size from the above values
      gg.UpdateSizeFromWidthDepth();
      // Scans all graphs, do not call gg.Scan(), that is an internal method
      AstarPath.active.Scan();

      // var guo = new GraphUpdateObject(bounds);
      // AstarPath.active.UpdateGraphs(guo);

        // MyGUO guo = new MyGUO();
        // guo.offset = Vector3.up*2;
        // guo.bounds = bounds;//new Bounds(Vector3.zero, Vector3.one*10);
        // AstarPath.active.UpdateGraphs(guo);

      // Do size as well to match the baseplate
      AstarPath.active.Scan();
      // pathfinding.active.Scan();

      rB.DoTransport(/*evt.roomDir, */DoorTrigger.ROOMDIRECTION.NULL, evt._pCC.transform);
      rB.OnRoomEnter();
      evt.BaseRoom.gameObject.SetActive(false);
      wait = new WaitForSeconds(0.5f);
      yield return wait; 

      gameObject.GetComponent<CanvasGroup>().alpha=0;
      RoomLoadScreenEvent rcevt = Events.RoomLoadScreenEvent;
      rcevt.NextRoom = null;
      EventManager.Broadcast(rcevt);
   }

    void OnDestroy()
    {
      StopCoroutine(coroutine_RoomLoad);
      EventManager.RemoveListener<RoomLoadScreenEvent>(OnRoomLoadScreen);
    }


}
