using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorControls : MonoBehaviour
{
    public List<Sprite> sprites;

    private BoxCollider2D mouse_collider;
    private Vector3 mouse_pos;

    private bool can_paint;
    private bool painting;

    private int selected_id;
    private Sprite selected_sprite;

	void Start()
    {
        mouse_collider = GetComponent<BoxCollider2D>();

        update_selected_sprite(0);
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
            other.GetComponent<Tile>().paint(selected_id, selected_sprite);
    }

    void track_mouse()
    {
        mouse_pos = Input.mousePosition;
        transform.position = Camera.main.ScreenToWorldPoint(mouse_pos);

        can_paint = !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    void handle_selection_controls()
    {
        if (Input.GetButtonDown("SelectedTilePrev"))
        {
            --selected_id;

            if (selected_id < 0)
                selected_id = sprites.Count - 1;

            update_selected_sprite(selected_id);
        }

        if (Input.GetButtonDown("SelectedTileNext"))
        {
            ++selected_id;

            if (selected_id >= sprites.Count)
                selected_id = 0;

            update_selected_sprite(selected_id);
        }
    }

    void update_selected_sprite(int id)
    {
        this.selected_id = id;
        this.selected_sprite = sprites[id];
    }
}
