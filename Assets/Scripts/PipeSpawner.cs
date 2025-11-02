using System.Collections;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [Header("Pipe Settings")]
    [SerializeField] private GameObject pipePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnInterval = 1.0f;
    [SerializeField] private float maxSpawnInterval = 1.8f;
    [SerializeField] private float minHeight = -2.5f;
    [SerializeField] private float maxHeight = 1.5f;

    [Header("Difficulty Settings")]
    [SerializeField] private float difficultyIncreaseInterval = 10f; // a cada 10s
    [SerializeField] private float spawnReductionRate = 0.05f; // diminui 5% por vez
    [SerializeField] private float minSpawnLimit = 0.5f; // limite mínimo

    [SerializeField] private float pipeSpeedIncrease = 0.3f; // aumenta 0.3 por ciclo
    [SerializeField] private float maxPipeSpeed = 10f;

    private float timeElapsed = 0f;
    private float currentPipeSpeed = 4f;

    private void Start()
    {
        StartCoroutine(SpawnPipes());
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        // Aumenta dificuldade a cada X segundos
        if (timeElapsed >= difficultyIncreaseInterval)
        {
            timeElapsed = 0f;
            IncreaseDifficulty();
        }
    }

    private IEnumerator SpawnPipes()
    {
        while (true)
        {
            float randomHeight = Random.Range(minHeight, maxHeight);
            Vector3 pipePosition = new Vector3(8f, randomHeight, 0);

            // Instancia o cano e define a velocidade atual
            GameObject pipe = Instantiate(pipePrefab, pipePosition, Quaternion.identity);
            var pipeScript = pipe.GetComponent<Pipe>();
            if (pipeScript != null)
                pipeScript.SetSpeed(currentPipeSpeed);

            // Aguarda o próximo spawn
            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    private void IncreaseDifficulty()
    {
        // Reduz intervalo de spawn
        minSpawnInterval = Mathf.Max(minSpawnLimit, minSpawnInterval - spawnReductionRate);
        maxSpawnInterval = Mathf.Max(minSpawnLimit + 0.2f, maxSpawnInterval - spawnReductionRate);

        // Aumenta velocidade dos canos
        currentPipeSpeed = Mathf.Min(maxPipeSpeed, currentPipeSpeed + pipeSpeedIncrease);
    }
}
