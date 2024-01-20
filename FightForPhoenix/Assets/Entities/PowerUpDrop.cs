using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerUpDrop : MonoBehaviour
{
    private GameManager m_GameManager;

    void Awake()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var hasHitPlayer = other.GetComponent<Player>() != null;
        var hasHitPlanet = other.GetComponent<Tilemap>() != null;
        if (hasHitPlanet || hasHitPlayer){
            Debug.Log($"Power up destroyed!");
            Destroy(gameObject);
            if (hasHitPlayer) {
                m_GameManager.ObtainedSuperSpeed();
            }
        }       
    }
}