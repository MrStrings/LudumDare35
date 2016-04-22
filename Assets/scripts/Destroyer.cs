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



	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == tagToDestroy) {
			Instantiate (boom, coll.transform.position, Quaternion.identity);

			coll.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			coll.gameObject.GetComponent<Collider2D> ().enabled = false;
			coll.gameObject.GetComponent<Walker> ().enabled = false;
			coll.gameObject.GetComponent<Animator> ().SetTrigger ("still");
			coll.gameObject.GetComponent<Animator> ().SetInteger ("character_direction", 0);
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

