using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
    enum Phase
    {
        Player1Choice = 1,
        Player2Choice = 2
    }
    public class UINewGame : MonoBehaviour
    {
        [SerializeField] Image _labelObj;
        [SerializeField] GameObject _uiMapChoiceArrowLeft;
        [SerializeField] GameObject _uiMapChoiceArrowRight;

        [SerializeField] Sprite _labelPlayer1;
        [SerializeField] Sprite _labelPlayer2;

        //[SerializeField] Sprite _playerIcon;

        //Save Player Choice and Map

        private Phase _phase;

        //private Dictionary<Master, bool> _masterList = null; Change to Master Info Later
        

        private void OnEnable()
        {
            _phase = Phase.Player1Choice;
            _labelObj.sprite = _labelPlayer1;
            EnableUIArrow(true);
            //load from Scripable Object
        }

        private void RefreshView()
        {
            //Change Sprite by choosen master
            //Change text by choosen Master
        }

        private void EnableUIArrow(bool isEnable)
        {
            _uiMapChoiceArrowLeft.SetActive(isEnable);
            _uiMapChoiceArrowRight.SetActive(isEnable);
        }

        public void OnButtonDeclineClick()
        {
            switch (_phase)
            {
                case Phase.Player1Choice:
                    //back to main menu
                    break;
                case Phase.Player2Choice:
                    //Change UI
                    _labelObj.sprite = _labelPlayer1;
                    _phase = Phase.Player1Choice;
                    EnableUIArrow(true);

                    //Submit Info
                    //TODO: Add Choosen Master to List
                    break;
            }            
        }

        public void OnButtonAcceptClick()
        {
            switch (_phase)
            {
                case Phase.Player1Choice:
                    _labelObj.sprite = _labelPlayer2;
                    _phase = Phase.Player2Choice;
                    EnableUIArrow(false);

                    //TODO: Remove Choosen Master from List

                    break;
                case Phase.Player2Choice:
                    //Init game
                    break;
            }
        }

        public void OnButtonNextMapClick()
        {
            
        }

        public void OnButtonPrevMapClick()
        {

        }

        public void OnButtonNextMasterClick()
        {

        }

        public void OnButtonPrevMasterClick()
        {

        }
    }
}
