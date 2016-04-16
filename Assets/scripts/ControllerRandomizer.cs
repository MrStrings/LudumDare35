using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControllerRandomizer : MonoBehaviour {

	/* Class for easily defining 4 keys */
	public class WalkBundle {
		public KeyCode left, right, up, down;

		public WalkBundle(KeyCode left, KeyCode right, KeyCode up, KeyCode down) {
			this.left = left;
			this.right = right;
			this.up = up;
			this.down = down;
		}
	}

	/* Class for easily defining 2 keys */
	public class WalkPair {
		public KeyCode negative, positive;

		public WalkPair(KeyCode negative, KeyCode positive) {
			this.negative = negative;
			this.positive = positive;
		}
	}

	public delegate void ChangedInput();
	public event ChangedInput ChangedInputEvent;


	public Walker player;
	public float changeInSeconds =  5, tellNextSecondsAfter = 3;
	public float timeBetweenDificulties = 15;

	public Text nextKeyTextVertical, nextKeyTextHorizontal, keyTextVertical, keyTextHorizontal, warningText;

	[HideInInspector]
	public float timeOfLastChange;

	private List<WalkBundle> easyBundle;
	private List<WalkPair> mediumBundleVertical, mediumBundleHorizontal;
	private WalkBundle nextKeys;
	private bool didChange, didChangeOnce;

	// Use this for initialization
	void Start () {
		
		easyBundle = new List<WalkBundle> ();
		mediumBundleHorizontal = new List<WalkPair> ();
		mediumBundleVertical = new List<WalkPair> ();
		nextKeys = new WalkBundle (player.left, player.right, player.up, player.down);

		timeOfLastChange = 0;

		SetBundles ();

	}
	
	// Update is called once per frame
	void Update () {

		//timeText.text = ((int)Mathf.Ceil((timeOfLastChange + changeInSeconds) - Time.timeSinceLevelLoad)).ToString ();

		keyTextVertical.text = player.up.ToString ().ToUpper () + player.down.ToString ().ToUpper ();
		keyTextHorizontal.text = player.left.ToString ().ToUpper () + player.right.ToString ().ToUpper ();

		if (didChange) {
			nextKeyTextVertical.text = (player.up != nextKeys.up ? nextKeys.up.ToString ().ToUpper () : " ") +
									   (player.down != nextKeys.down ? nextKeys.down.ToString ().ToUpper () : " ");
			nextKeyTextHorizontal.text = (player.left != nextKeys.left ? nextKeys.left.ToString ().ToUpper () : " ") +
										 (player.right != nextKeys.right ? nextKeys.right.ToString ().ToUpper () : " ");
		} else {
			nextKeyTextHorizontal.text = "";
			nextKeyTextVertical.text = "";
		}

		//Give player new input type
		if (Time.timeSinceLevelLoad > timeOfLastChange + changeInSeconds) {
			if (!didChange)
				ChangeInput ();
			
			SetInput (nextKeys.left, nextKeys.right, nextKeys.up, nextKeys.down);
			if (ChangedInputEvent != null)
				ChangedInputEvent ();

			timeOfLastChange = Time.timeSinceLevelLoad;
			didChange = false;
		}

		//Define next input type
		if (!didChange && Time.timeSinceLevelLoad > timeOfLastChange + tellNextSecondsAfter) {
			ChangeInput ();
			didChange = true;
		}



		//Verify if the user pressed the wrong keys
		if (Input.anyKey && !Input.GetKey (player.up) && !Input.GetKey (player.down)
		    && !Input.GetKey (player.left) && !Input.GetKey (player.right)) {
			warningText.text = "!";
		} else {
			warningText.text = "";
		}
	}


	public void ChangeInput(){
		/*if (Time.timeSinceLevelLoad < timeBetweenDificulties) {
			nextKeys = easyBundle [Random.Range (0, easyBundle.Count)];

		} else if (Time.timeSinceLevelLoad < 2 * timeBetweenDificulties) {
			WalkPair horizontal;

			horizontal = mediumBundleHorizontal [Random.Range (0, mediumBundleHorizontal.Count)];
			nextKeys.left = horizontal.negative;
			nextKeys.right = horizontal.positive;


		} else */if (Time.timeSinceLevelLoad < timeBetweenDificulties && didChangeOnce) {
			WalkPair vertical, horizontal;
			int i = Random.Range (0, 2);

			if (i == 0) {
				vertical = mediumBundleVertical [Random.Range (0, mediumBundleVertical.Count)];
				nextKeys.up = vertical.positive;
				nextKeys.down = vertical.negative;
			} else {
				horizontal = mediumBundleHorizontal [Random.Range (0, mediumBundleHorizontal.Count)];
				nextKeys.left = horizontal.negative;
				nextKeys.right = horizontal.positive;
			}

		} else {// if (Time.timeSinceLevelLoad < 4 * timeBetweenDificulties) {
			WalkPair vertical, horizontal;

			vertical = mediumBundleVertical [Random.Range (0, mediumBundleVertical.Count)];
			horizontal = mediumBundleHorizontal [Random.Range (0, mediumBundleHorizontal.Count)];

			nextKeys.up = vertical.positive;
			nextKeys.down = vertical.negative;
			nextKeys.left = horizontal.negative;
			nextKeys.right = horizontal.positive;

			didChangeOnce = true;
		}
	}



	public void SetInput(KeyCode left, KeyCode right, KeyCode up, KeyCode down) {
		player.up = up;
		player.down = down;
		player.left = left;
		player.right = right;
	}



	public void SetBundles() {
		WalkBundle bundle;

		bundle = new WalkBundle (KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S);
		easyBundle.Add (bundle);
		bundle = new WalkBundle (KeyCode.S, KeyCode.F, KeyCode.E, KeyCode.D);
		easyBundle.Add (bundle);
		bundle = new WalkBundle (KeyCode.D, KeyCode.G, KeyCode.R, KeyCode.F);
		easyBundle.Add (bundle);
		bundle = new WalkBundle (KeyCode.F, KeyCode.H, KeyCode.T, KeyCode.G);
		easyBundle.Add (bundle);
		bundle = new WalkBundle (KeyCode.G, KeyCode.J, KeyCode.Y, KeyCode.H);
		easyBundle.Add (bundle);
		bundle = new WalkBundle (KeyCode.H, KeyCode.K, KeyCode.U, KeyCode.J);
		easyBundle.Add (bundle);
		bundle = new WalkBundle (KeyCode.J, KeyCode.L, KeyCode.I, KeyCode.K);
		easyBundle.Add (bundle);


		WalkPair pair;
		/*pair = new WalkPair (KeyCode.Q, KeyCode.W);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.W, KeyCode.E);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.E, KeyCode.R);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.R, KeyCode.T);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.T, KeyCode.Y);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.Y, KeyCode.U);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.U, KeyCode.I);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.I, KeyCode.O);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.O, KeyCode.P);
		mediumBundleHorizontal.Add (pair);

		pair = new WalkPair (KeyCode.A, KeyCode.S);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.S, KeyCode.D);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.D, KeyCode.F);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.F, KeyCode.G);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.G, KeyCode.H);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.H, KeyCode.J);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.J, KeyCode.K);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.K, KeyCode.L);
		mediumBundleHorizontal.Add (pair);*/


		pair = new WalkPair (KeyCode.Z, KeyCode.X);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.X, KeyCode.C);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.C, KeyCode.V);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.V, KeyCode.B);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.B, KeyCode.N);
		mediumBundleHorizontal.Add (pair);
		pair = new WalkPair (KeyCode.N, KeyCode.M);
		mediumBundleHorizontal.Add (pair);






		pair = new WalkPair (KeyCode.A, KeyCode.Q);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.S, KeyCode.W);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.D, KeyCode.E);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.F, KeyCode.R);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.G, KeyCode.T);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.H, KeyCode.Y);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.J, KeyCode.U);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.K, KeyCode.I);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.L, KeyCode.O);
		mediumBundleVertical.Add (pair);

		/*pair = new WalkPair (KeyCode.A, KeyCode.Z);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.S, KeyCode.X);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.D, KeyCode.C);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.F, KeyCode.V);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.G, KeyCode.B);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.H, KeyCode.N);
		mediumBundleVertical.Add (pair);
		pair = new WalkPair (KeyCode.J, KeyCode.M);
		mediumBundleVertical.Add (pair);*/
	}
}
