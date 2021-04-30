using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty //acts as multiplier for now, could change enemy spawns/intensity, etc. 
{
    eEasy,
    eNormal,
    eHard
}
public enum Mods //added soon(tm)
{
    eHidden, //removes more lights from the scene
    eBigger, //makes player hitbox larger
    eDanger, //player has lower maxHP. Could have a slider of difficulty (Danger 1 = 75%hp, Danger 2 = 50%, Danger 3 = 25%, Danger 4 = 1hp)??
    eTougher, //player has more maxHP
}
public class ModController : MonoBehaviour
{

    void Awake()
	{
        DontDestroyOnLoad(this.gameObject);
	}
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
