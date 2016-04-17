using UnityEngine;
using System.Collections;

public class AreaSpawner : MonoBehaviour {

	public GameObject[] objects;
	public float maxTime, minTime, decreaseRate;

	public float spawnDistance;
	public int maxSpawns;
	public Transform center;

	private float lastSpawnTime;
	private float currentRate;

	private float enemiesSpawn;

	// Use this for initialization
	void Start () {
		lastSpawnTime = -Time.timeSinceLevelLoad - maxTime;
		enemiesSpawn = 0;
	}
	
	// Update is called once per frame
	void Update () {
		currentRate = Mathf.Max (minTime, maxTime - decreaseRate * Time.timeSinceLevelLoad);

		if (enemiesSpawn < maxSpawns && Time.timeSinceLevelLoad > lastSpawnTime + currentRate) {
			Vector2 diff = Random.insideUnitCircle * spawnDistance;
			Vector3 distance = center.position + Vector3.right * diff.x + Vector3.up * diff.y;
			Instantiate (objects [Random.Range (0, objects.Length)], distance, Quaternion.identity);

			lastSpawnTime = Time.timeSinceLevelLoad;
			enemiesSpawn++;
		}
	}
}
