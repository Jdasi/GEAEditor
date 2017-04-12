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
        uint area = grid.width * grid.height;
        int[] tiles = new int[area];

        for (int i = 0; i < area; ++i)
        {
            tiles[i] = grid.tiles[i].get_id();
        }

        JSWLevel level = new JSWLevel(tiles);

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
