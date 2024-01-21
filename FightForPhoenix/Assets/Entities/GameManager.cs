using UnityEngine;
using UnityEngine.Tilemaps;
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

    public static Vector3Int[] PhoenixTilesToRemove = new Vector3Int[] { Vector3Int.zero, new Vector3Int(0, -1, 0), new Vector3Int(-1, 0, 0), new Vector3Int(-1, -1, 0), };

    enum GameState
    {
        None,
        Playing,
        Lost,
        Won
    }

    [SerializeField] GameObject plasmaShot;
    public Config config;
    public GameObject tilemapGameObject;
    public SpriteRenderer explosion;
    public SpriteRenderer explosion2;
    public Transform phoenix;
    public Image loseScreen;
    public Image winScreen;
    public Image white;
    public Image black;
    public Text m_TimerText;
    public Text ingameDialogueText;
    public AudioSource musicSource;
    public AudioSource SFXSource;

    GameState m_GameState;
    TestPlayerControlledEnemy TestControlledPlayerEnemies;
    Player m_Player;
    float m_PhaseTimeRemaining;
    PowerUpType m_PowerUpType;
    float m_PowerUpTimeRemaining;
    int m_PreviousNumberOfHits;
    float m_TimeUntilDialogueDisappear;
    // Shoot
    float currentFireTimer = 0f;
    bool canShoot = true;
    // Lose explosion
    bool m_ShowPlanetExplosion;
    int m_ExplosionPlanetIndex = 0;
    float m_ExplosionPlanetTimer = 0;
    int m_ExplosionPlanetIndex2;
    float m_ExplosionPlanetTimer2;
    // Tile win
    float m_TileWinTimer;
    Vector3Int m_Tile;
    int m_TileWinIndex;

    protected void Start()
    {
        NumberOfHits = 0;
        TestControlledPlayerEnemies = FindObjectOfType<TestPlayerControlledEnemy>();
        m_Player = FindObjectOfType<Player>();
        m_Player.TrailRenderer.enabled = false;
        m_PhaseTimeRemaining = config.TimeUntilNextPhase;
        m_TimerText.text = string.Empty;

        musicSource.clip = config.Gameplay;
        musicSource.Play();

        loseScreen.enabled = false;
        winScreen.enabled = false;
        white.enabled = false;
        black.enabled = true;

        var index = Random.Range(0, config.planetVeryStartDialouge.Length);
        ingameDialogueText.text = config.planetVeryStartDialouge[index];
        m_TimeUntilDialogueDisappear = config.TimeUntilDialogueDisappear;

        m_GameState = GameState.None;
        LeanTween.color(black.rectTransform, Color.clear, 1.5f).setOnComplete(() =>
        {
            m_GameState = GameState.Playing;
        });
    }

    protected void Update()
    {
        if (m_GameState == GameState.None) return;

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
            PlayerObtainedSuperSpeed();
        }
        if (Actions.TestPowerupDrop) {
            DropPowerup(new Vector3(0, 5, 0));
        }

        PlayerShoot(); //test shoot
        ShotTimer();

        // Timers
        m_PhaseTimeRemaining -= Time.deltaTime;
        m_PowerUpTimeRemaining -= Time.deltaTime;
        m_TimeUntilDialogueDisappear -= Time.deltaTime;
        m_TileWinTimer -= Time.deltaTime;

        GameState current = m_GameState;

        // Dialogue
        if (ingameDialogueText) {
            if (m_TimeUntilDialogueDisappear <= 0) {
                ingameDialogueText.text = string.Empty;
            }
        }
        if (m_GameState == GameState.Playing) {
            if (m_TimerText) {
                m_TimerText.text = string.Format("{0:0.00}", m_PhaseTimeRemaining);
            }
            if (m_PreviousNumberOfHits != NumberOfHits) {
                m_PreviousNumberOfHits = NumberOfHits;

                if (m_TimeUntilDialogueDisappear > 0.01) {
                    var index = Random.Range(0, config.planetLosingTooFastDialouge.Length);
                    ingameDialogueText.text = config.planetLosingTooFastDialouge[index];
                } else {
                    var index = Random.Range(0, config.planetHitDialouge.Length);
                    ingameDialogueText.text = config.planetHitDialouge[index];
                }
                m_TimeUntilDialogueDisappear = config.TimeUntilDialogueDisappear;
            }

            // Just so we can continually mess with the trail length for now
            m_Player.TrailRenderer.time = config.TrailLength;
        }

        if (m_ShowPlanetExplosion) {
            explosion.sprite = UpdateSpriteAnimation(config.planetExplosions, 0.5f, ref m_ExplosionPlanetIndex, ref m_ExplosionPlanetTimer);
            explosion2.sprite = UpdateSpriteAnimation(config.planetExplosions2, 0.5f, ref m_ExplosionPlanetIndex2, ref m_ExplosionPlanetTimer2);
        }

        if (NumberOfHits > config.MaxNumberOfPlanetHealth || Actions.TestLose) {
            m_GameState = GameState.Lost;
        }
        if (m_PhaseTimeRemaining <= 0 || Actions.TestWin) {
            m_GameState = GameState.Won;
        }
        if (m_PowerUpTimeRemaining <= 0) {
            m_PowerUpType = PowerUpType.None;
            m_Player.TrailRenderer.enabled = false;
            m_Player.TrailRenderer.startColor = Color.clear;
        }

        // Planet to Phoenix Animiation
        if (m_GameState == GameState.Won) {
            if (m_TileWinTimer < 0) {
                m_TileWinTimer = config.TileWinTimer;

                var tilemap = tilemapGameObject.GetComponentInChildren<Tilemap>();
                tilemap.SetTile(m_Tile, null);
                m_TileWinIndex += 1;
                if (m_TileWinIndex < PhoenixTilesToRemove.Length) {
                    m_Tile = PhoenixTilesToRemove[m_TileWinIndex];
                }
            }
        }


        var hasChangedState = current != m_GameState;
        if (hasChangedState && m_GameState == GameState.Won) {
            Debug.Log($"{m_GameState}");

            m_TimeUntilDialogueDisappear = config.TimeUntilDialogueDisappear;

            var index = Random.Range(0, config.planetWonDialouge.Length - 1);
            ingameDialogueText.text = config.planetWonDialouge[index];

            m_TileWinTimer = config.TileWinTimer;
            m_Tile = Vector3Int.zero;
            m_TileWinIndex = 0;
        }
        if (hasChangedState && m_GameState == GameState.Lost) {
            Debug.Log($"{m_GameState}");
            m_TimeUntilDialogueDisappear = config.TimeUntilDialogueDisappear;

            var index = Random.Range(0, config.planetLostDialouge.Length - 1);
            ingameDialogueText.text = config.planetLostDialouge[index];

            LeanTween.scale(tilemapGameObject, Vector3.zero, 2).setOnComplete(() =>
            {
                musicSource.clip = config.Lose;
                musicSource.Play();
                m_ShowPlanetExplosion = true;
                m_ExplosionPlanetIndex = 0;
                m_ExplosionPlanetTimer = 0;
                LeanTween.scale(explosion2.gameObject, Vector3.one * 10, 1.5f);
                LeanTween.scale(explosion .gameObject, Vector3.one * 10, 1.5f).setOnComplete(() =>
                {
                   m_ShowPlanetExplosion = false;
                   explosion.sprite = null;
                   explosion2.sprite = null;
                   Debug.Log("Now maybe show the Game over screen... after a fade to and from black?");
                });
            });
        }
    }

    private static Sprite UpdateSpriteAnimation(Sprite[] sprites, float duration, ref int index, ref float timer)
    {
        var sprite = sprites[index];
        if ((timer += Time.deltaTime) >= (duration / sprites.Length)) {
            timer = 0;
            index = (index + 1) % sprites.Length;
        }
        return sprite;
    }

    public void PlayerShoot()
    {
        if (canShoot && Actions.Shoot) {
            canShoot = false;
            Instantiate(plasmaShot, m_Player.BulletSpawn.position, m_Player.transform.rotation);
        }
    }
    public void ShotTimer()
    {
        if (!canShoot) {
            currentFireTimer += Time.deltaTime;
        }
        if (currentFireTimer >= config.FireRate) {
            currentFireTimer = 0f;
            canShoot = true;
        }
    }

    public void PlayerObtainedSuperSpeed()
    {
        m_PowerUpType = PowerUpType.ExtraSpeed;
        m_PowerUpTimeRemaining = config.ExtraSpeedTime;
        m_Player.TrailRenderer.enabled = true;
        m_Player.TrailRenderer.startColor = config.ExtraSpeedColor;
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

    public void DropPowerup(Vector3 position)
    {
        Debug.Log("We should drop a power up...");
        Instantiate(config.PowerUpDropPrefab, position, Quaternion.identity);
    }


    // Collisions
    internal void OnEnemyCollision(OnEnemyCollision onEnemyCollision, Collision2D other)
    {
        Debug.Log($"Enemy named '{onEnemyCollision.name}' blew up!");
        Destroy(onEnemyCollision.gameObject);

        SFXSource.clip = config.Explosion;
        SFXSource.Play();

        if (Random.value <= config.DropRate) {
            DropPowerup(other.transform.position);
        }
    }

    internal void OnPlanetCollision(Tilemap tilemap, Vector3Int hitPosition)
    {
        Debug.Log($"Planet hit!!");
        tilemap.SetTile(hitPosition, null);
        NumberOfHits++;

        SFXSource.clip = config.Explosion;
        SFXSource.Play();
    }

    internal void OnPowerupCollision(PowerUpDrop powerUpDrop, Collider2D other)
    {
        var hasHitPlayer = other.GetComponent<Player>() != null;
        var hasHitPlanet = other.GetComponent<Tilemap>() != null;
        if (hasHitPlanet || hasHitPlayer) {
            Destroy(powerUpDrop.gameObject);
            if (hasHitPlayer) {
                Debug.Log($"Obtained powerup!");
                PlayerObtainedSuperSpeed();
            }
        }
    }
}
