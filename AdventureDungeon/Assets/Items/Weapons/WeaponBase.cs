using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// using UnityEngine.InputSystem;

public class Projectile
{
  public GameObject projectile;
  public float projectileSpeed = 50f;
  // the time before the projectile self-destroys
  public float projectileLifetime = 5f;
  public float projectileAngleFix = -90f;
}

public class WeaponBase : MonoBehaviour
{
  public string weaponName;
  public GameObject weaponSprite;

  // How far away the weapon is from the body
  public float playerBodyOffset=1f;
  public float projectileWeaponOffset=1f;

  public GameObject projectile;
  public float projectileSpeed = 5f;
  public bool fixAngle = false;
  public float projectileAngleFix = 90f;
  
  public EnemyManager m_EnemyManager;
  Vector3 pos;
  float dist;
  Transform nearestEnemy;
  public Vector2 direction;

  Vector2 mousePosition;

  // maybe have enum for `Vector2.up, Vector2.left, Vector2.right`
  
  // Start is called before the first frame update
  void Start()
  {
      weaponSprite.transform.localPosition = new Vector2(0, playerBodyOffset);

      m_EnemyManager = FindObjectOfType<EnemyManager>();
      pos = transform.position;
      dist = float.PositiveInfinity;
  }

  // Update is called once per frame
  void Update()
  {
    // pos = transform.position;
    // dist = float.PositiveInfinity;
    // foreach(var obj in m_EnemyManager.Enemies)
    // {
    //     var d = (pos - obj.transform.position).sqrMagnitude;
    //     // Debug.Log($"n: {obj} \t d: {d}");
    //     if(d < dist)
    //     {
    //         nearestEnemy = obj.gameObject.transform;
    //         dist = d;
    //     }
    // }

    if(m_EnemyManager.AllHostiles.Count!=0)
    {
      nearestEnemy = m_EnemyManager./*Enemies*/AllHostiles.OrderBy(t=>(t.gameObject.transform.position - transform.parent.transform.position).sqrMagnitude)
                          //  .Take(3)   //or use .FirstOrDefault();  if you need just one
                          //  .ToArray();
                          .FirstOrDefault().transform;
    }

    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    // Vector2 direction = mousePosition - (Vector2)transform.position;
    direction = (nearestEnemy==null ? mousePosition : (Vector2)nearestEnemy.position) - (Vector2)transform.position;
    float angle = Vector2.SignedAngle(Vector2.up, direction);
    transform.eulerAngles = new Vector3 (0, 0, angle);

    if (Input.GetButtonDown("Attack") && false) 
    {
      // GameObject ball = Instantiate(projectile, transform.position, transform.rotation);
      // ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector2(0, launchVelocity));

      GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
      shot.transform.position = (Vector2)transform.TransformPoint(/* Vector3.right * 2 */ Vector3.up * projectileWeaponOffset);
      shot.transform.eulerAngles = new Vector3 (0, 0, angle+(fixAngle ? projectileAngleFix:0));
      shot.GetComponent<Rigidbody2D>().velocity = direction.normalized * projectileSpeed;
    }

    
  }
}
