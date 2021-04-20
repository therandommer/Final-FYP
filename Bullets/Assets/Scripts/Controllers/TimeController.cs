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
    public float timeScale = 1.0f;
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
            timePassed += Time.deltaTime * timeScale;
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
