using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SquareSelectorCreator))]
[RequireComponent(typeof(IObjectTweener))]
public abstract class Board : MonoBehaviour
{
    public const int BOARD_SIZE = 8;
    public const float s = 1.5f;

    [SerializeField] private Transform bottomLeftSquareTransform;
    [SerializeField] private float squareSize;

    private Text WPawnCounter;
    private Text WBishopCounter;
    private Text WKnightCounter;
    private Text WRookCounter;
    private Text WQueenCounter;

    private Text BPawnCounter;
    private Text BBishopCounter;
    private Text BKnightCounter;
    private Text BRookCounter;
    private Text BQueenCounter;

    private int wpc = 0;
    private int wbc = 0;
    private int wkc = 0;
    private int wrc = 0;
    private int wqc = 0;

    private int bpc = 0;
    private int bbc = 0;
    private int bkc = 0;
    private int brc = 0;
    private int bqc = 0;

    private int turn = 1;

    private Piece[,] grid;
    private Piece selectedPiece;
    private ChessGameController chessController;
    private SquareSelectorCreator squareSelector;
    private IObjectTweener tweener;

    private Transform planeStage;

    protected virtual void Awake()
    {
        squareSelector = GetComponent<SquareSelectorCreator>();
        CreateGrid();
    }

    public void SetDependencies(ChessGameController chessController, Transform plane, Text WPawnCounter, Text WBishopCounter, Text WKnightCounter, Text WRookCounter, Text WQueenCounter,
        Text BPawnCounter, Text BBishopCounter, Text BKnightCounter, Text BRookCounter, Text BQueenCounter)
    {
        this.chessController = chessController;
        this.planeStage = plane;

        this.WPawnCounter = WPawnCounter;
        this.WBishopCounter = WBishopCounter;
        this.WKnightCounter = WKnightCounter;
        this.WRookCounter = WRookCounter;
        this.WQueenCounter = WQueenCounter;

        this.BPawnCounter = BPawnCounter;
        this.BBishopCounter = BBishopCounter;
        this.BKnightCounter = BKnightCounter;
        this.BRookCounter = BRookCounter;
        this.BQueenCounter = BQueenCounter;
    }

