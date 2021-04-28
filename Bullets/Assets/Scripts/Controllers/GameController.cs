using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameObject spawnHolder;
    public AudioClip levelMusic;
    public GameObject audioSource;
    public int baseBpm = 190; //used to compare for speed/intensity calcs
    float speedScalar = 1.0f; //the more the average is different to the base, the higher. If its lower than average its lower
    List<float> intensitySpeeds = new List<float>();
    AudioSource thisSource;
    bool canPause = false;
    private void OnEnable()
    {
        Actions.OnPlayerKilled += PlayerDead;
        Actions.OnLevelStart += StartMusic;
        Actions.OnLevelRestart += StopMusic;
        Actions.OnLevelComplete += DisablePause;
        Actions.OnPause += PauseMusic;
        Actions.OnNewBPMAverage += CalculateIntensity;
    }
    private void OnDisable()
    {
        Actions.OnPlayerKilled -= PlayerDead;
        Actions.OnLevelStart -= StartMusic;
        Actions.OnLevelRestart -= StopMusic;
        Actions.OnLevelComplete -= DisablePause;
        Actions.OnPause -= PauseMusic;
        Actions.OnNewBPMAverage -= CalculateIntensity;
    }
    void Start()
    {
        thisSource = audioSource.GetComponent<AudioSource>();
        thisSource.clip = levelMusic;
        Actions.OnSongChanged?.Invoke(audioSource.GetComponent<AudioSource>().clip.length);
    }

    void Update()
    {
        if(Input.GetButtonDown("Pause") && canPause)
		{
            Actions.OnPause?.Invoke();
            Debug.Log("Pausing");
		}
    }
    void CalculateIntensity(float _thisBeat)
	{
        Debug.Log("Received bpm of: " + _thisBeat);
        speedScalar = _thisBeat / baseBpm;
        intensitySpeeds.Add(speedScalar);
        Debug.Log($"Intensity of the song is {speedScalar}");
	}
    private void PlayerDead(Player playerRef)
	{
        thisSource.Stop();
        canPause = false;
	}
    void StartMusic() //initialises the music once the countdown hits 0
    {
        canPause = true;
        Debug.Log("Starting music");
        thisSource.Play();
    }
    void PauseMusic()
	{
        if (thisSource.isPlaying)
            thisSource.Pause();
        else
            thisSource.UnPause();
	}
    void StopMusic() //restarts music to default level music
	{
        canPause = false;
        thisSource.Stop();
        thisSource.clip = levelMusic;
        Actions.OnSongChanged?.Invoke(audioSource.GetComponent<AudioSource>().clip.length);
    }
    void DisablePause()
	{
        thisSource.Stop();
        canPause = false;
	}
    public void SendRestartAction() //used for UI button press
	{
        Actions.OnLevelRestart?.Invoke();
        Debug.Log("Level restarting");
	}
    public List<float> GetIntensitySpeeds()
	{
        return intensitySpeeds;
	}
    public string GetSongName()
	{
        return levelMusic.name;
	}
}
