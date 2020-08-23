using UnityEngine;

public class Health : MonoBehaviour
{
    private const float MAX_HEALTH = 0;

    [SerializeField] private float health = MAX_HEALTH;

    public void takeDamage(float ammount)
    {
        health -= ammount;

        die();
    }

    public delegate void DeathHandler(GameObject deadSlave);
    public event DeathHandler dead;
    public void die()
    {
        if (health <= 0)
        {
            dead?.Invoke(gameObject);

            Destroy(gameObject);
        }
    }
}