using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour {

	public enum State {NameInput, LeaderboardDisplay}

	class Score {
		public string status;
		public string p1, p2, p3, p4, p5, p6, p7, p8, p9, p10;
		public string s1, s2, s3, s4, s5, s6, s7, s8, s9, s10;
	}


	public Text leaderboardText, playerText, infoText, scoreText;
	public float maxNameLength = 10;

	private string playerName = "";
	private State state = State.NameInput;
	private string token = "5777e7ade0d3614674758851947";
	private string url;
	private WWW www;
	private Score score;

	private bool didset = false, didget = false;
	private bool waitingToEndRequest = false;

	// Use this for initialization
	void Start () {
		url = "http://gmscoreboard.com/handle_score.php?tagid=" + token;
		score = new Score ();

		scoreText.text = "Your Score:\n" + ScoreSystem.time.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.NameInput) {
			NameInput ();
		} else if (state == State.LeaderboardDisplay) {

			if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Space)) {
				SceneManager.LoadScene("Main Menu");
			}

			if (!didset) {
				if (playerName.Length > 0) {
					waitingToEndRequest = true;
					SendRequest (playerName, ScoreSystem.time.ToString());
				}
					
				didset = true;
			}
			if (!waitingToEndRequest && !didget) {
				GetRequest ();
				didget = true;
			}

			if (score.p1 != null)
				DisplayLeaderboard ();
		}
	}

	void NameInput() {

		if (playerName.Length > 0) {
			playerText.text = playerName;
			infoText.text = "Press Enter\nTo Submit";
		} else {
			playerText.text = "__________";
			infoText.text = "Press Enter\nTo Dismiss";
		}

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
					playerText.enabled = false;
					scoreText.enabled = false;
					leaderboardText.enabled = true;
					leaderboardText.text = "Loading...";

					infoText.text = "Press Enter\nTo Menu";
				}
			}
		}

	}


	void DisplayLeaderboard() {
		leaderboardText.text = "1- " + score.p1 + ": " + score.s1 +
							"\n2- " + score.p2 + ": " + score.s2 +
							"\n3- " + score.p3 + ": " + score.s3 +
							"\n4- " + score.p4 + ": " + score.s4 +
							"\n5- " + score.p5 + ": " + score.s5 +
							"\n6- " + score.p6 + ": " + score.s6 +
							"\n7- " + score.p7 + ": " + score.s7 +
							"\n8- " + score.p8 + ": " + score.s8 +
							"\n9- " + score.p9 + ": " + score.s9 +
							"\n10- " + score.p10 + ": " + score.s10;
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
			score = JsonUtility.FromJson<Score> (www.text);
			waitingToEndRequest = false;
		} else {
			Debug.Log ("Error: " + www.error);
			leaderboardText.text = "\n\n\n Couldn't Access \n Leaderboard. \n\n\n Press Enter \n to Return to MENU";
		}
	}

}
