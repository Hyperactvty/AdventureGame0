using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExperienceBar : MonoBehaviour
{
    public UserBase userBase;
    UserBase _ub;

    public GameObject experienceBar;
    public GameObject barTitle;
    public GameObject experienceNotification;

    int MATH_FOR_LEVEL;
    double curExp=0, levelEQ=550; double expThresh = 550;

    // Start is called before the first frame update
    void Start()
    {
      int pn = 0;
      for(int i=0; i < 20; i++) {
          double levelBarStep = 50;
          double trshld = getXP_LOG(i, 4.3f, 0.5f);
          double r = System.Math.Ceiling(getXP_EXP(i)/levelBarStep)*levelBarStep; //getXP_EXP(i, 4.3f, 1.5f, 1.2f);
          double addToPrev = 0; 
          for(int j=0; j < i; j++) {
              // addToPrev += getXP_EXP(j, 4.3f, 1.5f, 1.2f)*pn;
              addToPrev += (System.Math.Ceiling(getXP_EXP(j)/levelBarStep)*levelBarStep)*pn;
              // Console.WriteLine($"i > {i} \t j > {j} \t {addToPrev - (getXP_EXP(j, 4.3f, 1.5f, 1.2f)*pn)} > {addToPrev}");
          }
          //Console.WriteLine($"Level {i} -> {i+1}\t:\t{r} -> ExpULU @50/k : {r/(enemy_drop*trshld)}");
          // Console.WriteLine($"Level {i} -> {i+1}\t:\t{r} -> ExpULU @{enemy_drop*trshld}/k : {r/(enemy_drop*trshld)}");

          // Console.WriteLine($"Level {i} -> {i+1}\t:\t{r} -> ExpULU @{enemy_drop}/k : {(r/(enemy_drop ))} + {addToPrev/enemy_drop} = [{(r+addToPrev)/enemy_drop}]");
          pn=1;
      }
      // _ub = userBase;
      // Debug.Log($"_ub:{_ub.CurrentUser.Username}");
      // DoBarLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoBarLoad() 
    {
      Debug.Log($"USERBASE > {userBase.name}");
      Debug.Log($"USERBASE USER > {userBase.CurrentUser}");
      Debug.Log($"USERBASE EXP > {userBase.CurrentUser.Experience}");
      // MATH_FOR_LEVEL = MathLevel(userBase.CurrentUser.Experience);
      (MATH_FOR_LEVEL, curExp, levelEQ) = MathLevel(userBase.CurrentUser.Experience);
      barTitle.GetComponent<TextMeshProUGUI>().text = $"LVL {MATH_FOR_LEVEL} [{curExp}/{levelEQ}]";
      // barTitle.GetComponent<TextMeshProUGUI>().text = string.Format("XP {0}/{1}",(userBase.CurrentUser.Experience).ToString(), (MATH_FOR_LEVEL).ToString());
      
    }

    /** @date : 05/09/23 @19:46 -> 05/12/23 @13:36 */
    public void OnExperienceGain(double expGain) 
    {
        if(userBase == null) 
        {
          Debug.Log("Userbase is NULL, fixing...");
          Debug.Log($"{transform}");
          // userBase = this.gameObject.GetComponentInParent<UserBase>();
          userBase = transform.gameObject.GetComponentInParent<UserBase>();
          // userBase = transform.parent.parent.parent.gameObject.GetComponentInParent<UserBase>();
          Debug.Log($"Userbase now > {userBase}");
        }
        Debug.Log($"USERBASE > {userBase}");
        Debug.Log($"USERBASE USER > {userBase.CurrentUser.Username}");
        Debug.Log($"USERBASE EXP > {userBase.CurrentUser.Experience}");
        (MATH_FOR_LEVEL, curExp, levelEQ) = MathLevel(userBase.CurrentUser.Experience);
        // barTitle.GetComponent<TextMeshProUGUI>().text = string.Format("XP {0}/{1}",(userBase.CurrentUser.Experience).ToString(), (MATH_FOR_LEVEL).ToString());
        barTitle.GetComponent<TextMeshProUGUI>().text = $"LVL {MATH_FOR_LEVEL} [{curExp}/{levelEQ}]";
        StartCoroutine(DoExpNotif(expGain));
      
    }

    // IEnumerator Next()
    // {
    //     from = transform.localPosition;
    //     to = new Vector3(transform.localPosition.x-1920, transform.localPosition.y, transform.localPosition.z);
    //     tra = transform;
    //     var t = 0f;
  
    //     previous.SetActive(false);
    //     next.SetActive(false);
    
    //     while (t < 1f)
    //     {
    //         t += 1 * Time.deltaTime;
    //         tra.localPosition = Vector3.Lerp(from, to, t);
    //         yield return null;
    //     }
    //     tra.localPosition = to;
  
    //     previous.SetActive(true);
    //     next.SetActive(true);
  
    //     if(transform.localPosition.x == -3840)
    //     {
    //         next.SetActive(false);
    //     }
    //     else
    //     {
    //         previous.SetActive(true);
    //     }
    // }

    // IEnumerator MoveFromTo(Vector3 from, Vector3 to, float speed, Transform tra)
    // {
    //     var t = 0f;
    
    //     while (t < 1f)
    //     {
    //         t += speed * Time.deltaTime;
    //         tra.localPosition = Vector3.Lerp(from, to, t);
    //         yield return null;
    //     }
    // }


    // private void UpdateAdStatus()
    // {
    //     stringBuilder.Clear();
  
    //     if (GameStats.Instance.GetRemoveAds())
    //     {
    //         stringBuilder.Append("OFF");
    //         //txtAdsStatus.color = new Color(15f, 98f, 230f, 255f);
    //         txtAdsStatus.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    //     }
    //     else
    //     {
    //         stringBuilder.Append("ON");
    //         //txtAdsStatus.color = new Color(222f, 41f, 22f, 255f);
    //         txtAdsStatus.color = new Color(222f, 41f, 22f, 255f);
    //     }
  
    //     txtAdsStatus.text = stringBuilder.ToString();
    // }


    /** @date : 05/12/23 @13:37 -> @14:22 */
    IEnumerator DoExpNotif(double expGain)
    {
      TextMeshProUGUI comp = experienceNotification.GetComponent<TextMeshProUGUI>();

      Vector3 from = new Vector3(0,12,-10);//en.transform.localPosition;
      // Vector3 to = new Vector3(en.transform.localPosition.x-1920, en.transform.localPosition.y, transform.localPosition.z);
      Vector3 to = new Vector3(0,-5,-10);
      float speed = 1f;
      Transform tra = experienceNotification.transform;
      var t = 0f;

      comp.text = $"XP +{expGain}";
      comp.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

      // experienceNotification.GetComponent<RectTransform>().sizeDelta = new Vector2(0,12);
      experienceNotification.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,12,0);
      yield return new WaitForSeconds(1.5f);
      // experienceNotification.GetComponent<RectTransform>().sizeDelta  = new Vector2(0,25);
      // experienceNotification.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,25,0);
      while (t < 1f)
      {
          t += speed * Time.deltaTime;
          tra.localPosition = Vector3.Lerp(from, to, t);

          comp.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f),new Color(1.0f, 1.0f, 1.0f, 0.0f),t*1.5f);


          yield return null;
      }
      
    }

    (int, double, double) MathLevel(double exp) 
    {
      int givenLevel=0;
      double levelBarStep = 50;

      /*double*/ levelEQ = System.Math.Ceiling(getXP_EXP(givenLevel)/levelBarStep)*levelBarStep;
      if(exp >= levelEQ) 
      {
        int i=0; 
        double sub = exp;
        // for(int i=0; i < 20; i++) {
        while(sub > 0) {
            /* The experience multiplier based on current world level  */
            // double trshld = getXP_LOG(i, 4.3f, 0.5f);

            levelEQ = System.Math.Ceiling(getXP_EXP(i)/levelBarStep)*levelBarStep;
            Debug.Log($"lvl -> {i} \t r.exp > {sub} \t exp_cap : {(System.Math.Ceiling(getXP_EXP(i)/levelBarStep)*levelBarStep)}");
            if(sub >= (System.Math.Ceiling(getXP_EXP(i)/levelBarStep)*levelBarStep))
            {
              sub-=(System.Math.Ceiling(getXP_EXP(i)/levelBarStep)*levelBarStep);
              i++;
            }
            else {
              givenLevel = i;
              break;
            }
            curExp = sub;
            
            // temp
            if(i> 255) { break; }
        }
      }
      return (givenLevel, curExp, levelEQ);
    }

    // For `EXP Gain`
    double getXP_LOG(int lvl, float a, float b)
    {
        double xp = a * System.Math.Sqrt(lvl) + b;
        xp = System.Math.Ceiling((float)xp);
        return xp;
    }
    
    // For `Level`
    double getXP_EXP(int lvl/*, float a, float b, float c*/)
    {
        float a=4.3f,b=1.5f,c=1.2f;
        double xp = a * (System.Math.Pow(b, lvl)) + c;
        xp = System.Math.Floor((float)xp*100);
        return xp;
    }
    

    double CalcEXPBasedOnLvl(int userLevel, int enemyLevel) 
    {
      double r = userLevel < enemyLevel-2 ? 0.25 : userLevel >= enemyLevel+2 ? -1.5 : 1;
      // if(userLevel < enemyLevel-2) { r = 1; }
      // else if(userLevel >= enemyLevel+2) { r = -1; }
      // else { r = 0; }
      return r;
    }
}
