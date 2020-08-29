using UnityEngine;
using DiceSystem;
using System.Collections;

namespace MyGame
{

    public partial class GameManager
    {
        private Master currentMaster;

        private void trySummoningSlaveWithDice(Master master)
        {
            ignoreUserInput = true;

            diceManager1.StartRollingDiceLeft();

            this.currentMaster = master;

            diceManager1.getDiceRoller().diceRolledSuccessfully += OnSummoningDiceRollSuccess;
        }

        private void OnSummoningDiceRollSuccess()
        {
            var resultFace = diceManager1.GetFace();

            if (resultFace == Face.One || resultFace == Face.Two || resultFace == Face.Three)
                currentMaster.summonSlaveAt(Random.Range(0, player1Master.SlavesData.Count), 1, 1);

            diceManager1.getDiceRoller().diceRolledSuccessfully -= OnSummoningDiceRollSuccess;

            switchTurn();

            StartCoroutine(turnOffDices());
        }

        private IEnumerator turnOffDices()
        {
            yield return new WaitForSeconds(2.0f);

            diceManager1.turnOffDice();
            diceManager2.turnOffDice();

            ignoreUserInput = false;
        }
    }
}