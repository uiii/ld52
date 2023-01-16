using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
	public GameController game;

	public int cropCount;
	public int harvestedCount;
	public int burntCount;
	public int burninCount;
	public int harvestedPercent;
	public int remainingCropPercent;

	public float health = 100;

	public Image harvestedBar;
	public Image burntBar;
	public TMP_Text percentText;

	public Image healthBar;

	void Update()
	{
		if (cropCount > 0) {
			//harvestedBar.fillAmount = (float) harvestedCount / cropCount;
			burntBar.fillAmount = (float) burntCount / cropCount;
			//harvestedPercent = Mathf.RoundToInt(harvestedBar.fillAmount * 100);

			remainingCropPercent = Mathf.RoundToInt(((float) (cropCount - burntCount) / cropCount) * 100);

			percentText.text = remainingCropPercent.ToString() + " %";
		}

		healthBar.fillAmount = health / 100f;
	}

	public void AddCrop(int count)
	{
		cropCount += count;
	}

	public void AddHarvested(int count)
	{
		harvestedCount += count;
	}

	public void AddBurnt(int count)
	{
		burntCount += count;
	}

	public void AddBurning(int count)
	{
		burninCount += count;

		if (burninCount <= 0) {
			game.Victory();
		}
	}

	public void DecreaseHealth(float amount)
	{
		health -= amount;

		if (health <= 0) {
			game.GameOver();
		}
	}
}
