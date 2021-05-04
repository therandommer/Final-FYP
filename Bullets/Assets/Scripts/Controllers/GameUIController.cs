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
	public TextMeshProUGUI speedText;
	float speedNumber;
	public GameObject levelFailedUI;
	public TextMeshProUGUI failedSongNameText;
	public TextMeshProUGUI failedFinalScoreText;
	public TextMeshProUGUI timeRemainingText;
	public TextMeshProUGUI retryText;
	public float textHideTime = 3.0f;
	//will update each bar each x% of the way through the song
	public int maxSegmentBars = 10;
	public List<GameObject> progressBars;
	public Color defaultColour;
	public Color completeColour;
	bool isFinished = false;
	int retrys = 0;
    void OnEnable()
	{
		Actions.OnPlayerHit += UpdateHealthText;
		Actions.UpdatePlayerHealth += UpdateHealthText2;
		Actions.OnWeaponGot += UpdateWeaponText;
		Actions.OnShieldGot += UpdateShieldText;
		Actions.OnLevelComplete += DisplayCompleteUI;
		Actions.OnPlayerKilled += DisplayFailedUI;
		Actions.OnLevelRestart += UpdateRestartUI;
		Actions.ResetBars += ResetBars;
		Actions.OnLevelStart += SetInitialUI;
		Actions.OnNewSongSegment += UpdateProgressUI;
		Actions.OnNewBPMSpeed += UpdateSpeedUI;
	}
	void OnDisable()
	{
		Actions.OnPlayerHit -= UpdateHealthText;
		Actions.UpdatePlayerHealth -= UpdateHealthText2;
		Actions.OnWeaponGot -= UpdateWeaponText;
		Actions.OnShieldGot -= UpdateShieldText;
		Actions.OnLevelComplete -= DisplayCompleteUI;
		Actions.OnPlayerKilled -= DisplayFailedUI;
		Actions.OnLevelRestart -= UpdateRestartUI;
		Actions.ResetBars -= ResetBars;
		Actions.OnLevelStart -= SetInitialUI;
		Actions.OnNewSongSegment -= UpdateProgressUI;
		Actions.OnNewBPMSpeed -= UpdateSpeedUI;
	}
	void Start()
	{
		for(int i = 0; i < maxSegmentBars; ++i)
		{
			progressBars.Add(GameObject.Find("Bar " + i));
		}
	}
    void UpdateHealthText(int _newHealth)
	{
		healthText.text = $"Health: {_newHealth}";
	}
	void UpdateHealthText2(int _newHealth)
	{
		UpdateHealthText(_newHealth);
	}
	void UpdateWeaponText(int _newWeapon)
	{
		weaponBoostText.text = $"Weapon: {_newWeapon}";
	}
	void UpdateShieldText(int _newShield)
	{
		shieldBoostText.text = $"Shield: {_newShield}";
	}
	void SetInitialUI()
	{
		isFinished = false;
		Debug.Log("Ready to initialise UI");
		//progressBars[0].GetComponent<Image>().color = completeColour;
	}
	void UpdateProgressUI(int _newSegment)
	{
		//Debug.Log("Updating " + _newSegment + " progress");
		progressBars[_newSegment].GetComponent<Image>().color = completeColour;
	}
	void UpdateRestartUI()
	{
		retrys++;
		retryText.enabled = true;
		retryText.text = $"Attempts: {retrys}";
		StartCoroutine("HideText", retryText);
	}
	void UpdateSpeedUI(int _index)
	{
		if(!isFinished)
		{
			speedNumber = FindObjectOfType<GameController>().GetExistingIntensity(_index - 1);
			speedText.text = $"Speed: {speedNumber}";
		}
	}
	public float GetSpeedNumber()
	{
		return speedNumber;
	}
	void ResetBars(float _width)
	{
		for(int i = 0; i < progressBars.Count; ++i)
		{
			progressBars[i].GetComponent<Image>().color = defaultColour;
			progressBars[i].GetComponent<RectTransform>().sizeDelta = new Vector2(_width, 0);
		}
	}
	IEnumerator HideText(TextMeshProUGUI _thisText)
	{
		yield return new WaitForSeconds(textHideTime);
		_thisText.enabled = !_thisText.enabled;
	}
	void DisplayCompleteUI()
	{
		isFinished = true;
		levelFinishedUI.SetActive(true);
		songNameText.text = GameObject.FindObjectOfType<MusicController>().GetSongName();
		finalScoreText.text = GetComponent<ScoreController>().scoreText.text;
	}
	void DisplayFailedUI(GameObject _player)
	{
		isFinished = true;
		levelFailedUI.SetActive(true);
		failedSongNameText.text = GameObject.FindObjectOfType<MusicController>().GetSongName();
		failedFinalScoreText.text = GetComponent<ScoreController>().scoreText.text;
	}
}
