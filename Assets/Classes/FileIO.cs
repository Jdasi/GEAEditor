using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class FileIO
{
    public List<JSWLevel> load_levels()
    {
        string str = File.ReadAllText(Application.dataPath + "/levels.json");
        JsonData levels_data = JsonMapper.ToObject(str);

        List<JSWLevel> levels = new List<JSWLevel>();

        for (int i = 0; i < levels_data.Count; ++i) 
        {
            JSWLevel level = new JSWLevel();

            level.name = "level_" + (i + 1).ToString();
            level.width = (int)levels_data[i]["width"];
            level.height = (int)levels_data[i]["height"];
            level.description = (string)levels_data[i]["description"];
            level.tileset = (string)levels_data[i]["tileset"];

            level.tile_ids = new int[levels_data[i]["tiles"].Count];
            for (int j = 0; j < level.width * level.height; ++j)
            {
                level.tile_ids[j] = (int)levels_data[i]["tiles"][j];
            }

            levels.Add(level);
        }

        return levels;
    }

    public void save_levels()
    {
        //JsonData levels_data = JsonMapper.ToJson(settings.get_levels());
        //JSWLevel[] levels = level_manager.get_levels();

        //foreach (var level in levels)
        {
            //string start_text = "\"" + level.name + "\":";
        }

        //File.WriteAllText(Application.dataPath + "/levels.json", levels_data.ToString());
    }
}
