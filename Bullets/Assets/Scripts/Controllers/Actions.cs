using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//used to call various actions across controllers, etc. reduces number of references required between scripts
public static class Actions
{
    public static Action<GameObject> OnPlayerKilled;
    public static Action<int> OnPlayerHit; //updates player health on the UI
    public static Action<int> UpdatePlayerHealth; //update player health without triggering damage functions
    public static Action<Enemy> OnEnemyKilled;
    public static Action<Bullet> OnBulletHit; //bullet score for hitting enemies
    public static Action OnPause;
    public static Action OnLevelStart;
    public static Action OnLevelComplete;
    public static Action OnLevelRestart;
    public static Action<float> ResetBars;
    public static Action<float> OnSongChanged;
    public static Action<string> OnLoadNewSongData;
    public static Action<float> OnNewBPMAverage;
    public static Action<int> OnNewBPMSpeed; //needs to be called on spawners, enemies, players and bullets. 
    public static Action<int> OnSceneChanged;
    public static Action<int> OnSceneRequest;
    public static Action<SongInfo> OnLoadedSongInfo;
    public static Action<int> OnNewSongSegment;
    public static Action<TimeController> OnTimeReached;
    public static Action<Drop> OnCollectableAcquired;
    public static Action<int> OnWeaponGot;
    public static Action<int> OnShieldGot;
}
