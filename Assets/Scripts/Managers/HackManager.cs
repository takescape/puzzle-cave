using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HackManager : Singleton<HackManager>
{
    #region Fields
    [Header("Settings")]
    [SerializeField] private int clickTimesToOpenPanel = 3;
    [SerializeField] private float maxSecondsToInvalidateClick = .5f;
    [SerializeField] private string password = "1275";
    [SerializeField] private bool usePassword;
	[Header("References")]
    [SerializeField] private GameObject passwordPanel;
    [SerializeField] private GameObject cheatsPanel;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private Transform cheatsButtonList;
    [SerializeField] private HackManagerCheatButton cheatsButtonPrefab;
    [Header("Debug")]
    [SerializeField, ReadOnly] private int clickCount;
    [SerializeField, ReadOnly] private float clickTimer;
    [SerializeField, ReadOnly] private string typedPassword;
    [SerializeField, ReadOnly] private bool isCheatsPopulated;
    #endregion

    #region Unity Messages
    protected override void Awake()
    {
        base.Awake();
        HidePanels();
    }

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
    public void TryOpenPasswordPanel()
    {
        clickCount++;
        clickTimer = 0;

        if (clickCount >= clickTimesToOpenPanel)
        {
            clickCount = 0;
            clickTimer = 0;
            OpenPasswordPanel();
        }
    }

    // method used in unity events of hack manager UI password panel button
    public void TryOpenCheatsPanel()
    {
        // save typed password for the game session
        typedPassword = passwordInputField.text;

        if (typedPassword == password)
            OpenCheatsPanel();

        // clear input field to indicate password incorrect
        passwordInputField.text = "";
    }

    // method used in unity events on close button of hack manager UI
    public void HidePanels()
    {
        passwordPanel.SetActive(false);
        cheatsPanel.SetActive(false);
        closeButton.SetActive(false);
    }
    #endregion

    #region Private Methods
    private void OpenPasswordPanel()
    {
		// if password is disabled or if already typed password
		// then open the cheats panel directly
		if (!usePassword || typedPassword == password)
        {
            OpenCheatsPanel();
            return;
        }

        passwordPanel.SetActive(true);
        cheatsPanel.SetActive(false);
        closeButton.SetActive(true);
    }

    private void OpenCheatsPanel()
    {
        passwordPanel.SetActive(false);
        cheatsPanel.SetActive(true);
        closeButton.SetActive(true);

        PopulateCheats();
    }

    private void PopulateCheats()
    {
        if (isCheatsPopulated)
            return;

        foreach (var cheatAction in CreateCheatsList())
        {
            HackManagerCheatButton cheatButton = Instantiate(cheatsButtonPrefab, cheatsButtonList);
            cheatButton.SetCheatAction(cheatAction);

            // sanitize method name wwith regex to exclude unnecessary info
            string methodName = cheatAction.Method.Name;
            string pattern = @"<.*?>(g__)?(.*?)\|";
            Match match = Regex.Match(methodName, pattern);
            if (match.Success)
                methodName = match.Groups[2].Value;

            cheatButton.SetCheatName(methodName);
        }

        isCheatsPopulated = true;
    }
    #endregion

    #region Cheats
    // create all the Actions for the cheats in this method
    // it will be populate automatically on the panel
    private List<Action> CreateCheatsList()
    {
        List<Action> list = new List<Action>();

		// <----------   create functions for cheats bellow   ----------->

		// cheat that goes to main menu scene
		void GoToMainMenu()
		{
			HidePanels();
			GameManager.GoToMainMenu();
		}
		list.Add(GoToMainMenu);

		// cheat that goes to the next scene on build settings
		void GoToNextLevel()
        {
            HidePanels();
            GameManager.NextLevel();
        }
        list.Add(GoToNextLevel);

        // cheat that reloads current scene
        void RetryCurrentLevel()
        {
            HidePanels();
            GameManager.ReloadScene();
        }
        list.Add(RetryCurrentLevel);

        // <------------------------------------------------------------->

        return list;
    }
    #endregion
}
