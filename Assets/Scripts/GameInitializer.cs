using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using Photon.Pun;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{
    [Header("Game mode dependent objects")]

    [SerializeField] private SingleplayerChessGameController singleplayerControllerPrefab;
    [SerializeField] private MultiplayerChessGameController multiplayerControllerPrefab;
    [SerializeField] private MultiplayerBoard multiplayerBoardPrefab;
    [SerializeField] private SinglePlayerBoard singleplayerBoardPrefab;

    [Header("Scene references")]
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private CameraSetup cameraSetup;
    [SerializeField] private ChessUIManager uiManager;
    [SerializeField] private Transform boardAnchor;
    [SerializeField] private Transform plane;
    [SerializeField] public Text WPawnCounter;
    [SerializeField] public Text WBishopCounter;
    [SerializeField] public Text WKnightCounter;
    [SerializeField] public Text WRookCounter;
    [SerializeField] public Text WQueenCounter;
    [SerializeField] public Text BPawnCounter;
    [SerializeField] public Text BBishopCounter;
    [SerializeField] public Text BKnightCounter;
    [SerializeField] public Text BRookCounter;
    [SerializeField] public Text BQueenCounter;

    public void CreateMultiplayerBoard()
    {
        if (!networkManager.IsRoomFull())
            PhotonNetwork.Instantiate(multiplayerBoardPrefab.name, boardAnchor.position, boardAnchor.rotation);
    }

    public void CreateSinglePlayerBoard()
    {
        Instantiate(singleplayerBoardPrefab, boardAnchor);
    }

    public void InitializeMultiplayerController()
    {
        MultiplayerBoard board = FindObjectOfType<MultiplayerBoard>();
        MultiplayerChessGameController controller = Instantiate(multiplayerControllerPrefab);
        controller.SetDependencies(cameraSetup, uiManager, board, plane);
        controller.InitializeGame();
        controller.SetNetworkManager(networkManager);
        networkManager.SetDependencies(controller);
        board.SetDependencies(controller, plane, WPawnCounter, WBishopCounter, WKnightCounter, WRookCounter, WQueenCounter, BPawnCounter, BBishopCounter, BKnightCounter, BRookCounter, BQueenCounter);
        board.SetDependencies2(plane);
    }

    public void InitializeSingleplayerController()
    {
        SinglePlayerBoard board = FindObjectOfType<SinglePlayerBoard>();
        SingleplayerChessGameController controller = Instantiate(singleplayerControllerPrefab);
        controller.SetDependencies(cameraSetup, uiManager, board, plane);
        controller.InitializeGame();
        board.SetDependencies(controller, plane, WPawnCounter, WBishopCounter, WKnightCounter, WRookCounter, WQueenCounter, BPawnCounter, BBishopCounter, BKnightCounter, BRookCounter, BQueenCounter);
        controller.StartNewGame();
    }
}
