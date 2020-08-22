using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceSystem;
using Face = DiceSystem.Face;
public class DiceManager : MonoBehaviour
{
    [SerializeField] private GameObject _dice;
    [SerializeField] private GameObject _dicePrefab;
    private DiceRoller _diceRoller;
    private Face _face;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if(_dice == null)
        {
            _dice = Instantiate(_dicePrefab);
        }
        _diceRoller = _dice.GetComponent<DiceRoller>(); _diceRoller.TurnOff();
        _diceRoller.TurnOff();
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

    public Face GetFace()
    {
        return _face;
    }

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
