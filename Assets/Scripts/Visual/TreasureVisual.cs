using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureVisual : MonoBehaviour
{
	[SerializeField] private int levelQuantity = 8;
	[SerializeField] private GameObject treasureChest;

	private void Awake()
	{
		treasureChest.SetActive(false);
	}

	private void Start()
	{
		if (GameManager.CurrentLevel >= levelQuantity)
			treasureChest.SetActive(true);
	}
}
