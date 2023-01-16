using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressController : MonoBehaviour
{
	public Stats stats;

	public Image harvested;
	public Image burnt;
	public TMP_Text percent;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (stats.cropCount > 0) {
			harvested.fillAmount = (float) stats.harvestedCount / stats.cropCount;
			burnt.fillAmount = (float) stats.burntCount / stats.cropCount;
			percent.text = Mathf.RoundToInt(harvested.fillAmount * 100).ToString() + " %";
		}
	}
}
