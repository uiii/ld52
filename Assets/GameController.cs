using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public FieldController field;
	public MenuController menu;
	public Stats stats;

	public GameObject gameOverDialog;
	public GameObject victoryDialog;

	public TMP_Text gameOverResultText;
	public TMP_Text victoryResultText;

	public InputControls controls;

	public bool isMenu = false;

	static int difficulty = 1;

	void Awake()
	{
		controls = new InputControls();

		controls.Game.Pause.performed += ctx => ShowMenu();
		controls.Game.Restart.performed += ctx => NewGame();
		controls.Game.Startfire.performed += ctx => StartFire();
	}

	void Start()
	{
		if (isMenu) {
			ShowMenu();
		} else {
			Resume();
		}
	}

	public void ShowMenu()
	{
		menu.Show(true);
		Time.timeScale = 0;
		controls.Menu.Enable();
		controls.Game.Disable();
		controls.Harvester.Disable();
	}

	public void Resume()
	{
		menu.Show(false);
		Time.timeScale = 1;
		controls.Menu.Disable();
		controls.Game.Enable();
		controls.Harvester.Enable();
	}

	public void NewGame()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("Difficulty");
	}

	public void StartEasyGame()
	{
		Time.timeScale = 1;
		difficulty = 1;
		SceneManager.LoadScene("Game");
	}

	public void StartNormalGame()
	{
		Time.timeScale = 1;
		difficulty = 2;
		SceneManager.LoadScene("Game");
	}

	public void StartHardGame()
	{
		Time.timeScale = 1;
		difficulty = 3;
		SceneManager.LoadScene("Game");
	}

	public void StartHellGame()
	{
		Time.timeScale = 1;
		difficulty = 4;
		SceneManager.LoadScene("Game");
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void GameOver()
	{
		Time.timeScale = 0;

		gameOverDialog.SetActive(true);
	}

	public void Victory()
	{
		Time.timeScale = 0;
		victoryResultText.text = victoryResultText.text.Replace("XXX", stats.remainingCropPercent.ToString());
		victoryDialog.SetActive(true);
	}

	public void GoToMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	public void StartFire()
	{
		Vector2Int[] positions = new Vector2Int[0];

		if (difficulty == 1) {
			positions = new [] {
				new Vector2Int(25, 75),
				new Vector2Int(50, 50),
				new Vector2Int(75, 25),
			};
		} else if (difficulty == 2) {
			positions = new [] {
				new Vector2Int(25, 25),
				new Vector2Int(25, 75),
				new Vector2Int(75, 75),
				new Vector2Int(75, 25),
			};
		} else if (difficulty == 3) {
			positions = new [] {
				new Vector2Int(25, 25),
				new Vector2Int(25, 75),
				new Vector2Int(40, 50),
				new Vector2Int(60, 50),
				new Vector2Int(75, 75),
				new Vector2Int(75, 25),
			};
		} else if (difficulty == 4) {
			positions = new [] {
				new Vector2Int(12, 12),
				new Vector2Int(12, 37),
				new Vector2Int(12, 62),
				new Vector2Int(12, 84),
				new Vector2Int(37, 12),
				new Vector2Int(37, 37),
				new Vector2Int(37, 62),
				new Vector2Int(37, 84),
				new Vector2Int(62, 12),
				new Vector2Int(62, 37),
				new Vector2Int(62, 62),
				new Vector2Int(62, 84),
				new Vector2Int(84, 12),
				new Vector2Int(84, 37),
				new Vector2Int(84, 62),
				new Vector2Int(84, 84),
			};
		}

		foreach (var position in positions) {
			var crop = field.GetCrop(new Vector2Int(19, 7) + position);
			crop.StartFire();
		}
	}
}
