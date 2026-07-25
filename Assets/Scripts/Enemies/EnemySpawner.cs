using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn & Patrol Area")]
    [SerializeField] private Vector2 zoneCenter;
    [SerializeField] private Vector2 zoneSize = new Vector2(10f, 10f);

    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(zoneCenter == Vector2.zero)
        {
            zoneCenter = transform.position;
        }

        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i< enemyCount; i++)
        {
            //Escoger una poscion aleatoria dentro de la zona
            Vector2 spawnPosition = GetRandomSpawnPosition();

            //Instanciar el enemigo en la posicion aleatoria
            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            //Configurar el enemigo con la zona de patrulla
            if(spawnedEnemy.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.SetPatrolZone(zoneCenter, zoneSize);
            }
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(zoneCenter.x - zoneSize.x / 2f, zoneCenter.x + zoneSize.x / 2f);
        float randomY = Random.Range(zoneCenter.y - zoneSize.y / 2f, zoneCenter.y + zoneSize.y / 2f);
        return new Vector2(randomX, randomY);
    }

    // Gizmos Visuales para la zona de spawn y patrulla
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 drawCenter = (zoneCenter == Vector2.zero) ? (Vector2)transform.position : zoneCenter;
        Gizmos.DrawWireCube(drawCenter, zoneSize);
    }
}
