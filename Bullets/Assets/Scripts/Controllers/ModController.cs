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
    eNone
}
public class ModController : MonoBehaviour
{
    int seed;
    float modScoreMultiplier = 1.0f;
    float difficultyMultiplier = 1.0f;
    Difficulty difficulty = Difficulty.eNormal;
    Mods mods;
    void Start()
    {
        seed = Random.Range(0, int.MaxValue);
        modScoreMultiplier = 1.0f;
    }
    public int GetSeed()
	{
        return seed;
	}
    public Difficulty GetDifficulty()
	{
        return difficulty;
	}
    public Mods GetMods()
	{
        return mods;
	}
    public float GetModScoreMultiplier()
	{
        modScoreMultiplier = difficultyMultiplier;
        //insert other mods here
        switch (mods)
		{
            case Mods.eBigger:
                break;
            case Mods.eDanger:
                break;
            case Mods.eHidden:
                modScoreMultiplier *= 1.15f;
                break;
            case Mods.eTougher:
                break;
            default:
                break;
		}
        return modScoreMultiplier;
	}
    public void SetMod(int _newMod)
	{
        switch(_newMod)
		{
            case 1:
                mods = Mods.eHidden;
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
		}
	}
    public void RemoveMod(int _removeMod)
	{
        switch (_removeMod)
        {
            case 1:
                if (mods == Mods.eHidden)
                    mods = Mods.eNone;
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }
    public void SetDifficulty(int _newDifficulty)
	{
        switch(_newDifficulty)
		{
            case 1:
                difficulty = Difficulty.eEasy;
                difficultyMultiplier = 0.5f;
                break;
            case 2:
                difficulty = Difficulty.eNormal;
                difficultyMultiplier = 1.0f;
                break;
            case 3:
                difficulty = Difficulty.eHard;
                difficultyMultiplier = 1.5f;
                break;
            default:
                Debug.LogError("Invalid Difficulty");
                break;
		}
	}
}
