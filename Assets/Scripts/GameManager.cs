using UnityEngine;

public static class MOUSE_BUTTON
{
    public const int PRIMARY = 0;
    public const int SECONDARY = 1;

    public const int MIDDLE = 2;
}

namespace MyGame
{

    public class GameManager : MonoBehaviour
    {
        private enum PLAYER_TURN
        {
            PLAYER_1, PLAYER_2
        }
        private static PLAYER_TURN currentPlayerTurn = PLAYER_TURN.PLAYER_1;

        [SerializeField] private CharacterTilePositioning player1Movement, player2Movement = null;

        [SerializeField] private Master player1Master, player2Master;

        [SerializeField] private Map map;

        [SerializeField] private GameObject prevSelectedCharacter;

        private void Start()
        {
            map.initTiles();

            var player1 = GameObject.Find("Player 1 Master");
            var player2 = GameObject.Find("Player 2 Master");
            player1Master = player1.GetComponent<Master>();
            player2Master = player2.GetComponent<Master>();
            player1Movement = player1.GetComponent<CharacterTilePositioning>();
            player2Movement = player2.GetComponent<CharacterTilePositioning>();

            player1Movement.spawnAtTile(map.getTileAt(0, 0));
            player2Movement.spawnAtTile(map.getTileAt(2, 2));

            addOnSelectEventHandlers();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentPlayerTurn == PLAYER_TURN.PLAYER_1)
                {
                    player1Master.summonSlaveAt(Random.Range(0, player1Master.SlavePrefabSet.Count), 1, 1);
                }
                else if (currentPlayerTurn == PLAYER_TURN.PLAYER_2)
                {
                    player2Master.summonSlaveAt(Random.Range(0, player2Master.SlavePrefabSet.Count), 1, 1);
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
            }
            else if (prevSelectedCharacter != targetCharacter) // Did player select the same character twice?
            {
                slaveAttackProcessing(targetCharacter);

                prevSelectedCharacter = null;
            }
        }

        private void OnTileSelect(GameObject selectedTile)
        {
            if (prevSelectedCharacter != null && selectedTile.transform.childCount == 0)
            {
                prevSelectedCharacter.GetComponent<CharacterTilePositioning>().moveToTile(selectedTile);

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
            var prevSlave = prevSelectedCharacter.GetComponent<Slave>();
            var prevSelectedCharMaster = prevSlave.getMaster();
            var selectedCharMaster = targetCharacter.GetComponent<Slave>().getMaster();

            // Only deal damage if 2 slaves aren't from the same master and prev slave can attack because target is within its attack range
            if (prevSelectedCharMaster != selectedCharMaster && prevSlave.canAttackAtTile(targetCharacter.transform.parent.gameObject))
            {
                var ammountOfDamage = prevSelectedCharacter.GetComponent<Slave>().getAttackDamage();
                targetCharacter.GetComponent<Health>().takeDamage(ammountOfDamage);

                var message = prevSelectedCharacter.name + " attacked " + targetCharacter + " for " + ammountOfDamage.ToString() + " damage";
                Debug.Log(message);

                switchTurn();
            }
        }

        /// <summary>
        /// Add OnCharacterSelect when new slave is summoned
        /// </summary>
        public void OnSummoningSlave(GameObject newSlave)
        {
            newSlave.GetComponent<Slave>().selectedChar += OnCharacterSelect;

            switchTurn();
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
                // TODO: Insert code here
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