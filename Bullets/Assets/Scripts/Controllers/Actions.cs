using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//used to call various actions across controllers, etc. reduces number of references required between scripts
public static class Actions
{
    public static Action<Player> OnPlayerKilled;
    public static Action<int> OnPlayerHit; //updates player health on the UI
    public static Action<Enemy> OnEnemyKilled;
    public static Action<Bullet> OnBulletHit; //bullet score for hitting enemies
    public static Action OnPause;
    public static Action<float> OnSongChanged;
    public static Action<TimeController> OnTimeReached;
    public static Action<Drop> OnCollectableAcquired;
    public static Action<int> OnWeaponGot;
    public static Action<int> OnShieldGot;
}
