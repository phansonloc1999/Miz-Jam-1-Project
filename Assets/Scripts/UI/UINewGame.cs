using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    enum Phase
    {
        Player1Choice = 1,
        Player2Choice = 2
    }
    public class UINewGame : MonoBehaviour
    {
        [SerializeField] UIMainMenu _mainMenu;

        [SerializeField] Image _imgLabel;
        [SerializeField] Image _imgMaster;
        [SerializeField] TextMeshProUGUI _tmpName;
        [SerializeField] GameObject _uiMapChoiceArrowLeft;
        [SerializeField] GameObject _uiMapChoiceArrowRight;

        [SerializeField] Sprite _labelPlayer1;
        [SerializeField] Sprite _labelPlayer2;

        [SerializeField] MasterListData masterListData;

        //For Choosing phase        
        private Phase _phase;
        List<MasterData> _masterList = null;        
        private int _currentIndex;

        private MasterData _player1Choice;
        private MasterData _player2Choice;
        //TODO: private MapData 

        private void Awake()
        {
            _masterList = new List<MasterData>();
            
            foreach(var masterData in masterListData.lstMasterData)
            {
                _masterList.Add(masterData);
            }
            ResetView();
        }

        private void OnDisable()
        {
            ResetView();
        }
        #region private methods
        private void ResetView()
        {
            _phase = Phase.Player1Choice;
            _imgLabel.sprite = _labelPlayer1;
            EnableUIArrow(true);

            _player1Choice = _player2Choice = null;
            _currentIndex = 0;
            ChangeUI();
        }

        private void EnableUIArrow(bool isEnable)
        {
            _uiMapChoiceArrowLeft.SetActive(isEnable);
            _uiMapChoiceArrowRight.SetActive(isEnable);
        }

        private void SetNextMasterIndex()
        {
            int Count = _masterList.Count;
            if(_currentIndex == Count - 1)
            {
                _currentIndex = 0;
            }
            else
            {
                _currentIndex++;
            }
        }

        private void SetPrevMasterIndex()
        {
            int Count = _masterList.Count;
            if (_currentIndex == 0)
            {
                _currentIndex = Count - 1;
            }
            else
            {
                _currentIndex--;
            }
        }

        private void ChooseMaster(ref MasterData masterChoice)
        {
            masterChoice = _masterList[_currentIndex];
            _masterList.Remove(masterChoice);
            _currentIndex = 0;
        }

        private void ChangeUI()
        {
            _imgMaster.sprite = _masterList[_currentIndex].masterSprite;
            _tmpName.text = _masterList[_currentIndex].masterName;
        }
        #endregion


        #region public methods
        public void OnButtonDeclineClick()
        {
            switch (_phase)
            {
                case Phase.Player1Choice:
                    _mainMenu.RefreshView();
                    break;
                case Phase.Player2Choice:
                    _imgLabel.sprite = _labelPlayer1;
                    _phase = Phase.Player1Choice;
                    EnableUIArrow(true);

                    _masterList.Add(_player1Choice);
                    _player1Choice = null;
                    ChangeUI();
                    break;
            }            
        }

        public void OnButtonAcceptClick()
        {
            switch (_phase)
            {
                case Phase.Player1Choice:
                    _imgLabel.sprite = _labelPlayer2;
                    _phase = Phase.Player2Choice;
                    EnableUIArrow(false);

                    ChooseMaster(ref _player1Choice);
                    ChangeUI();
                    break;
                case Phase.Player2Choice:
                    ChooseMaster(ref _player2Choice);

                    //Init game
                    Debug.Log(_player1Choice.masterName);
                    Debug.Log(_player2Choice.masterName);
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
            SetNextMasterIndex();
            ChangeUI();
        }

        public void OnButtonPrevMasterClick()
        {
            SetPrevMasterIndex();
            ChangeUI();
        }
        #endregion
    }
}
