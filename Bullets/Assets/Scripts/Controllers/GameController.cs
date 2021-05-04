using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameObject spawnHolder;
    public AudioClip levelMusic;
    public AudioSource thisSource;
    public GameObject audioSource;
    public int baseBpm = 120; //used to compare for speed/intensity calcs
    float speedScalar = 1.0f; //the more the average is different to the base, the higher. If its lower than average its lower
    List<float> intensitySpeeds = new List<float>();
    
    bool canPause = false;

    List<float> existingIntensitySpeeds = new List<float>();
    private void OnEnable()
    {
        Actions.OnPlayerKilled += PlayerDead;
        Actions.OnLevelStart += StartMusic;
        Actions.OnLevelRestart += StopMusic;
        Actions.OnLevelComplete += DisablePause;
        Actions.OnPause += PauseMusic;
        Actions.OnPlayerKilled += StopMusic2;
        Actions.OnNewBPMAverage += CalculateIntensity;
        Actions.OnLoadedSongInfo += LoadIntensity;
    }
    private void OnDisable()
    {
        Actions.OnPlayerKilled -= PlayerDead;
        Actions.OnLevelStart -= StartMusic;
        Actions.OnLevelRestart -= StopMusic;
        Actions.OnLevelComplete -= DisablePause;
        Actions.OnPause -= PauseMusic;
        Actions.OnPlayerKilled -= StopMusic2;
        Actions.OnNewBPMAverage -= CalculateIntensity;
        Actions.OnLoadedSongInfo -= LoadIntensity;
    }
    void Start() //initialises music for the level using the data provided from MusicController
    {
        audioSource = FindObjectOfType<MusicController>().gameObject;
        levelMusic = audioSource.GetComponent<MusicController>().GetSongClip();
        if(levelMusic)
		{
            thisSource = audioSource.GetComponent<MusicController>().GetSource();
            thisSource.clip = levelMusic;
        }
        
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
        //Debug.Log("Received bpm of: " + _thisBeat);
        speedScalar = _thisBeat / baseBpm;
        intensitySpeeds.Add(speedScalar);
        //Debug.Log($"Intensity of the song is {speedScalar}");
	}
    void LoadIntensity(SongInfo _thisSong)
	{
        existingIntensitySpeeds = _thisSong.intensity;
        existingIntensitySpeeds.Add(existingIntensitySpeeds[existingIntensitySpeeds.Count-1]); //adds an extra of the final intensity
	}
    public float GetExistingIntensity(int _index)
	{
        //Debug.Log("Requesting intensity: " + _index);
        if (_index < existingIntensitySpeeds.Count)
        {
            return existingIntensitySpeeds[_index];
        }
        else
            return existingIntensitySpeeds.LastOrDefault();
	}
    private void PlayerDead(GameObject playerRef)
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
    void StopMusic2(GameObject playerRef)
	{
        StopMusic();
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
    public int GetIntensityCount()
	{
        return intensitySpeeds.Count;
	}
    public void SendSceneRequest(int _scene)
	{
        Actions.OnSceneRequest?.Invoke(_scene);
	}
}
