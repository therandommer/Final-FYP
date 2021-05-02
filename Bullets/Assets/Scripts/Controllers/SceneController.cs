using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public void ChangeScene(int _id)
	{
		Debug.Log($"Loading scene {_id}");
		Actions.OnSceneChanged?.Invoke(_id);
		SceneManager.LoadScene(_id);
	}
	public void QuitGame()
	{
		Debug.Log("Quitting Game");
		Application.Quit();
	}
}
