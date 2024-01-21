using System.Security.Cryptography;
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
    [SerializeField] GameObject plasmaShot;
    public static int NumberOfHits { get; set; }

    enum GameState
    {
        Playing,
        Lost,
        Won
    }

    public Config config;
    public GameObject tilemapGameObject;
    public SpriteRenderer explosion;
    public Transform phoenix;
    public Text m_TimerText;
    public Text ingameDialogueText;

    public AudioSource musicSource;
    public AudioSource SFXSource;


    GameState m_GameState;
    TestPlayerControlledEnemy TestControlledPlayerEnemies;
    Player m_Player;
    float m_TimeRemaining;
    PowerUpType m_PowerUpType;
    float m_PowerUpTimeRemaining;
    int m_PreviousNumberOfHits;
    float timeUntilDialogueDisappear;

    const float fireRate = 0.2f;
    float currentFireTimer = 0f;
    bool canShoot = true;
    private bool m_ShowPlanetExplosion;
    private int m_ExplosionPlanetIndex = 0;
    private float m_ExplosionPlanetTimer = 0;

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

        musicSource.clip = config.Gameplay;
        musicSource.Play();
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
            ObtainedSuperSpeed();
        }
        if (Actions.TestPowerupDrop) {
            DropPowerup(new Vector3(0, 10, 0));
        }

        PlayerShoot(); //test shoot
        ShotTimer();

        GameState current = m_GameState;
        m_TimeRemaining -= Time.deltaTime;
        m_PowerUpTimeRemaining -= Time.deltaTime;
        timeUntilDialogueDisappear -= Time.deltaTime;

        // Dialogue
        if (ingameDialogueText) {
            if (timeUntilDialogueDisappear <= 0) {
                ingameDialogueText.text = string.Empty;
            }
        }
        if (m_GameState == GameState.Playing) {
            if (m_TimerText) {
                m_TimerText.text = string.Format("{0:0.00}", m_TimeRemaining);
            }
            if (m_PreviousNumberOfHits != NumberOfHits) {
                m_PreviousNumberOfHits = NumberOfHits;

                if (timeUntilDialogueDisappear > 0.01) {
                    var index = Random.Range(0, config.planetLosingTooFastDialouge.Length);
                    ingameDialogueText.text = config.planetLosingTooFastDialouge[index];
                } else {
                    var index = Random.Range(0, config.planetHitDialouge.Length);
                    ingameDialogueText.text = config.planetHitDialouge[index];
                }
                timeUntilDialogueDisappear = config.TimeUntilDialogueDisappear;
            }

            // Just so we can continually mess with the trail length for now
            m_Player.TrailRenderer.time = config.TrailLength;
        }

        if (m_ShowPlanetExplosion) {
            explosion.sprite = UpdateSpriteAnimation(config.planetExplosions, 0.5f, ref m_ExplosionPlanetIndex, ref m_ExplosionPlanetTimer);
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

            timeUntilDialogueDisappear = config.TimeUntilDialogueDisappear;

            var index = Random.Range(0, config.planetWonDialouge.Length - 1);
            ingameDialogueText.text = config.planetWonDialouge[index];
        }
        if (hasChangedState && m_GameState == GameState.Lost || Actions.TestLose) {
            Debug.Log($"{m_GameState}");
            timeUntilDialogueDisappear = config.TimeUntilDialogueDisappear;

            var index = Random.Range(0, config.planetLostDialouge.Length - 1);
            ingameDialogueText.text = config.planetLostDialouge[index];

            LeanTween.scale(tilemapGameObject, Vector3.zero, 2).setOnComplete(() =>
            {
                Debug.Log("Planet death animation complete");

                musicSource.clip = config.Lose;
                musicSource.Play();
                m_ShowPlanetExplosion = true;
                m_ExplosionPlanetIndex = 0;
                m_ExplosionPlanetTimer = 0;
                LeanTween.scale(explosion.gameObject, Vector3.one * 5, 1.5f).setOnComplete(() =>
                {
                    m_ShowPlanetExplosion = false;
                    explosion.sprite = null;
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

    public void PlayerShoot() {
        if(Actions.Shoot && canShoot){
            GameObject shot = Instantiate(plasmaShot, 
                m_Player.BulletSpawn.position, m_Player.transform.rotation);
            canShoot = false;
        }
    }
    public void ShotTimer() {
        if(!canShoot) {
            currentFireTimer += Time.deltaTime;
        }
        if(currentFireTimer >= fireRate) {
            currentFireTimer = 0f;
            canShoot = true;
        }
    }

    public void ObtainedSuperSpeed()
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

    public void DropPowerup(Vector3 position) {
        Debug.Log("We should drop a power up");

        var go = Instantiate(config.PowerUpDropPrefab, position, Quaternion.identity);
    }

    internal void OnEnemyCollision(OnEnemyCollision onEnemyCollision, Collision2D other)
    {
        Destroy(onEnemyCollision.gameObject);
        Debug.Log($"enemy named '{onEnemyCollision.name}' blew up!");

        SFXSource.clip = config.Explosion;
        SFXSource.Play();

        if (Random.value <= config.DropRate) {
            DropPowerup(other.transform.position);
        }
    }

    internal void OnPlanetCollision(Tilemap tilemap, Vector3Int hitPosition)
    {
        tilemap.SetTile(hitPosition, null);
        NumberOfHits++;
        Debug.Log($"Got hit!!");

        SFXSource.clip = config.Explosion;
        SFXSource.Play();
    }




}
