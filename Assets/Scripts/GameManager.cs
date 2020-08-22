using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum PLAYER_TURN
    {
        PLAYER_1, PLAYER_2
    }
    private static PLAYER_TURN currentPlayerTurn = PLAYER_TURN.PLAYER_1;

    [SerializeField]
    private Master player1Master, player2Master = null;

    [SerializeField]
    private Map map;

    private void Start()
    {
        map.initTiles();

        player1Master.moveToTile(map.getTile(0, 0));
        player2Master.moveToTile(map.getTile(1, 1));
    }

    private static void changeTurn()
    {
        if (currentPlayerTurn == PLAYER_TURN.PLAYER_1) currentPlayerTurn = PLAYER_TURN.PLAYER_2;
        else currentPlayerTurn = PLAYER_TURN.PLAYER_1;
    }

}