using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffsUI : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private HealthUI healthUI;
	[SerializeField] private GameObject dmgDebuffObj;
	[SerializeField] private GameObject timeDebuffObj;
	[SerializeField] private GameObject missDebuffObj;

	private void Awake()
	{
		dmgDebuffObj.SetActive(false);
		timeDebuffObj.SetActive(false);
		missDebuffObj.SetActive(false);

		healthUI.Health.OnDmgHealthBreak += OnDmgDebuff;
		healthUI.Health.OnTimeHealthBreak += OnTimeDebuff;
		healthUI.Health.OnMissHealthBreak += OnMissDebuff;
	}

	private void OnDestroy()
	{
		healthUI.Health.OnDmgHealthBreak -= OnDmgDebuff;
		healthUI.Health.OnTimeHealthBreak -= OnTimeDebuff;
		healthUI.Health.OnMissHealthBreak -= OnMissDebuff;
	}

	private void OnDmgDebuff()
	{
		dmgDebuffObj.SetActive(true);
	}

	private void OnTimeDebuff()
	{
		timeDebuffObj.SetActive(true);
	}

	private void OnMissDebuff()
	{
		missDebuffObj.SetActive(true);
	}
}
