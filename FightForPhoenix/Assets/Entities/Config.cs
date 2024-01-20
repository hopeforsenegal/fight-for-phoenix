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


    public Vector2 GetDirectionVector2(Transform callingEntity, Transform entityTarget) {
        Vector2 facingDirection = Target.position - callingEnemy.position;
        return facingDirection.Normalize();
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
