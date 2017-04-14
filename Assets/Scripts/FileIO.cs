using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class FileIO : MonoBehaviour
{
    private JsonData level_data;
    private EditableGrid grid;

	void Start()
    {
        grid = GameObject.FindObjectOfType<EditableGrid>();
	}

    public void save_level()
    {
        JSWLevel level = new JSWLevel(grid.get_tile_ids());

        level_data = JsonMapper.ToJson(level);
        File.WriteAllText(Application.dataPath + "/Level.json", level_data.ToString());
    }
}

public class JSWLevel
{
    public int [] tiles;

    public JSWLevel(int[] tiles)
    {
        this.tiles = tiles;
    }
}
