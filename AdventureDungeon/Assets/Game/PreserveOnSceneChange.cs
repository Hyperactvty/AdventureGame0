using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreserveOnSceneChange : MonoBehaviour
{
  void Awake()
  {
      DontDestroyOnLoad(this.gameObject);
  }
}
