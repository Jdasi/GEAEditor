using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorControls : MonoBehaviour
{
    public bool controls_enabled = true; // Used to disable controls while in the menus.

    private MainMenuManager main_menu_manager;
    private TileSelectionManager tile_selection_manager;
    private LevelManager level_manager;
    private EditableGrid editable_grid;

    private TileType tile_type_to_paint;
    private Vector3 mouse_pos;
    private bool cursor_over_ui;

    public bool setting_waypoint;
    private Enemy selected_enemy;
    private bool mouse_up;

	void Start()
    {
        main_menu_manager = GameObject.FindObjectOfType<MainMenuManager>();
        tile_selection_manager = GameObject.FindObjectOfType<TileSelectionManager>();
        level_manager = GameObject.FindObjectOfType<LevelManager>();
        editable_grid = GameObject.FindObjectOfType<EditableGrid>();
	}
	
	void Update()
    {
        track_mouse();

        if (Input.GetKeyDown(KeyCode.Escape))
            main_menu_manager.toggle_main_menu();

        // Reset waypoint stuff if it's not active.
        if (!editable_grid.waypoint_mode_enabled())
        {
            setting_waypoint = false;
            selected_enemy = null;
        }
        
        // Don't go any further if controls are disabled.
        if (!controls_enabled)
            return;

        handle_selection_controls();
        handle_mouse_controls();
	}

    void handle_mouse_controls()
    {
        if (cursor_over_ui)
            return;

        if (editable_grid.waypoint_mode_enabled())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

                if (hit != null && hit.collider != null && hit.collider.tag == "Tile")
                {
                    EditableTile tile = hit.collider.GetComponent<EditableTile>();
                    handle_waypoint_setting(tile);
                }
            }
        } else { // Painting mode.
            if (Input.GetMouseButton(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

                if (hit != null && hit.collider != null && hit.collider.tag == "Tile")
                {
                    EditableTile tile = hit.collider.GetComponent<EditableTile>();
                    editable_grid.paint_tile(tile, tile_type_to_paint);
                }
            }
        }
    }

    void track_mouse()
    {
        mouse_pos = Input.mousePosition;
        transform.position = Camera.main.ScreenToWorldPoint(mouse_pos);

        cursor_over_ui = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    void handle_selection_controls()
    {
        if (Input.GetButtonDown("SelectedTileNext"))
        {
            tile_selection_manager.select_next_tile();
        }

        if (Input.GetButtonDown("SelectedTilePrev"))
        {
            tile_selection_manager.select_prev_tile();
        }

        if (Input.GetButtonDown("TilePageNext"))
        {
            tile_selection_manager.next_tile_page();
        }

        if (Input.GetButtonDown("TilePagePrev"))
        {
            tile_selection_manager.prev_tile_page();
        }
    }

    void handle_waypoint_setting(EditableTile tile)
    {
        if (setting_waypoint)
        {
            selected_enemy.set_waypoint(tile.get_tiles_index(), tile.transform.position);
            setting_waypoint = false;

            editable_grid.disable_waypoint_mode();
            editable_grid.enable_waypoint_mode();
        } else {
            // If tile does not represent an enemy.
            if (tile.get_tile_type().id != 4)
                return;

            selected_enemy = editable_grid.get_enemy(tile.get_tiles_index());
            setting_waypoint = true;
        }
    }

    public void update_tile_type_to_paint(TileType type)
    {
        tile_type_to_paint = type;
    }

    public void toggle_waypoint_mode()
    {
        if (level_manager.get_current_level() == null)
            return;

        if (editable_grid.waypoint_mode_enabled())
        {
            editable_grid.disable_waypoint_mode();   
        } else {
            editable_grid.enable_waypoint_mode();
        }
    }

    public void set_waypoint_mode(bool enable)
    {
        if (enable)
            editable_grid.enable_waypoint_mode();
        else
            editable_grid.disable_waypoint_mode();
    }
}
