using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ChessUIManager : MonoBehaviour
{
	bool singleplayerOn = false;
	bool multiplayerOn = false;

	[Header("Dependencies")]
	[SerializeField] private NetworkManager networkManager;

	[Header("Buttons")]
	[SerializeField] private Button whiteTeamButtonButton;
	[SerializeField] private Button blackTeamButtonButton;

	[Header("Texts")]
	[SerializeField] private Text finishText;
	[SerializeField] private Text connectionStatus;

	[Header("Screen Gameobjects")]
	[SerializeField] private GameObject GameOverScreen;
	[SerializeField] private GameObject ConnectScreen;
	[SerializeField] private GameObject TeamSelectionScreen;
	[SerializeField] private GameObject GameModeSelectionScreen;
	[SerializeField] private GameObject SingleplayerGameScreen;
	[SerializeField] private GameObject MultiplayerGameScreen;
	[SerializeField] private GameObject OptionsScreen;
	[SerializeField] private GameObject GuideScreen;
	[SerializeField] private GameObject SingleplayerGameMenu;
	[SerializeField] private GameObject MultiplayerGameMenu;

	[Header("Other UI")]
	[SerializeField] private Dropdown gameLevelSelection;
	[SerializeField] private GameObject TakenPieces;
	[SerializeField] private GameObject TakenPieces2;
	[SerializeField] private GameObject PlaneFinderSingleplayer;
	[SerializeField] private GameObject PlaneFinderMultiplayer;

	private void Awake()
	{
		gameLevelSelection.AddOptions(Enum.GetNames(typeof(ChessLevel)).ToList());
		OnGameLaunched();
		PlaneFinderSingleplayer.SetActive(false);
		PlaneFinderMultiplayer.SetActive(false);
	}


	public void ExitGame()
	{
		Application.Quit();
	}

	public void ShowOptionsScreen()
	{
		OptionsScreen.SetActive(true);
		GameModeSelectionScreen.SetActive(false);
		SingleplayerGameMenu.SetActive(false);
		MultiplayerGameMenu.SetActive(false);
		TakenPieces.SetActive(false);
		TakenPieces2.SetActive(false);
	}

	public void ShowGuideScreen()
	{
		GuideScreen.SetActive(true);
		GameModeSelectionScreen.SetActive(false);
	}
	public void ShowSingleplayerGameMenu()
	{
		SingleplayerGameMenu.SetActive(true);
		SingleplayerGameScreen.SetActive(false);
		TakenPieces.SetActive(false);
		TakenPieces2.SetActive(false);
	}
	public void ShowMultiplayerGameMenu()
	{
		MultiplayerGameMenu.SetActive(true);
		MultiplayerGameScreen.SetActive(false);
		TakenPieces.SetActive(false);
		TakenPieces2.SetActive(false);
	}

	public void UnpauseSingleplayer()
	{
		SingleplayerGameScreen.SetActive(true);
		SingleplayerGameMenu.SetActive(false);
	}

	public void UnpauseMultiplayer()
	{
		MultiplayerGameScreen.SetActive(true);
		MultiplayerGameMenu.SetActive(false);
	}

	public void OptionsBack()
	{
		OptionsScreen.SetActive(false);
		if (singleplayerOn == true)
		{
			SingleplayerGameMenu.SetActive(true);
			TakenPieces.SetActive(true);
			TakenPieces2.SetActive(true);
		}
		else if (multiplayerOn == true)
		{
			MultiplayerGameMenu.SetActive(true);
			TakenPieces.SetActive(true);
			TakenPieces2.SetActive(true);
		}
		else
		{
			GameModeSelectionScreen.SetActive(true);
		}
	}

	public void OnGameLaunched()
	{


		connectionStatus.gameObject.SetActive(false);
		GameOverScreen.SetActive(false);
		TeamSelectionScreen.SetActive(false);
		ConnectScreen.SetActive(false);
		GameModeSelectionScreen.SetActive(true);
		SingleplayerGameScreen.SetActive(false);
		MultiplayerGameScreen.SetActive(false);
		OptionsScreen.SetActive(false);
		GuideScreen.SetActive(false);
		SingleplayerGameMenu.SetActive(false);
		MultiplayerGameMenu.SetActive(false);
		TakenPieces.SetActive(false);
		TakenPieces2.SetActive(false);


	}

	public void OnSinglePlayerModeSelected()
	{
		GameOverScreen.SetActive(false);
		TeamSelectionScreen.SetActive(false);
		ConnectScreen.SetActive(false);
		GameModeSelectionScreen.SetActive(false);
		SingleplayerGameScreen.SetActive(true);
		MultiplayerGameScreen.SetActive(false);
		OptionsScreen.SetActive(false);
		GuideScreen.SetActive(false);
		SingleplayerGameMenu.SetActive(false);
		MultiplayerGameMenu.SetActive(false);
		TakenPieces.SetActive(true);
		TakenPieces2.SetActive(true);
		singleplayerOn = true;
		PlaneFinderSingleplayer.SetActive(true);
	}

	public void OnMultiPlayerModeSelected()
	{
		connectionStatus.gameObject.SetActive(true);
		GameOverScreen.SetActive(false);
		TeamSelectionScreen.SetActive(false);
		ConnectScreen.SetActive(true);
		GameModeSelectionScreen.SetActive(false);
		SingleplayerGameScreen.SetActive(false);

	}

	internal void OnGameFinished(string winner)
	{

		GameOverScreen.SetActive(true);
		TeamSelectionScreen.SetActive(false);
		ConnectScreen.SetActive(false);
		finishText.text = string.Format("{0} won", winner);
	}

	public void OnConnect()
	{
		networkManager.SetPlayerLevel((ChessLevel)gameLevelSelection.value);
		networkManager.Connect();

		multiplayerOn = true;
	}

	public void SetConnectionStatusText(string status)
	{
		connectionStatus.text = status;
	}

	internal void ShowTeamSelectionScreen()
	{
		GameOverScreen.SetActive(false);
		TeamSelectionScreen.SetActive(true);
		ConnectScreen.SetActive(false);
	}

	public void OnGameStarted()
	{
		GameOverScreen.SetActive(false);
		TeamSelectionScreen.SetActive(false);
		ConnectScreen.SetActive(false);
		connectionStatus.gameObject.SetActive(false);
		GameModeSelectionScreen.SetActive(false);
		//MultiplayerGameScreen.SetActive(true);
		//TakenPieces.SetActive(true);
		//TakenPieces2.SetActive(true);
		//SingleplayerGameScreen.SetActive(false);
	}

	public void SelectTeam(int team)
	{
		networkManager.SetPlayerTeam(team);
		//PlaneFinderMultiplayer.SetActive(true);
		GameOverScreen.SetActive(false);
		TeamSelectionScreen.SetActive(false);
		ConnectScreen.SetActive(false);
		connectionStatus.gameObject.SetActive(false);
		GameModeSelectionScreen.SetActive(false);
		MultiplayerGameScreen.SetActive(true);
		TakenPieces.SetActive(true);
		TakenPieces2.SetActive(true);
	}

	internal void RestrictTeamChoice(TeamColor occpiedTeam)
	{
		Button buttonToDeactivate = occpiedTeam == TeamColor.White ? whiteTeamButtonButton : blackTeamButtonButton;
		buttonToDeactivate.interactable = false;
	}
}