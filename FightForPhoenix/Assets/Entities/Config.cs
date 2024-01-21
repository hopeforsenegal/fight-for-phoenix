using System;
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
    [Header("Game/Music")]
    public AudioClip Intro; // @TODO: Get this into Kiki's scene eventually
    public AudioClip Gameplay;
    public AudioClip Level_Transition;  // @TODO: We don't have this yet
    public AudioClip Lose;
    public AudioClip Menu;
    [Header("Game/SFX")]
    public AudioClip Explosion;


    [Header("Explosions")]
    public Sprite[] planetExplosions;
    public Sprite[] planetExplosions2;

    [Header("Dialouge")]
    public int TimeUntilDialogueDisappear = 3;
    public string[] planetHitDialouge;
    public string[] planetLostDialouge;
    public string[] planetWonDialouge;
    public string[] planetLosingTooFastDialouge;
    public string[] planetWinningTooFastDialouge;   // @TODO: We don't know when ships blow up yet
    /*
    // option 1
    public Sprite[] sprites;
    public string[] strings;
    // option 2
    struct thing{
        Sprite sprite;
        string stringy;
    }
    thing[] things;
    */

    // Planet
    public int MaxNumberOfPlanetHealth = 10;

    // Player
    public int Speed = 100;

    // Power up
    [Header("Powerup")]
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

    public Vector3 WaveMove(Vector3 direction, float counter, bool sine = true)
    {
        float slideDirection;
        slideDirection = sine ? Mathf.Sin(counter) : Mathf.Cos(counter);
        return new Vector3(0f, slideDirection, 0f) * WaveSize;
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
    public Quaternion LookAtPlanet(Transform callingEntity, Transform entityTarget)
    {
        var offset = 90f;
        Vector2 facingDirection = GetDirectionVector2(callingEntity, entityTarget);
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(Vector3.forward * (angle + offset));

    }
}
