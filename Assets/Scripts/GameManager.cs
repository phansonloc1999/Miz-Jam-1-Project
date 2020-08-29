using UnityEngine;

public static class MOUSE_BUTTON
{
    public const int PRIMARY = 0;
    public const int SECONDARY = 1;

    public const int MIDDLE = 2;
}

namespace MyGame
{

    public partial class GameManager : MonoBehaviour
    {
        private enum PLAYER_TURN
        {
            PLAYER_1, PLAYER_2
        }
        private static PLAYER_TURN currentPlayerTurn = PLAYER_TURN.PLAYER_1;

        [SerializeField] private CharacterTilePositioning player1Positioning, player2Positioning = null;

        [SerializeField] private Master player1Master, player2Master;

        [SerializeField] private Map map;

        [SerializeField] private GameObject prevSelectedCharacter;

        [SerializeField] private DiceManager diceManager1;
        [SerializeField] private DiceManager diceManager2;

        private void Start()
        {
            map.initTiles();

            var player1 = GameObject.Find("Player 1 Master");
            var player2 = GameObject.Find("Player 2 Master");
            player1Master = player1.GetComponent<Master>();
            player2Master = player2.GetComponent<Master>();
            player1Positioning = player1.GetComponent<CharacterTilePositioning>();
            player2Positioning = player2.GetComponent<CharacterTilePositioning>();

            player1Positioning.spawnAtTile(map.getTileAt(0, 0));
            player2Positioning.spawnAtTile(map.getTileAt(2, 2));

            addOnSelectEventHandlers();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentPlayerTurn == PLAYER_TURN.PLAYER_1)
                {
                    trySummoningSlaveWithDice(player1Master);
                }
                else if (currentPlayerTurn == PLAYER_TURN.PLAYER_2)
                {
                    trySummoningSlaveWithDice(player2Master);
                }
            }
        }

        private void switchTurn()
        {
            if (currentPlayerTurn == PLAYER_TURN.PLAYER_1) currentPlayerTurn = PLAYER_TURN.PLAYER_2;
            else currentPlayerTurn = PLAYER_TURN.PLAYER_1;

            Debug.Log("Turn switched");
        }

        private void OnCharacterSelect(GameObject targetCharacter)
        {
            if (prevSelectedCharacter == null)
            {
                selectPrevCharacterProcessing(targetCharacter);

                if (prevSelectedCharacter != null)
                {
                    map.flashPossibleMoveTiles(prevSelectedCharacter);
                }
            }
            else if (prevSelectedCharacter != targetCharacter) // Did player select the same character twice?
            {
                slaveAttackProcessing(targetCharacter);

                map.stopFlashingTiles();

                prevSelectedCharacter = null;
            }
        }

        private void OnTileSelect(GameObject selectedTile)
        {
            if (prevSelectedCharacter != null && selectedTile.transform.childCount == 0)
            {
                prevSelectedCharacter.GetComponent<CharacterTilePositioning>().moveToTile(selectedTile);

                map.stopFlashingTiles();

                switchTurn();
                prevSelectedCharacter = null;
            }
        }

        private void addOnSelectEventHandlers()
        {
            player2Master.selectedChar += OnCharacterSelect;
            player1Master.selectedChar += OnCharacterSelect;

            for (int row = 0; row < map.NUM_OF_TILE_ROW; row++)
            {
                for (int column = 0; column < map.NUM_OF_TILE_COLUMN; column++)
                {
                    map.getTileAt(row, column).GetComponent<Tile>().selectedTile += OnTileSelect;
                }
            }
        }

        private void slaveTryAttackingAnother(GameObject targetCharacter)
        {
            var prevSelectedSlave = prevSelectedCharacter.GetComponent<Slave>();
            var masterOfPrevSelectedChar = prevSelectedSlave.getMaster();
            var masterOfSelectedChar = targetCharacter.GetComponent<Slave>().getMaster();

            // If 2 slaves aren't from the same master and prev slave can attack because target is within its attack range
            if (masterOfPrevSelectedChar != masterOfSelectedChar && prevSelectedSlave.canAttackAtTile(targetCharacter.transform.parent.gameObject))
            {
                var ammountOfDamage = prevSelectedCharacter.GetComponent<Slave>().getAttackDamage();
                targetCharacter.GetComponent<Health>().takeDamage(ammountOfDamage);

                var message = prevSelectedCharacter.name + " attacked " + targetCharacter + " for " + ammountOfDamage.ToString() + " damage";
                Debug.Log(message);

                switchTurn();
                map.stopFlashingTiles();
            }
        }

        /// <summary>
        /// Add OnCharacterSelect when new slave is summoned
        /// </summary>
        public void OnSummoningSlave(GameObject newSlave)
        {
            newSlave.GetComponent<Slave>().selectedChar += OnCharacterSelect;
        }

        private void slaveAttackProcessing(GameObject targetCharacter)
        {
            // Slave attacks slave?
            if (prevSelectedCharacter.tag == "Slave" && targetCharacter.tag == "Slave")
            {
                slaveTryAttackingAnother(targetCharacter);
            }

            // Slave attacks master?
            else if (prevSelectedCharacter.tag == "Slave" && targetCharacter.tag == "Master")
            {
                var prevSelectedSlave = prevSelectedCharacter.GetComponent<Slave>();

                // Slave attacks enemy master and the target is within its attack range?
                if (prevSelectedSlave.getMaster() != targetCharacter && prevSelectedSlave.canAttackAtTile(targetCharacter.transform.parent.gameObject))
                {
                    targetCharacter.GetComponent<Health>().takeDamage(prevSelectedSlave.getAttackDamage());
                }
            }

            // Master attacks master?
            else if (prevSelectedCharacter.tag == "Master" && targetCharacter.tag == "Master")
            {
                StartCoroutine(masterAttackMaster());
            }
        }

        private void selectPrevCharacterProcessing(GameObject targetCharacter)
        {
            if (targetCharacter.tag == "Slave")
            {
                var targetCharacterMaster = targetCharacter.GetComponent<Slave>().getMaster();
                if (
                        (targetCharacterMaster == player1Master.gameObject && currentPlayerTurn == PLAYER_TURN.PLAYER_1)
                        || (targetCharacterMaster == player2Master.gameObject && currentPlayerTurn == PLAYER_TURN.PLAYER_2)
                   )
                    prevSelectedCharacter = targetCharacter;
            }

            else if (targetCharacter.tag == "Master")
            {
                if (
                    (targetCharacter.name == "Player 1 Master" && currentPlayerTurn == PLAYER_TURN.PLAYER_1)
                        || (targetCharacter.name == "Player 2 Master" && currentPlayerTurn == PLAYER_TURN.PLAYER_2)
                    )
                    prevSelectedCharacter = targetCharacter;
            }
        }
    }
}