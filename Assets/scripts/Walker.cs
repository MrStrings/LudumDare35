using UnityEngine;
using System.Collections;

public class Walker : MonoBehaviour {



	public KeyCode left, right, up, down;
	public float moveDelay;
	public AudioClip[] clips;

	private float timeOfLastMovement;
	private Vector2 currentDirection;

	private Animator animator;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		timeOfLastMovement = -Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void FixedUpdate() {
	
		if (Time.timeSinceLevelLoad > timeOfLastMovement + moveDelay) {
			currentDirection = Vector2.zero;


			if (Input.GetKey (left)) {
				currentDirection.x--;
				timeOfLastMovement = Time.timeSinceLevelLoad;
			}
			if (Input.GetKey (right)) {
				currentDirection.x++;
				timeOfLastMovement = Time.timeSinceLevelLoad;
			}
			if (Input.GetKey (up)) {
				currentDirection.y++;
				timeOfLastMovement = Time.timeSinceLevelLoad;
			}
			if (Input.GetKey (down)) {
				currentDirection.y--;
				timeOfLastMovement = Time.timeSinceLevelLoad;
			}

			currentDirection = currentDirection.normalized;

			SetAnimation ();

			PixelMover.Move (transform, currentDirection.x, currentDirection.y);
		}
	}


	void SetAnimation() {

		int aux_animator_variable = animator.GetInteger ("character_direction");

		if (currentDirection.x == 0 && currentDirection.y == 0) {
			if (aux_animator_variable != 0) {
				animator.SetInteger ("character_direction", 0);
				animator.SetTrigger ("still");
			}
		} else if (currentDirection.x < 0) {
			if (aux_animator_variable != -2) {
				animator.SetInteger ("character_direction", -2);
				animator.SetTrigger ("left");
			}
		} else if (currentDirection.x > 0) {
			if (aux_animator_variable != 2) {
				animator.SetInteger ("character_direction", 2);
				animator.SetTrigger ("right");
			}
		} else if (currentDirection.y > 0) {
			if (aux_animator_variable != 1) {
				animator.SetInteger ("character_direction", 1);
				animator.SetTrigger ("up");
			}
		} else if (currentDirection.y < 0) {
			if (aux_animator_variable != -1) {
				animator.SetInteger ("character_direction", -1);
				animator.SetTrigger ("down");
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Wall") {
			transform.position = transform.position - (Vector3)currentDirection;
		}
	}




	public void PlayClip() {
		AudioSource.PlayClipAtPoint (clips [Random.Range (0, clips.Length)], Vector3.zero, 0.5f);
	}
}
