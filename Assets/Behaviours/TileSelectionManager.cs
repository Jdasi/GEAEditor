using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelectionManager : MonoBehaviour
{
    public GameObject gui_tile_prefab;
    public Texture2D tileset;

    private List<GUITile> gui_tiles = new List<GUITile>();
    private int selected_gui_tile_index;
    private TileType selected_tile_type;

    private List<TileType> tile_types = new List<TileType>();
    private int tile_size = 60;

    private const int MAX_DISPLAYED_TILES = 10;
    private int current_page = 1;
    private int min_offset;
    private int max_offset;

	void Start()
    {
        load_tileset();

        init_gui_tiles();

        selection_changed_event(0, 0);
	}

    void load_tileset()
    {
        int size_x = Mathf.FloorToInt(tile_size);
        int size_y = Mathf.FloorToInt(tile_size);

        int width_times =  Mathf.FloorToInt(tileset.width / tile_size);
        int height_times = Mathf.FloorToInt(tileset.height / tile_size);

        for (int y = height_times - 1; y >= 0; --y)
        {
            for (int x = 0; x < width_times; ++x)
            {
                Rect rect = new Rect(0 + (x * size_x), 0 + (y * size_y), size_x, size_y);

                TileType tile_type = new TileType();
                tile_type.id = tile_types.Count;
                tile_type.sprite = Sprite.Create(tileset, rect, new Vector2(0.5f, 0.5f));

                tile_types.Add(tile_type);
            }
        }
    }

    void init_gui_tiles()
    {
        for (int i = 0; i < MAX_DISPLAYED_TILES; ++i)
        {
            GameObject obj = Instantiate(gui_tile_prefab, this.transform) as GameObject;
            obj.name = "GUITile" + i;

            GUITile gui_tile = obj.GetComponent<GUITile>();
            gui_tile.initialise();
            gui_tile.set_manager(this, i);

            gui_tiles.Add(obj.GetComponent<GUITile>());
        }

        update_min_max();
    }

    void update_min_max()
    {
        min_offset = (MAX_DISPLAYED_TILES * current_page) - MAX_DISPLAYED_TILES;
        max_offset = (MAX_DISPLAYED_TILES * current_page) - 1;

        selection_changed_event(0, min_offset);

        update_gui_tiles();
    }

    void update_gui_tiles()
    {
        for (int i = 0; i < MAX_DISPLAYED_TILES; ++i)
        {
            int type_index = min_offset + i;
            gui_tiles[i].update_info(tile_types[type_index]);
        }
    }

    public TileType get_selected_tile_type()
    {
        return selected_tile_type;
    }

    public TileType get_tile_type_by_id(int tile_id)
    {
        return tile_types[tile_id];
    }

    public void select_next_tile()
    {
        ++selected_gui_tile_index;

        // Correct any errors with incrementing the index.
        if ((selected_gui_tile_index > MAX_DISPLAYED_TILES - 1) || 
            (selected_gui_tile_index + min_offset >= tile_types.Count))
        {
            selected_gui_tile_index = 0;
        }

        selection_changed_event(selected_gui_tile_index, selected_gui_tile_index + min_offset);
    }

    public void select_prev_tile()
    {
        --selected_gui_tile_index;

        // Correct any errors with decrementing the index.
        if (selected_gui_tile_index < 0)
        {
            selected_gui_tile_index = MAX_DISPLAYED_TILES - 1;

            if (selected_gui_tile_index + min_offset >= tile_types.Count)
                selected_gui_tile_index = tile_types.Count - 1 - min_offset;
        }

        selection_changed_event(selected_gui_tile_index, selected_gui_tile_index + min_offset);
    }

    public void next_tile_page()
    {
        if (max_offset >= tile_types.Count - 1)
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

    public void selection_changed_event(int index, int type_id)
    {
        foreach (var gui_tile in gui_tiles)
        {
            gui_tile.is_current_selection(false);
        }

        selected_gui_tile_index = index;

        gui_tiles[selected_gui_tile_index].is_current_selection(true);
        selected_tile_type = tile_types[type_id];
    }
}
