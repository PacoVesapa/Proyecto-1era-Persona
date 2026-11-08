using System.Collections;
using TMPro;
using UnityEngine;

public class GeneradorDeOleadas : MonoBehaviour
{
    [Header("Configuración de Oleadas")]
    public Transform[] spawnPoints;        // Puntos de spawn en el mapa

    [Header("Prefabs de Enemigos")]
    public GameObject normalEnemyPrefab;   // Enemigo normal (80% de probabilidad)
    public GameObject bigEnemyPrefab;      // Enemigo grande (20% de probabilidad)

    [Header("Estado de la Ronda")]
    public int currentRound = 0;           // Ronda actual
    public TMP_Text TextoRonda;                     // Texto que muestra la ronda actual
    public int enemiesLeft;                // Enemigos que quedan vivos en la ronda
    public TMP_Text TextoEnemigos;                  // Texto que muestra los enemigos restantes
    private int enemiesToSpawn;            // Cuántos enemigos se crearán en esta ronda
    private int enemiesSpawnedSoFar;       // Cuántos se han creado ya
    private float spawnInterval = 2.0f;    // Tiempo entre el spawn de cada enemigo
    private bool isSpawning = false;
    public GameObject PanelVictoria; // Panel de victoria
    void Start()
    {
        NextRound();
    }

    void Update()
    {
        // Si no está spawneando y ya no quedan enemigos, empieza la siguiente ronda
        if (!isSpawning && enemiesLeft <= 0 && currentRound < 5)
        {
            NextRound();
        }
        else if (currentRound >= 5 && enemiesLeft <= 0)
        {
            Debug.Log("¡Has ganado!");
            PanelVictoria.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego
        }
    }

    void NextRound()
    {
        currentRound++;
        TextoRonda.text = "Ronda: " + currentRound + "/5";
        // Fórmula estilo CoD: Cada ronda hay más enemigos que la anterior
        enemiesToSpawn = Mathf.RoundToInt(currentRound * 3.5f + 5);
        enemiesLeft = enemiesToSpawn;
        enemiesSpawnedSoFar = 0;
        ActualizarTextoEnemigos();

        Debug.Log("Iniciando Ronda: " + currentRound + " | Enemigos totales: " + enemiesToSpawn);

        StartCoroutine(SpawnWave());
    }

    public IEnumerator SpawnWave()
    {
        isSpawning = true;

        while (enemiesSpawnedSoFar < enemiesToSpawn)
        {
            SpawnEnemy();
            enemiesSpawnedSoFar++;

            // Espera unos segundos antes de spawnear al siguiente
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    void SpawnEnemy()
    {
        // Elige un punto de spawn aleatorio de la lista
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // 2. Sistema de probabilidad (Porcentaje de 0 a 100)
        float randomChance = Random.Range(0f, 100f);
        GameObject prefabToSpawn;

        if (randomChance <= 80f)
        {
            prefabToSpawn = normalEnemyPrefab; // 80% de probabilidad (de 0 a 80)
        }
        else
        {
            prefabToSpawn = bigEnemyPrefab;    // 20% de probabilidad (de 80.01 a 100)
        }

        // 3. Instanciar el enemigo elegido
        GameObject newEnemy = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

        // Le pasa la referencia de este manager al enemigo para que avise al morir
        newEnemy.GetComponent<ReceiveDamage>().waveManager = this;
    }

    // Este método lo llamará el enemigo cuando muera
    public void EnemyDied()
    {
        enemiesLeft--;
        ActualizarTextoEnemigos();
    }

    public void ActualizarTextoEnemigos()
    {
        TextoEnemigos.text = "Enemigos restantes: " + enemiesLeft;
    }
}