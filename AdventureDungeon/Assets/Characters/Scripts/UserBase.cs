using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using UnityEngine.Networking;
using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TMPro;

[System.Serializable]
public class User 
{
  public string UserId; /*{ get; set; }*/
  public string Username { get; set; }
  
  public int User_Level { get; set; }
  public double Experience { get; set; }

  /** Currency */
  public int Coin { get; set; }
  // public int Primordial_Gem { get; set; }
  public int Diamond { get; set; }
  public int Crystal { get; set; }

  public int Backpack_Max_Size { get; set; } // 40
  public int Vault_Max_Size { get; set; } // 100

}

public class UserBase : MonoBehaviour
{
    private System.Random rand = new System.Random();
    public User CurrentUser;
    private ExperienceBar eb;

    public TextMeshProUGUI debugText;

    // Equippable item `List`
    public static List<Equippable> EquippableList = new List<Equippable>();
    public static List<Weapon> WeaponList = new List<Weapon>(); 
    public static List<Weapon> _WeaponList = new List<Weapon>();

#region DB

    /* FOR THE DB COMMUNICATIONS */
    private ToDB.UserData _ud;
    private ToDB.EquippableChestDB _ecdb;

    // http://localhost:5000/equippable/chest
    // IEnumerator Download_GetEquippable(string slot, System.Action<ToDB.EquippableChestDB> callback = null)
    IEnumerator Download_GetEquippable(string slot, System.Action<Equippable> callback = null)
    {
      //https://us-east-2.aws.data.mongodb-api.com/app/data-ggtgs/endpoint/data/v1
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:5000/equippable/" + slot))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                if (callback != null)
                {
                    callback.Invoke(null);
                }
            }
            else
            {
                if (callback != null)
                {
                    Debug.Log($"0Callback from the API (Download) > {JsonConvert.DeserializeObject(request.downloadHandler.text)}");
                    // var dbText = JsonConvert.DeserializeObject<ToDB.Root>(request.downloadHandler.text);
                    // var dbText = JsonConvert.DeserializeObject(request.downloadHandler.text);
                    // debugText.text = dbText.ToString();
                    // Debug.Log($"Callback from the API (Download) > {JsonConvert.DeserializeObject<ToDB.Root>(request.downloadHandler.text.ToString())}");

                    // Debug.Log($"1Callback from the API (Download) > {JsonUtility.FromJson<ToDB.EquippableChestDB>(request.downloadHandler.text.ToString())}");


                    // callback.Invoke(ToDB.EquippableChestDB.Parse(request.downloadHandler.text));
                    bool isArray = false;
                    try
                    {
                      foreach (Equippable item in JsonConvert.DeserializeObject<Equippable[]>(request.downloadHandler.text))
                      {
                        EquippableList.Add(item);
                        // callback.Invoke(item);
                        // Debug.Log($"Stringified (Download) > {item.Stringify()}");
                        // Debug.Log($"2Callback from the API (Download) > {Equippable.Parse(item.Stringify())}");
                        // Debug.Log($"3Callback from the API (Download) > {item.Name}");
                        // Debug.Log($"2Callback from the API (Download) > {Equippable.Parse(item)}");
                      }
                      isArray = true;
                    }
                    catch (System.Exception e)
                    {
                      Debug.Log($"Error > {e}");
                    }
                    finally
                    {
                      foreach (var item in EquippableList)
                      {
                        Debug.Log($"Equippable Item > {item.Name} [{item.EquippableSlot}]");
                      }
                    }
                    if(!isArray) {
                      // callback.Invoke(JsonConvert.DeserializeObject<ToDB.EquippableChestDB>(request.downloadHandler.text));
                      // Debug.Log($"2Callback from the API (Download) > {ToDB.EquippableChestDB.Parse(request.downloadHandler.text)}");
                      callback.Invoke(JsonConvert.DeserializeObject<Equippable>(request.downloadHandler.text));
                      Debug.Log($"2Callback from the API (Download) > {Equippable.Parse(request.downloadHandler.text)}");
                    }
                    

                }
            }

