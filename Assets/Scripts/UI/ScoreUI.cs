using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

	private void Awake()
	{
		scoreText.text = "Damage: 0";
	}

	private void LateUpdate()
    {
        scoreText.text = $"Damage: {GameManager.TurnScore}";
    }
}
