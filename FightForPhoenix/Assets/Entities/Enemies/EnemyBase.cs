using UnityEngine;

public class EnemyBase : using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBase", menuName = "FightForPhoenix/EnemyBase", order = 0)]
public class EnemyBase : ScriptableObject {
    public float Speed {set;get;}

    

    void LookAtPlanet() {
        var offset = 90f;
        Vector2 facingDirection = Target.position - transform.position;
        facingDirection.Normalize();
        moveDirection = new Vector3(facingDirection.x, facingDirection.y, 0f);
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;       
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
}

