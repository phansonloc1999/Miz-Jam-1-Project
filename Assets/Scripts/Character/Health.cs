using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float MAX_HEALTH;

    [SerializeField] private float health;

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

            Debug.LogError(name + "is dead!!");

            Destroy(gameObject);
        }
    }

    public void loadMaxHealth(float maxHealth)
    {
        MAX_HEALTH = maxHealth;
        health = MAX_HEALTH;
    }
}