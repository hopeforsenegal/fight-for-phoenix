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

    public int ExtraSpeedSpeed = 20;
    public int ExtraSpeedTime = 10;
    public Color ExtraSpeedColor = Color.yellow;


    //Enemy baseline
    enum EnemyType { DodgesLeftRight, Forward}
    public Transform Target;
    public float WaveSize; 
    //Entity will try to slam into planet
    public bool PlanetSeeking;
    public Vector3 MoveDirection;

    public Vector3 WaveMove(Vector2 curPos, float counter) {
        return curPos = new Vector3(Mathf.Sin(counter) * WaveSize, 0.0f, 0.0f);
    }

    public (Quaternion, Vector3) LookAtPlanet(Transform callingEnemy) {
        Vector3 dir;
        Quaternion newRot;
        var offset = 90f;
        Vector2 facingDirection = Target.position - callingEnemy.position;
        facingDirection.Normalize();
        dir = new Vector3(facingDirection.x, facingDirection.y, 0f);
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        newRot = Quaternion.Euler(Vector3.forward * (angle + offset));
        //callingEnemy.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        return (newRot, dir);
    }
}
