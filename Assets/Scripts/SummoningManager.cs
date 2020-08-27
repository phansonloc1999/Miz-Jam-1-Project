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
            diceManager.StartRollingDice();

            this.currentMaster = master;

            diceManager.getDiceRoller().summoningDiceRollSuccess += OnSummoningDiceRollSuccess;
        }

        private void OnSummoningDiceRollSuccess()
        {
            var resultFace = diceManager.GetFace();

            if (resultFace == Face.One || resultFace == Face.Two || resultFace == Face.Three)
                currentMaster.summonSlaveAt(Random.Range(0, player1Master.SlavesData.Count), 1, 1);

            diceManager.getDiceRoller().summoningDiceRollSuccess -= OnSummoningDiceRollSuccess;

            switchTurn();
        }
    }
}