// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.UI;
// // using UnityEngine.UIElements;


// public class PlayerSpriteDisplayUI : MonoBehaviour
// {
//     public GameObject playerSprite;
//     // public RawImage placeholder;
//     public RectTransform placeholder;

//     UserBase _ub;

//     // Start is called before the first frame update
//     void Start()
//     {
//         _ub = gameObject.GetComponentInParent<UserBase>();
//         //GetComponent<>();
//         // var foundPlayerObjects = FindObjectsByType<PlayerCharacterController>(FindObjectsSortMode.None).ToList().Where(_u => _u.CurrentUser.UserId == );
//         // List<PlayerStatsBase>
//         var foundPlayerObjects = FindObjectsByType<PlayerStatsBase>(FindObjectsSortMode.None).Select(_u => _u);
//         GameObject grabbedPlayerSprite = foundPlayerObjects.Where(_u => _u.UserId == _ub.CurrentUser.UserId ).FirstOrDefault().gameObject;
//         GameObject spritePrefab = grabbedPlayerSprite.GetComponentInChildren<PlayerSpriteController>().gameObject;

//         Debug.Log($"Spirite Count : {spritePrefab.transform.childCount}");

//         PlayerSpriteController _newPSC = gameObject.AddComponent(typeof(PlayerSpriteController)) as PlayerSpriteController;
//         _newPSC.isUI = true;
//         // _newPSC.enabled = false;
//         for (int i = spritePrefab.transform.childCount-1; i >= 0; i--)
//         {
//           var _gc = spritePrefab.transform.GetChild(i);
//           GameObject spriteInstance = Instantiate(new GameObject(), transform.localPosition, Quaternion.identity, transform);
//           spriteInstance.name = _gc.name;
//           Image img = spriteInstance.AddComponent(typeof(Image)) as Image;
//           img.sprite = _gc.GetComponent<SpriteRenderer>().sprite;
//           img.SetNativeSize();
//           // RectTransform img = gameObject.AddComponent(typeof(RectTransform)) as RectTransform;
//           RectTransform _sI = spriteInstance.GetComponent<RectTransform>();
//           _sI.pivot = new Vector2(0.5f,0.5f);
//           _sI.anchorMin = new Vector2(0.5f, 0);
//           _sI.anchorMax = new Vector2(0.5f, 0);

//           _sI.anchoredPosition = ((Vector2)_gc.transform.position*(100f/3f))+ new Vector2(0,50);

//           switch (_gc.name)
//           {
//             case "Head":
//               _newPSC.HeadRect = _sI;
//               _newPSC.modifiedHeadPos = _sI.anchoredPosition;
//               break;
//             case "Body":
//               _newPSC.BodyRect = _sI;
//               _newPSC.modifiedBodyPos = _sI.anchoredPosition;
//               break;
//             case "BodyTall":
//               _newPSC.BodyTallRect = _sI;
//               _newPSC.modifiedBodyTallPos = _sI.anchoredPosition;
//               break;
//             case "BackWaist":
//               _newPSC.BackWaistRect = _sI;
//               _newPSC.modifiedBackWaistPos = _sI.anchoredPosition;
//               break;
//             default:
//               break;
//           }
//         }

        
//         // GameObject spriteInstance = Instantiate(spritePrefab, transform.localPosition, Quaternion.identity, transform);
//         // RectTransform _sI = spriteInstance.GetComponent<RectTransform>();
//         // _sI.anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
