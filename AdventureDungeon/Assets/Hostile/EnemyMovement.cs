using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public struct Movement 
{
  public float Direction;// { get; set; }
  // public enum TagType = {Level, Player, Enemy};// { get; set; }
  public float Weight;// { get; set=>0; }

}

public class EnemyMovement : MonoBehaviour
{
    EnemyController ec;
    Rigidbody2D rb;
    float detectionRange = 7.5f;

    private Vector2 moveDirection;
    

    // public enum AIState
    // {
    //     Patrol,
    //     Follow,
    //     Attack,
    // }

    AIPath aip;    
    /** The Weights for each `Tag` */
    // List<Movement>() tagMovement = new List<Movement>();
    // Movement tagPlayer;
    // tagPlayer.TagType = Movement.TagType.Player;
    // tagMovement.Add(tagPlayer);

    List<Movement> pointMovement = new List<Movement>();


    void Start() 
    {
      rb = this.gameObject.GetComponent<Rigidbody2D>();
      aip = this.gameObject.GetComponent<AIPath>();
      ec = this.gameObject.GetComponent<EnemyController>();
      
      DoEnemyStatSync();


      int amt = 16;
      for (int i = 0; i < amt; i++)
      {
        // Debug.Log($"Creating line with rotation {(360.0/amt)*(i+1)}");
        // rotation = (360/i)*amt;
        Movement m = new Movement();
        m.Direction = i;
        pointMovement.Add(m);
      }

      Debug_CreateDotLines();
    }

    // Update is called once per frame
    void Update()
    {
      // ProcessInputs();
      // TestCone(tar.transform.position);
      // Collider[] hitColliders = Physics.OverlapSphere((Vector2)this.transform.position, 13.0f);
      // foreach (var hitCollider in hitColliders)
      // {
      //     // hitCollider.SendMessage("AddDamage");
      //     Debug.Log($"Collider : {hitCollider}");
      // }


      Inspect();
    }

    /** Syncs the enemy stats with the varios other scripts needed for the enemy */
    void DoEnemyStatSync()
    {
      EnemyStats es = ec.enemyStats;
      
      aip.maxSpeed = es.Movement_Speed;
    }

    void Slowed(double time)
    {
      aip.maxSpeed*=0.3f;
    }

    RaycastHit2D[] castStar = new RaycastHit2D[0];
    public void Inspect()
    {
        /*RaycastHit2D[] */castStar = Physics2D.CircleCastAll(transform.position, detectionRange, Vector2.zero);
        foreach (RaycastHit2D raycastHit in castStar)
        {
          if(raycastHit.collider.tag == "Enemy") { continue; }
          // (transform.position - raycastHit.collider.transform.position).normalized
          // Debug.Log($"{Vector2.Dot((transform.position - raycastHit.collider.transform.position).normalized, Vector2.zero)}\t{(transform.position - raycastHit.collider.transform.position).normalized}\t{transform.position} - {raycastHit.collider.transform.position}\t{raycastHit.collider.name}");
          Vector2 nv = (transform.position - raycastHit.collider.transform.position);
          float angle = Vector2.Angle(new Vector2(0.0f, 1.0f), nv);
          if (nv.x < 0.0f) {
              angle = 360f - angle;
          }
          // Debug.Log($"Angle : {angle} ({(transform.position - raycastHit.collider.transform.position).normalized})");

          // nma.SetDestination(raycastHit.collider.transform.position);
        }
    }

    void FixedUpdate()
    {
      // Do Physics
    }

    // var dot = away_vector.dot(normal)
    // favoring the sides
    // var weight = 1.0 - abs(dot - 0.65)

    // float cutoff = 22.5f;
    // Transform tar;
    // public bool TestCone(Vector2 inputPoint)
    // {
    //   // float cosAngle = Vector2.Dot((inputPoint - (Vector2)this.transform.position).normalized, this.transform.forward);
    //   float cosAngle = Vector2.Dot((inputPoint - (Vector2)this.transform.position).normalized, Vector2.zero);
    //   float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg/* * Mathf.Deg2Rad*/;
    //   // Debug.Log($"{inputPoint} \t cosAngle : {cosAngle} \t angle : {angle} \t angle < cutoff : {angle} < {cutoff}");

    //   return angle < cutoff;
    //   // float moveX = Input.GetAxisRaw("Horizontal");
    //   // float moveY = Input.GetAxisRaw("Vertical");

    //   // moveDirection = new Vector2(moveX, moveY).normalized;
    // }

