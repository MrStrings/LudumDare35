using UnityEngine;
using System.Collections;

public class PixelPerfectonator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (Mathf.Round (transform.position.x), 
			Mathf.Round (transform.position.y), Mathf.Round (transform.position.z));

	}
}
