using UnityEngine;

public enum PowerUpType
{
    None,    // Normal shot normal movement

    ExtraSpeed,
    Shotgun,
    Laser,
}

enum EnemyType
{
    DodgesLeftRight,
    Forward
}

[CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/Config", order = 1)]
public class Config : ScriptableObject
{
    [Header("Game")]
    public int TimeUntilNextPhase = 60;
    [Header("Audio/Music")]
    public AudioClip Intro; // @TODO: Get this into Kiki's scene eventually
    public AudioClip Gameplay;
    public AudioClip Level_Transition;  // @TODO: We don't have this yet
    public AudioClip Lose;
    public AudioClip Menu;
    public AudioClip Victory;
    [Header("Audio/SFX")]
    public AudioClip Explosion;
    public AudioClip Powerup;

    [Header("Animations")]
    public Sprite[] planetExplosions;
    public Sprite[] planetExplosions2;
    public Sprite[] phoenix;
    public Vector2 shakeAmount;
    public Sprite[] winAnimations;

    [Header("Dialouge/Intro")]
    public Sprite[] intro_sprites;

    public string[] intro_strings;
    [Header("Dialouge/Ingame")]
    public int TimeUntilDialogueDisappear = 3;
    public string[] planetVeryStartDialouge;
    public string[] planetHitDialouge;
    public string[] planetLostDialouge;
    public string[] planetWonDialouge;
    public string[] planetLosingTooFastDialouge;
    public string[] planetWinningTooFastDialouge;   // @TODO: We don't know when ships blow up yet

    [Header("Planet")]
    public int MaxNumberOfPlanetHealth = 10;
    public float TileWinTimer = 0.5f;

    [Header("Player")]
    public int Speed = 100;
    public float FireRate = 0.2f;
    public float BulletLifetime = 2;

    [Header("Enemies")]
    public int ChargerSpeed = 10;
    public int DodgerSpeed = 5;
    public float DodgerSinCosWidth = 5f;
    public float EnemyDeathDelay = 0.1f;
    [Header("Enemies/Spawn")]
    public float baseRange = 10;
    public float rangeMod = 10;
    public float spawnDelay = 2;
    public GameObject[] enemies;

    [Header("Power up")]
    public float TrailLength = 0.5f;
    public float DropRate = 0.2f;

    public GameObject PowerUpDropPrefab;

    [Header("Powerup/ExtraSpeed")]
    public int ExtraSpeedSpeed = 20;
    public int ExtraSpeedTime = 10;
    public Color ExtraSpeedColor = Color.yellow;

    [Header("Enemy")]
    //Enemy baseline
    public float WaveSize;
    //Entity will try to slam into planet
    public bool PlanetSeeking;

    public float WaveMove(float counter, bool sine = true)
    {
        return sine ? Mathf.Sin(counter) : Mathf.Cos(counter);
    }

    public Vector2 GetDirectionVector2(Transform callingEntity, Transform entityTarget)
    {
        Vector2 facingDirection = entityTarget.position - callingEntity.position;
        facingDirection.Normalize();
        return facingDirection;
    }
    //split Vec3 direction from lookAtPlanet to simplify
    public Vector3 GetDirectionVector3(Transform callingEntity, Transform entityTarget)
    {
        Vector2 baseDirection = GetDirectionVector2(callingEntity, entityTarget);
        return new Vector3(baseDirection.x, baseDirection.y, 0f);
    }
    public Quaternion LookAtTarget(Transform callingEntity, Transform entityTarget)
    {
        var offset = 90f;
        Vector2 facingDirection = GetDirectionVector2(callingEntity, entityTarget);
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(Vector3.forward * (angle + offset));
    }
}
