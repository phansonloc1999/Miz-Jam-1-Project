using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DiceSystem
{
    public enum Face
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6
    }

    public class Dice : MonoBehaviour
    {
        [SerializeField] Transform _faceOnePos;
        [SerializeField] Transform _faceTwoPos;
        [SerializeField] Transform _faceThreePos;      

        public Face GetFaceUp()
        {
            Vector3 faceOneNorm = _faceOnePos.position - gameObject.transform.position;
            float value = Vector3.Dot(faceOneNorm.normalized, Vector3.up);
            if (Mathf.Approximately(value, 1f))
            {
                return Face.One;
            }
            else if(Mathf.Approximately(value, -1f))
            {
                return Face.Six;
            }

            Vector3 faceTwoNorm = _faceTwoPos.position - gameObject.transform.position;
            value = Vector3.Dot(faceTwoNorm.normalized, Vector3.up);
            if (Mathf.Approximately(value, 1f))
            {
                return Face.Two;
            }
            else if (Mathf.Approximately(value, -1f))
            {
                return Face.Five;
            }

            Vector3 faceThreeNorm = _faceThreePos.position - gameObject.transform.position;
            value = Vector3.Dot(faceThreeNorm.normalized, Vector3.up);
            if (Mathf.Approximately(value, 1f))
            {
                return Face.Three;
            }
            else if (Mathf.Approximately(value, -1f))
            {
                return Face.Four;
            }
            return Face.None;
        }
    }
}