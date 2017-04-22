using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorControls : MonoBehaviour
{
    public bool controls_enabled = true; // Used to disable controls while in the menus.

    private TileSelectionManager tile_selection_manager;
    private EditableGrid editable_grid;
    private BoxCollider2D mouse_collider;
    private Vector3 mouse_pos;

    private TileType tile_type_to_paint;
    private bool cursor_over_ui;
    private bool waypoint_mode;

	void Start()
    {
        tile_selection_manager = GameObject.FindObjectOfType<TileSelectionManager>();
        editable_grid = GameObject.FindObjectOfType<EditableGrid>();
        mouse_collider = GetComponent<BoxCollider2D>();
	}
	
	void Update()
    {
        track_mouse();
        handle_selection_controls();

        mouse_collider.enabled = Input.GetMouseButton(0) && controls_enabled;
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (cursor_over_ui)
            return;

        if (other.tag != "Tile")
            return;

        editable_grid.paint_tile(other.GetComponent<EditableTile>(), tile_type_to_paint);
    }

    void track_mouse()
    {
        mouse_pos = Input.mousePosition;
        transform.position = Camera.main.ScreenToWorldPoint(mouse_pos);

        cursor_over_ui = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    void handle_selection_controls()
    {
        if (!controls_enabled)
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

    public void update_tile_type_to_paint(TileType type)
    {
        tile_type_to_paint = type;
    }
}
