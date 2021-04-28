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
	public TextMeshProUGUI retryText;
	public float textHideTime = 3.0f;
	int retrys = 0;
    void OnEnable()
	{
		Actions.OnPlayerHit += UpdateHealthText;
		Actions.OnWeaponGot += UpdateWeaponText;
		Actions.OnShieldGot += UpdateShieldText;
		Actions.OnLevelComplete += DisplayCompleteUI;
		Actions.OnPlayerKilled += DisplayFailedUI;
		Actions.OnLevelRestart += UpdateRetryUI;
	}
    void OnDisable()
	{
		Actions.OnPlayerHit -= UpdateHealthText;
		Actions.OnWeaponGot -= UpdateWeaponText;
		Actions.OnShieldGot -= UpdateShieldText;
		Actions.OnLevelComplete -= DisplayCompleteUI;
		Actions.OnPlayerKilled -= DisplayFailedUI;
		Actions.OnLevelRestart -= UpdateRetryUI;
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
	void UpdateRetryUI()
	{
		retrys++;
		retryText.enabled = true;
		retryText.text = $"Attempts: {retrys}";
		StartCoroutine("HideText", retryText);
	}
	IEnumerator HideText(TextMeshProUGUI _thisText)
	{
		yield return new WaitForSeconds(textHideTime);
		_thisText.enabled = !_thisText.enabled;
	}
	void DisplayCompleteUI()
	{
		levelFinishedUI.SetActive(true);
		songNameText.text = Camera.main.GetComponent<AudioSource>().clip.name;
		finalScoreText.text = GetComponent<ScoreController>().scoreText.text;
		if(GameObject.Find("Player").activeInHierarchy)
		{
			GameObject.Find("Player").SetActive(false);
		}
		
	}
	void DisplayFailedUI(Player thisPlayer)
	{
		levelFinishedUI.SetActive(true);
		songNameText.text = Camera.main.GetComponent<AudioSource>().clip.name;
		finalScoreText.text = GetComponent<ScoreController>().scoreText.text;
		GameObject.Find("Player").SetActive(false);
	}
}
