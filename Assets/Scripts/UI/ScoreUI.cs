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
		int whiteDmg = GameManager.GetCurrentDamage(HealthType.White);
		int redDmg = GameManager.GetCurrentDamage(HealthType.Red);
		int blueDmg = GameManager.GetCurrentDamage(HealthType.Blue);
		int purpleDmg = GameManager.GetCurrentDamage(HealthType.Purple);

		string whiteStr = whiteDmg > 0 ? whiteDmg.ToString() : string.Empty;
		string redStr = redDmg > 0 ? $" <color=red>x{redDmg}</color>" : string.Empty;
		string blueStr = blueDmg > 0 ? $" <color=blue>x{blueDmg}</color>" : string.Empty;
		string purpleStr = purpleDmg > 0 ? $" <color=purple>x{purpleDmg}</color>" : string.Empty;

		scoreText.text = $"DAMAGE: {whiteStr}{redStr}{blueStr}{purpleStr}";
    }
}
