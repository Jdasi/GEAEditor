using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public int columns = 10;
    public int rows = 10;
    public GameObject tile_prefab;
    public List<GameObject> list = new List<GameObject>();

    private Vector2 tile_size;

	void Start()
    {
        var blah = tile_prefab.GetComponent<SpriteRenderer>().sprite.texture;
        float tile_width = blah.width;
        float tile_height = blah.height;
        tile_size = new Vector2(tile_width / 100, tile_height / 100);

        int area = columns * rows;
        for (int i = 0; i < area; ++i)
        {
            Vector3 pos = new Vector3(0 + tile_size.x * (i % columns),
                                      0 - tile_size.y * (i / columns),
                                      0);

            GameObject obj = Instantiate(tile_prefab, pos, Quaternion.identity) as GameObject;
            list.Add(obj);
        }
	}
	
	void Update()
    {
		
	}
}
