using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameObject spawnHolder;
    private AudioClip levelMusic;
    public GameObject audioSource;
    private void OnEnable()
    {
        Actions.OnPlayerKilled += playerDead;
    }
    private void OnDisable()
    {
        Actions.OnPlayerKilled -= playerDead;
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource.GetComponent<AudioSource>().clip = levelMusic;
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
}
