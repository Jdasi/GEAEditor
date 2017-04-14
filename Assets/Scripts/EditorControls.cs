using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorControls : MonoBehaviour
{
    private TileSelectionManager tile_selection_manager;
    private BoxCollider2D mouse_collider;
    private Vector3 mouse_pos;

    private bool can_paint;
    private bool painting;

    private int tile_id_to_paint;
    private Sprite current_sprite;

    private bool spawn_placed;

	void Start()
    {
        tile_selection_manager = GameObject.FindObjectOfType<TileSelectionManager>();
        mouse_collider = GetComponent<BoxCollider2D>();
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

        paint_tile(other.GetComponent<EditableTile>());
    }

    void track_mouse()
    {
        mouse_pos = Input.mousePosition;
        transform.position = Camera.main.ScreenToWorldPoint(mouse_pos);

        can_paint = !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            tile_selection_manager.next_tile_page();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            tile_selection_manager.prev_tile_page();
        }
    }

    void paint_tile(EditableTile tile)
    {
        if (tile_id_to_paint == 1)
        {
            if (!spawn_placed)
                spawn_placed = true;
            else
                return;
        }
        else
        {
            if (tile.get_id() == 1)
                spawn_placed = false;
        }

        tile.paint(tile_id_to_paint, current_sprite);
    }

    public void update_selected_sprite(int tile_id, Sprite sprite)
    {
        this.tile_id_to_paint = tile_id;
        this.current_sprite = sprite;
    }
}
