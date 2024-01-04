using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public enum SUCCESS_CODE {SUCCESS, WARN, ERROR};


[System.Serializable]
public class CoordinateNode
{
  /* DEBUG */ public string GUID;
  public Vector3 positionStart;// { get; set; }
  public Vector3 positionEnd;// { get; set; }
  public enum TERRAIN_TYPE {Small, Medium, Large, X_Large, Bridge, Arena};
  public TERRAIN_TYPE terrainType;// { get; set; }
  public List<CoordinateNode> childCoordObjects;// {get; set;}

  public CoordinateNode(
    Vector3 positionStart, Vector3 positionEnd, 
    string terrainType, List<CoordinateNode> childCoordObjects
  ) {
    TERRAIN_TYPE _tType; Enum.TryParse<TERRAIN_TYPE>(terrainType, out _tType);

    // this.GUID = System.Guid.NewGuid().ToString();
    this.positionStart = positionStart;
    this.positionEnd = positionEnd;
    this.terrainType = _tType;
    this.childCoordObjects = childCoordObjects;
  }

  public string Stringify() 
  {
      return JsonUtility.ToJson(this);
  }
}

[System.Serializable]
public class VectorNode
{
  /* DEBUG */ public string GUID;
  public Vector3 vector;// { get; set; }
  public List<CoordinateNode> list;// {get; set;}
  public bool blocked;

  public VectorNode( Vector3 vector, List<CoordinateNode> list ) {
    this.vector = vector;
    this.list = list;
    this.blocked = false;
  }

  public string Stringify() 
  {
      return JsonUtility.ToJson(this);
  }
}

public class TerrainBehaviour : MonoBehaviour
{
    private System.Random rand = new System.Random();
    // private List<(Vector3 vector, List<CoordinateNode> list)> startNodeList = new List<(Vector3, List<CoordinateNode>)>();

    // [SerializeField]
    // private List<VectorNode> startNodeList = new List<VectorNode>();
    // [SerializeField]
    // private List<VectorNode> endNodeList = new List<VectorNode>();

    [SerializeField]
    private List<VectorNode> nodeList = new List<VectorNode>();

    [Header("Coordinate Node List")] [Tooltip("A list containing all the nodes for the terrain")]
    public int hardLimitNodeConnections = 2;
    public List<CoordinateNode> coordNodes = new List<CoordinateNode>(); // This is generated for the level

