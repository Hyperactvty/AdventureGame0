using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGroup : MonoBehaviour
{
    public GameObject[] panels;
    public TabGroup tabGroup;
    public int panelIndex;

    private void Awake()
    {
      ShowCurrentPanel();
    }

    void ShowCurrentPanel()
    {
        for (int i = 0; i < panels.Length; i++)
        {
          panels[i].gameObject.SetActive(i==panelIndex);
        }
    }

    public void SetPageIndex(int index)
    {
      panelIndex = index;
      ShowCurrentPanel();
    }
}
