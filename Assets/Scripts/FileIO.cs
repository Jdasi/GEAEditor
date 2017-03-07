using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class FileIO : MonoBehaviour
{
    public JSWLevel level = new JSWLevel(new int[] { 0, 0, 1, 0, 2, 3, 4 });
    JsonData level_data;

	void Start()
    {
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
