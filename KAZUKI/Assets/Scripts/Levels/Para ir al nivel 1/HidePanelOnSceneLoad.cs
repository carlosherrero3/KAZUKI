using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePanelOnSceneLoad : MonoBehaviour
{
    public GameObject panelToHide;

    void Start()
    {
        if (panelToHide != null)
        {
            panelToHide.SetActive(false);
        }
    }
}
