using UnityEngine;
using System.Collections;

public class Metamorphosis : MonoBehaviour {


	public ControllerRandomizer randomizer;
	Animator animator;

	public int lastState;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		randomizer.ChangedInputEvent += Transition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void Transition() {
		lastState = animator.GetInteger("state");

		animator.SetInteger("state", 0);
		animator.SetTrigger ("transform");

		Invoke("Shapeshift", 1f);
	}

	public void Shapeshift() {
		animator.SetInteger ("state", 1);
		animator.SetInteger ("character_direction", 0);
	}


}
