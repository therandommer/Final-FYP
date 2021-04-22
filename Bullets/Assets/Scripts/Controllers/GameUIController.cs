using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{
	public TextMeshProUGUI healthText;
	public TextMeshProUGUI shieldBoostText;
	public TextMeshProUGUI weaponBoostText;
	public GameObject levelFinishedUI;
	public TextMeshProUGUI songNameText;
	public TextMeshProUGUI finalScoreText;
	public GameObject levelFailedUI;
	public TextMeshProUGUI timeRemainingText;
    void OnEnable()
	{
		Actions.OnPlayerHit += UpdateHealthText;
		Actions.OnWeaponGot += UpdateWeaponText;
		Actions.OnShieldGot += UpdateShieldText;
		Actions.OnLevelComplete += DisplayCompleteUI;
		Actions.OnPlayerKilled += DisplayFailedUI;
	}
    void OnDisable()
	{
		Actions.OnPlayerHit -= UpdateHealthText;
		Actions.OnWeaponGot -= UpdateWeaponText;
		Actions.OnShieldGot -= UpdateShieldText;
		Actions.OnLevelComplete -= DisplayCompleteUI;
		Actions.OnPlayerKilled -= DisplayFailedUI;
	}
	
    void UpdateHealthText(int _newHealth)
	{
		healthText.text = $"Health: {_newHealth} / 100";
	}
	void UpdateWeaponText(int _newWeapon)
	{
		weaponBoostText.text = $"Weapon: {_newWeapon}";
	}
	void UpdateShieldText(int _newShield)
	{
		shieldBoostText.text = $"Shield: {_newShield}";
	}
	void DisplayCompleteUI()
	{
		levelFinishedUI.SetActive(true);
		songNameText.text = Camera.main.GetComponent<AudioSource>().clip.name;
		finalScoreText.text = GetComponent<ScoreController>().scoreText.text;
		GameObject.Find("Player").SetActive(false);
	}
	void DisplayFailedUI(Player thisPlayer)
	{
		levelFinishedUI.SetActive(true);
		songNameText.text = Camera.main.GetComponent<AudioSource>().clip.name;
		finalScoreText.text = GetComponent<ScoreController>().scoreText.text;
		GameObject.Find("Player").SetActive(false);
	}
}
