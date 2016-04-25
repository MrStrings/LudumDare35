using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KeyboardChooser : MonoBehaviour {

	public Text text;
	public AudioClip clip;
	public string[] options = {"< QWERTY >", "< AZERTY >", "< DVORAK >"};


	private int i;

	private Animator textAnimator;

	// Use this for initialization
	void Start () {
		textAnimator = text.gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			i = (i + 1) % options.Length;
			text.text = options [i];
			AudioSource.PlayClipAtPoint (clip, Vector3.forward * -10, 0.5f);

		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			i = (i == 0 ? options.Length - 1 : (i - 1) % options.Length);
			text.text = options [i];
			AudioSource.PlayClipAtPoint (clip, Vector3.forward * -10, 0.5f);
		}


		if (i == 0)
			ControllerRandomizer.keyboardType = ControllerRandomizer.KeyboardType.QWERTY;
		if (i == 1)
			ControllerRandomizer.keyboardType = ControllerRandomizer.KeyboardType.AZERTY;
		if (i == 2)
			ControllerRandomizer.keyboardType = ControllerRandomizer.KeyboardType.DVORAK;
		
	}
}