            Debug.Log($"From the API (Download) > {request.downloadHandler.text.ToString()}");
        }
    }

    IEnumerator Upload_GetEquippable(string slot, System.Action<bool> callback = null)
    {
        using (UnityWebRequest request = new UnityWebRequest("http://localhost:5000/equippable/" + slot, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(slot);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                if(callback != null) 
                {
                    callback.Invoke(false);
                }
            }
            else
            {
                if(callback != null) 
                {
                    callback.Invoke(request.downloadHandler.text != "{}");
                }
            }
            Debug.Log($"From the API as URL > {request.url}");
            Debug.Log($"From the API > {request.result}");
        }
    }

    IEnumerator Download(string id, System.Action<ToDB.UserData> callback = null)
    {
      //https://us-east-2.aws.data.mongodb-api.com/app/data-ggtgs/endpoint/data/v1
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:5000/users/" + id))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                if (callback != null)
                {
                    callback.Invoke(null);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback.Invoke(ToDB.UserData.Parse(request.downloadHandler.text));
                }
            }
        }
    }

    IEnumerator Upload(string profile, System.Action<bool> callback = null)
    {
        using (UnityWebRequest request = new UnityWebRequest("http://localhost:5000/users", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(profile);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                if(callback != null) 
                {
                    callback.Invoke(false);
                }
            }
            else
            {
                if(callback != null) 
                {
                    callback.Invoke(request.downloadHandler.text != "{}");
                }
            }
        }
    }

    // IEnumerator Upload_User(string profile, System.Action<bool> callback = null)
    // {
    //     using (UnityWebRequest request = new UnityWebRequest("http://localhost:5000/users", "POST"))
    //     {
    //         request.SetRequestHeader("Content-Type", "application/json");
    //         byte[] bodyRaw = Encoding.UTF8.GetBytes(profile);
    //         request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //         request.downloadHandler = new DownloadHandlerBuffer();
    //         yield return request.SendWebRequest();

    //         if (request.isNetworkError || request.isHttpError)
    //         {
    //             Debug.Log(request.error);
    //             if(callback != null) 
    //             {
    //                 callback.Invoke(false);
    //             }
    //         }
    //         else
    //         {
    //             if(callback != null) 
    //             {
    //                 callback.Invoke(request.downloadHandler.text != "{}");
    //             }
    //         }
    //         Debug.Log($"From the API as URL > {request.url}");
    //         Debug.Log($"From the API > {request.result}");

    //         Debug.Log($"BodyRaw (Upload) > {bodyRaw}");
    //         Debug.Log($"From the API (Upload) > {request.uploadHandler.data.ToString()}");
    //         Debug.Log($"From the API (Upload->Download) > {request.downloadHandler.text.ToString()}");
    //     }
    // }

    void Upload_User()
    {
      List<ToDB.UserData> _allUsers = null;
      string query =
    @"query {
        getUsers { _id, username }
    }";
      GraphQL.APIGraphQL.Query(query, new {}, response => _allUsers = response.GetList<ToDB.UserData>("getUsers")); // works
      
      string mutation =
        @"mutation ($username: String!) {
            addUser(username:$username)
            {username}
        }";
        // @"mutation ($username: String!) {
        //     token: addUser(username:$username) 
        // }";

      GraphQL.APIGraphQL.Query(mutation, new {username = _ud.Username}, callback);
    }
    
    private void callback (GraphQL.GraphQLResponse response) {
      Debug.Log($"Callback (UserBase) [response] > {response.Get<string>("data")}");
      // string token = response.Get<string>("token");
      // Debug.Log($"Callback [Userbase] > {token}");
    }

    void Download_GetAllWeapons()
    {
      // List<Weapon> weaponsList = new List<Weapon>();
      string query =
        @"query {
            getAllWeaponItem { itemId,name,description,rarity,
            gearSet,sprite,baseAttack,baseAPS,droppableSources{sourceId,sourceType,chance},
            itemStats{stat, modifier,statValue}}
        }";
      // GraphQL.APIGraphQL.Query(query, new {}, response => WeaponList = response.GetList<Weapon>("getAllWeaponItem")); // works
      GraphQL.APIGraphQL.Query(query, new {}, response => {
        Debug.Log($"RESPONSE > {response.Raw}");
        foreach (Weapon item in JsonConvert.DeserializeObject<Weapon[]>((JObject.Parse(response.Raw.ToString())["data"]["getAllWeaponItem"]).ToString()))
        {
          WeaponList.Add(item);
          // callback.Invoke(item);
          // Debug.Log($"Stringified (Download) > {item.Stringify()}");
          // Debug.Log($"2Callback from the API (Download) > {Equippable.Parse(item.Stringify())}");
          // Debug.Log($"3Callback from the API (Download) > {item.Name}");
          // Debug.Log($"2Callback from the API (Download) > {Equippable.Parse(item)}");
        }
        Debug.Log($"Querying to get WEAPONS: {WeaponList.Count}");
      }); // works
      // Debug.Log($"Querying to get WEAPONS: {WeaponList.Count}");
    //   Debug.Log($"Querying to get WEAPONS: {WeaponList.Count}");
    // //   foreach (Weapon item in JsonConvert.DeserializeObject<Weapon[]>(request.downloadHandler.text))
    // //   {
    // //     WeaponList.Add(item);
    // //     // callback.Invoke(item);
    // //     // Debug.Log($"Stringified (Download) > {item.Stringify()}");
    // //     // Debug.Log($"2Callback from the API (Download) > {Equippable.Parse(item.Stringify())}");
    // //     // Debug.Log($"3Callback from the API (Download) > {item.Name}");
    // //     // Debug.Log($"2Callback from the API (Download) > {Equippable.Parse(item)}");
    // //   }
    }

    string[] tempFileContents;

