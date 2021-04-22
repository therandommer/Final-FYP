using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameObject spawnHolder;
    public AudioClip levelMusic;
    public GameObject audioSource;
    AudioSource thisSource;
    private void OnEnable()
    {
        Actions.OnPlayerKilled += playerDead;
        Actions.OnLevelStart += StartMusic;
    }
    private void OnDisable()
    {
        Actions.OnPlayerKilled -= playerDead;
        Actions.OnLevelStart -= StartMusic;
    }
    void Start()
    {
        thisSource = audioSource.GetComponent<AudioSource>();
        thisSource.clip = levelMusic;
        Actions.OnSongChanged?.Invoke(audioSource.GetComponent<AudioSource>().clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
		{
            Actions.OnPause?.Invoke();
            Debug.Log("Pausing");
		}
    }
    private void playerDead(Player playerRef)
	{

	}
    void StartMusic()
    {
        Debug.Log("Starting music");
        thisSource.Play();
    }
}
