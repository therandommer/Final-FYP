using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtonListButton : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI thisText;
	[SerializeField]
	private ButtonList buttonController;
	[SerializeField]
	private int thisId;
	public void SetText(string _textString)
	{
		thisText.text = _textString;
	}
	public void SetId(int _newId)
	{
		thisId = _newId;
	}
	public void OnClick()
	{
		buttonController.StartSong(thisId);
	}
}
