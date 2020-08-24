using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float MAX_HEALTH;

    [SerializeField] private float health;

    private void Start()
    {
        health = MAX_HEALTH;
    }

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