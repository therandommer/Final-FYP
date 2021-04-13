using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity/Enemy/Hold", order = 1)]
public class HoldEnemyEntity : Enemy
{
    public float holdTimer = 5.0f; //timer before enemy leaves
    public List<Vector2> movePoints; //used for movement
}
