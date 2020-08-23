using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slave : MonoBehaviour
{
    [SerializeField] private float ATTACK_DAMAGE;

    [SerializeField] private float ATTACK_RANGE;

    public float getAttackDamage()
    {
        return ATTACK_DAMAGE;
    }

    public delegate void SelectCharacterHandler(GameObject selectedChar);
    public event SelectCharacterHandler selectedChar;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(MOUSE_BUTTON.PRIMARY))
        {
            selectedChar?.Invoke(this.gameObject);
        }
    }
}
