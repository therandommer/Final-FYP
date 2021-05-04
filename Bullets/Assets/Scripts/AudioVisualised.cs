using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioVisualised : MonoBehaviour
{
    float averageBeatsPerSecond = 0;
    List<float> beats = new List<float>();
    List<float> beatAverage = new List<float>();
    List<float> songAverageBpm = new List<float>();
    float averageBpm = 0.0f;
    public int normalWeighting = 0; //number of average bpms filled with the default bpm for the song
    public int segments = 20; //how many times to check for average bpm and display
    float refreshTime; //time before refresh

    public TextMeshProUGUI songName;
    public List<RectTransform> barTransforms = new List<RectTransform>();
    public List<RectTransform> staticBarTransforms = new List<RectTransform>();
    public Color staticDefaultColour = Color.red;
    //apply to both bars
    float defaultWidth = 35.0f;
    public float maxBarHeight = 100.0f;
    //bar = updated at runtime, static is loaded from file. Might need to move static functionality elsewhere as its mostly a UI thing. Fine here for now
    public float barScalar = 1000.0f; //amount to be multiplied by
    public float staticBarScalar = 3.0f; //amount to be divided by
    bool isStopped = true; // set to false during level gameplay, set to true during main mneu and end of level
    TimeController thisTime;
    AudioProcessor processor;
    GameController gC;
    void OnEnable()
    {
        Actions.OnLevelComplete += FinishBpm;
        Actions.OnLevelRestart += ResetBpm;
        Actions.OnLevelStart += SetRefreshRate;
        Actions.ResetBars += ResetBars; //only for the dynamic bars
        Actions.OnLoadedSongInfo += CalculateStaticBars;
        Actions.OnSceneChanged += InitialiseScene;
        Actions.OnSceneRequest += ResetBarColour;
    }
    void OnDisable()
    {
        Actions.OnLevelComplete -= FinishBpm;
        Actions.OnLevelRestart -= ResetBpm;
        Actions.OnLevelStart -= SetRefreshRate;
        Actions.ResetBars -= ResetBars;
        Actions.OnLoadedSongInfo -= CalculateStaticBars;
        Actions.OnSceneChanged -= InitialiseScene;
    }
    void Start()
    {
        processor = FindObjectOfType<AudioProcessor>();
        processor.onBeat.AddListener(OnBeatDetected);
        processor.onSpectrum.AddListener(OnSpectrum);
        Invoke("InitialiseBase", 2.0f);
    }
    void InitialiseScene(int _songIndex)
    {
        if (!thisTime && _songIndex == 0)
        {
            thisTime = FindObjectOfType<TimeController>();
        }
    }
    void OnBeatDetected()
    {
        if(thisTime != null)
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
            beatAverage.Add(averageBeatsPerSecond * 20);
        }
    }
    void OnSpectrum(float[] spectrum)
    {
        for (int i = 0; i < spectrum.Length; ++i)
        {
            if (spectrum[i] * (barScalar / GetComponent<AudioSource>().volume) > maxBarHeight)
            {
                barTransforms[i].sizeDelta = new Vector2(defaultWidth, maxBarHeight);
            }
            else
            {
                barTransforms[i].sizeDelta = new Vector2(defaultWidth, spectrum[i] * (barScalar / GetComponent<AudioSource>().volume));
            }
            Vector3 start = new Vector3(i, 0, 0);
            Vector3 end = new Vector3(i, spectrum[i], 0);
            Debug.DrawLine(start, end);
        }
    }
    void AddToTotalAverage(float _newBpm)
    {
        averageBpm = 0;
        songAverageBpm.Add(_newBpm);
        for (int i = 0; i < songAverageBpm.Count; ++i)
        {
            averageBpm += songAverageBpm[i];
        }
        averageBpm /= songAverageBpm.Count;
        //Debug.Log("Song average is " + averageBpm);
    }
    void Update()
    {
        //Debug.Log(thisTime.timePassed % refreshTime);
        if (thisTime)
        {
            if (thisTime.timePassed % refreshTime <= 0.2f && beatAverage.Count > 0 && thisTime.timePassed > 0 && !isStopped)
            {
                CalculateBeatAverageUpdate();
            }
        }
        else if (!thisTime && !isStopped)
        {
            thisTime = FindObjectOfType<TimeController>();
            gC = FindObjectOfType<GameController>();
        }
    }
    void ResetBarColour(int _scene)
	{
        if (_scene == 0)
        {
            for (int i = 0; i < staticBarTransforms.Count; ++i)
            { 
                staticBarTransforms[i].gameObject.GetComponent<Image>().color = staticDefaultColour;
            }
        }
    }
    void FinishBpm()
    {
        if (beatAverage.Count > 0)
        {
            CalculateBeatAverageUpdate();
            isStopped = true;
            ResetBpm();
        }
    }
    void ResetBpm()
    {
        beats.Clear();
        beatAverage.Clear();
        songAverageBpm.Clear();
        Actions.ResetBars?.Invoke(defaultWidth);
        InitialiseBase();
    }
    void ResetBars(float _defaultWidth) //resetting the dynamic bars to 0 for when the song initialises
    {

    }
    void InitialiseBase()
    {
        if (gC)
        {
            for (int i = 0; i < normalWeighting; ++i)
            {
                beatAverage.Add(gC.baseBpm);
            }
        }
    }
    void SetRefreshRate()
    {
        if (!thisTime)
            thisTime = FindObjectOfType<TimeController>();
        refreshTime = thisTime.maxTime / (segments * Mathf.CeilToInt((thisTime.maxTime / 120))); //every 2 minutes more segements are required for the song
        //Debug.Log("Refresh time now has:" + refreshTime);
        isStopped = false;
    }
    void CalculateBeatAverageUpdate()
    {
        float totalBeats = 0.0f;
        for (int i = 0; i < beatAverage.Count; ++i)
        {
            totalBeats += beatAverage[i];
        }
        float averageBpm = totalBeats / beatAverage.Count;
        AddToTotalAverage(averageBpm);
        Actions.OnNewBPMAverage?.Invoke(averageBpm);
        //Debug.Log("New bpm and the index is: " + songAverageBpm.Count);
        Actions.OnNewBPMSpeed?.Invoke(songAverageBpm.Count); //sends the current index for the song progression to other objects to update their movement speed. 
        beatAverage.Clear();
    }
    public List<float> GetBPMAverages()
    {
        return songAverageBpm;
    }
    void CalculateStaticBars(SongInfo _thisSong)
    {
        songName.text = _thisSong.songName;
        Debug.Log("Received Song Info for Song: " + _thisSong.songName);
        float elementsPerLoop = Mathf.CeilToInt(_thisSong.bpm.Count / staticBarTransforms.Count);
        //Debug.Log("Elements per loop: " + elementsPerLoop);

        for (int i = 0; i < staticBarTransforms.Count; ++i)
        {
            float _tmpBpmAverage = 0.0f;
            int processed = 0;
            int iOffset = Mathf.CeilToInt(i * elementsPerLoop);
            for (int j = 0; j < elementsPerLoop; ++j)
            {
                if (j + iOffset <= _thisSong.bpm.Count)
                {
                    //Debug.Log("Elements/loop = " + elementsPerLoop);
                    processed = j;
                    _tmpBpmAverage += _thisSong.bpm[j + iOffset];
                }
                else if (j + iOffset > _thisSong.bpm.Count)
                {
                    Debug.Log("Finished processing bpm. J = " + j + "/" + i + "/" + _thisSong.bpm.Count);
                    return;
                }
            }
            if (_tmpBpmAverage / processed / staticBarScalar > maxBarHeight)
            {
                staticBarTransforms[i].sizeDelta = new Vector2(defaultWidth, maxBarHeight);
            }
            else
            {
                staticBarTransforms[i].sizeDelta = new Vector2(defaultWidth, _tmpBpmAverage / processed / staticBarScalar);
            }
        }
    }
    public void SetIsStopped(bool _state)
    {
        isStopped = _state;
    }

    public int GetSegments()
    {
        return segments;
    }
}
