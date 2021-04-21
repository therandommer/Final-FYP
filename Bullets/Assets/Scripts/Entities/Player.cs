using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity/Player", order = 1)]
public class Player : Entity
{
    public float boostMult = 1.5f;
    public List<GameObject> bullets = null;
    public int shieldLevel = 1;
    public int maxShieldLevel = 5;
    public int weaponLevel = 1;
    public float startFireRate = 1.0f;
    void Start()
    {
    }
}
