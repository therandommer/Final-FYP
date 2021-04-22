using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity/Enemy/Hold", order = 1)]
public class HoldEnemyEntity : Enemy
{
    public float holdTimer = 5.0f; //timer before enemy leaves
    public int totalStops = 2; //points to move between before it stops
    public Vector2 xRange = new Vector2(-9.0f, 9.0f);
    public Vector2 yRange = new Vector2(-4.0f, 4.0f);
    //public List<Vector2> movePoints; //used for movement
}
