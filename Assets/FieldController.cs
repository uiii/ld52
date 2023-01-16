using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldController : MonoBehaviour
{
	public Tilemap tilemap;
	public CropController cropsPrefab;

	public GameController game;
	public Stats stats;

	public TileBase fieldTile;
	public TileBase burntTile;

	public CropController[,] crops;

	void Start()
	{
		crops = new CropController[tilemap.size.x, tilemap.size.y];

		for (int x = 0; x < tilemap.size.x; ++x) {
			for (int y = tilemap.size.y; y >= 0; --y) {
				var tilePosition = tilemap.origin + new Vector3Int(x, y, 0);

				var tile = tilemap.GetTile(tilePosition);

				if (tile != null && tile.name == fieldTile.name) {
					var crop = Instantiate(cropsPrefab, tilemap.GetCellCenterWorld(tilePosition), Quaternion.identity);
					crop.stats = stats;
					crop.field = this;
					crop.position = new Vector2Int(x, y);

					crops[x, y] = crop;
					GetCrop(crop.position + Vector2Int.up)?.SetEdge(false);

					stats.AddCrop(1);
				}
			}
		}

		game.StartFire();
	}

	public void SetBurnt(Vector3 position)
	{
		tilemap.SetTile(tilemap.WorldToCell(position), burntTile);
	}

	public CropController GetCrop(Vector2Int position)
	{
		var rect = new RectInt(Vector2Int.zero, (Vector2Int) tilemap.size);

		if (rect.Contains(position)) {
			return crops[position.x, position.y];
		}

		return null;
	}
}
