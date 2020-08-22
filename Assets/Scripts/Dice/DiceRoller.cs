using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DiceSystem
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Dice))]

    public class DiceRoller : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Dice _diceComponent;
        private bool _hasLanded;
        private bool _thown;
        private Face _face;

        public event Action<Face> OnDiceRollSuccessful;

        private void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _diceComponent = gameObject.GetComponent<Dice>();
            _rigidbody.useGravity = false;
            _thown = false;
            _hasLanded = false;
        }

        #region public Methods
        public void Setup(Vector3 pos)
        {
            _rigidbody.useGravity = false;
            gameObject.SetActive(true);
            gameObject.transform.position = pos;
            _thown = false;
            _hasLanded = false;
        }

        public void TurnOff()
        {
            Debug.Log("turn off");
            gameObject.SetActive(false);
            _thown = false;
            _hasLanded = false;
            _rigidbody.useGravity = false;
        }

        public void TossDice()
        {
            if(!_thown && !_hasLanded)
            {
                _thown = true;
                _rigidbody.useGravity = true;
                _rigidbody.AddTorque(Random.Range(0, 400), Random.Range(0, 400), Random.Range(0, 400));
                StartCoroutine(CheckDice());
            }
        }

        IEnumerator CheckDice()
        {
            while(_thown && !_hasLanded)
            {                
                if (_rigidbody.IsSleeping() && _thown && !_hasLanded)
                {
                    _hasLanded = true;
                    _rigidbody.useGravity = false;

                    Face face = _diceComponent.GetFaceUp();
                    if (face != Face.None)
                    {
                        OnDiceRollSuccessful?.Invoke(face);
                    }
                    yield break;
                }
                yield return null;
            }
        }
        #endregion

        
    }
}