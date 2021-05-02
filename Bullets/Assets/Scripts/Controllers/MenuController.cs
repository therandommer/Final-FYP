using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu = null;
    [SerializeField]
    GameObject mainUi = null;
    private void OnEnable()
	{
        Actions.OnPause += TogglePause;
        Actions.OnSceneChanged += ToggleMain;
	}
    private void OnDisable()
	{
        Actions.OnPause -= TogglePause;
        Actions.OnSceneChanged -= ToggleMain;
    }

    void ToggleMain(int _scene)
	{
        if(_scene != 0 && mainUi.activeInHierarchy)
		{
            mainUi.SetActive(false);
		}
        if(_scene == 0 && !mainUi.activeInHierarchy)
		{
            mainUi.SetActive(true);
		}
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
