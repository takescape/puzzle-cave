using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class HackManager : MonoBehaviour
{
    #region Fields
    [Header("Settings")]
    [SerializeField] private int clickTimesToOpenPanel = 3;
    [SerializeField] private float maxSecondsToInvalidateClick = .5f;
    [Header("Debug")]
    [SerializeField, ReadOnly] private int clickCount;
    [SerializeField, ReadOnly] private float clickTimer;
    #endregion

    #region Unity Messages
    private void Update()
    {
        if (clickCount > 0)
        {
            clickTimer += Time.deltaTime;

            if (clickTimer > maxSecondsToInvalidateClick)
            {
                clickCount = 0;
                clickTimer = 0;
            }
        }
    }
    #endregion

    #region Public Methods
    // method used in unity events of hack manager UI button
    public void TryOpenPanel()
    {
        clickCount++;
        clickTimer = 0;
        Debug.Log("Clicked to open panel");

        if (clickCount >= clickTimesToOpenPanel)
        {
            clickCount = 0;
            clickTimer = 0;
            Debug.Log("Opened hack panel");
        }
    }
    #endregion
}
