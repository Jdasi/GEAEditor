using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditableGrid : MonoBehaviour
{
    public uint width = 10;
    public uint height = 10;
    public GameObject editable_tile_prefab;

    private List<EditableTile> tiles = new List<EditableTile>();

	void Start()
    {
        init_grid();
	}

    void init_grid()
    {
        // Get initial editable sprite prefab and get its size.
        var sprite = editable_tile_prefab.GetComponent<SpriteRenderer>().sprite.texture;
        float tile_width = sprite.width;
        float tile_height = sprite.height;

        // Divide size by 100 to match Unity's unit measurements.
        var tile_size = new Vector2(tile_width / 100, tile_height / 100);

        // Create the grid of tiles based on width & height.
        uint area = width * height;
        for (int i = 0; i < area; ++i)
        {
            Vector3 pos = new Vector3(0 + tile_size.x * (i % width), 0 - tile_size.y * (i / width), 0);
            GameObject obj = Instantiate(editable_tile_prefab, pos, Quaternion.identity) as GameObject;

            obj.transform.SetParent(this.transform);
            obj.name = "Tile" + i;

            tiles.Add(obj.GetComponent<EditableTile>());
        }
    }

    public int[] get_tile_ids()
    {
        int[] tile_ids = new int[width * height];

        for (int i = 0; i < tiles.Count; ++i)
        {
            tile_ids[i] = tiles[i].get_id();
        }

        return tile_ids;
    }
}
