using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

	public GameObject fadeout;
	public AudioSource source;

	private bool invoked;


	// Use this for initialization
	void Start () {
		source = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (invoked) {
			source.volume = Mathf.Max (0, source.volume - Time.deltaTime*2);
			
		} else if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Space)) {
			Invoke ("loadScene", 2f);
			fadeout.SetActive (true);
			invoked = true;
		} 
	}



	void loadScene() {
		SceneManager.LoadScene("game");
	}
}