    float lDir = 0;//pointMovement.Select(x => x.Weight).Max();
    void OnDrawGizmos()
    {
      // Gizmos.color = TestCone(tar.transform.position) ? Color.green : Color.red;
      // Gizmos.DrawLine(this.transform.position, tar.position);

      int amt = 16;

      // Working Gizmo System
      if(false) {
        foreach (RaycastHit2D raycastHit in castStar)
        {
          if(raycastHit.collider.tag == "Enemy") { continue; }
          Gizmos.color = raycastHit.collider.tag == "Level" ? Color.red : raycastHit.collider.tag == "Player" ? Color.green : Color.blue;
          Gizmos.DrawLine(this.transform.position, raycastHit.transform.position);

          Vector2 nv = (transform.position - raycastHit.collider.transform.position);
          float angle = Vector2.Angle(new Vector2(0.0f, -1.0f), nv);
          if (nv.x < 0.0f) {
              angle = 360f - angle;
          }
          // if(raycastHit.collider.tag == "Player"){Debug.Log($"Angle : {angle} @ {(int)(angle/(360.0f/amt))}");}

          pointMovement.ForEach(num => {
            Gizmos.color = (num.Direction+1)%2==0 ? Color.green : Color.white;

            // get largest .Weight
            // lDir = pointMovement.Select(x => x.Weight).Max();
            
            if(raycastHit.collider.tag != "Player") { Gizmos.DrawLine(this.transform.position, (Vector2)((Vector2)this.transform.position+DegreeToVector2((360.0f/amt)*(num.Direction+1.5f)) * ((detectionRange * num.Weight)+1)));return; }
            int inSection = ((int)(angle/(360.0f/amt)))+3;
            if(inSection >= amt) {inSection%=amt;}
            if(num.Direction == (inSection)) {
              // Debug.Log($"Angle : {angle} @ {inSection} line will be long...");
              num.Weight = 1.0f;
              lDir = inSection;
            }
            else {
              // if lDir in range of approx 3 or 4
              double valmin = (lDir-3); double valmax = (lDir+3);
              
              double w = num.Direction > lDir ? Normalize(num.Direction, valmin, valmax) : Normalize(num.Direction, valmin, valmax) ; 
              num.Weight = (float)w; //0.0f;

            }
            Gizmos.DrawLine(this.transform.position, (Vector2)((Vector2)this.transform.position+DegreeToVector2((360.0f/amt)*(num.Direction+1.5f)) * ((detectionRange * num.Weight)+1)));
            return;
          });
        }
      }
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    
    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    /** Weights for a more "interactive" movement system */
    double Normalize(double val, double valmin, double valmax/*, double min, double max*/) 
    {
        // temp until global `amt`
        int amt = 16;
        double r=0;
        // if(val > valmax || valmin > valmax) {r = (valmin - valmax)/amt;}
        // if(val < valmin) {r = (val - valmin)/amt;}
        // if(val < valmin || valmax > valmin) {r = (val - valmin)/amt;}
        
        if((valmin <= val && val <= valmax) || (valmin+amt <= val || val <= valmax-amt)) 
        {
          // valmax%=amt;
          // if(valmin < 0) { valmin+=amt; }

          // if(valmax >= amt) {r=0.375;}
          // else if(valmin < amt) {r=0.275;}
          // else {r = ((val-valmin)+(valmax - val))/amt;}

          if((val - valmin == 2 || val - valmin+amt == 2) || (valmax - val == 2 || (valmax-amt) - val == 2) || /*negative*/ (valmin+amt - val == -2)) // || (val - valmin == -2 || val - valmin+amt == -2) || (valmax - val == -2 || (valmax-amt) - val == -2)
          {
            r = 0.75;
          }
          else if((val - valmin == 1 || val - valmin+amt == 1) || (valmax - val == 1 || (valmax-amt) - val == 1) || /*negative*/ (valmin+amt - val == -1))
          {
            r = 0.5;
          }
          else if((val - valmin == 0 || val - valmin+amt == 0) || (valmax - val == 0 || (valmax-amt) - val == 0) || /*negative*/ (valmin+amt - val == 0))
          {
            r = 0.25;
          }
          // // else { r=0.66; }

          // else if ((valmin + val == 2 || valmin+amt + val == 2) || (val + valmax == 2 || val + (valmax-amt) == 2)) 
          // {
          //   r = 1.75;
          // }

          // Debug.Log($"{valmin} + {val} == -2 || {valmin+amt} - {val} == -2) || ({val + valmax} == -2 || {val} - ({valmax}+{amt}) == -2)");
          // Debug.Log($"(({valmin + val} == -2 || {valmin+amt - val} == -2) || ({val + valmax} == -2 || {val - (valmax+amt)} == -2))");
          // Debug.Log($"(({valmin + val == -2} || {valmin+amt - val == -2}) || ({val + valmax == -2} || {val - (valmax+amt) == -2}))");
          
        }

        // val = 4
        // ((1 / 3) * (1))
        // double r = ((((val - valmin) / (valmax - valmin)) * (max - min)) + min)-2;
        // if(r<0) { r=0; }
        return r;
    }


    void Debug_CreateDotLines()
    {
      int amt = 16;
      for (int i = 0; i < amt; i++)
      {
        // Debug.Log($"Creating line with ratation {(360.0/amt)*(i+1)}");
        // GameObject.Instantiate();
        // rotation = (360/i)*amt;
      }
    }

    void DetermineMoveDirection()
    {

    }
}
