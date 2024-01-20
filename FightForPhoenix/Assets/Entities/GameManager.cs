using UnityEngine;
using UnityEngine.UI;

// We could populate these actions from the Config if we really wanted to
public static class Actions
{
    public static bool Left  => Input.GetKey(KeyCode.A);
    public static bool Right => Input.GetKey(KeyCode.D);

    public static bool Shoot => Input.GetKey(KeyCode.Space);

    // Take these out once we like these sequences
    public static bool TestLose => Input.GetKey(KeyCode.Alpha1);
    public static bool TestWin  => Input.GetKey(KeyCode.Alpha2);

    public static bool TestSuperSpeed  => Input.GetKey(KeyCode.Alpha3);
    public static bool TestPowerupDrop => Input.GetKeyDown(KeyCode.Alpha4);
}

public class GameManager : MonoBehaviour
{
    public static int NumberOfHits { get; set; }

    enum GameState
    {
        Playing,
        Lost,
        Won
    }

    public Config config;
    public GameObject tilemapGameObject;
    public GameObject explosion;
    public Transform phoenix;
    public Text m_TimerText;

    GameState m_GameState;
    TestPlayerControlledEnemy TestControlledPlayerEnemies;
    Player m_Player;
    float m_TimeRemaining;
    PowerUpType m_PowerUpType;
    float m_PowerUpTimeRemaining;

    protected void Start()
    {
        NumberOfHits = 0;
        m_GameState = GameState.Playing;
        TestControlledPlayerEnemies = FindObjectOfType<TestPlayerControlledEnemy>();
        m_Player = FindObjectOfType<Player>();
        m_Player.TrailRenderer.enabled = false;
        m_TimeRemaining = config.TimeUntilNextPhase;

        // Set the drop rate of power ups. We might move this to be in update so we can test better.
        var enemyCollisions = FindObjectsOfType<OnEnemyCollision>();
        foreach (var e in enemyCollisions) {
            e.dropRate = config.DropRate;
        }
    }

    protected void Update()
    {
        // Test Stuff
        if (TestControlledPlayerEnemies) {
            if (Actions.Left) {
                Vector3 v = -TestControlledPlayerEnemies.transform.right * 1;  // -transform.right = left
                TestControlledPlayerEnemies.Rigidbody.velocity = v;
            }
            if (Actions.Right) {
                Vector3 v = transform.right * 1;            // transform.right = right
                TestControlledPlayerEnemies.Rigidbody.velocity = v;
            }
        }
        if (Actions.TestSuperSpeed) {
            m_PowerUpType = PowerUpType.ExtraSpeed;
            m_PowerUpTimeRemaining = config.ExtraSpeedTime;
            m_Player.TrailRenderer.enabled = true;
            m_Player.TrailRenderer.startColor = config.ExtraSpeedColor;
        }
        if (Actions.TestPowerupDrop) {
            DropPowerup(new Vector3(0, 10, 0));
        }

        GameState current = m_GameState;
        m_TimeRemaining -= Time.deltaTime;
        m_PowerUpTimeRemaining -= Time.deltaTime;

        if (m_GameState == GameState.Playing) {
            m_TimerText.text = string.Format("{0:0.00}", m_TimeRemaining);

            // Just so we can continually mess with the trail length for now
            m_Player.TrailRenderer.time = config.TrailLength;
        }
        if (NumberOfHits > config.MaxNumberOfPlanetHealth) {
            m_GameState = GameState.Lost;
        }
        if (m_TimeRemaining <= 0) {
            m_GameState = GameState.Won;
        }
        if (m_PowerUpTimeRemaining <= 0) {
            m_PowerUpType = PowerUpType.None;
            m_Player.TrailRenderer.enabled = false;
            m_Player.TrailRenderer.startColor = Color.clear;
        }


        var hasChangedState = current != m_GameState;
        if (hasChangedState && m_GameState == GameState.Won || Actions.TestWin) {
            Debug.Log($"{m_GameState}");
            // What happens when we win?
        }
        if (hasChangedState && m_GameState == GameState.Lost || Actions.TestLose) {
            Debug.Log($"{m_GameState}");
            // What happens when we lose?
            LeanTween.scale(tilemapGameObject, Vector3.zero, 2).setOnComplete(() =>
            {
                Debug.Log("Planet death animation complete");
                LeanTween.scale(explosion, Vector3.one * 5, 0.75f).setOnComplete(() =>
                {
                    Debug.Log("Now maybe show the Game over screen... after a fade to and from black?");
                });
            });
        }
    }

    protected void FixedUpdate()
    {
        // Player Movement
        var direction = 0;
        if (Actions.Left)  direction = 1;
        if (Actions.Right) direction = -1;
        var speed = m_PowerUpType == PowerUpType.ExtraSpeed ? config.Speed + config.ExtraSpeedSpeed : config.Speed;
        var angle = direction * speed * Time.fixedDeltaTime;
        m_Player.transform.RotateAround(phoenix.transform.position, Vector3.forward, angle);

        // Other
    }

    public void DropPowerup(Vector3 position) {
        Debug.Log("We should drop a power up");

        var go = Instantiate(config.PowerUpDropPrefab, position, Quaternion.identity);
    }
}
