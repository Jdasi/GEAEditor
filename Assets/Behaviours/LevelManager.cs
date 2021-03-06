﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class LevelManager : MonoBehaviour
{
    public Text level_text;

    private EditableGrid editable_grid;
    private FileIO file_io = new FileIO();

    private SortedDictionary<string, JSWLevel> levels;
    private JSWLevel current_level;

	void Start()
    {
        editable_grid = GameObject.FindObjectOfType<EditableGrid>();

        levels = file_io.load_levels();
    }

    string brute_force_level_name()
    {
        // Level names start from level_1, level_2, level_3, etc.
        int counter = 1;
        string level_name_prefix = "level_";

        // Brute force level name.
        while (levels.ContainsKey(level_name_prefix + (counter).ToString())) {++counter;}

        return level_name_prefix + counter.ToString();
    }

    public void load_level(string level_name)
    {
        current_level = levels[level_name];
        level_text.text = level_name;

        editable_grid.init_grid(current_level);
    }

    public void delete_level(string level_name)
    {
        if (levels[level_name] == current_level)
        {
            current_level = null;
            level_text.text = "No level loaded";

            editable_grid.reset_grid();
        }

        levels.Remove(level_name);

        save_levels();
    }

    public void save_levels()
    {
        if (current_level != null)
        {
            current_level.tile_ids = editable_grid.get_tile_ids();
            current_level.enemy_waypoints = editable_grid.get_enemy_waypoints();
        }

        file_io.save_levels(levels);
    }

    public JSWLevel get_current_level()
    {
        return current_level;
    }

    public void create_level(int width, int height, string description)
    {
        JSWLevel level = new JSWLevel();

        level.width = width;
        level.height = height;
        level.description = description;
        level.tileset = "Tileset_1";
        level.tile_ids = new int[width * height];

        string level_name = brute_force_level_name();
        levels.Add(level_name, level);

        save_levels();

        load_level(level_name);
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
