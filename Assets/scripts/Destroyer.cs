using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Destroyer : MonoBehaviour {

	public string tagToDestroy;
	public GameObject boom, fadeOut;
	public string gameOverScreen;
	public float timeToChangeScreen,timeToFadeOut;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == tagToDestroy) {
			Instantiate (boom, other.transform.position, Quaternion.identity);

			other.GetComponent<SpriteRenderer> ().enabled = false;
			other.GetComponent<BoxCollider2D> ().enabled = false;
			other.GetComponent<Walker> ().enabled = false;
			other.GetComponent<Animator> ().SetTrigger ("still");
			other.GetComponent<Animator> ().SetInteger ("character_direction", 0);
			FindObjectOfType<ScoreSystem> ().enabled = false;
			FindObjectOfType<AudioManager> ().activateChange = true;

			Destroy(GameObject.FindGameObjectWithTag ("GameController"));


			Invoke ("FadeOut", timeToFadeOut);
			Invoke ("GameOverMenu", timeToChangeScreen);

		}
	}


	void GameOverMenu() {

		SceneManager.LoadScene (gameOverScreen);
	}



	void FadeOut() {
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		FindObjectOfType<AudioManager> ().activateChange = false;
		FindObjectOfType<AudioManager> ().source.volume = 0;
		Instantiate (fadeOut, camera.transform.TransformPoint(Vector3.zero) + Vector3.forward, Quaternion.identity);
	}
}

