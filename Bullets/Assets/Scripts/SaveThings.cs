using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
[System.Serializable]
public class SongInfo
{
	public string songName;
	public List<float> bpm;
	public List<float> intensity;
	public int seed;//used for randomisation, unique to each song imported
	public SongInfo(string _songName, List<float> _bpms, List<float> _intensitys, int _newSeed)
	{
		songName = _songName;
		bpm = _bpms;
		intensity = _intensitys;
		seed = _newSeed;
	}
}
[System.Serializable]
public class ScoreListing
{
	public string songName;
	int score;
	int attempts;
	public ScoreListing(string _songName, int _score, int _attempts)
	{
		songName = _songName;
		score = _score;
		attempts = _attempts;
	}
}
//save class to start saving a variety of things, currently saves basic song info
public class SaveThings : MonoBehaviour
{
	public AudioVisualised thisAudio;
	public GameController thisIntensity;
	public MusicController thisMusic;
	SongInfo thisInfo;
	void OnEnable()
	{
		Actions.OnLevelComplete += SaveSong;
		Actions.OnLevelStart += LoadSong;
		Actions.OnLoadNewSongData += LoadNewSong;
	}
	void OnDisable()
	{
		Actions.OnLevelComplete -= SaveSong;
		Actions.OnLevelStart -= LoadSong;
		Actions.OnLoadNewSongData -= LoadNewSong;
	}
	void Start()
	{
		thisMusic = FindObjectOfType<MusicController>();
		thisIntensity = FindObjectOfType<GameController>();
		thisAudio = FindObjectOfType<AudioVisualised>();
	}
	void SaveSong() //can adapt these functions to include scores, etc. 
	{
		if (!thisMusic)
		{
			thisMusic = FindObjectOfType<MusicController>();
		}
		if (!thisIntensity)
		{
			thisIntensity = FindObjectOfType<GameController>();
		}
		else
		{
			string destination = thisMusic.GetSongDirectory() + "SongData/" + thisMusic.GetSongName() + ".dat";
			FileStream file;
			if (!Directory.Exists(thisMusic.GetSongDirectory() + "SongData/"))
				Directory.CreateDirectory(thisMusic.GetSongDirectory() + "SongData/");
			if (File.Exists(destination))
				file = File.OpenWrite(destination);
			else
				file = File.Create(destination);
			Debug.Log($"Saving song {thisMusic.GetSongName()} to directory: {thisMusic.GetSongDirectory()}");
			Debug.Log($"Data saved: {thisMusic.GetSongName()}||{thisAudio.GetBPMAverages()}||{thisIntensity.GetIntensitySpeeds()}");
			List<float> tmpBpm = thisAudio.GetBPMAverages();
			List<float> tmpIntensity = thisIntensity.GetIntensitySpeeds();
			foreach (float i in tmpBpm)
			{
				Debug.Log("Bpm average: " + i);
			}
			foreach (float i in tmpIntensity)
			{
				Debug.Log("Bpm average: " + i);
			}
			//adds song info gathered from the first run to the file, along with a randomised seed for future use and to keep it consistent
			SongInfo info = new SongInfo(thisMusic.GetSongName(), thisAudio.GetBPMAverages(), thisIntensity.GetIntensitySpeeds(), Random.Range(0,int.MaxValue));
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(file, info);
			file.Close();
			Debug.Log("Saved Data");
		}
	}
	void LoadNewSong(string _songName)
	{
		Debug.Log("Loading song data");
		LoadSong();
	}
	void LoadSong()
	{
		if (!thisMusic)
		{
			thisMusic = FindObjectOfType<MusicController>();
		}
		string destination = thisMusic.GetSongDirectory() + "SongData/" + thisMusic.GetSongName() + ".dat";
		Debug.Log("Attempting to load data for song at: " + destination);
		FileStream file;

		if (File.Exists(destination))
			file = File.OpenRead(destination);
		else
		{
			SaveAndLoadDefault(); //will be caused on first runs of song, until song data is created
			return;
		}
		Debug.Log($"Loading song through file: {thisMusic.GetSongDirectory() + "SongData/" + thisMusic.GetSongName() + ".dat"}");
		BinaryFormatter bf = new BinaryFormatter();
		thisInfo = (SongInfo)bf.Deserialize(file);
		file.Close();
		Actions.OnLoadedSongInfo?.Invoke(thisInfo);
		Debug.Log($"Loaded song, {thisInfo.songName}");
	}
	void SaveAndLoadDefault()
	{
		if (!thisMusic)
		{
			thisMusic = FindObjectOfType<MusicController>();
		}
		if(!thisIntensity)
		{
			thisIntensity = FindObjectOfType<GameController>();
		}
		string destination = thisMusic.GetSongDirectory() + $"SongData/{thisMusic.GetSongName()} D.dat";
		FileStream file;
		if (!Directory.Exists(thisMusic.GetSongDirectory() + "SongData/"))
			Directory.CreateDirectory(thisMusic.GetSongDirectory() + "SongData/");
		if (File.Exists(destination))
			file = File.OpenWrite(destination);
		else
			file = File.Create(destination);
		List<float> defaultBpms = new List<float>();
		//provides some spoofed data for the first run so it feels dynamic, albeit random
		for(int i = 0; i < Mathf.RoundToInt((thisMusic.GetMusicLength()/120) * (thisAudio.GetSegments() * Mathf.CeilToInt(thisMusic.GetMusicLength() / 120))); ++i)
		{
			defaultBpms.Add(Random.Range(80.0f,220.0f));
			//Debug.Log("Generated bpm: " + defaultBpms[i]);
		}
		List<float> defaultIntesnity = new List<float>();
		for(int i = 0; i < Mathf.RoundToInt((thisMusic.GetMusicLength()/120) * (thisAudio.GetSegments() * Mathf.CeilToInt(thisMusic.GetMusicLength() / 120))); ++i)
		{
			defaultIntesnity.Add(Random.Range(0.35f,1.25f));
			//Debug.Log("Generated bpm: " + defaultIntesnity[i]);
		}
		SongInfo info = new SongInfo(thisMusic.GetSongName() + " ||D||", defaultBpms, defaultIntesnity, Random.Range(0, int.MaxValue));
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, info);
		file.Close();
		Debug.Log("Saved Default Data");

		Debug.Log($"Loading default through file: {destination}");

		if (File.Exists(destination))
			file = File.OpenRead(destination);
		else
		{
			Debug.LogError("File Not Found"); //will be caused on first run of song, until song data is created
			return;
		}
		thisInfo = (SongInfo)bf.Deserialize(file);
		file.Close();
		Actions.OnLoadedSongInfo?.Invoke(thisInfo);
	}
}
