using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireType
{
	single,
	burst,
	auto
}

public abstract class Entity : ScriptableObject
{
	public SpriteRenderer sprite;
	int health;
	protected void OnEnable()
	{
		Actions.OnPause += Pause;
	}
	protected void OnDisable()
	{
		Actions.OnPause -= Pause;
	}
	protected virtual void Pause()
	{

	}
	protected virtual void SpawnThis(Vector2 spawnPos)
	{
		Instantiate(this, spawnPos, Quaternion.identity, GameController.spawnHolder.transform);
	}
	protected virtual void SpawnBullet(GameObject bullet, GameObject spawnObject)
	{
		Instantiate(bullet, spawnObject.transform.position, spawnObject.transform.rotation, spawnObject.transform);
	}
    protected virtual void Die(Player playerRef)
	{

	}
	protected virtual void Die(Enemy enemyRef)
	{

	}
}
