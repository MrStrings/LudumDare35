using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {



	public Transform player;
	public float moveDelay;


	private float timeOfLastMovement;
	public Vector2 currentDirection;
	private Vector3 lastPosition;
	public int timesRepeated = 0;
	private float timeSinceLastHit;

	private Animator animator;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		timeOfLastMovement = -Time.timeSinceLevelLoad;
		player = GameObject.FindWithTag("Player").transform;
	}

	
	// Update is called once per frame
	void Update () {

	}


	void FixedUpdate() {

		if (Time.timeSinceLevelLoad > timeOfLastMovement + moveDelay) {

			Vector3 playerDirection = GetPlayerDirection ();

			currentDirection = Vector2.zero;


			if (playerDirection.x > 0.1f) {
				currentDirection.x++;
				timeOfLastMovement = Time.timeSinceLevelLoad;
			}
			if (playerDirection.x < -0.1f) {
				currentDirection.x--;
				timeOfLastMovement = Time.timeSinceLevelLoad;
			}
			if (playerDirection.y > 0.1f) {
				currentDirection.y++;
				timeOfLastMovement = Time.timeSinceLevelLoad;
			}
			if (playerDirection.y < -0.1f) {
				currentDirection.y--;
				timeOfLastMovement = Time.timeSinceLevelLoad;
			}

			SetAnimation ();

			lastPosition = transform.position;
			PixelMover.Move (transform, currentDirection.x, currentDirection.y);

			PositionVerify ();
		}
	}


	void SetAnimation() {

		int aux_animator_variable = animator.GetInteger ("direction");

		if (currentDirection.x == 0 && currentDirection.y == 0) {
			if (aux_animator_variable != 0) {
				animator.SetInteger ("direction", 0);
				animator.SetTrigger ("still");
			}
		} else if (currentDirection.x == -1) {
			if (aux_animator_variable != -2) {
				animator.SetInteger ("direction", -2);
				animator.SetTrigger ("left");
			}
		} else if (currentDirection.x == 1) {
			if (aux_animator_variable != 2) {
				animator.SetInteger ("direction", 2);
				animator.SetTrigger ("right");
			}
		} else if (currentDirection.y == 1) {
			if (aux_animator_variable != 1) {
				animator.SetInteger ("direction", 1);
				animator.SetTrigger ("up");
			}
		} else if (currentDirection.y == -1) {
			if (aux_animator_variable != -1) {
				animator.SetInteger ("direction", -1);
				animator.SetTrigger ("down");
			}
		}
	}

	Vector3 GetPlayerDirection() {
		return (player.position - transform.position).normalized;

	}


	void PositionVerify() {

		if (Time.timeSinceLevelLoad < timeSinceLastHit + 0.2f) {
			currentDirection = new Vector2 (-currentDirection.y, currentDirection.x);
			PixelMover.Move (transform, currentDirection.x, currentDirection.y);
			if (lastPosition.x == transform.position.x && lastPosition.y == transform.position.y) {
				currentDirection = new Vector2 (-currentDirection.y, currentDirection.x);
				PixelMover.Move (transform, currentDirection.x, currentDirection.y);

				if (lastPosition.x == transform.position.x && lastPosition.y == transform.position.y) {
					currentDirection = new Vector2 (-currentDirection.y, currentDirection.x);
					PixelMover.Move (transform, currentDirection.x, currentDirection.y);
				} else {
					lastPosition = transform.position;
				}
			} else {
				lastPosition = transform.position;
			}
				

		}


		timeSinceLastHit = Time.timeSinceLevelLoad;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Wall") {
			transform.position = lastPosition;

			PositionVerify ();
				
		}
	}
}
