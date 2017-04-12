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

    private int current_tile_id;
    private Sprite current_sprite;

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
        if (other.tag == "Tile" && painting)
            other.GetComponent<EditableTile>().paint(current_tile_id, current_sprite);
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

    public void update_selected_sprite(int tile_id, Sprite sprite)
    {
        this.current_tile_id = tile_id;
        this.current_sprite = sprite;
    }
}
