using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropController : MonoBehaviour
{
	public FieldController field;
	public Stats stats;

	public Sprite innerSprite;
	public Sprite edgeSprite;

	public GameObject firePrefab;

	public Vector2Int position;

	private SpriteRenderer spriteRenderer;
	private CropController topNeighbor;
	private bool isBeingCropped = false;

	public bool isBurning = false;

	private float nextFireSpreadTime = 0;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		if (isBeingCropped) {
			spriteRenderer.color = new Color(1f,1f,1f,Mathf.Clamp01(spriteRenderer.color.a - (100f * Time.deltaTime)));
		}

		if (isBurning) {
			spriteRenderer.color = new Color(1f,1f,1f,Mathf.Clamp01(spriteRenderer.color.a - (0.1f * Time.deltaTime)));
		}

		if (spriteRenderer.color.a == 0) {
			var topNeighbor = field.GetCrop(position + Vector2Int.up);
			if (topNeighbor != null) {
				topNeighbor.GetComponent<SpriteRenderer>().sprite = edgeSprite;
			}

			if (isBurning) {
				stats.AddBurning(-1);
			}

			isBeingCropped = false;
			Destroy(gameObject);
		}
	}

	void FixedUpdate()
	{
		if (isBurning && Time.fixedTime > nextFireSpreadTime) {
			TrySpreadFire(position + Vector2Int.up);
			TrySpreadFire(position + Vector2Int.down);
			TrySpreadFire(position + Vector2Int.left);
			TrySpreadFire(position + Vector2Int.right);

			nextFireSpreadTime = Time.fixedTime + 2f + Random.value;
		}
	}

	private void TrySpreadFire(Vector2Int position)
	{
		var neighbor = field.GetCrop(position);

		if (neighbor == null) {
			return;
		}

		var fireSpreadProbability = (1f - spriteRenderer.color.a) / 0.5f;

		if (Random.value <= fireSpreadProbability) {
			neighbor.StartFire();
		}
	}

	public void SetEdge(bool isEdge)
	{
		GetComponent<SpriteRenderer>().sprite = isEdge ? edgeSprite : innerSprite;
	}

	public void StartFire()
	{
		if (isBurning || this == null) {
			return;
		}

		var fire = Instantiate(firePrefab, transform.position, Quaternion.identity, transform);

		isBurning = true;
		nextFireSpreadTime = Time.fixedTime + 2f;

		field.SetBurnt(transform.position);
		stats.AddBurnt(1);
		stats.AddBurning(1);
	}

	public void Crop()
	{
		isBeingCropped = true;

		if (!isBurning) {
			stats.AddHarvested(1);
		}
	}
}
