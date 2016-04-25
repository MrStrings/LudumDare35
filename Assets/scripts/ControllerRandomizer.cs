using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControllerRandomizer : MonoBehaviour {

	public enum KeyboardType {QWERTY, AZERTY, DVORAK};


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

	public delegate void ChoseInput();
	public event ChoseInput ChosenInputEvent;

	[HideInInspector]
	public static KeyboardType keyboardType = KeyboardType.QWERTY;

	public Walker player;
	public float changeInSeconds =  10, changeInSeconds_hard = 5, tellNextSecondsBefore = 3;
	public float timeBetweenDificulties = 70;
	public float firstChangeTime = 10;

	public Text nextKeyTextVertical, nextKeyTextHorizontal, keyTextVertical, keyTextHorizontal, warningText;
	public AudioClip warningAudio;

	[HideInInspector]
	public float timeOfLastChange;

	private List<WalkBundle> easyBundle;
	private List<WalkPair> mediumBundleVertical, mediumBundleHorizontal;
	private WalkBundle nextKeys;
	private bool didChange, didChangeOnce, changedToHard;

	// Use this for initialization
	void Start () {
		
		easyBundle = new List<WalkBundle> ();
		mediumBundleHorizontal = new List<WalkPair> ();
		mediumBundleVertical = new List<WalkPair> ();
		nextKeys = new WalkBundle (player.left, player.right, player.up, player.down);

		timeOfLastChange = 0;

		SetBundles ();

		if (keyboardType == KeyboardType.QWERTY) {
			keyTextVertical.text = "WS";
			keyTextHorizontal.text = "AD";
			SetInput (KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S);
		} else if (keyboardType == KeyboardType.AZERTY) {
			keyTextVertical.text = "ZS";
			keyTextHorizontal.text = "AD";
			SetInput (KeyCode.A, KeyCode.D, KeyCode.Z, KeyCode.S);
		} else if (keyboardType == KeyboardType.DVORAK) {
			keyTextVertical.text = "PU";
			keyTextHorizontal.text = "EI";
			SetInput (KeyCode.E, KeyCode.I, KeyCode.P, KeyCode.U);
		}

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
		if (Time.timeSinceLevelLoad > firstChangeTime && Time.timeSinceLevelLoad > timeOfLastChange + changeInSeconds) {
			if (!didChange)
				ChangeInput ();
			
			SetInput (nextKeys.left, nextKeys.right, nextKeys.up, nextKeys.down);
			if (ChangedInputEvent != null)
				ChangedInputEvent ();

			timeOfLastChange = Time.timeSinceLevelLoad;
			didChange = false;
		}

		//Define next input type
		if (!didChange && Time.timeSinceLevelLoad > timeOfLastChange +
			(Time.timeSinceLevelLoad > firstChangeTime ? changeInSeconds : firstChangeTime) - tellNextSecondsBefore) {
			ChangeInput ();
			didChange = true;

			if (ChosenInputEvent != null)
				ChosenInputEvent ();
		}



		//Verify if the user pressed the wrong keys
		if (Input.anyKey && !Input.GetKey (player.up) && !Input.GetKey (player.down)
		    && !Input.GetKey (player.left) && !Input.GetKey (player.right)) {
			warningText.text = "!";
			if (Input.anyKeyDown)
				AudioSource.PlayClipAtPoint (warningAudio, Vector3.zero, 0.1f);
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


		} else */
		if (didChangeOnce) {
			WalkPair vertical, horizontal;
			int i = Random.Range (0, 2);

			if (i == 0) {
				vertical = mediumBundleVertical [Random.Range (0, mediumBundleVertical.Count)];

				if (vertical.negative == player.down && vertical.positive == player.up) //Gives one more chance for changing
					vertical = mediumBundleVertical [Random.Range (0, mediumBundleVertical.Count)];
					
				nextKeys.up = vertical.positive;
				nextKeys.down = vertical.negative;
			} else {
				horizontal = mediumBundleHorizontal [Random.Range (0, mediumBundleHorizontal.Count)];

				if (horizontal.negative == player.left && horizontal.positive == player.right) //Gives one more chance for changing
					horizontal = mediumBundleHorizontal [Random.Range (0, mediumBundleHorizontal.Count)];
				

				nextKeys.left = horizontal.negative;
				nextKeys.right = horizontal.positive;
			}

		} else {
			WalkPair vertical, horizontal;

			vertical = mediumBundleVertical [Random.Range (0, mediumBundleVertical.Count)];
			horizontal = mediumBundleHorizontal [Random.Range (0, mediumBundleHorizontal.Count)];


			if (vertical.negative == player.down && vertical.positive == player.up) //Gives one more chance for changing
				vertical = mediumBundleVertical [Random.Range (0, mediumBundleVertical.Count)];

			if (horizontal.negative == player.left && horizontal.positive == player.right) //Gives one more chance for changing
				horizontal = mediumBundleHorizontal [Random.Range (0, mediumBundleHorizontal.Count)];
			

			nextKeys.up = vertical.positive;
			nextKeys.down = vertical.negative;
			nextKeys.left = horizontal.negative;
			nextKeys.right = horizontal.positive;

			didChangeOnce = true;
		}


		if (Time.timeSinceLevelLoad >= timeBetweenDificulties && !changedToHard) {
			changeInSeconds = changeInSeconds_hard;
			AudioManager audio = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
			audio.activateChange = true;
			changedToHard = true;
		} else if (Time.timeSinceLevelLoad >= 2 * timeBetweenDificulties)
			changeInSeconds = tellNextSecondsBefore + 1;
	}



	public void SetInput(KeyCode left, KeyCode right, KeyCode up, KeyCode down) {
		player.up = up;
		player.down = down;
		player.left = left;
		player.right = right;
	}

	public void SetBundles() {
		WalkBundle bundle;
		WalkPair pair;

		if (keyboardType == KeyboardType.QWERTY) {

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




		} else if (keyboardType == KeyboardType.AZERTY) {

			bundle = new WalkBundle (KeyCode.Q, KeyCode.D, KeyCode.Z, KeyCode.S);
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
			bundle = new WalkBundle (KeyCode.K, KeyCode.M, KeyCode.O, KeyCode.L);
			easyBundle.Add (bundle);


			pair = new WalkPair (KeyCode.W, KeyCode.X);
			mediumBundleHorizontal.Add (pair);
			pair = new WalkPair (KeyCode.X, KeyCode.C);
			mediumBundleHorizontal.Add (pair);
			pair = new WalkPair (KeyCode.C, KeyCode.V);
			mediumBundleHorizontal.Add (pair);
			pair = new WalkPair (KeyCode.V, KeyCode.B);
			mediumBundleHorizontal.Add (pair);
			pair = new WalkPair (KeyCode.B, KeyCode.N);
			mediumBundleHorizontal.Add (pair);

			pair = new WalkPair (KeyCode.Q, KeyCode.A);
			mediumBundleVertical.Add (pair);
			pair = new WalkPair (KeyCode.S, KeyCode.Z);
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
			pair = new WalkPair (KeyCode.M, KeyCode.P);
			mediumBundleVertical.Add (pair);





		} else if (keyboardType == KeyboardType.DVORAK) {


			bundle = new WalkBundle (KeyCode.E, KeyCode.I, KeyCode.P, KeyCode.U);
			easyBundle.Add (bundle);
			bundle = new WalkBundle (KeyCode.U, KeyCode.D, KeyCode.Y, KeyCode.I);
			easyBundle.Add (bundle);
			bundle = new WalkBundle (KeyCode.I, KeyCode.H, KeyCode.F, KeyCode.D);
			easyBundle.Add (bundle);
			bundle = new WalkBundle (KeyCode.D, KeyCode.T, KeyCode.G, KeyCode.H);
			easyBundle.Add (bundle);
			bundle = new WalkBundle (KeyCode.H, KeyCode.N, KeyCode.C, KeyCode.T);
			easyBundle.Add (bundle);
			bundle = new WalkBundle (KeyCode.T, KeyCode.S, KeyCode.R, KeyCode.N);
			easyBundle.Add (bundle);


			pair = new WalkPair (KeyCode.Q, KeyCode.J);
			mediumBundleHorizontal.Add (pair);
			pair = new WalkPair (KeyCode.J, KeyCode.K);
			mediumBundleHorizontal.Add (pair);
			pair = new WalkPair (KeyCode.K, KeyCode.X);
			mediumBundleHorizontal.Add (pair);
			pair = new WalkPair (KeyCode.X, KeyCode.B);
			mediumBundleHorizontal.Add (pair);
			pair = new WalkPair (KeyCode.B, KeyCode.M);
			mediumBundleHorizontal.Add (pair);
			pair = new WalkPair (KeyCode.M, KeyCode.W);
			mediumBundleHorizontal.Add (pair);
			pair = new WalkPair (KeyCode.W, KeyCode.V);
			mediumBundleHorizontal.Add (pair);
			pair = new WalkPair (KeyCode.V, KeyCode.Z);
			mediumBundleHorizontal.Add (pair);


			pair = new WalkPair (KeyCode.U, KeyCode.P);
			mediumBundleVertical.Add (pair);
			pair = new WalkPair (KeyCode.I, KeyCode.Y);
			mediumBundleVertical.Add (pair);
			pair = new WalkPair (KeyCode.D, KeyCode.F);
			mediumBundleVertical.Add (pair);
			pair = new WalkPair (KeyCode.H, KeyCode.G);
			mediumBundleVertical.Add (pair);
			pair = new WalkPair (KeyCode.T, KeyCode.C);
			mediumBundleVertical.Add (pair);
			pair = new WalkPair (KeyCode.N, KeyCode.R);
			mediumBundleVertical.Add (pair);
			pair = new WalkPair (KeyCode.S, KeyCode.L);
			mediumBundleVertical.Add (pair);
		}

	}



	void OnDestroy() {

		if (nextKeyTextVertical != null)
			nextKeyTextVertical.text = "";
		if (nextKeyTextHorizontal != null)
			nextKeyTextHorizontal.text = "";
		if (keyTextVertical != null)
			keyTextVertical.text = "";
		if (keyTextHorizontal != null)
			keyTextHorizontal.text = "";
		if (warningText != null)
			warningText.text = "";
	}
}
