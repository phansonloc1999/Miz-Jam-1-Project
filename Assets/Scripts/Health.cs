using UnityEngine;

public class Health : MonoBehaviour
{
    private const float MAX_HEALTH = 0;

    [SerializeField] private float health = MAX_HEALTH;

    public void takeDamage(float ammount)
    {
        health -= ammount;

        deathCheck();
    }

    public void deathCheck()
    {
        if (health <= 0) Destroy(gameObject);
    }
}