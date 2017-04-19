using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class LevelManager : MonoBehaviour
{
    private EditableGrid grid;
    private FileIO file_io = new FileIO();

    private Dictionary<string, JSWLevel> levels = new Dictionary<string, JSWLevel>();
    private JSWLevel current_level;

	void Start()
    {
        grid = GameObject.FindObjectOfType<EditableGrid>();

        levels = file_io.load_levels();
    }

    void load_level(string level_name)
    {
        current_level = levels[level_name];

        grid.init_grid(current_level);
    }

    public void save_levels()
    {
        file_io.save_levels(levels);
    }

    public JSWLevel get_current_level()
    {
        return current_level;
    }

    public List<string> get_level_names()
    {
        List<string> level_names = new List<string>();

        foreach (var level in levels)
        {
            level_names.Add(level.Key);
        }

        return level_names;
    }
}
