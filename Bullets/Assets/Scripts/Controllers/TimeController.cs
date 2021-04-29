using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//controls overall timeline and spawning events, etc. 
public class TimeController : MonoBehaviour
{
    public bool isPaused = false;
    bool musicStarted = false;
    public float timePassed = 0.0f;
    public float maxTime = 0.0f;
    public float timeToNextSpawn = 0.0f;
    public float startDelay = 3.0f;
    public float endDelay = 5.0f;
    float previousTimeScale = 1.0f;
    public int totalSongSegments = 10;
    int songSegment = 1; 
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI nextSpawnTimerText;
    void OnEnable()
    {
        Actions.OnPause += TogglePause;
        Actions.OnSongChanged += UpdateSongLength;
        Actions.OnPlayerKilled += PlayerDead;
        Actions.OnLevelRestart += ResetTime;
    }
    void OnDisable()
    {
        Actions.OnPause -= TogglePause;
        Actions.OnSongChanged -= UpdateSongLength;
        Actions.OnPlayerKilled -= PlayerDead;
        Actions.OnLevelRestart -= ResetTime;
    }
    void Start()
    {
        timePassed = -startDelay;
    }
    void Update()
    {
        if(!isPaused)
		{
            if(timePassed<maxTime && timePassed >=0.0f)
			{
                timePassed += Time.deltaTime;
                timerText.text = $"Time: {(Mathf.FloorToInt(timePassed / 60).ToString())} : {(Mathf.FloorToInt(timePassed % 60).ToString())}/{(Mathf.FloorToInt(maxTime / 60).ToString())} : {(Mathf.FloorToInt(maxTime % 60).ToString())}";
                nextSpawnTimerText.text = $"Next Pack: {(Mathf.CeilToInt(timeToNextSpawn / 60).ToString())} : {(Mathf.CeilToInt(timeToNextSpawn % 60).ToString())}";
                if (timePassed >= 0.0f && !musicStarted)
                {
                    Actions.OnLevelStart?.Invoke();
                    musicStarted = true;
                }
            }
            if(timePassed<0) //overwrite previous
			{
                timePassed += Time.deltaTime;
                timerText.text = $"Get Ready: {(Mathf.FloorToInt(-timePassed % 60).ToString())}";
			}
            if(timePassed>=maxTime)
			{
                Actions.OnLevelComplete?.Invoke();
			}
            Debug.Log($"Requirement for bar update = {maxTime / totalSongSegments * songSegment}");
           if (timePassed>= maxTime / totalSongSegments * songSegment)
            {
                Actions.OnNewSongSegment?.Invoke(songSegment-1);
                songSegment++;
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
    void PlayerDead(Player thisPlayer)
	{
        Time.timeScale = 0;
    }
    void ResetTime()
	{
        timePassed = -startDelay;
        isPaused = false;
        musicStarted = false;
        songSegment = 1;
        Time.timeScale = 1;
	}
    public void UpdateSongLength(float _time)
	{
        maxTime = _time + endDelay;
	}
}
