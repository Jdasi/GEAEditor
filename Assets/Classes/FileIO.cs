using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class FileIO
{
    List<string> get_object_keys(JsonData data)
    {
        List<string> keys = new List<string>();

        foreach (string key in data.Keys)
        {
            keys.Add(key);
        }

        return keys;
    }

    public SortedDictionary<string, JSWLevel> load_levels()
    {
        string file_name = Application.dataPath + "/levels.json";

        if (!File.Exists(file_name))
            return new SortedDictionary<string, JSWLevel>();

        JsonData levels_data = JsonMapper.ToObject(File.ReadAllText(file_name));
        List<string> keys = get_object_keys(levels_data);

        SortedDictionary<string, JSWLevel> levels = new SortedDictionary<string, JSWLevel>();

        for (int level_index = 0; level_index < levels_data.Count; ++level_index)
        {
            JSWLevel new_level = new JSWLevel();

            new_level.width = (int)levels_data[level_index]["width"];
            new_level.height = (int)levels_data[level_index]["height"];
            new_level.description = (string)levels_data[level_index]["description"];
            new_level.tileset = (string)levels_data[level_index]["tileset"];

            // Get all tile ids.
            new_level.tile_ids = new int[levels_data[level_index]["tile_ids"].Count];
            for (int id_index = 0; id_index < new_level.width * new_level.height; ++id_index)
            {
                new_level.tile_ids[id_index] = (int)levels_data[level_index]["tile_ids"][id_index];
            }

            // Get all enemy waypoints.
            new_level.enemy_waypoints = new int[levels_data[level_index]["enemy_waypoints"].Count];
            for (int id_index = 0; id_index < levels_data[level_index]["enemy_waypoints"].Count; ++id_index)
            {
                new_level.enemy_waypoints[id_index] = (int)levels_data[level_index]["enemy_waypoints"][id_index];
            }

            levels.Add(keys[level_index], new_level);
        }

        return levels;
    }

    public void save_levels(SortedDictionary<string, JSWLevel> levels)
    {
        JsonData levels_data = JsonMapper.ToJson(levels);
        File.WriteAllText(Application.dataPath + "/levels.json", levels_data.ToString());
    }
}
