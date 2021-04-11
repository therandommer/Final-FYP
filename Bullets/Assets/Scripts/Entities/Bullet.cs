using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity/Bullet", order = 1)]


public class Bullet : Entity
{
    public float lifetime = 3.0f;
	public int damage = 1;
    public enum BulletType
	{
		basic,
		wobble
	}
	public enum BulletFaction
	{
		player,
		enemy
	}
	public enum FireDirection
	{
		up,
		upRight,
		upLeft,
		down,
		downLeft,
		downRight,
		right,
		left
	}
	public BulletFaction thisFaction;
	public BulletType thisType;
	public FireDirection thisFireDirection;
}
