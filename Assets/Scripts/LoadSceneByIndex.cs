using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneByIndex : MonoBehaviour {

	public void LoadScene (int sceneByIndex)
	{
		SceneManager.LoadScene (sceneByIndex);
	}

	public void RestartScene ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

}
