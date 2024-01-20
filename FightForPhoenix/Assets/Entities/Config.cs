using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/Config", order = 1)]
public class Config : ScriptableObject
{
    // Game
    public int TimeUntilNextPhase = 60;

    // Planet
    public int MaxNumberOfPlanetHealth = 10;

    // Player
    public int Speed = 100;
    public int PowerSpeed = 20;

    //Enemy baseline
    enum EnemyType { DodgesLeftRight, Forward}
    public float WaveSize; 
    //Entity will try to slam into planet
    public bool PlanetSeeking;

    public Vector3 WaveMove(Vector3 direction, Vector2 curPos, float counter, bool sine) {
        return curPos = Mathf.Sin(counter) * WaveSize * direction;
    }

    public (Quaternion, Vector3) LookAtPlanet(Transform callingEntity, Transform entityTarget) {
        Vector3 dir;
        Quaternion newRot;
        var offset = 90f;
        Vector2 facingDirection = Target.position - callingEnemy.position;
        facingDirection.Normalize();
        dir = new Vector3(facingDirection.x, facingDirection.y, 0f);
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        newRot = Quaternion.Euler(Vector3.forward * (angle + offset));
        return (newRot, dir);
    }
}
