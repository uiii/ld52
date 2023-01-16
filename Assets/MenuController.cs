using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public GameController game;

	public GameObject menuUi;

	public GameObject[] buttons;

	public GameObject buttonsBox;

	public bool canResume = false;

	int selectedButtonIndex = 0;

	void Awake()
	{
		game.controls.Menu.Resume.performed += ctx => game.Resume();
		game.controls.Menu.Fire.performed += ctx => game.StartFire();
	}

	void Start()
	{
		buttons[0].GetComponentInChildren<Button>().Select();
	}

	public void Show(bool show)
	{
		menuUi.gameObject.SetActive(show);

		if (show) {
			buttons[0].GetComponentInChildren<Button>().Select();
		}
	}

	void Resume()
	{
		if (canResume) {
			game.Resume();
		}
	}
}
