using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnCollisionEnter2D(Collision2D col)
    private void OnTriggerEnter2D(Collider2D col)
    {
      if(col.GetComponent<Collider2D>().tag == "Player") {return;}
      // Debug.Log($"Collision {col.GetComponent<Collider2D>().name}");

      // Destroy(gameObject);
    }
}
