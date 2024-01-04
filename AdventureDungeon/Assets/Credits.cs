using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CREDITTYPE
{
    Title,
    Credit,
    Break,
}

struct Credit
{
  public string title;
  public string description;
  public string url;
  public CREDITTYPE type;

  // public static NewCredit(string t, string d, string url=null)
  public Credit(string t, string d, string url=null, CREDITTYPE type=CREDITTYPE.Credit) 
  {
    this.title = t;
    this.description = d;
    this.url = url;
    this.type = type;
  }

  // public Credit NewCredit(string t, string d, string url=null)
  // {
  //   this.title = t;
  //   this.description = d;
  //   this.url = url;
  // }
}

public class Credits : MonoBehaviour
{
    List<Credit> creditList = new List<Credit>();
    Credit nc = new Credit("Lead Programmer","A* Pathfinding Project","https://arongranberg.com/astar/front");
    // Credit nc = Credit.NewCredit("AI Pathing","A* Pathfinding Project","https://arongranberg.com/astar/front");
    // nc.NewCredit("AI Pathing","A* Pathfinding Project","https://arongranberg.com/astar/front");
    // Start is called before the first frame update
    void Start()
    {
      creditList.Add(new Credit("Lead Programmer","Brayden \"Hyperactvty\" Thompson"));
      creditList.Add(new Credit("Lead Developer","Brayden \"Hyperactvty\" Thompson"));
      creditList.Add(new Credit("Icons","Google's MUI Icons","https://fonts.google.com/icons?icon.set=Material+Icons"));
      creditList.Add(new Credit("AI Pathing","A* Pathfinding Project","https://arongranberg.com/astar/front"));
      creditList.Add(new Credit("Database Querying Methods","rafaelnsantos","https://github.com/rafaelnsantos/unity-graphql-client/blob/master/README.md"));
      creditList.Add(new Credit("Hub Enviroment [Pixel Art Top Down - Basic]","cainos","https://cainos.itch.io/"));

      creditList.Add(new Credit("Special Thanks","", null, CREDITTYPE.Title));
      creditList.Add(new Credit("Emotional Support Hamster","Little Man"));
      creditList.Add(new Credit("Stack Overflow","For when my brain can't do math"));

      // DB
      // On data base side, you would have players, themes and players related theme table, with.
      // Players, if more than one, will store basics information about each player. ID, loging data, etc.

      // On themes side, you have theme ID and related theme data.

      // One player related theme table, you need column for player ID, column for theme ID. That is simplest form.
      // But you can expand by adding other data, including column storing IsThemeActive. Etc.

      // PHP
      // PHP side will need add, or remove references of player to theme, from player related theme table.
      // Alternatively just toggle IsThemeActive cell, for matching player / theme.

      // In php you have listener by using GET, to receive player ID following by its data in same request.
      // Something like for example
      // ?playerID=5&themeID=3&themeActive=1

      // If you got only one player, then you can shrink to
      // ?themeID=3&themeActive=1

      // But for more players, you may also need authentication hash code. That providing, you concerns about something hacking application, and changing ID in Unity application registry.

      // Unity
      // Either way these request will be sent from Unity.

      // That's pretty much minimum what you may need. 

    }

}
