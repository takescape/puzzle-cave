using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackManager : MonoBehaviour
{
    #region Fields
    [Header("References")]
    [SerializeField] private Button openButton;
    #endregion

    #region Public Methods
    // method used in unity events of hack manager UI button
    public void OpenPanel()
    {
        Debug.Log("Test opening hack panel");
    }
    #endregion
}
