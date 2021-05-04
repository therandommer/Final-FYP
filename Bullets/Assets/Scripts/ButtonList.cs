using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonList : MonoBehaviour
{
	public MusicController thisMusic;
	[SerializeField]
	private GameObject buttonTemplate;
	private List<GameObject> buttons = new List<GameObject>();
	void OnEnable()
	{
		if (!thisMusic)
		{
			thisMusic = FindObjectOfType<MusicController>();
		}
		RemakeList();
	}
	void Start()
	{
		if (!thisMusic)
		{
			thisMusic = FindObjectOfType<MusicController>();
		}
		RemakeList();
	}
	public void RemakeList()
	{
		if(buttons.Count>0)
		{
			foreach(GameObject button in buttons)
			{
				Destroy(button.gameObject);
			}
			buttons.Clear();
		}
		for (int i = 0; i < thisMusic.GetSongNumber(); ++i)
		{
			GameObject button = Instantiate(buttonTemplate) as GameObject;
			button.SetActive(true);
			button.GetComponent<ButtonListButton>().SetText(thisMusic.GetSpecificSongName(i));
			button.GetComponent<ButtonListButton>().SetId(i);
			button.transform.SetParent(buttonTemplate.transform.parent, false);
			buttons.Add(button);
		}
	}
	public void StartSong(int _index)
	{
		Debug.Log("Starting song: " + _index);
		thisMusic.SetSong(_index);
	}
	void CreateButtons()
	{

	}
}
