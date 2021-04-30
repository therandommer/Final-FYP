using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu = null;
    private void OnEnable()
	{
        Actions.OnPause += TogglePause;
        
	}
    private void OnDisable()
	{
        Actions.OnPause -= TogglePause;
    }

    void TogglePause()
	{
        if(pauseMenu == null)
            pauseMenu = GameObject.Find("Pause UI");
        if (!pauseMenu.activeInHierarchy)
		{
            pauseMenu.SetActive(true);
        }
        else
		{
            pauseMenu.SetActive(false);
        }  
	}
    public void SendPauseAction()
	{
        Actions.OnPause?.Invoke();
	}
    public void SendRestartAction()
	{
        Actions.OnLevelRestart?.Invoke();
	}
}
