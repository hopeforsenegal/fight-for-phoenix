using System;
using UnityEngine;

[Flags]
public enum PowerUpType
{
    None = 0,

    ExtraSpeed = 1 << 0,
}

[CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/Config", order = 1)]
public class Config : ScriptableObject
{
    // Game
    public int TimeUntilNextPhase = 60;

    // Planet
    public int MaxNumberOfPlanetHealth = 10;

    // Player
    public int Speed = 100;

    // Power up
    public float TrailLength = 0.5f;
    public float DropRate = 0.2f;

    public GameObject PowerUpDropPrefab;

    public int ExtraSpeedSpeed = 20;
    public int ExtraSpeedTime = 10;
    public Color ExtraSpeedColor = Color.yellow;


    //Enemy baseline
    enum EnemyType { DodgesLeftRight, Forward}
    public float WaveSize; 
    //Entity will try to slam into planet
    public bool PlanetSeeking;

    public Vector3 WaveMove(Vector3 direction, float counter, bool sine=true) {
        float slideDirection;
        slideDirection = sine ? Mathf.Sin(counter) : Mathf.Cos(counter);
        return new Vector3(0f, slideDirection,0f) * WaveSize;
    }

    public Vector2 GetDirectionVector2(Transform callingEntity, Transform entityTarget) {
        Vector2 facingDirection = entityTarget.position - callingEntity.position;
        facingDirection.Normalize();
        return facingDirection;
    }
    //split Vec3 direction from lookAtPlanet to simplify
    public Vector3 GetDirectionVector3(Transform callingEntity, Transform entityTarget) {
        Vector2 baseDirection = GetDirectionVector2(callingEntity, entityTarget);
        return new Vector3(baseDirection.x, baseDirection.y, 0f);
    }
    public Quaternion LookAtPlanet(Transform callingEntity, Transform entityTarget) {
        var offset = 90f;
        Vector2 facingDirection = GetDirectionVector2(callingEntity, entityTarget);
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(Vector3.forward * (angle + offset));

    }
}
