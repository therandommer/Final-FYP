using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChildren : MonoBehaviour
{
    void OnEnable()
    {
        Actions.OnLevelComplete += DestroyChild;
        Actions.OnLevelRestart += DestroyChild2;
    }
    void OnDisable()
    {
        Actions.OnLevelComplete -= DestroyChild;
        Actions.OnLevelRestart -= DestroyChild2;
    }
    void DestroyChild()
	{
        foreach(Transform child in this.transform)
		{
            GameObject.Destroy(child.gameObject);
		}
	}
    void DestroyChild2() //called to do same as above, just called at potentially different times
	{
        DestroyChild();
	}
}
