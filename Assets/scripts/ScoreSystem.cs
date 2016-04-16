using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {

	public Text timeText;
	public static int time;

	// Use this for initialization
	void Start () {
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		time = (int)Time.timeSinceLevelLoad;
		timeText.text = time.ToString ();
	}
}
