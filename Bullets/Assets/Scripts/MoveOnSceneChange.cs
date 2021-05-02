using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSceneChange : MonoBehaviour
{
    bool isMainMenu = true;
    public Vector2 menuPos;
    public Vector2 gamePos;
    void OnEnable()
	{
        Actions.OnSceneChanged += MoveThis;
	}
    void OnDisable()
	{
        Actions.OnSceneChanged -= MoveThis;
	}
    void MoveThis(int _index)
	{
        isMainMenu = !isMainMenu;
        if (isMainMenu)
            this.transform.position = menuPos;
        else
            this.transform.position = gamePos;
    }
}
