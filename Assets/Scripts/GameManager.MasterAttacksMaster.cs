using System.Collections;
using DiceSystem;
using UnityEngine;

namespace MyGame
{
    public partial class GameManager
    {
        private int diceRolledTimes = 0;

        [SerializeField] private int player1VictoryTimes = 0;

        [SerializeField] private int player2VictoryTimes = 0;

        private IEnumerator masterAttackMaster()
        {
            if (player1VictoryTimes >= 2)
            {
                Debug.LogWarning("Player1 is victorious");
                resetCounters();
                yield break;
            }
            if (player2VictoryTimes >= 2)
            {
                Debug.LogWarning("Player2 is victorious");
                resetCounters();
                yield break;
            }

            if (diceRolledTimes >= 3)
            {
                if (player1VictoryTimes > player2VictoryTimes)
                    Debug.LogWarning("Player1 is victorious");
                else if (player2VictoryTimes > player1VictoryTimes)
                    Debug.LogWarning("Player2 is victorious");

                resetCounters();
                yield break;
            }

            if (diceRolledTimes != 0)
                yield return new WaitForSeconds(2.0f);

            diceManager1.StartRollingDiceLeft();
            diceManager1.getDiceRoller().diceRolledSuccessfully += OnDiceRolledSuccessfully;

            diceManager2.StartRollingDiceRight();
            diceManager2.getDiceRoller().diceRolledSuccessfully += OnDiceRolledSuccessfully;
        }

        private void OnDiceRolledSuccessfully()
        {
            if (diceManager1.getDiceRoller().isRollingFinished() && diceManager2.getDiceRoller().isRollingFinished())
            {
                if (diceManager1.GetFace() > diceManager2.GetFace())
                    player1VictoryTimes++;
                else if (diceManager2.GetFace() > diceManager1.GetFace())
                    player2VictoryTimes++;

                diceRolledTimes++;
                diceManager1.getDiceRoller().diceRolledSuccessfully -= OnDiceRolledSuccessfully;
                diceManager2.getDiceRoller().diceRolledSuccessfully -= OnDiceRolledSuccessfully;
                StartCoroutine(masterAttackMaster());
            }
        }

        private void resetCounters()
        {
            diceRolledTimes = 0;
            player1VictoryTimes = 0;
            player2VictoryTimes = 0;
        }
    }
}
