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

        [SerializeField] private CharacterPosition player1Movement, player2Movement = null;

        [SerializeField] private Master player1Master, player2Master;

        [SerializeField] private Map map;

        [SerializeField] private GameObject prevSelectedCharacter;

        private void Start()
        {
            map.initTiles();

            // TEST CODE
            var player1 = GameObject.Find("Player 1 Master");
            var player2 = GameObject.Find("Player 2 Master");
            player1Master = player1.GetComponent<Master>();
            player2Master = player2.GetComponent<Master>();
            player1Movement = player1.GetComponent<CharacterPosition>();
            player2Movement = player2.GetComponent<CharacterPosition>();

            player1Movement.spawnAtTile(map.getTileAt(0, 0));
            player2Movement.spawnAtTile(map.getTileAt(2, 2));

            player1Movement.moveToTile(map.getTileAt(1, 1));
            player1Master.summonSlaveAt(0, 3, 3);
            player2Master.summonSlaveAt(0, 3, 4);

            addOnEventHandlers();
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
                if (targetCharacter.tag == "Slave")
                {
                    var targetMaster = targetCharacter.GetComponent<Slave>().getMaster();
                    if (
                            (targetMaster == player1Master.gameObject && currentPlayerTurn == PLAYER_TURN.PLAYER_1)
                            || (targetMaster == player2Master.gameObject && currentPlayerTurn == PLAYER_TURN.PLAYER_2)
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
            else if (prevSelectedCharacter != targetCharacter) // Did player select the same character twice?
            {
                if (prevSelectedCharacter.tag == "Slave" && targetCharacter.tag == "Slave")
                {
                    slaveTryAttackingAnother(targetCharacter);
                }
                else if (prevSelectedCharacter.tag == "Slave" && targetCharacter.tag == "Master")
                {
                    var prevSelectedSlave = prevSelectedCharacter.GetComponent<Slave>();
                    if (prevSelectedSlave.getMaster() != targetCharacter) // Did slave attack enemy master?
                    {
                        targetCharacter.GetComponent<Health>().takeDamage(prevSelectedSlave.getAttackDamage());
                    }
                }

                prevSelectedCharacter = null;
            }
        }

        private void OnTileSelect(GameObject selectedTile)
        {
            if (prevSelectedCharacter != null && selectedTile.transform.childCount == 0)
            {
                prevSelectedCharacter.GetComponent<CharacterPosition>().moveToTile(selectedTile);

                switchTurn();
                prevSelectedCharacter = null;
            }
        }

        private void addOnEventHandlers()
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
            var prevSelectedCharMaster = prevSelectedCharacter.GetComponent<Slave>().getMaster();
            var selectedCharMaster = targetCharacter.GetComponent<Slave>().getMaster();

            // Only deal damage if 2 slaves aren't from the same master
            if (prevSelectedCharMaster != selectedCharMaster)
            {
                var damage = prevSelectedCharacter.GetComponent<Slave>().getAttackDamage();
                targetCharacter.GetComponent<Health>().takeDamage(damage);

                var message = prevSelectedCharacter.name + " attacked " + targetCharacter + " for " + damage.ToString() + " damage";
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
        }
    }
}