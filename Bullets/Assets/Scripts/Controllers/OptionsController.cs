using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//global options and settings for everything to reference on game/scene start, alongside handling game quit
public class OptionsController : MonoBehaviour
{
    float musicVolume = 0.3f;
    float sfxVolume = 0.3f; //sounds that aren't music
	public Texture2D cursorTexture;
	void Awake()
	{
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}
	void Start()
	{
		
	}
    public float getMusicVolume()
	{
        return musicVolume;
	}
    public void setMusicVolume(float _newVal)
	{
        musicVolume = _newVal;
	}
    public float getSfxVolume()
	{
        return sfxVolume;
	}
    public void setSfxVolume(float _newVal)
	{
        sfxVolume = _newVal;
	}
    public void QuitGame()
	{
        Application.Quit();
	}
}
