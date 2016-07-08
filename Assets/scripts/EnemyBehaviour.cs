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
	private float timeSinceLastHit;

	private bool wait;

	private Animator animator;

	private float sin, cos;
	private Vector2 offset;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		timeOfLastMovement = -Time.timeSinceLevelLoad;
		player = GameObject.FindWithTag("Player").transform;

		FindObjectOfType<ControllerRandomizer> ().ChangedInputEvent += OnMetamorphosis;

		sin = Mathf.Sin (-Mathf.PI/4);
		cos = Mathf.Cos (-Mathf.PI/4);

		offset = GetComponent<CircleCollider2D> ().offset;
	}

	
	// Update is called once per frame
	void Update () {
	}


	void FixedUpdate() {

		if (Time.timeSinceLevelLoad > timeOfLastMovement + moveDelay) {

			if (!wait) {
				Vector3 playerDirection = GetPlayerDirection ();

				currentDirection = Vector2.zero;

				if (playerDirection.x > 0.1f) {
					currentDirection.x++;
				}
				if (playerDirection.x < -0.1f) {
					currentDirection.x--;
				}
				if (playerDirection.y > 0.1f) {
					currentDirection.y++;
				}
				if (playerDirection.y < -0.1f) {
					currentDirection.y--;
				}


				/* Raycasting to avoid getting stuck in things */
				RaycastHit2D hit = Physics2D.Raycast (transform.position + (Vector3)offset, currentDirection, currentDirection.magnitude * 10);
				//Debug.DrawRay (transform.position + (Vector3)offset, currentDirection * 10, Color.red, 0.5f);
				for (int i = 0; i < 8 && hit && hit.transform.tag != "Player"; i++) {
					currentDirection = new Vector2 (currentDirection.x * cos - currentDirection.y * sin,
						currentDirection.x * sin + currentDirection.y * cos);
					hit = Physics2D.Raycast (transform.position + (Vector3)offset, currentDirection, currentDirection.magnitude);
				//	Debug.DrawRay (transform.position + (Vector3)offset, currentDirection * 10, Color.red, 0.5f);

					wait = true;
					CancelInvoke ("StopWait");
					Invoke ("StopWait", timeOfRecovery);
				}

			}

			currentDirection = currentDirection.normalized;

			SetAnimation ();

			timeOfLastMovement = Time.timeSinceLevelLoad;
			PixelMover.Move (transform, 1.5f * currentDirection.x, currentDirection.y*1.5f);
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
		} else if (currentDirection.x < 0) {
			if (aux_animator_variable != -2) {
				animator.SetInteger ("direction", -2);
				animator.SetTrigger ("left");
			}
		} else if (currentDirection.x > 0) {
			if (aux_animator_variable != 2) {
				animator.SetInteger ("direction", 2);
				animator.SetTrigger ("right");
			}
		} else if (currentDirection.y > 0) {
			if (aux_animator_variable != 1) {
				animator.SetInteger ("direction", 1);
				animator.SetTrigger ("up");
			}
		} else if (currentDirection.y < 0) {
			if (aux_animator_variable != -1) {
				animator.SetInteger ("direction", -1);
				animator.SetTrigger ("down");
			}
		}
	}


	Vector3 GetPlayerDirection() {
		return ((Vector2)(player.position - transform.position)).normalized;

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

	void StopWait() {
		wait = false;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Enemy" && coll.gameObject.GetComponent<EnemyBehaviour>().enemyCode > enemyCode) {
			enabled = false;
			Invoke ("SetEnabled", 2);
			animator.SetInteger ("direction", 0);
			animator.SetTrigger ("still");
		}
	
	}

	void OnDestroy() {
		ControllerRandomizer ctrl = FindObjectOfType<ControllerRandomizer> ();

		if (ctrl)
			ctrl.ChangedInputEvent -= OnMetamorphosis;
	}
}
