using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public struct EnemySpriteTextures
{
    public Sprite CenterSprite;

    public Sprite UpSprite;
    public Sprite UpRightSprite;
    public Sprite RightSprite;
    public Sprite DownRightSprite;
    public Sprite DownSprite;
    public Sprite DownLeftSprite;
    public Sprite LeftSprite;
    public Sprite UpLeftSprite;

    public List<Sprite> SpriteParticles;
}

public class EnemySpriteController : MonoBehaviour
{
    public EnemySpriteTextures enemySprites;
    public float angle=0f;
    public bool lookingAtPlayer=false;

    SpriteRenderer rootSprite;

    EnemyManager m_EnemyManager;
    Vector3 pos;
    float dist;
    public Transform nearestPlayer;
    // Start is called before the first frame update
    void Start()
    {
      rootSprite = gameObject.GetComponent<SpriteRenderer>();
      m_EnemyManager = FindObjectOfType<EnemyManager>();
      pos = transform.position;
      dist = float.PositiveInfinity;
    }

    // Update is called once per frame
    void Update()
    {
      if(m_EnemyManager.Players.Count!=0)
      {
        nearestPlayer = m_EnemyManager.Players.OrderBy(t=>(t.gameObject.transform.position - transform.position).sqrMagnitude)
                            //  .Take(3)   //or use .FirstOrDefault();  if you need just one
                            //  .ToArray();
                            .FirstOrDefault().transform;
        lookingAtPlayer=true;
      } else { lookingAtPlayer=false; }
      Vector2 direction = (nearestPlayer==null ? new Vector2(0,0) : (Vector2)nearestPlayer.position) - (Vector2)transform.position;
      angle = Vector2.SignedAngle(Vector2.up, direction);
      
      switch (angle)
      {
        case <=-157.5f or >157.5f:
          // Debug.Log($"<=-157.5f or >157.5f (S): Nearest player Angle : {angle}");
          rootSprite.sprite = enemySprites.DownSprite;
          break;
        case >-157.5f and <=-112.5f:
          // Debug.Log($">-157.5f and <=-112.5f (SE): Nearest player Angle : {angle}");
          rootSprite.sprite = enemySprites.DownRightSprite;
          break;
        case >-112.5f and <=-67.5f:
          // Debug.Log($">-112.5f and <=-67.5f (E): Nearest player Angle : {angle}");
          rootSprite.sprite = enemySprites.RightSprite;
          break;
        case >-67.5f and <=-22.5f:
          // Debug.Log($">-67.5f and <=-22.5f (NE): Nearest player Angle : {angle}");
          rootSprite.sprite = enemySprites.UpRightSprite;
          break;
        case >-22.5f and <=22.5f:
          // Debug.Log($">-22.5f and <=22.5f (N): Nearest player Angle : {angle}");
          rootSprite.sprite = enemySprites.UpSprite;
          break;
        case >22.5f and <=67.5f:
          // Debug.Log($">22.5f and <=67.5f (NW): Nearest player Angle : {angle}");
          rootSprite.sprite = enemySprites.UpLeftSprite;
          break;
        case >67.5f and <=112.5f:
          // Debug.Log($">67.5f and <=112.5f (W): Nearest player Angle : {angle}");
          rootSprite.sprite = enemySprites.LeftSprite;
          break;
        case >112.5f and <=157.5f:
          // Debug.Log($">112.5f and <=157.5f (SW): Nearest player Angle : {angle}");
          rootSprite.sprite = enemySprites.DownLeftSprite;
          break;
        default:
          // Debug.Log($"Default : Nearest player Angle : {angle}");
          break;
      }

    }
}
