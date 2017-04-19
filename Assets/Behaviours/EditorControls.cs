using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorControls : MonoBehaviour
{
    private TileSelectionManager tile_selection_manager;
    private EditableGrid editable_grid;
    private BoxCollider2D mouse_collider;
    private Vector3 mouse_pos;

    private TileType tile_type_to_paint;
    private bool can_paint;
    private bool painting;

	void Start()
    {
        tile_selection_manager = GameObject.FindObjectOfType<TileSelectionManager>();
        editable_grid = GameObject.FindObjectOfType<EditableGrid>();
        mouse_collider = GetComponent<BoxCollider2D>();

        tile_type_to_paint = tile_selection_manager.get_selected_tile_type();
	}
	
	void Update()
    {
        track_mouse();
        handle_selection_controls();

        mouse_collider.enabled = painting = Input.GetMouseButton(0) && can_paint;
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (!(other.tag == "Tile" && painting))
            return;

        editable_grid.paint_tile(other.GetComponent<EditableTile>(), tile_type_to_paint);
    }

    void track_mouse()
    {
        mouse_pos = Input.mousePosition;
        transform.position = Camera.main.ScreenToWorldPoint(mouse_pos);

        can_paint = !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    void handle_selection_controls()
    {
        bool selection_changed = false;

        if (Input.GetButtonDown("SelectedTileNext"))
        {
            tile_selection_manager.select_next_tile();
            selection_changed = true;
        }

        if (Input.GetButtonDown("SelectedTilePrev"))
        {
            tile_selection_manager.select_prev_tile();
            selection_changed = true;
        }

        if (Input.GetButtonDown("TilePageNext"))
        {
            tile_selection_manager.next_tile_page();
            selection_changed = true;
        }

        if (Input.GetButtonDown("TilePagePrev"))
        {
            tile_selection_manager.prev_tile_page();
            selection_changed = true;
        }

        if (selection_changed)
        {
            tile_type_to_paint = tile_selection_manager.get_selected_tile_type();
        }
    }
}
