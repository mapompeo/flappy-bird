using System.Collections;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pipePrefab;

    [SerializeField] private float minSpawnInterval = 1;
    [SerializeField] private float maxSpawnInterval = 1.8f;
    [SerializeField] private float minHeight = -2.5f;
    [SerializeField] private float maxHeight = 1.5f;

    private void Start()
    {
        StartCoroutine(SpawnPipes());
    }

    private IEnumerator SpawnPipes()
    {
        while (true)
        {
            float randomHeight = Random.Range(minHeight, maxHeight);
            Vector3 pipePosition = new Vector3(8f, randomHeight, 0);
            Instantiate(pipePrefab, pipePosition, Quaternion.identity);
            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval);
        }
    }
}