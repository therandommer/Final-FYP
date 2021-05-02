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
	public SongInfo(string _songName, List<float> _bpms, List<float> _intensitys)
	{
		songName = _songName;
		bpm = _bpms;
		intensity = _intensitys;
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
			//destination + " "
			//string destination = Application.persistentDataPath + "/SongData/" + thisIntensity.GetSongName() + ".dat";
			FileStream file;
			if (!Directory.Exists(thisMusic.GetSongDirectory() + "SongData/"))
				Directory.CreateDirectory(thisMusic.GetSongDirectory() + "SongData/");
			if (File.Exists(destination))
				file = File.OpenWrite(destination);
			else
				file = File.Create(destination);
			Debug.Log($"Saving song {thisMusic.GetSongName()} to directory: {thisMusic.GetSongDirectory()}");
			Debug.Log($"Data saved: {thisMusic.GetSongName()}||{thisAudio.GetBPMAverages()}||{thisIntensity.GetIntensitySpeeds()}");
			SongInfo info = new SongInfo(thisMusic.GetSongName(), thisAudio.GetBPMAverages(), thisIntensity.GetIntensitySpeeds());
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
		//string destination = Application.persistentDataPath + "/songData/" + thisIntensity.GetSongName() + ".dat";
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
		/*for(int i = 0; i < thisInfo.bpm.Count; ++i)
		{
			Debug.Log("Bpms for song: " + thisInfo.songName + " = " + thisInfo.bpm[i]);
		}*/

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
		for(int i = 0; i < 20; ++i)
		{
			defaultBpms.Add(Random.Range(140.0f,240.0f));
		}
		List<float> defaultIntesnity = new List<float>();
		for(int i = 0; i< 10; ++i)
		{
			defaultIntesnity.Add(Random.Range(0.0f,1.0f));
		}
		SongInfo info = new SongInfo(thisMusic.GetSongName() + " D", defaultBpms, defaultIntesnity);
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
