using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditableGrid : MonoBehaviour
{
    public GameObject editable_tile_prefab;

    private List<EditableTile> editable_tiles = new List<EditableTile>();
    private TileSelectionManager tile_selection_manager;
    private bool spawn_placed;

    void Start()
    {
        tile_selection_manager = GameObject.FindObjectOfType<TileSelectionManager>();
    }

    public void init_grid(JSWLevel level)
    {
        // Get initial editable sprite prefab and get its size.
        Texture2D texture = editable_tile_prefab.GetComponent<SpriteRenderer>().sprite.texture;
        float tile_width = texture.width;
        float tile_height = texture.height;

        // Divide size by 100 to match Unity's unit measurements.
        var tile_size = new Vector2(tile_width / 100, tile_height / 100);

        // Create the grid of tiles based on width & height.
        int area = level.width * level.height;
        for (int i = 0; i < area; ++i)
        {
            Vector3 pos = new Vector3(0 + tile_size.x * (i % level.width), 0 - tile_size.y * (i / level.width), 0);
            GameObject obj = Instantiate(editable_tile_prefab, pos, Quaternion.identity) as GameObject;

            obj.transform.SetParent(this.transform);
            obj.name = "Tile" + i;

            EditableTile tile = obj.GetComponent<EditableTile>();
            tile.initialise();
            paint_tile(tile, tile_selection_manager.get_tile_type_by_id(level.tile_ids[i]));

            editable_tiles.Add(tile);
        }

        // Center camera on the grid.
        Vector3 new_cam_pos = editable_tiles[area / 2].transform.position;
        new_cam_pos.z = Camera.main.transform.position.z;

        Camera.main.transform.position = new_cam_pos; 
    }

    public void paint_tile(EditableTile tile, TileType tile_type_to_paint)
    {
        if (tile_type_to_paint.id == 1)
        {
            if (!spawn_placed)
                spawn_placed = true;
            else
                return;
        }
        else
        {
            if (tile.get_tile_type().id == 1)
                spawn_placed = false;
        }

        tile.paint(tile_type_to_paint);
    }
}
