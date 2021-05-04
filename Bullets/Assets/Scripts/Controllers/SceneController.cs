using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
	public AudioSource thisAudio;
	void OnEnable()
	{
		Actions.OnSceneRequest += ChangeScene;
	}
	void OnDisable()
	{
		Actions.OnSceneRequest -= ChangeScene;
	}
    public void ChangeScene(int _id)
	{
		//Debug.Log($"Loading scene {_id}");
		Actions.OnSceneChanged?.Invoke(_id);
		if (_id == 0)
			thisAudio.loop = true; //allows the main menu to loop songs again
		SceneManager.LoadScene(_id);
	}
	public void QuitGame()
	{
		Debug.Log("Quitting Game");
		Application.Quit();
	}
}
