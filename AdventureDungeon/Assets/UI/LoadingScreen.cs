using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class LoadingScreen : MonoBehaviour
{
    List<KeyValuePair<string, string>> elementColours = new List<KeyValuePair<string, string>>();
    List<string> TipsList = new List<string>();
    public TextMeshProUGUI tipText;
    string prevTip="";
    private System.Random rand = new System.Random();
    private IEnumerator coroutine_TipChange;
    private IEnumerator coroutine_SceneLoaded;

    // Start is called before the first frame update
    void Start()
    {
        elementColours.Add(new KeyValuePair<string, string>("Fire", "#c22b2b"));
        elementColours.Add(new KeyValuePair<string, string>("Water", "#2b4cc2"));
        elementColours.Add(new KeyValuePair<string, string>("Wood", "#4ec22b"));

        TipsList.Add("Dying is probably not good for your health.");
        TipsList.Add("Be sure to equip gear that is a higher level than your current gear.");
        TipsList.Add("Check back regularly for new updates!");
        TipsList.Add("<b><color=#493ede>Hamsters</color></b> don't like being tossed. <b>Don't.</b>");
        TipsList.Add("Rarity is as follows: <color=#fff>Common</color>, <color=#2bc26c>Uncommon</color>, <color=#2b77c2>Rare</color>, <color=#532bc2>Epic</color>, <color=#c26c2b>Legendary</color>, <color=#c22b2b>Mythic</color>, <color=#c22b9f>Limited</color>, and <color=#8d2bc2>Limited (A)</color>."); // Limited (A) #422bc2
        TipsList.Add("Remember: <b><color=#c22b2b>[Fire]</color></b> > <b><color=#4ec22b>[Wood]</color></b> > <b><color=#2b4cc2>[Water]</color></b> > <b><color=#c22b2b>[Fire]</color></b>");
        TipsList.Add("The <b><color=#c22b9f>[Bass Cannon]</color></b> can <color=#c29c2b>stun</color> enemies for a short period of time.");

        TipsList.Add("<b><color=#2b4cc2>Saruman the Soggy</color></b> is cursed to be forever soggy...");

        coroutine_TipChange = TipChangeCoroutine();
        StartCoroutine(coroutine_TipChange);
        coroutine_SceneLoaded = SceneLoadedCoroutine();
        StartCoroutine(coroutine_SceneLoaded);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TipChangeCoroutine() {
      //Declare a yield instruction.
      WaitForSeconds wait = new WaitForSeconds(5f);
 
      // for(int i = 0; i < 10; i++) {
      while(true)
      {
        List<string> chosenTip = TipsList.Where(_e => _e != prevTip).ToList().Select(_e => _e).ToList();
        prevTip = chosenTip[rand.Next(chosenTip.Count)];
        tipText.text = prevTip;
        yield return wait; //Pause the loop for 5 seconds.
      }
   }

   IEnumerator SceneLoadedCoroutine() {
      //Declare a yield instruction.
      WaitForSeconds wait = new WaitForSeconds(7f);
 
      yield return wait; // Just temporary until have system in place to ensure level is completley loaded

      LoadScreenEvent loadedSceneEvent = Events.LoadScreenEvent;
      loadedSceneEvent.Loading = false;
      EventManager.Broadcast(loadedSceneEvent);
    
   }
}
