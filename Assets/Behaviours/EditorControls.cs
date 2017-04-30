using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorControls : MonoBehaviour
{
    public Button waypoint_button;
    public GameObject waypoint_mode_panel;

    private MainMenuManager main_menu_manager;
    private TileSelectionManager tile_selection_manager;
    private LevelManager level_manager;
    private EditableGrid editable_grid;

    private TileType tile_type_to_paint;
    private bool cursor_over_ui;

    private bool setting_waypoint;
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

        handle_selection_controls();
        handle_mouse_controls();
	}

    // Main method for handling painting / waypoint setting via mouse raycasting.
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

    // Detects if the mouse is over UI elements.
    void track_mouse()
    {
        cursor_over_ui = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    // Keyboard controls for selecting tiles.
    void handle_selection_controls()
    {
        if (editable_grid.waypoint_mode_enabled() ||
            main_menu_manager.is_menu_open())
            return;

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

    // Performs an action based on the current stage of waypoint setting.
    void handle_waypoint_setting(EditableTile tile)
    {
        if (setting_waypoint)
        {
            selected_enemy.set_waypoint(tile.get_tiles_index(), tile.transform.position);
            setting_waypoint = false;

            editable_grid.refresh_waypoints();
        } else {
            // If tile does not represent an enemy.
            if (tile.get_tile_type().id != 4)
                return;

            selected_enemy = editable_grid.get_enemy(tile.get_tiles_index());
            setting_waypoint = true;
        }

        waypoint_mode_panel.transform.FindChild("TooltipText").GetComponent<Text>().text = setting_waypoint ?
            "Select a destination" : "Select an Enemy";
    }

    // Purely aesthetic function to make it more apparent waypoint mode is active or inactive.
    void show_waypoint_mode_elements(bool show)
    {
        waypoint_mode_panel.SetActive(show);
        waypoint_mode_panel.transform.FindChild("TooltipText").GetComponent<Text>().text = "Select an Enemy";

        if (show)
        {
            waypoint_button.transform.FindChild("Text").GetComponent<Text>().color = Color.white;

            var colors = waypoint_button.colors;
            colors.normalColor = Color.red;
            colors.highlightedColor = Color.grey;

            waypoint_button.colors = colors;
        } else {
            waypoint_button.transform.FindChild("Text").GetComponent<Text>().color = Color.black;

            var colors = waypoint_button.colors;
            colors.normalColor = Color.white;

            Color color;
            ColorUtility.TryParseHtmlString("#4f99FFFF", out color);

            colors.highlightedColor = color;

            waypoint_button.colors = colors;
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

        set_waypoint_mode(!editable_grid.waypoint_mode_enabled());
    }

    public void set_waypoint_mode(bool enable)
    {
        if (enable)
        {
            editable_grid.enable_waypoint_mode();
        } else {
            editable_grid.disable_waypoint_mode();            
        }

        show_waypoint_mode_elements(enable);
    }
}
