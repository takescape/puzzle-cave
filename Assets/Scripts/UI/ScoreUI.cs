using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

	private void Awake()
	{
		scoreText.text = "DAMAGE: 0";
	}

	private void LateUpdate()
    {
		int whiteDmg = TurnManager.GetCurrentDamage(HealthType.White);
		int redDmg = TurnManager.GetCurrentDamage(HealthType.Red);
		int blueDmg = TurnManager.GetCurrentDamage(HealthType.Blue);
		int purpleDmg = TurnManager.GetCurrentDamage(HealthType.Purple);

		string whiteStr = whiteDmg > 0 ? whiteDmg.ToString() : string.Empty;
		string redStr = redDmg > 0 ? $" <color=red>{(whiteDmg > 0 ? "+": string.Empty)}{redDmg}</color>" : string.Empty;
		bool anyDmgBeforeBlue = whiteDmg > 0 || redDmg > 0;
		string blueStr = blueDmg > 0 ? $" <color=blue>{(anyDmgBeforeBlue ? "+" : string.Empty)}{blueDmg}</color>" : string.Empty;
		bool anyDmgBeforePurple = whiteDmg > 0 || redDmg > 0 || blueDmg > 0;
		string purpleStr = purpleDmg > 0 ? $" <color=purple>{(anyDmgBeforePurple ? "+" : string.Empty)}{purpleDmg}</color>" : string.Empty;

		bool hasAnyDmg = whiteDmg > 0 || redDmg > 0 || blueDmg > 0 || purpleDmg > 0;
		scoreText.text = $"DAMAGE: {(hasAnyDmg ? $"{whiteStr}{redStr}{blueStr}{purpleStr}" : "0")}";
    }
}
