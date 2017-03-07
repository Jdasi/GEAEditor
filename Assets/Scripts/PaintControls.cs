using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintControls : MonoBehaviour
{
    private Camera cam;
    private BoxCollider2D mouse_collider;
    private LevelEditor level_editor;

	void Start()
    {
        cam = Camera.main;
        mouse_collider = GetComponent<BoxCollider2D>();
        level_editor = GameObject.FindObjectOfType<LevelEditor>();
	}
	
	void Update()
    {
		if (Input.GetMouseButton(0))
        {
            Vector3 mouse_pos = Input.mousePosition;
            transform.position = cam.ScreenToWorldPoint(mouse_pos);

            mouse_collider.enabled = true;
        }
        else
        {
            mouse_collider.enabled = false;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Tile")
            return;

        other.GetComponent<Tile>().paint_tile(level_editor.selected_id, level_editor.selected_sprite);
    }
}
