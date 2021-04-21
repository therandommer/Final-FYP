using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//controls overall timeline and spawning events, etc. 
public class TimeController : MonoBehaviour
{
    public bool isPaused = false;
    bool needsSongTimeUpdate = true;
    public float timePassed = 0.0f;
    public float maxTime = 0.0f;
    public float timeToNextSpawn = 0.0f;
    float previousTimeScale = 1.0f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI nextSpawnTimerText;
    public TextMeshProUGUI endOfSongTimeText;

    void OnEnable()
    {
        Actions.OnPause += TogglePause;
        Actions.OnSongChanged += UpdateSongLength;
    }
    void OnDisable()
    {
        Actions.OnPause -= TogglePause;
        Actions.OnSongChanged -= UpdateSongLength;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!isPaused)
		{
            timePassed += Time.deltaTime;
            timerText.text = $"Time: {(Mathf.FloorToInt(timePassed/60).ToString())} : {(Mathf.FloorToInt(timePassed % 60).ToString())}";
            nextSpawnTimerText.text = $"Next Pack: {(Mathf.CeilToInt(timeToNextSpawn / 60).ToString())} : {(Mathf.CeilToInt(timeToNextSpawn % 60).ToString())}";
            if(needsSongTimeUpdate)
			{
                endOfSongTimeText.text = $"Song time: {(Mathf.FloorToInt(maxTime / 60).ToString())} : {(Mathf.FloorToInt(maxTime % 60).ToString())}";
			}
        }
    }

    void TogglePause()
    {
        if(Time.timeScale > 0)
		{
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
		}
        else if(Time.timeScale == 0)
		{
            Time.timeScale = previousTimeScale;
		}
        isPaused = !isPaused;
        Debug.Log($"Toggling pause for {this.name}");
    }
    public void SendPauseAction()
	{
        Actions.OnPause?.Invoke();
    }
    public void UpdateSongLength(float _time)
	{
        maxTime = _time;
        needsSongTimeUpdate = true;
	}
}
