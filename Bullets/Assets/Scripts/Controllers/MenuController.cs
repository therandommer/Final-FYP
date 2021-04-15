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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TogglePause()
	{
        if (!pauseMenu.activeInHierarchy)
            pauseMenu.SetActive(true);
        else
            pauseMenu.SetActive(false);
	}
}
