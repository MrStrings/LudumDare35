using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour {

	public enum State {NameInput, LeaderboardDisplay}

	class Score {
		public string status;
		public string p1, p2, p3, p4, p5, p6, p7, p8, p9, p10;
		public string s1, s2, s3, s4, s5, s6, s7, s8, s9, s10;
	}


	public Text playerTextBox;
	public float maxNameLength = 10;

	private string playerName = "";
	private State state = State.NameInput;
	private string token = "5777e7ade0d3614674758851947";
	private string url;
	private WWW www;

	private bool didset = false, didget = false;
	private bool waitingToEndRequest = false;

	// Use this for initialization
	void Start () {
		url = "http://gmscoreboard.com/handle_score.php?tagid=" + token;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.NameInput) {
			NameInput ();
		} else if (state == State.LeaderboardDisplay) {

			if (!didset) {
				if (playerName.Length > 0) {
					waitingToEndRequest = true;
					SendRequest (playerName, "1");
				}
					
				didset = true;
			}
			if (!waitingToEndRequest && !didget) {
				GetRequest ();
				didget = true;
			}
		}
	}

	void NameInput() {

		foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown (kcode)) {
				if (((char)kcode >= 'a' && (char)kcode <= 'z') ||
					((char)kcode >= '0' && (char)kcode <= '9')) {
					if (playerName.Length <= maxNameLength)
						playerName += (char)kcode;
				} else if (kcode == KeyCode.Backspace) {
					if (playerName.Length > 1) {
						playerName = playerName.Substring (0, playerName.Length - 1);
					} else if (playerName.Length == 1) {
						playerName = "";
					}
				} else if (kcode == KeyCode.Return) {
					state = State.LeaderboardDisplay;
				}
			}
		}

		if (playerName.Length > 0)
			playerTextBox.text = playerName;
		else 
			playerTextBox.text = "________";

	}


	void GetRequest() {
		www = new WWW (url+"&getscore=10");
		StartCoroutine(WaitForRequest(www));
	}


	void SendRequest(string playerName, string newScore) {
		www = new WWW (url + "&player="+ playerName + "&score=" + newScore);
		StartCoroutine(WaitForRequest(www));
	}


	IEnumerator WaitForRequest(WWW www)
	{

		yield return www;

		// check for errors
		if (www.error == null) {
			Debug.Log ("WWW Ok!: " + www.text);
			Score score = JsonUtility.FromJson<Score> (www.text);
			waitingToEndRequest = false;
			//print (score.p1);
		} else {
			Debug.Log ("WWW Error: " + www.error);
		}
	}

}
