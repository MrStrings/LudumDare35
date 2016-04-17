using UnityEngine;
using System.Collections;

public class BoundControl : MonoBehaviour {

	public float bounds_x_min, bounds_y_min, bounds_x_max, bounds_y_max;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x > bounds_x_max) {
			transform.position += (Vector3.right * (bounds_x_max - transform.position.x));
		} else if (transform.position.x < bounds_x_min) {
			transform.position += (Vector3.right * (bounds_x_min - transform.position.x));
		}



		if (transform.position.y > bounds_y_max) {
			transform.position += (Vector3.up * (bounds_y_max - transform.position.y));
		} else if (transform.position.y < bounds_y_min) {
			transform.position += (Vector3.up * (bounds_y_min - transform.position.y));
		}
	
	}
}
