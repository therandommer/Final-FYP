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
		basic, //fire in set direction, regardless of orientation
		towardsTarget, //towards either player or enemy depending on faction, will get position at time of firing
		curveTowardsTarget //same as towards but will home in with a curve
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