    // Start is called before the first frame update
    void Start()
    {
      /* Calls the path-generating method */
      GenerateTerrainPaths();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector3 PointOnCircle(float radius, float angleInDegrees, System.Drawing.PointF origin)
    {
        // Convert from degrees to radians via multiplication by PI/180        
        float x = (float)(radius * Math.Cos(angleInDegrees * Math.PI / 180F)) + origin.X;
        float y = (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + origin.Y;

        System.Drawing.PointF res = new System.Drawing.PointF(x, y);
        return new Vector3(res.X, 0, res.Y);
    }
    
    public string DEBUG_AlphaNaming(int v) {
      string res = ""; string temp_res = "";
      int itr=0; int temp_itr=0; int _temp_int = 0;
      for (int i = 0; i < Mathf.Ceil(v/26)+1; i++)
      {
        for (char c = 'A'; c <= 'Z'; c++)
        {
            temp_itr=itr;
            if(temp_itr%26 == 0) {
              _temp_int=0;
              for (char _c = 'A'; _c <= 'Z'; _c++)
              {
                if((int)(itr/26) == _temp_int) {
                  temp_res = $"{_c}";
                  break;
                }
                _temp_int++;
              }
            }
            if(itr==v) {
              res+=c;
              return $"{temp_res}{res}";
            }
            itr++;
        } 
      }
      
      return res;
    }

    public class CoordGen
    {
      public float _l;
      public CoordinateNode _childOf;
      public CoordinateNode _newCoordNode;
      public int _index;
      public float vectorDirection;

      public CoordGen(
        float _l, CoordinateNode _childOf,
        CoordinateNode _newCoordNode, int _index,
        float vectorDirection
      ) {

        this._l = _l;
        this._childOf = _childOf;
        this._newCoordNode = _newCoordNode;
        this._index = _index;
        this.vectorDirection = vectorDirection;
      }
    }

    public CoordGen GenerateCoordinateNode(int i) {
      // Random length
      float _l = UnityEngine.Random.Range(2, 12); // 2.0f, 12.0f
      bool _childPoint = rand.Next(2)==1; // The switch to see if the child is connected to the start or the end of the `CoordinateNode` object.
      List<VectorNode> availableVectors = nodeList.Select(q => q).Where(q => !q.blocked).ToList();
      foreach (var _aV in availableVectors)
      {
        Debug.Log($"[{_aV.vector}] -> {_aV.list.Count()} ({_aV.blocked})");
      }
      // int _index = rand.Next(availableVectors.Count);
      int _index = UnityEngine.Random.Range(0, availableVectors.Count);
      CoordinateNode _childOf = _index!=0 ? availableVectors[_index].list[rand.Next(availableVectors[_index].list.Count())] : null;

      float _x = 0;
      float _y = 0;
      if( _index!=0 ) {
        _x = _childPoint ? _childOf.positionStart.x : _childOf.positionEnd.x;
        _y = _childPoint ? _childOf.positionStart.z : _childOf.positionEnd.z;
      }
      
      float vectorDirection = rand.Next(8) * 45.0f;

      CoordinateNode _newCoordNode = new CoordinateNode(
        _childPoint ? new Vector3(_x, 0, _y) : PointOnCircle(_l, vectorDirection, new System.Drawing.PointF(_x, _y)),
        _childPoint ? PointOnCircle(_l, vectorDirection, new System.Drawing.PointF(_x, _y)) : new Vector3(_x, 0, _y),
        "Medium",
        new List<CoordinateNode>()
      );

      /* DEBUG */ _newCoordNode.GUID = DEBUG_AlphaNaming(i);
      CoordGen res = new CoordGen(_l, _childOf, _newCoordNode, _index, vectorDirection);
      return res;
    }

    public SUCCESS_CODE GenerateTerrainPaths(int pathAmount = -1) {
      try {
        int _pathAmt = pathAmount!=-1 ? pathAmount : rand.Next(10)+30;
        /* DEBUG : For Testing ONLY! */
        for (int i = 0; i < _pathAmt; i++)
        {
          #region Legacy - Choosing Points
          // if(false) {
          //   // Random length
          //   float _l = UnityEngine.Random.Range(2, 12); // 2.0f, 12.0f
          //   bool _childPoint = rand.Next(2)==1; // The switch to see if the child is connected to the start or the end of the `CoordinateNode` object.
          //   int _index = rand.Next(coordNodes.Count);
          //   CoordinateNode _childOf = _index!=0 ? coordNodes[_index] : null;

          //   float _x = 0;
          //   float _y = 0;
          //   if( _index!=0 ) {
          //     _x = _childPoint ? _childOf.positionStart.x : _childOf.positionEnd.x;
          //     _y = _childPoint ? _childOf.positionStart.z : _childOf.positionEnd.z;
          //   }
            
          //   float vectorDirection = rand.Next(8) * 45.0f;

          //   CoordinateNode _newCoordNode = new CoordinateNode(
          //     _childPoint ? new Vector3(_x, 0, _y) : PointOnCircle(_l, vectorDirection, new System.Drawing.PointF(_x, _y)),
          //     _childPoint ? PointOnCircle(_l, vectorDirection, new System.Drawing.PointF(_x, _y)) : new Vector3(_x, 0, _y),
          //     "Medium",
          //     new List<CoordinateNode>()
          //   );
          // }
          #endregion // Legacy - Choosing Points
          
          // // Random length
          // float _l = UnityEngine.Random.Range(2, 12); // 2.0f, 12.0f
          // bool _childPoint = rand.Next(2)==1; // The switch to see if the child is connected to the start or the end of the `CoordinateNode` object.
          // List<VectorNode> availableVectors = nodeList.Select(q => q).Where(q => !q.blocked).ToList();
          // foreach (var _aV in availableVectors)
          // {
          //   Debug.Log($"[{_aV.vector}] -> {_aV.list.Count()} ({_aV.blocked})");
          // }
          // // int _index = rand.Next(availableVectors.Count);
          // int _index = UnityEngine.Random.Range(0, availableVectors.Count);
          // CoordinateNode _childOf = _index!=0 ? availableVectors[_index].list[rand.Next(availableVectors[_index].list.Count())] : null;

          // float _x = 0;
          // float _y = 0;
          // if( _index!=0 ) {
          //   _x = _childPoint ? _childOf.positionStart.x : _childOf.positionEnd.x;
          //   _y = _childPoint ? _childOf.positionStart.z : _childOf.positionEnd.z;
          // }
          
          // float vectorDirection = rand.Next(8) * 45.0f;

          // CoordinateNode _newCoordNode = new CoordinateNode(
          //   _childPoint ? new Vector3(_x, 0, _y) : PointOnCircle(_l, vectorDirection, new System.Drawing.PointF(_x, _y)),
          //   _childPoint ? PointOnCircle(_l, vectorDirection, new System.Drawing.PointF(_x, _y)) : new Vector3(_x, 0, _y),
          //   "Medium",
          //   new List<CoordinateNode>()
          // );

          // /* DEBUG */ _newCoordNode.GUID = DEBUG_AlphaNaming(i);

          // CoordinateNode _newCoordNode = GenerateCoordinateNode(i);
          CoordGen _cg = GenerateCoordinateNode(i);
          float _l = _cg._l;
          CoordinateNode _childOf = _cg._childOf;
          CoordinateNode _newCoordNode = _cg._newCoordNode;
          int _index = _cg._index;
          float vectorDirection = _cg.vectorDirection;

          /* Adds the Start and End nodes to their respective lists */
          // if(startNodeList.Select(q => q.vector).Where(q => q ==_newCoordNode.positionStart).Count()==0) {
          //   startNodeList.Add(new VectorNode(_newCoordNode.positionStart, new List<CoordinateNode>(){_newCoordNode}));  // TYPEOF : List<(Vector3, List<CoordinateNode>)>
          // } else {
          //   startNodeList.Select(q => q).Where(q => q.vector ==_newCoordNode.positionStart).First().list.Add(_newCoordNode);
          //   /* Checks to see if the vector has 3 entries. If `true`, removes that vector from being selected */
          //   // List<(Vector3 vector, List<CoordinateNode> list)> _snl = startNodeList.Select(q => q).Where(q => q.vector ==_newCoordNode.positionStart).First().list;
          //   if( startNodeList.Select(q => q).Where(q => q.vector ==_newCoordNode.positionStart).First().list.Count() >= hardLimitNodeConnections ) {
          //     startNodeList.Select(q => q).Where(q => q.vector ==_newCoordNode.positionStart).First().blocked=true;
          //   }
          // }
          // endNodeList.Add(_newCoordNode.positionEnd);
          List<Vector3> iterList = new List<Vector3>() {_newCoordNode.positionStart, _newCoordNode.positionEnd};
          for (int _vItr = 0; _vItr < iterList.Count(); _vItr++) {
            if(nodeList.Select(q => q.vector).Where(q => q ==iterList[_vItr]).Count()==0) {
              nodeList.Add(new VectorNode(iterList[_vItr], new List<CoordinateNode>(){_newCoordNode}));  // TYPEOF : List<(Vector3, List<CoordinateNode>)>
            } else {
              nodeList.Select(q => q).Where(q => q.vector ==iterList[_vItr]).First().list.Add(_newCoordNode);
              /* Checks to see if the vector has 3 entries. If `true`, removes that vector from being selected */
              if( nodeList.Select(q => q).Where(q => q.vector ==iterList[_vItr]).First().list.Count() >= 1 ) {
                if(nodeList.Select(q => q).Where(q => q.vector ==iterList[_vItr]).First().blocked==true) {
                  Debug.Log($"[DELETE] -> GUID {_newCoordNode.GUID}");
                  // SUCCESS_CODE resolved = GenerateTerrainPaths(1); // This one may work!
                  // _newCoordNode = GenerateCoordinateNode(i);
                  _cg = GenerateCoordinateNode(i);
                  _l = _cg._l;
                  _childOf = _cg._childOf;
                  _newCoordNode = _cg._newCoordNode;
                  _index = _cg._index;
                  vectorDirection = _cg.vectorDirection;
                }
                nodeList.Select(q => q).Where(q => q.vector ==iterList[_vItr]).First().blocked=true;
              }
            }
          }

          // if(nodeList.Select(q => q.vector).Where(q => q ==_newCoordNode.positionStart).Count()==0) {
          //   nodeList.Add(new VectorNode(_newCoordNode.positionStart, new List<CoordinateNode>(){_newCoordNode})); 
          //   if(nodeList.Select(q => q.vector).Where(q => q ==_newCoordNode.positionEnd).Count()==0) {
          //     nodeList.Add(new VectorNode(_newCoordNode.positionEnd, new List<CoordinateNode>(){_newCoordNode}));  
          //   }
          // } else if(nodeList.Select(q => q.vector).Where(q => q ==_newCoordNode.positionEnd).Count()==0) {
          //   nodeList.Add(new VectorNode(_newCoordNode.positionEnd, new List<CoordinateNode>(){_newCoordNode}));  // TYPEOF : List<(Vector3, List<CoordinateNode>)>
          // } else {
          //   nodeList.Select(q => q).Where(q => q.vector ==_newCoordNode.positionEnd).First().list.Add(_newCoordNode);
          //   /* Checks to see if the vector has 3 entries. If `true`, removes that vector from being selected */
          //   if( endNodeList.Select(q => q).Where(q => q.vector ==_newCoordNode.positionEnd).First().list.Count() >= 1 ) {
          //     endNodeList.Select(q => q).Where(q => q.vector ==_newCoordNode.positionEnd).First().blocked=true;
          //   }
          // }

          if( _index!=0 ) {
            _childOf.childCoordObjects.Add(_newCoordNode);
            Debug.Log($"[{_childOf.GUID}] Child Count -> {_childOf.childCoordObjects.Count}");
          }
          // _newCoordNode.childCoordObjects.Add(_childOf);

          coordNodes.Add(_newCoordNode);
          // Debug.Log(_newCoordNode.Stringify());

          /* DEBUG */
          Color _col = new Color(rand.Next(255)/255.0f,rand.Next(255)/255.0f,rand.Next(255)/255.0f,1.0f);
          GameObject sphere0 = GameObject.CreatePrimitive(PrimitiveType.Sphere); sphere0.name = $"sphStart_{i}";
          GameObject sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere); sphere1.name = $"sphEnd_{i}";
          GameObject _pathingGuide = GameObject.CreatePrimitive(PrimitiveType.Plane); _pathingGuide.name = $"pathGuide_{i}";
          _pathingGuide.name = _newCoordNode.GUID;
          GameObject _terrainPath = GameObject.CreatePrimitive(PrimitiveType.Plane); _terrainPath.name = $"tPath_{i}";
          _terrainPath.name = _newCoordNode.GUID;

          sphere0.transform.position = _newCoordNode.positionStart;
          sphere0.GetComponent<Renderer>().material.color = _col;
          sphere1.transform.position = _newCoordNode.positionEnd;
          sphere1.GetComponent<Renderer>().material.color = _col;
          
          Vector3 p1 = sphere0.transform.position;
          Vector3 p2 = sphere1.transform.position;
          
          Vector3 scale = p1 - p2;
          scale.x = Mathf.Abs(scale.x);
          scale.y = Mathf.Abs(scale.y);
          scale.z = Mathf.Abs(scale.z);
          
          Vector3 center = (p1 + p2) * 0.5f;

          // Destroy(_pathingGuide.GetComponent<MeshCollider>());
          // _pathingGuide.GetComponent<MeshCollider>().isTrigger=true;
          _pathingGuide.tag = "LevelPreGen_Path"; /* Sets the tag so the collisions know what to look for */
          _pathingGuide.transform.position = center;
          _pathingGuide.transform.localScale = new Vector3((_l/10.0f)-0.2f, 1, 0.05f);
          _pathingGuide.transform.Rotate(0.0f, -vectorDirection, 0.0f, Space.World /*Space.Self*/);
          _pathingGuide.GetComponent<Renderer>().material.color = _col;
          _pathingGuide.AddComponent<TerrainPathBase>();

          
          // _childOf.Select(x => x.GetType().GetProperty("positionStart").GetValue(x, null))
          // var checkConflicts = coordNodes.Select(x => x.GetType().GetProperty("positionStart"));
                    //  .ToList();
                    //  .Count() == 1;

          if(_index!=0) {
            // var checkConflicts = _childOf.childCoordObjects.Select(x => x.GetType().GetProperty("positionStart"));
            List<string> debug_StartList = new List<string>();
            List<string> debug_EndList = new List<string>();

            // var checkConflictsStart = coordNodes.Select(
            //   /* Selecting all items in the list */
            //   x => x.childCoordObjects.Select(y => y)
            //   .Where(z => 
            //     {
            //       debug_tempOutput.Add($"[CONFLICT_START_CHECK] -> {z.positionStart} == {_newCoordNode.positionStart}");
            //       if(z.positionStart == _newCoordNode.positionStart) { return z.positionStart == _newCoordNode.positionStart; }
            //       else { return false; }
            //     }
            //   )
            // );

            /* DEBUG */ foreach (var cItem in coordNodes)
            {
              if(cItem.positionStart == _newCoordNode.positionStart && cItem.GUID != _newCoordNode.GUID) {
                debug_StartList.Add($"[CONFLICT_START_CHECK] -> [{_newCoordNode.GUID}] {_newCoordNode.positionStart} == [{cItem.GUID}] {cItem.positionStart} : {cItem.positionStart == _newCoordNode.positionStart}");
              }
              if(cItem.positionEnd == _newCoordNode.positionEnd && cItem.GUID != _newCoordNode.GUID) {
                debug_EndList.Add($"[CONFLICT_END_CHECK] -> [{_newCoordNode.GUID}] {_newCoordNode.positionEnd} == [{cItem.GUID}] {cItem.positionEnd} : {cItem.positionEnd == _newCoordNode.positionEnd}");
              }
            }
            /* DEBUG */ foreach (var item in debug_StartList)
            {
              Debug.Log(item);
            }
            /* DEBUG */ foreach (var item in debug_EndList)
            {
              Debug.Log(item);
            }

            var checkConflicts1 = _childOf.childCoordObjects.Select(x => x)
              .Where(x => 
                x.positionStart == _newCoordNode.positionStart ||
                x.positionEnd == _newCoordNode.positionEnd
              );
            // Debug.Log($"Check Conflicts1 [{_childOf.GUID}] -> {checkConflicts1}");
            foreach (var x in checkConflicts1)
            {
              // Debug.Log($"\t[{_childOf.GUID}] -> {x.GUID}");
            }
          }

          // Check to see if there are any collisions
          // -> If true, search for any `CoordinateNode` with the same `positionStart` OR `positionEnd`
                     
          // Check rotation to see if it's the same

          // If true, then check to see if it has no child nodes and to see if it's shorter than the current one.

          _terrainPath.transform.position = center;
          _terrainPath.transform.localScale = new Vector3((_l/10.0f)+0.1f, 1, 0.2f /* This changes from what `terrainType` is set */);
          _terrainPath.transform.Rotate(0.0f, -vectorDirection, 0.0f, Space.World /*Space.Self*/);
          _terrainPath.GetComponent<Renderer>().material.color = _col;
          Destroy(_terrainPath);
        }
        Debug.Log($"[SUCCESS] -> @TerrainBehaviour.GenerateTerrainPaths()");

        /* TEMP_DEBUG : CHECKING `startNodeList` LIST */
        foreach (var item in nodeList)
        {
          if(item.list.Count() > hardLimitNodeConnections) {
            Debug.Log($"\t[SNList] -> {item.vector} : {item.list.Count()}");
          }
          // foreach (var result in item.list)
          // {
          //   Debug.Log($"\t\t -> {result.Stringify()}");
          // }
        }

        return SUCCESS_CODE.SUCCESS;
      }
      catch (System.Exception ex)
      {
        Debug.Log($"[ERROR] -> {ex}");
        return SUCCESS_CODE.ERROR;
      }
      
    }

    // public List<GameObject> SelectAllPoints(float _x, float _y) {

    // }
}
