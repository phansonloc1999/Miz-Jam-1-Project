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
        private bool _thrown;
        private bool _finishedRolling;
        private Face _face;

        public event Action<Face> OnDiceRollSuccessful;
        public delegate void DiceRollSuccessHandler();
        public event DiceRollSuccessHandler diceRolledSuccessfully;

        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _diceComponent = gameObject.GetComponent<Dice>();
            _rigidbody.useGravity = false;
            _thrown = false;
            _hasLanded = false;
        }

        #region public Methods
        public void Setup(Vector3 pos)
        {
            _rigidbody.useGravity = false;
            gameObject.SetActive(true);
            gameObject.transform.position = pos;
            _thrown = false;
            _hasLanded = false;
            _finishedRolling = false;

            //reset dice config
            _rigidbody.velocity = Vector3.zero;
            gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        public void TurnOff()
        {
            gameObject.SetActive(false);
            _thrown = false;
            _hasLanded = false;
            _rigidbody.useGravity = false;
        }

        public void TossDice()
        {
            gameObject.SetActive(true);
            if (!_thrown && !_hasLanded)
            {
                _thrown = true;
                _rigidbody.useGravity = true;
                _rigidbody.AddTorque(Random.Range(10, 500), Random.Range(10, 500), Random.Range(10, 500));
                StartCoroutine(CheckDice());
            }
        }

        IEnumerator CheckDice()
        {
            while (_thrown && !_hasLanded)
            {
                if (_rigidbody.IsSleeping() && _thrown && !_hasLanded)
                {
                    _hasLanded = true;
                    _rigidbody.useGravity = false;

                    Face face = _diceComponent.GetFaceUp();
                    if (face != Face.None)
                    {
                        _finishedRolling = true;
                        OnDiceRollSuccessful?.Invoke(face);
                        diceRolledSuccessfully?.Invoke();
                        Debug.Log("Called");
                    }
                    yield break;
                }
                yield return null;
            }
        }

        public bool isRollingFinished()
        {
            return _finishedRolling;
        }
        #endregion


    }
}