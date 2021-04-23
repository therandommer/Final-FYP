using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualised : MonoBehaviour
{
    float averageBeatsPerSecond = 0;
    List<float> beats = new List<float>();
    List<float> beatAverage = new List<float>();
    List<float> songAverageBpm = new List<float>();
    float averageBpm = 0.0f;
    public int segments = 20; //how many times to check for average bpm and display
    float refreshTime; //time before refresh
    TimeController thisTime;
    AudioProcessor processor;
    void OnEnable()
	{
        Actions.OnLevelComplete += FinishBpm;
        Actions.OnLevelRestart += ResetBpm;
        Actions.OnLevelStart += SetRefreshRate;
	}
    void OnDisable()
    {
        Actions.OnLevelComplete -= FinishBpm;
        Actions.OnLevelRestart -= ResetBpm;
        Actions.OnLevelStart -= SetRefreshRate;
    }
    void Start()
    {
        processor = FindObjectOfType<AudioProcessor>();
        thisTime = FindObjectOfType<TimeController>();
        processor.onBeat.AddListener(OnBeatDetected);
        processor.onSpectrum.AddListener(OnSpectrum);
    }
    void OnBeatDetected()
    {
        beats.Add(thisTime.timePassed);
        for (int i = 0; i < beats.Count; ++i)
        {
            if (beats[i] <= thisTime.timePassed - 1)
            {
                beats.RemoveAt(i);
            }
        }
        averageBeatsPerSecond = beats.Count;
        //Debug.Log($"BPM = {averageBeatsPerSecond * 30}");
        beatAverage.Add(averageBeatsPerSecond * 30);
    }
    void OnSpectrum(float[] spectrum)
	{

	}
    void AddToTotalAverage(float _newBpm)
	{
        averageBpm = 0;
        songAverageBpm.Add(_newBpm);
        for(int i = 0; i<songAverageBpm.Count; ++i)
		{
            averageBpm += songAverageBpm[i];
		}
        averageBpm /= songAverageBpm.Count;
        Debug.Log("Song average is " + averageBpm);
	}
    void Update()
    {
        //Debug.Log(thisTime.timePassed % refreshTime);
        if (thisTime.timePassed % refreshTime <= 0.2f && beatAverage.Count > 0)
        {
            CalculateBeatAverageUpdate();
        }
    }
    void FinishBpm()
	{
        if(beatAverage.Count > 0)
		{
            CalculateBeatAverageUpdate();
        }
	}
    void ResetBpm()
	{
        beats.Clear();
        beatAverage.Clear();
        songAverageBpm.Clear();
	}
    void SetRefreshRate()
	{
        refreshTime = thisTime.maxTime / segments;
	}
    void CalculateBeatAverageUpdate()
	{
        float totalBeats = 0.0f;
        for (int i = 0; i < beatAverage.Count; ++i)
        {
            totalBeats += beatAverage[i];
        }
        float averageBpm = totalBeats / beatAverage.Count;
        Debug.Log($"AverageBPM = {averageBpm}");
        Debug.Log("Size of beatAverage is" + beatAverage.Count);
        AddToTotalAverage(averageBpm);
        beatAverage.Clear();
    }
}
