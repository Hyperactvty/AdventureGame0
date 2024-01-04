using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float spawnDelay = 0.5f;
    // After spawn delay, activiate the enemy
    public GameObject enemy;
    public Vector2 _pos = new Vector2(0,0);
    // public Sprite warningSprite;

    public bool allowSpawn=false;

    private IEnumerator coroutine_Spawn;
    private System.Random rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
      coroutine_Spawn = SpawnCoroutine();
      StartCoroutine(coroutine_Spawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCoroutine() {
      if(_pos==new Vector2(0,0)) { 
        StopCoroutine(coroutine_Spawn);
      }
      RoomBase rB = transform.parent.gameObject.GetComponent<RoomBase>();
      enemy = rB.enemySpawn[rand.Next(rB.enemySpawn.Count)];
      float spawnRandomTime = Random.Range(0f,1.25f);
      //Declare a yield instruction.
      WaitForSeconds wait = new WaitForSeconds(spawnRandomTime);
      yield return wait; 

      // GameObject newEnemySpawn = Instantiate(new GameObject(), new Vector2(0,0),Quaternion.identity, transform);
      // newEnemySpawn.transform.localPosition = transform.localPosition;
      transform.localScale = enemy.transform.localScale;
      // var _sr = newEnemySpawn.AddComponent<SpriteRenderer>();
      // _sr.sprite = warningSprite;

      wait = new WaitForSeconds(spawnDelay);
      yield return wait; 
      Destroy(transform.parent.GetComponent<SpriteRenderer>());
      // Destroy(newEnemySpawn);
      wait = new WaitForSeconds(0.5f);
      yield return wait; 
      allowSpawn = true;

      GameObject instanceEnemy = Instantiate(enemy, new Vector2(0,0),Quaternion.identity, transform.parent);
      instanceEnemy.transform.localPosition = _pos;

      Debug.Log($"[EnemySpawn] Spawning Enemy : {enemy.name} @ {_pos}");
      
      StopCoroutine(coroutine_Spawn);
      Destroy(this.gameObject);
   }

   void OnDestroy()
   {
      StopCoroutine(coroutine_Spawn);
   }
}
