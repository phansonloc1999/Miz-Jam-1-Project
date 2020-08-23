
using UnityEngine;

public static class MOUSE_BUTTON
{
    public const int PRIMARY = 0;
    public const int SECONDARY = 1;

    public const int MIDDLE = 2;
}

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

    private void changeTurn()
    {
        if (currentPlayerTurn == PLAYER_TURN.PLAYER_1) currentPlayerTurn = PLAYER_TURN.PLAYER_2;
        else currentPlayerTurn = PLAYER_TURN.PLAYER_1;
    }

    private void OnCharacterSelect(GameObject targetCharacter)
    {
        if (prevSelectedCharacter == null)
        {
            prevSelectedCharacter = targetCharacter;
        }
        else if (prevSelectedCharacter != targetCharacter) // Did player select the same character twice?
        {
            if (prevSelectedCharacter.tag == "Slave" && targetCharacter.tag == "Slave")
            {
                slaveTryAttackingAnother(targetCharacter);

                prevSelectedCharacter = null;
            }

            if (prevSelectedCharacter.tag == "Slave" && targetCharacter.tag == "Master")
            {
                // TODO: Insert code here
            }
        }
    }

    private void OnTileSelect(GameObject selectedTile)
    {
        if (prevSelectedCharacter != null && selectedTile.transform.childCount == 0)
        {
            prevSelectedCharacter.GetComponent<CharacterPosition>().moveToTile(selectedTile);

            changeTurn();
            prevSelectedCharacter = null;
        }
    }

    private void addOnCharSelectToSlaves(Master master)
    {
        var summonedSlaves = master.getSummonedSlaves();
        for (int i = 0; i < summonedSlaves.Count; i++)
        {
            summonedSlaves[i].GetComponent<Slave>().selectedChar += OnCharacterSelect;
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
        addOnCharSelectToSlaves(player1Master);
        addOnCharSelectToSlaves(player2Master);
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

            changeTurn();
        }
    }
}