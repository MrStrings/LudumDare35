using UnityEngine;
using System.Collections;

public class DynamicLayering : MonoBehaviour {

	public Transform player;

	//Collider2D rend;

	void Start() {
	//	rend = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.position.y < transform.position.y)
			transform.position -= (Vector3.forward * transform.position.z);
		else 
			transform.position -= (Vector3.forward * (transform.position.z + 2));
	}
}