    private void CreateGrid()
    {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
    }

    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
    }

    internal void OnSetSelectedPiece(Vector2Int coords)
    {

        Piece piece = GetPieceOnSquare(coords);
        selectedPiece = piece;
    }

    internal void OnSelectedPieceMoved(Vector2Int intCoords)
    {

        Animate(selectedPiece, intCoords, GetPieceOnSquare(intCoords));
        TryToTakeOppositePiece(intCoords);
        UpdateBoardOnPieceMove(intCoords, selectedPiece.occupiedSquare, selectedPiece, null);
        selectedPiece.MovePiece(intCoords);
        DeselectPiece();
        EndTurn();
    }

    public Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x / s) + BOARD_SIZE / 2;
        int y = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).z / s) + BOARD_SIZE / 2;
        //Debug.Log(transform.InverseTransformPoint(inputPosition).x);
        //Debug.Log(transform.InverseTransformPoint(inputPosition).z);
        return new Vector2Int(x, y);
    }

    public void OnSquareSelected(Vector3 inputPosition)
    {

        if (!chessController.CanPerformMove())
            return;

        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        Piece piece = GetPieceOnSquare(coords);
        if (selectedPiece)
        {
            if (piece != null && selectedPiece == piece)
                DeselectPiece();
            else if (piece != null && selectedPiece != piece && chessController.IsTeamTurnActive(piece.team))
                SelectPiece(coords);
            else if (selectedPiece.CanMoveTo(coords))
                SelectedPieceMoved(coords);
        }
        else
        {
            if (piece != null && chessController.IsTeamTurnActive(piece.team))
                SelectPiece(coords);
        }
    }

    public abstract void SelectedPieceMoved(Vector2 coords);
    public abstract void SetSelectedPiece(Vector2 coords);



    private void SelectPiece(Vector2Int coords)
    {

        Piece piece = GetPieceOnSquare(coords);
        chessController.RemoveMovesEnablingAttakOnPieceOfType<King>(piece);
        SetSelectedPiece(coords);
        List<Vector2Int> selection = selectedPiece.avaliableMoves;
        ShowSelectionSquares(selection);
    }


    private void ShowSelectionSquares(List<Vector2Int> selection)
    {
        Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();
        for (int i = 0; i < selection.Count; i++)
        {
            Vector3 position = CalculatePositionFromCoords(selection[i]);
            bool isSquareFree = GetPieceOnSquare(selection[i]) == null;
            squaresData.Add(position, isSquareFree);
        }
        squareSelector.ShowSelection(squaresData, planeStage);
    }

    private void DeselectPiece()
    {
        selectedPiece = null;
        squareSelector.ClearSelection();
    }



    private void EndTurn()
    {
        turn = turn * -1;
        chessController.EndTurn();
    }

    public void UpdateBoardOnPieceMove(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
    {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = newPiece;
    }

    public Piece GetPieceOnSquare(Vector2Int coords)
    {
        if (CheckIfCoordinatesAreOnBoard(coords))
            return grid[coords.x, coords.y];
        return null;
    }

    public bool CheckIfCoordinatesAreOnBoard(Vector2Int coords)
    {
        if (coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
            return false;
        return true;
    }

    public bool HasPiece(Piece piece)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (grid[i, j] == piece)
                    return true;
            }
        }
        return false;
    }


    public void SetPieceOnBoard(Vector2Int coords, Piece piece)
    {
        if (CheckIfCoordinatesAreOnBoard(coords))
            grid[coords.x, coords.y] = piece;
    }

    private void TryToTakeOppositePiece(Vector2Int coords)
    {
        Piece piece = GetPieceOnSquare(coords);
        if (piece && !selectedPiece.IsFromSameTeam(piece))
        {

            TakePiece(piece);
        }
    }

    public void IsAttacking(Vector2Int coords, Piece p)
    {
        Piece piece = GetPieceOnSquare(coords);
        if (piece && !selectedPiece.IsFromSameTeam(piece))
        {

            p.isat = true;
        }
    }

    private async void TakePiece(Piece piece)
    {
        if (piece)
        {
            
            int a = piece.GetPiece();
            if (turn == 1)
            {
                Debug.Log(a);
                switch (a)
                {
                    case 1:
                        print("peon");
                        wpc++;
                        WPawnCounter.GetComponent<Text>().text = "" + wpc;
                        break;
                    case 2:
                        print("alfil");
                        wbc++;
                        WBishopCounter.GetComponent<Text>().text = "" + wbc;
                        break;
                    case 3:
                        print("caballo");
                        wkc++;
                        WKnightCounter.GetComponent<Text>().text = "" + wkc;
                        break;
                    case 4:
                        print("torre");
                        wrc++;
                        WRookCounter.GetComponent<Text>().text = "" + wrc;
                        break;
                    case 5:
                        print("reina");
                        wqc++;
                        WQueenCounter.GetComponent<Text>().text = "" + wqc;
                        break;
                    case 6:
                        print("rey");
                        break;

                }
            }
            else
            {
                switch (a)
                {
                    case 1:
                        print("peon");
                        bpc++;
                        BPawnCounter.GetComponent<Text>().text = "" + bpc;
                        break;
                    case 2:
                        print("alfil");
                        bbc++;
                        BBishopCounter.GetComponent<Text>().text = "" + bbc;
                        break;
                    case 3:
                        print("caballo");
                        bkc++;
                        BKnightCounter.GetComponent<Text>().text = "" + bkc;
                        break;
                    case 4:
                        print("torre");
                        brc++;
                        BRookCounter.GetComponent<Text>().text = "" + brc;
                        break;
                    case 5:
                        print("reina");
                        bqc++;
                        BQueenCounter.GetComponent<Text>().text = "" + bqc;
                        break;
                    case 6:
                        print("rey");
                        break;

                }
            }

            grid[piece.occupiedSquare.x, piece.occupiedSquare.y] = null;
            chessController.OnPieceRemoved(piece);




            //piece.ded = true;


            while (piece.ded2 == true)
            {
                await Task.Yield();
            }

            Destroy(piece.gameObject);


        }
    }

    private void Animate(Piece piece, Vector2Int coords, Piece att)
    {
        IsAttacking(coords, piece);
        piece.Animation(coords, att);

    }


    public void PromotePiece(Piece piece)
    {
        TakePiece(piece);
        chessController.CreatePieceAndInitialize(piece.occupiedSquare, piece.team, typeof(Queen));
    }

    internal void OnGameRestarted()
    {
        selectedPiece = null;
        CreateGrid();
    }



}