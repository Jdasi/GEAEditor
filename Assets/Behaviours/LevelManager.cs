using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class LevelManager : MonoBehaviour
{
    private EditableGrid grid;
    private FileIO file_io = new FileIO();

    private List<JSWLevel> levels = new List<JSWLevel>();
    private JSWLevel current_level;

	void Start()
    {
        grid = GameObject.FindObjectOfType<EditableGrid>();

        enumerate_levels();
	}

	void Update()
    {
		
	}

    void enumerate_levels()
    {
        levels = file_io.load_levels();
        current_level = levels[0];

        grid.init_grid(current_level.width, current_level.height, current_level.tile_ids);
    }

    public void save_levels()
    {
        // pass through levels list to FileIO and save.
    }

    public JSWLevel get_current_level()
    {
        return current_level;
    }
}
