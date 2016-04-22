using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public Transform player;
	public float moveDelay;

	public GameObject exclamation;
	public float timeOfRecovery = 1f;

	public float enemyCode;

	[HideInInspector]
	public float timeOfLastMovement;

	private Vector2 currentDirection = Vector2.right;
	private Vector3 lastPosition;
	private float timeSinceLastHit;
	private float spawnTime;

	private bool wait;

	private Animator animator;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		timeOfLastMovement = -Time.timeSinceLevelLoad;
		player = GameObject.FindWithTag("Player").transform;

		FindObjectOfType<ControllerRandomizer> ().ChangedInputEvent += OnMetamorphosis;

		spawnTime = Time.timeSinceLevelLoad;
	}

	
	// Update is called once per frame
	void Update () {
	}


	void FixedUpdate() {

		if (Time.timeSinceLevelLoad > timeOfLastMovement + moveDelay) {

			Vector3 playerDirection = GetPlayerDirection ();

			currentDirection = Vector2.zero;

			if (!wait) {
				if (playerDirection.x > 0.05f) {
					currentDirection.x++;
					timeOfLastMovement = Time.timeSinceLevelLoad;
				}
				if (playerDirection.x < -0.05f) {
					currentDirection.x--;
					timeOfLastMovement = Time.timeSinceLevelLoad;
				}
				if (playerDirection.y > 0.05f) {
					currentDirection.y++;
					timeOfLastMovement = Time.timeSinceLevelLoad;
				}
				if (playerDirection.y < -0.05f) {
					currentDirection.y--;
					timeOfLastMovement = Time.timeSinceLevelLoad;
				}
			}

			SetAnimation ();

			lastPosition = transform.position;
			PixelMover.Move (transform, currentDirection.x, currentDirection.y);
			/*Debug.Log ("Current: " + currentDirection.ToString());
			Debug.Log ("Distance: " + playerDirection.ToString());*/
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
		return ((Vector2)(player.position - transform.position)).normalized;

	}


	void PositionVerify() {
		
		if (Time.timeSinceLevelLoad < timeSinceLastHit + 0.2f) {

			if (Mathf.Abs (currentDirection.x) == Mathf.Abs (currentDirection.y)) {
				currentDirection = new Vector2 (0, currentDirection.y);
			} else if (Mathf.Abs (currentDirection.x) > Mathf.Abs (currentDirection.y)) {
				Debug.Log ("Enemy got here!" + currentDirection.ToString());
				currentDirection = new Vector2 (0, currentDirection.x);
			} else {
				Debug.Log ("Enemy got REALLY here!" + currentDirection.ToString());
				currentDirection = new Vector2 (-currentDirection.y, 0);
				Debug.Log ("Now..." + currentDirection.ToString());
			}
				
			PixelMover.Move (transform, currentDirection.x, currentDirection.y);

			/*if (lastPosition.x == transform.position.x && lastPosition.y == transform.position.y) {
				currentDirection = new Vector2 (-currentDirection.y, currentDirection.x);
				PixelMover.Move (transform, currentDirection.x, currentDirection.y);
				Debug.Log ("Verified!-1");

				if (lastPosition.x == transform.position.x && lastPosition.y == transform.position.y) {
					currentDirection = new Vector2 (-currentDirection.y, currentDirection.x);
					PixelMover.Move (transform, currentDirection.x, currentDirection.y);

					Debug.Log ("Verified!-2");
				} else {
					lastPosition = transform.position;
				}
			} else {*/
			lastPosition = transform.position;
		}
				

		//}


		timeSinceLastHit = Time.timeSinceLevelLoad;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Wall") {
			
			if (Time.timeSinceLevelLoad > spawnTime + 0.5f) {
				transform.position = transform.position - (Vector3)currentDirection;

				PositionVerify ();
			} else {
				Destroy (other.gameObject);
			}	
		} if (other.tag == "Enemy") {

			if (other.GetComponent<EnemyBehaviour> ().enemyCode > enemyCode) {
				wait = true;
				Invoke ("Enable", 1);

			}
				
				
			transform.position = transform.position - (Vector3)currentDirection;

		}
	}


	void Enable() {
		wait = false;
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Wall") {

			if (Time.timeSinceLevelLoad > spawnTime + 0.5f) {
				transform.position = transform.position - (Vector3)currentDirection;

				PositionVerify ();
			} else {
				Destroy (other.gameObject);
			}	
		} /*if (other.tag == "Enemy") {

			transform.position = transform.position - (Vector3)currentDirection;
			//PositionVerify ();
		}	*/
	}


	void OnMetamorphosis() {
		if (Mathf.Abs (player.position.x - transform.position.x) <= 32 &&
		    Mathf.Abs (player.position.y - transform.position.y) <= 32) {

			Instantiate (exclamation, transform.position + Vector3.up * 10, Quaternion.identity);

			enabled = false;
			Invoke ("SetEnabled", timeOfRecovery);
			animator.SetInteger ("direction", 0);
			animator.SetTrigger ("still");
		}
	}


	void SetEnabled() {
		enabled = true;
	}

	void OnDestroy() {
		ControllerRandomizer ctrl = FindObjectOfType<ControllerRandomizer> ();

		if (ctrl)
			ctrl.ChangedInputEvent -= OnMetamorphosis;
	}
}
