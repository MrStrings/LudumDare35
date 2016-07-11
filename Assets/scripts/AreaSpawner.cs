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

	public float enemiesSpawn;
	public float enemiesAlive;

	// Use this for initialization
	void Start () {
		lastSpawnTime = -Time.timeSinceLevelLoad - maxTime;
		enemiesSpawn = 0;
	}
	
	// Update is called once per frame
	void Update () {
		currentRate = Mathf.Max (minTime, maxTime - decreaseRate * Time.timeSinceLevelLoad);

		if (enemiesAlive < maxSpawns && Time.timeSinceLevelLoad > lastSpawnTime + currentRate) {
			Vector2 diff = Random.insideUnitCircle.normalized * spawnDistance;
			Vector3 distance = center.position + Vector3.right * Mathf.Ceil(diff.x) + Vector3.up * Mathf.Ceil(diff.y);
			GameObject obj = Instantiate (objects [Random.Range (0, objects.Length)], distance, Quaternion.identity) as GameObject;

			obj.GetComponent<EnemyBehaviour> ().enemyCode = enemiesSpawn;

			lastSpawnTime = Time.timeSinceLevelLoad;
			enemiesSpawn++;
			enemiesAlive++;
		}
	}
}
