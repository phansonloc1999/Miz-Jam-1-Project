using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceSystem;
using Face = DiceSystem.Face;
public class DiceManager : MonoBehaviour
{
    [SerializeField] private GameObject _dice;
    private DiceRoller _diceRoller;
    public Face _face;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if(_dice == null)
        {
            return;
        }
        _diceRoller = _dice.GetComponent<DiceRoller>();
        _diceRoller.OnDiceRollSuccessful += SetFace;
    }

    public void SetFace(Face face)
    {
        if (face == Face.None) {
            return;
        }
        _face = face;
        Debug.Log(_face);
    }

    // Update is called once per frame
    #region Test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _diceRoller.Setup(new Vector3(-2, 5, 0));
            _diceRoller.TossDice();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _diceRoller.Setup(new Vector3(2, 5, 0));
            _diceRoller.TossDice();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _diceRoller.TurnOff();
        }
    }
    #endregion
}
