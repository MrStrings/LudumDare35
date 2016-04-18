using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	public Text text;
	public AudioClip clip;

	public float changeTime = 1f;


	private bool changed1, changed2, changed3, changed4;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.timeSinceLevelLoad > changeTime && !changed1) {
			text.text = "Mr.Strings";
			AudioSource.PlayClipAtPoint(clip, transform.position, 5f);
			changed1 = true;
		}


		if (Time.timeSinceLevelLoad > 2*changeTime && !changed2) {
			text.text = "@MatheusMortatti";
			AudioSource.PlayClipAtPoint(clip, transform.position);
			changed2 = true;
		}


		if (Time.timeSinceLevelLoad > 3*changeTime && !changed3) {
			text.text = "@LeoCVF";
			AudioSource.PlayClipAtPoint(clip, transform.position);
			changed3 = true;
		}

		if (Time.timeSinceLevelLoad > 4*changeTime && !changed4) {
			text.text = "Presents...";
			AudioSource.PlayClipAtPoint(clip, transform.position);
			changed4 = true;
		}

		if (Time.timeSinceLevelLoad > 5*changeTime) {
			text.text = "";
			SceneManager.LoadScene ("Main Menu");
		}
	}
}
