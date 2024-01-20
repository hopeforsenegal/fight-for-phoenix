using System.Runtime.InteropServices;
using UnityEngine;

// We could populate these from the Config if we really wanted to
public static class Actions
{
    public static bool Left => Input.GetKey(KeyCode.A);
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

    GameState m_GameState;
    TestPlayerControlledEnemy m_Bullet;

    void Start()
    {
        NumberOfHits = 0;
        m_GameState = GameState.Playing;
        m_Bullet = FindObjectOfType<TestPlayerControlledEnemy>();
    }

    void Update()
    {
        if (Actions.Left) {
            Vector3 v = -m_Bullet.transform.right * 1;  // -transform.right = left
            m_Bullet.Rigidbody.velocity = v;
        }
        if (Actions.Right) {
            Vector3 v = transform.right * 1;            // transform.right = right
            m_Bullet.Rigidbody.velocity = v;
        }

        if (NumberOfHits > config.MaxNumberOfPlanetHealth) {
            m_GameState = GameState.Lost;
            Debug.Log($"{GameState.Lost}");
        }

        if (m_GameState == GameState.Won) {
            // What happens when we win?
        }
        if (m_GameState == GameState.Lost) {
            // What happens when we lose?
        }
    }

    void FixedUpdate()
    {
        // We should move player phsyics in here
        // once we have enemy ships
    }
}
