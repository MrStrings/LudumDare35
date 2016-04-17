using UnityEngine;
using System.Collections;

public class DynamicLayering : MonoBehaviour {

	void Start() {
	//	rend = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += (Vector3.forward * (transform.position.y - transform.position.z));
	}
}
