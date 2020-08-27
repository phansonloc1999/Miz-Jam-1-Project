using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceSystem;
using Face = DiceSystem.Face;
using System;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private GameObject _dice;
    [SerializeField] private GameObject _dicePrefab;

    private DiceRoller _diceRoller;
    [SerializeField] private Face _face;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_dice == null)
        {
            _dice = Instantiate(_dicePrefab);
        }
        _diceRoller = _dice.GetComponent<DiceRoller>(); _diceRoller.TurnOff();
        _diceRoller.TurnOff();
        _diceRoller.OnDiceRollSuccessful += SetFace;
    }

    public void SetFace(Face face)
    {
        if (face == Face.None)
        {
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
    }
    #endregion

    public void StartRollingDiceLeft()
    {
        _diceRoller.Setup(new Vector3(-5, 0, 8));
        _diceRoller.TossDice();
    }

    public void StartRollingDiceRight()
    {
        _diceRoller.Setup(new Vector3(5, 0, 8));
        _diceRoller.TossDice();
    }

    public DiceRoller getDiceRoller()
    {
        return _diceRoller;
    }
}
