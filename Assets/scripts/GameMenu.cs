using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

	public GameObject fadeout;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Space)) {
			Invoke ("loadScene", 2f);
			fadeout.SetActive (true);
		}
	}



	void loadScene() {
		SceneManager.LoadScene("game");
	}
}
