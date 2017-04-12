using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelectionManager : MonoBehaviour
{
    public GameObject gui_tile_prefab;
    public List<Sprite> sprites;

    private List<GUITile> gui_tiles = new List<GUITile>();
    private EditorControls editor_controls;

    private const int MAX_TILES = 10;
    public int current_page = 1;
    public int min_offset;
    public int max_offset;

    public int gui_tiles_index;
    private Sprite selected_sprite;

	void Start()
    {
        editor_controls = GameObject.FindObjectOfType<EditorControls>();

        init_gui_tiles();
        update_min_max();

        selection_changed_event(0, 0);
	}

    void init_gui_tiles()
    {
        for (int i = 0; i < MAX_TILES; ++i)
        {
            GameObject obj = Instantiate(gui_tile_prefab, this.transform) as GameObject;
            obj.name = "GUITile" + i;

            GUITile gui_tile = obj.GetComponent<GUITile>();
            gui_tile.initialise();
            gui_tile.set_manager(this, i);

            gui_tiles.Add(obj.GetComponent<GUITile>());
        }
    }

    void update_min_max()
    {
        min_offset = (MAX_TILES * current_page) - MAX_TILES;
        max_offset = (MAX_TILES * current_page) - 1;

        selection_changed_event(0, min_offset);

        update_gui_tiles();
    }

    void update_gui_tiles()
    {
        for (int i = 0; i < MAX_TILES; ++i)
        {
            gui_tiles[i].gameObject.SetActive(true);

            // Only show GUITiles that correlate to a sprite.
            int tile_id = min_offset + i;
            if (tile_id >= sprites.Count)
            {
                gui_tiles[i].gameObject.SetActive(false);
            }
            else
            {
                gui_tiles[i].update_info(tile_id, sprites[tile_id]);
            }
        }
    }

    public void select_next_tile()
    {
        ++gui_tiles_index;

        // Correct any errors with incrementing the index.
        if ((gui_tiles_index > MAX_TILES - 1) || 
            (gui_tiles_index + min_offset >= sprites.Count))
        {
            gui_tiles_index = 0;
        }

        selection_changed_event(gui_tiles_index, gui_tiles_index + min_offset);
    }

    public void select_prev_tile()
    {
        --gui_tiles_index;

        // Correct any errors with decrementing the index.
        if (gui_tiles_index < 0)
        {
            gui_tiles_index = MAX_TILES - 1;

            if (gui_tiles_index + min_offset >= sprites.Count)
                gui_tiles_index = sprites.Count - 1 - min_offset;
        }

        selection_changed_event(gui_tiles_index, gui_tiles_index + min_offset);
    }

    public void next_tile_page()
    {
        if (max_offset > sprites.Count)
            return;

        ++current_page;
        update_min_max();
    }

    public void prev_tile_page()
    {
        if (current_page == 1)
            return;

        --current_page;
        update_min_max();
    }

    public void selection_changed_event(int index, int tile_id)
    {
        foreach (var gui_tile in gui_tiles)
        {
            gui_tile.set_outline_enabled(false);
        }

        gui_tiles_index = index;

        gui_tiles[gui_tiles_index].set_outline_enabled(true);
        selected_sprite = sprites[tile_id];

        editor_controls.update_selected_sprite(gui_tiles_index + min_offset, selected_sprite);
    }
}
