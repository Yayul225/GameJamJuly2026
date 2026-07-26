using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Victory Settings")]
    [SerializeField] private int itemsRequiredToWin = 4;
    private int currentItemsDelivered = 0;

    [Header("Timed Events GameObjects")]
    [SerializeField] private GameObject bullEnemy;            // Spawns/Activates at count 8
    [SerializeField] private GameObject babyChickSpawner;    // Activates at count 6
    [SerializeField] private PlayerMovement player;     // Item/Trigger or Boost at count 5
    [SerializeField] private GameObject destroyableObstacles; // Spawns/Activates at count 4

    [Header("Scene Names")]
    [SerializeField] private string victorySceneName = "VictoryScene";
    [SerializeField] private string gameOverSceneName = "GameOverScene";

    private void Awake()
    {
        // Singleton setup so PlayerAttack can call GameManager.Instance anywhere
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Auto-find PlayerMovement if it wasn't assigned in the Inspector
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerMovement>();
        }
    }

    /// <summary>
    /// Called by CountDownScript whenever the timer count decreases.
    /// </summary>
    public void OnTimerTick(int currentCount)
    {
        Debug.Log($"[GameManager] Timer ticked: {currentCount}");

        // Count 8: Activar enemigo toro
        if (currentCount == 8 && babyChickSpawner != null)
        {
            bullEnemy.SetActive(true);
            
            Debug.Log("Pollitos activados");
        }

        // Count 6: Activate Baby Chick Spawner
        if (currentCount == 6 && bullEnemy != null)
        {
            babyChickSpawner.SetActive(true);
            Debug.Log("Enemigo Toro Activado");
        }

        // Count 5: Player Speed Boost
        if (currentCount == 5 && player != null)
        {
            player.ApplySpeedBoost(1.5f);
            Debug.Log("Boost de Velocidad activado (x1.5)");
        }

        // Count 4: Destroyable Obstacles
        if (currentCount == 4 && destroyableObstacles != null)
        {
            destroyableObstacles.SetActive(true);
            Debug.Log("Objetos destruibles activados");
        }
    }

    /// <summary>
    /// Call this whenever an item is successfully placed at a drop point.
    /// </summary>
    public void ItemDelivered()
    {
        currentItemsDelivered++;
        Debug.Log($"Item delivered! Total: {currentItemsDelivered}/{itemsRequiredToWin}");

        if (currentItemsDelivered >= itemsRequiredToWin)
        {
            TriggerVictory();
        }
    }

    private void TriggerVictory()
    {
        Debug.Log("Victoria, Todos los items entregados");
        SceneManager.LoadScene(victorySceneName);
    }

    public void TriggerGameOver()
    {
        Debug.Log("GAME OVER!");
        SceneManager.LoadScene(gameOverSceneName);
    }
}