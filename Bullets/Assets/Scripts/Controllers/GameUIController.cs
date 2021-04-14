using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{
	public TextMeshProUGUI healthText;
	public TextMeshProUGUI armourBoostText;
	public TextMeshProUGUI weaponBoostText;
    void OnEnable()
	{
		Actions.OnPlayerHit += UpdateHealthText;
	}
    void OnDisable()
	{
		Actions.OnPlayerHit -= UpdateHealthText;
	}

    void UpdateHealthText(int _newHealth)
	{
		healthText.text = $"Health: {_newHealth} / 100";
	}
}
