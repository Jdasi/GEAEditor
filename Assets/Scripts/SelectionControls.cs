using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionControls : MonoBehaviour
{
    private LevelEditor level_editor;
    
	void Start()
    {
		level_editor = GameObject.FindObjectOfType<LevelEditor>();
	}
	
	void Update()
    {
        if (Input.GetButtonDown("SelectedTilePrev"))
        {
            --level_editor.selected_id;

            if (level_editor.selected_id < 0)
                level_editor.selected_id = level_editor.sprites.Count - 1;

            level_editor.update_selected_sprite();
        }

		if (Input.GetButtonDown("SelectedTileNext"))
        {
            ++level_editor.selected_id;

            if (level_editor.selected_id >= level_editor.sprites.Count)
                level_editor.selected_id = 0;

            level_editor.update_selected_sprite();
        }
	}
}
