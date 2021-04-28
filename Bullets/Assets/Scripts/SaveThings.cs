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
//save class to start saving a variety of things, currently saves basic song info
public class SaveThings : MonoBehaviour
{
	public AudioVisualised thisAudio;
	public GameController thisIntensity;
	SongInfo thisInfo; 
    void OnEnable()
	{
		Actions.OnLevelComplete += SaveSong;
		Actions.OnLevelStart += LoadSong; //might need to change this
	}
	void OnDisable()
	{
		Actions.OnLevelComplete -= SaveSong;
		Actions.OnLevelStart -= LoadSong;
	}
	void SaveSong() //can adapt these functions to include scores, etc. 
	{
		string destination = Application.dataPath + "/SongData/" + thisIntensity.GetSongName() + ".dat";
		FileStream file;

		if (File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		SongInfo info = new SongInfo(thisIntensity.GetSongName(), thisAudio.GetBPMAverages(), thisIntensity.GetIntensitySpeeds());
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, info);
		file.Close();
		Debug.Log("Saved Data");
	}
	void LoadSong()
	{
		string destination = Application.dataPath + "/songData/" + thisIntensity.GetSongName() + ".dat";
		Debug.Log($"Loading song through file: {Application.dataPath + "/songData/" + thisIntensity.GetSongName() + ".dat"}");
		FileStream file;

		if (File.Exists(destination)) file = File.OpenRead(destination);
		else
		{
			Debug.LogError("File Not Found"); //will be caused on first run of song, until song data is created
			return;
		}

		BinaryFormatter bf = new BinaryFormatter();
		thisInfo = (SongInfo) bf.Deserialize(file);
		file.Close();
		/*for(int i = 0; i < thisInfo.bpm.Count; ++i)
		{
			Debug.Log("Bpms for song: " + thisInfo.songName + " = " + thisInfo.bpm[i]);
		}*/
		
		Actions.OnLoadedSongInfo?.Invoke(thisInfo);
		Debug.Log($"Loaded song, {thisInfo.songName}");
	}
}
