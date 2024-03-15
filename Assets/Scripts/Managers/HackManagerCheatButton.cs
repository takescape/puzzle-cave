using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HackManagerCheatButton : MonoBehaviour
{
    #region Fields
    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;

    private Action cheatAction;
    #endregion

    #region Properties
    #endregion

    #region Public Methods
    public void SetCheatAction(Action action)
    {
        cheatAction = action;
    }

    public void SetCheatName(string name)
    {
        text.text = name;
    }

    // use in unity events for button on click
    public void CallCheatAction()
    {
        cheatAction?.Invoke();
    }
    #endregion
}