#endregion //DB

    // Start is called before the first frame update
    void Start()
    {
      // TEMPORARY -> BEFORE MAKING ACCOUNTS
      // TODO: grab data from the database
        List<ToDB.UserData> temp_allUsers = null;
        string query =
      @"query {
          getUsers { _id, username }
      }";
        GraphQL.APIGraphQL.Query(query, new {}, response => temp_allUsers = response.GetList<ToDB.UserData>("getUsers")); // works
        // bool userExists = temp_allUsers.Select(_u => _u.userId == "4e521d940a-6725-3265-e9910381cd");

        Download_GetAllWeapons();

        if(false)
        {
          CurrentUser = GenerateNewUser("Hyper");
          Debug.Log(string.Format("UserId > {0} : {1}", CurrentUser.UserId, CurrentUser.Username));
          Debug.Log(string.Format("Spawning user `{0}`...",CurrentUser.UserId ));

          _ud = new ToDB.UserData();
          _ud.UserId = CurrentUser.UserId;
          _ud.Username = CurrentUser.Username;

          Debug.Log(JsonConvert.SerializeObject(_ud));
          // StartCoroutine(Upload_User(JsonConvert.SerializeObject(_ud), result => {
          //     Debug.Log(result);
          // }));
        }
        // Upload_User();

        // StartCoroutine(Upload_GetEquippable(EQUIPPABLESLOT.Chest.ToString().ToLower(), result => {
        //     Debug.Log(result);
        // }));

        StartCoroutine(Download_GetEquippable(/*EQUIPPABLESLOT.Chest.ToString().ToLower()*/ null, result => {
            Debug.Log(result);
        }));

        // if (File.Exists(GameEvent.tempDataFile))
        // {
        //     var fileContent = File.ReadAllText(GameEvent.tempDataFile);
        //     tempFileContents = string.Parse(fileContent);
        // }

        // Debug.Log(tempFileContents);

    }

    // Update is called once per frame
    void Update()
    {
      if(_WeaponList.Count != WeaponList.Count)
      {
        Debug.Log($"Inside Update (UserBase > WeaponList) : {WeaponList.Count}");
        _WeaponList = WeaponList;
      }
    }


    User GenerateNewUser(/*string uid, */string username)
    {
        User newUser = new User();
        newUser.UserId = System.Guid.NewGuid().ToString();
        // Debug.Log(generateNewUUID());
        newUser.Username = username;
        
        newUser.User_Level = 1;
        newUser.Experience = 0;
        newUser.Coin = 0;
        newUser.Diamond = 0;
        newUser.Crystal = 0;
        newUser.Backpack_Max_Size = 40;
        newUser.Vault_Max_Size = 100;

        return newUser;
    }

    void DoPlayerLoadRitual() 
    {
      // Load `ExperienceBar`
      eb.DoBarLoad();
      Debug.Log($"The Player > {CurrentUser}");

    }

    public void DoExperienceGain(double expGain) 
    {
      CurrentUser.Experience += expGain;
      Debug.Log($"EXP Gain for user {CurrentUser.Username} [{CurrentUser.UserId}] > {CurrentUser.Experience-expGain} + {expGain} = {CurrentUser.Experience}");
      eb.OnExperienceGain(CurrentUser.Experience);
    }
}
