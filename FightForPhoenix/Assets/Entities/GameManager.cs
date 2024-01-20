using UnityEngine;

// We could populate these actions from the Config if we really wanted to
public static class Actions
{
    public static bool Left  => Input.GetKey(KeyCode.A);
    public static bool Right => Input.GetKey(KeyCode.D);

    public static bool Shoot => Input.GetKey(KeyCode.Space);
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

    GameState m_GameState;
    TestPlayerControlledEnemy TestControlledPlayerEnemies;
    Player m_Player;
    bool m_HasPowerUp;

    protected void Start()
    {
        NumberOfHits = 0;
        m_GameState = GameState.Playing;
        TestControlledPlayerEnemies = FindObjectOfType<TestPlayerControlledEnemy>();
        m_Player = FindObjectOfType<Player>();
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

        GameState current = m_GameState;

        if (NumberOfHits > config.MaxNumberOfPlanetHealth) {
            m_GameState = GameState.Lost;
        }

        var hasChangedState = current != m_GameState;
        if (hasChangedState && m_GameState == GameState.Won) {
            Debug.Log($"{m_GameState}");
            // What happens when we win?
        }
        if (hasChangedState && m_GameState == GameState.Lost) {
            Debug.Log($"{m_GameState}");
            // What happens when we lose?
            LeanTween.scale(tilemapGameObject, Vector3.zero, 2).setOnComplete(() =>
            {
                Debug.Log("Planet death animation complete");
            });
        }
    }

    protected void FixedUpdate()
    {
        // Player Movement
        var direction = 0;
        if (Actions.Left)  direction = 1;
        if (Actions.Right) direction = -1;
        var speed = m_HasPowerUp ? config.Speed + config.PowerSpeed : config.Speed;
        var angle = direction * speed * Time.fixedDeltaTime;
        m_Player.transform.RotateAround(m_Player.phoenix.transform.position, Vector3.forward, angle);

        // Other
    }
}
