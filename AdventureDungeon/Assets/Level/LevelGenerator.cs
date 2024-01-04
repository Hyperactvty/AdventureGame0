using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] tiles;
    public int columns=6;
    public int rows=6;
    public int maxBlue = 2; 
    public int maxRed = 2; // Elite
    public int maxYellow = 1; //treasure
    public int maxGreen = 1; // Travelling Merchant
    public int howManyWhite=10;

    public RoomBase currentRoom;

    private int whiteTileCount; 
    private int greenTileCount;
    private int redTileCount;
    private int blueTileCount;
    private int yellowTileCount;
    private int tileToGenerate=0;
    private int resetHowManyWhite;

    // Start is called before the first frame update
    void Start()
    {
        resetHowManyWhite = howManyWhite;
        LoadMap();
        // EventManager.AddListener<RoomClearEvent>(OnStartLoadScreen);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Skill 1"))// Do this for each time the level loads
        {
          foreach (Transform _child in transform)
          {
            Destroy(_child.gameObject);
          }
          LoadMap();
        }
    }

    void LoadMap()
    {
      whiteTileCount=0; 
      greenTileCount=0;
      redTileCount=0; 
      blueTileCount=0;
      yellowTileCount=0;

      // GameObject[][] roomLayout = new GameObject[rows][columns];
      // var roomLayout /*: Array*/ = new System.Array[rows];
      GameObject[][] roomLayout /*: Array*/ = new GameObject[rows][];
      for (int i = 0; i </*&lt:*/ rows; i++)
      {
        // var tmpArray = new System.Array();
        // var tmpArray = new List<GameObject[]>();//[rows];
        GameObject[] tmpArray = new GameObject[columns];//[rows];
        // for (int j = 0; j </*&lt:*/ columns; j++)
        // {
        //   tmpArray.Add(new GameObject[rows]);
        // }
        roomLayout[i] = tmpArray;
      }

      Debug.Log($"2D ARRAY SIZE : {roomLayout[0].Length}");
      

      // Change this based on how the room size (or don't)
      int roomSize = 42;

      RoomBase.ROOMTYPE RoomTypeConnection(int typeToGrab)
      {
        RoomBase.ROOMTYPE r;
        switch (typeToGrab)
        {
          case 0: r=RoomBase.ROOMTYPE.Boss; break;
          case 2: r=RoomBase.ROOMTYPE.Elite; break;
          case 3: r=RoomBase.ROOMTYPE.Treasure; break;
          case 4: r=RoomBase.ROOMTYPE.TravellingMerchant; break;
          default: r=RoomBase.ROOMTYPE.Standard; break;
        }
        return r;
          // if(typeToGrab==0) RoomBase.ROOMTYPE.Standard,
          // 1, RoomBase.ROOMTYPE.Boss,
          // 2,RoomBase.ROOMTYPE.Elite,
          // 3,RoomBase.ROOMTYPE.Treasure,
          // 4,RoomBase.ROOMTYPE.TravellingMerchant
      }
      bool hasStart = false;

      for (int x = 0; x < rows; x++)
      {
        for (int y = 0; y < columns; y++)
        {
          if(x >= 0 && x < 3 && y >= 0 && y < 3)
          {
            // White (Standard)
            tileToGenerate=1;
            CheckTile(tileToGenerate);
            GameObject obj;
            obj = Instantiate(tiles[tileToGenerate], new Vector2(x, y)*roomSize, Quaternion.identity);
            if(!hasStart) { obj.GetComponent<RoomBase>().roomType = RoomBase.ROOMTYPE.Start; hasStart=true; }
            obj.transform.parent = transform;
            roomLayout[x][y] = obj;
          }
          else if(x == rows-1 && y ==columns-1)
          {
            tileToGenerate=0;
            greenTileCount++;
            GameObject obj;
            obj = Instantiate(tiles[tileToGenerate], new Vector2(x, y)*roomSize, Quaternion.identity);
            obj.transform.parent = transform;
            roomLayout[x][y] = obj;
          }
          else if(x >= rows-3 && y >=columns-3)
          {
            tileToGenerate=1;
            CheckTile(tileToGenerate);
            GameObject obj;
            obj = Instantiate(tiles[tileToGenerate], new Vector2(x, y)*roomSize, Quaternion.identity);
            obj.transform.parent = transform;
            roomLayout[x][y] = obj;
          }
          else
          {
            Vector2 pos = new Vector2(x, y);
            tileToGenerate = Random.Range(0,5);
            CheckTile(tileToGenerate);
            GameObject obj;
            obj = Instantiate(tiles[tileToGenerate], new Vector2(x, y)*roomSize, Quaternion.identity);
            obj.GetComponent<RoomBase>().roomType = RoomTypeConnection(tileToGenerate);
            obj.transform.parent = transform;
            roomLayout[x][y] = obj;
          }
        }
      }

      // Recursive function for checking the rooms
      int[][] checkRoom = {
        new int[] {redTileCount, maxRed},
        new int[] {blueTileCount, maxBlue},
        new int[] {yellowTileCount, maxYellow}
      };

      for (int t = 0; t < checkRoom.Length; t++)
      {
        if(checkRoom[t][0] < checkRoom[t][1])
        {
          int _row;// = Random.Range(0,rows);
          int _column;// = Random.Range(0,columns);
          do
          {
            _row = Random.Range(0,rows);
            _column = Random.Range(0,columns);
            if(roomLayout[_row][_column].GetComponent<RoomBase>().roomType == RoomBase.ROOMTYPE.Standard)
            {
              break;
            }
          } while (true);
          
          // Destroys the existing tile and replaces it with an `Elite` room
          Destroy(roomLayout[_row][_column]);
          CheckTile(t+2);
          GameObject obj;
          obj = Instantiate(tiles[t+2], new Vector2(_row, _column)*roomSize, Quaternion.identity);
          obj.transform.parent = transform;
          roomLayout[_row][_column] = obj;
        }
      }
    }

      

    void CheckTile(int randTile) 
    {
      switch (randTile)
      {
        case 0: // Exit Tile
          if(greenTileCount >= maxGreen - 1)
          {
            tileToGenerate = 1;
            whiteTileCount++;
            howManyWhite--;
            break;
          }
          else 
          {
            if(howManyWhite <= 0)
            {
              greenTileCount++;
              howManyWhite = resetHowManyWhite;
              break;
            }
            else
            {
              tileToGenerate = 1;
              whiteTileCount++;
              howManyWhite--;
              break;
            }
          }
        case 1:
          whiteTileCount++;
          howManyWhite--;
          break;
        case 2:
          if(redTileCount >= maxRed)
          {
            tileToGenerate = 1;
            whiteTileCount++;
            howManyWhite--;
            break;
          }
          else { redTileCount++; break; }
        case 3:
          if(blueTileCount >= maxBlue)
          {
            tileToGenerate = 1;
            whiteTileCount++;
            howManyWhite--;
            break;
          }
          else { blueTileCount++; break; }
        case 4:
          if(yellowTileCount >= maxYellow)
          {
            tileToGenerate = 1;
            whiteTileCount++;
            howManyWhite--;
            break;
          }
          else { yellowTileCount++; break; }
        default:
          whiteTileCount++;
          howManyWhite--;
          break;
      }
    }

    // void OnRoomClear(RoomClearEvent evt) => currentRoom.EnableDoorTransport(evt);

    // void OnDestroy()
    // {
    //   EventManager.RemoveListener<RoomClearEvent>(OnStartLoadScreen);
    // }

}
