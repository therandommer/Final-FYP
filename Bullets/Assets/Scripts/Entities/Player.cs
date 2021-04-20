using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity/Player", order = 1)]
public class Player : Entity
{
    public enum FireType
    {
        single,
        burst,
        auto
    }
    public FireType fireType = FireType.single;
    public float boostMult = 1.5f;
    public List<GameObject> bullets = null;
    public int shieldLevel = 1;
    public int weaponLevel = 1;
    
    // Start is called before the first frame update
    void Start()
    {
    }
}
