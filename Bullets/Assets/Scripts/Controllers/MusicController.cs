using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

//placed on camera to help with audio visualisation and music references
public class MusicController : MonoBehaviour
{
    string[] allSongsList; //directories
    List<string> allSongNames = new List<string>(); //visible names of songs
    
    [SerializeField]
    AudioClip levelMusic;
    [SerializeField]
    AudioSource thisSource;
    string songDirectory;
    public TextMeshProUGUI directoryText;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        songDirectory = Application.streamingAssetsPath + "/Music/";
        thisSource = GetComponent<AudioSource>();
        directoryText.text = songDirectory;
        if(Directory.Exists(songDirectory))
            allSongsList = Directory.GetFiles(songDirectory, "*.wav");
        int i = 0;
        foreach(string song in allSongsList)
		{
            string tmpString = song;
            Debug.Log($"Tmp before / cull {tmpString}");
            tmpString = tmpString.Substring(tmpString.LastIndexOf('/') + 1);
            Debug.Log($"Tmp after / cull {tmpString}");
            int location = tmpString.IndexOf(".", System.StringComparison.Ordinal);
            if (location > 0)
                tmpString = tmpString.Substring(0, location);
            allSongNames.Add(tmpString);
		}
        //StartCoroutine(LoadSong(Random.Range(0, allSongs.Length - 1))); // loads a random song to play in the main menu from the directory provided
        StartCoroutine(LoadSong(0));
    }
    void OnEnable()
	{
        Actions.OnSceneChanged += SetSong;
	}
    void OnDisable()
    {
        Actions.OnSceneChanged -= SetSong;
    }
    void Start()
    {
        if (!thisSource)
        {
            thisSource = GetComponent<AudioSource>();
        }
    }
    IEnumerator LoadSong(int _index)
	{
        WWW request = GetAudioFromFile(_index);
        yield return request; //waits for the WWW request to complete
        levelMusic = request.GetAudioClip(); //turning the request into an audio clip
        levelMusic.name = allSongNames[_index];
        levelMusic.LoadAudioData();
        PlayAudioFile();
	}
    private WWW GetAudioFromFile(int _index) //downloading the data from the file provided
	{
        string audioToLoad = string.Format(allSongsList[_index]);
        WWW request = new WWW(audioToLoad);
        return request;
	}
    private void PlayAudioFile()
	{
        thisSource.clip = levelMusic;
        thisSource.Play();
	}
    public string GetSongName()
    {
        return levelMusic.name;
    }
    public AudioClip GetSongClip()
    {
        return levelMusic;
    }
    public void SetSong(AudioClip _levelSong)
    {
        levelMusic = _levelSong;
    }
    public AudioSource GetSource()
    {
        return thisSource;
    }
    public AudioClip GetMusic()
    {
        return levelMusic;
    }
    public void SetMusicList(string[] _newList)
	{
        allSongsList = _newList;
	}
    public void SetSongDirectory(string _newDir) //called on new directory being assigned will recreate the song list too
    {
        songDirectory = _newDir;
        SetMusicList(Directory.GetFiles(songDirectory, ".mp3"));
    }
    public string GetSongDirectory()
    {
        return songDirectory;
    }
}
