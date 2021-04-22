using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//acts as stats script for all entities, players, enemies, bullets, etc. inherit this
public abstract class Entity : ScriptableObject
{
	public Sprite thisSprite;
	public int health;
	public float moveSpeed;
	public bool isPaused = false; //used to stop update loops, etc.

	protected void OnEnable() //adds listener to action
	{
		Actions.OnPause += Pause;
	}
	protected void OnDisable() //removes listener from action
	{
		Actions.OnPause -= Pause;
	}
	protected virtual void Pause()
	{
		isPaused = !isPaused;
	}
}
