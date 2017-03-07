using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    public int columns = 10;
    public int rows = 10;
    public GameObject tile_prefab;
    public List<Tile> list = new List<Tile>();

    public int selected_id = 0;
    public Sprite selected_sprite;
    public List<Sprite> sprites = new List<Sprite>();

    private Vector2 tile_size;
    private Image selected_sprite_image;

	void Start()
    {
        var sprite = tile_prefab.GetComponent<SpriteRenderer>().sprite.texture;
        float tile_width = sprite.width;
        float tile_height = sprite.height;
        tile_size = new Vector2(tile_width / 100, tile_height / 100);

        int area = columns * rows;
        for (int i = 0; i < area; ++i)
        {
            Vector3 pos = new Vector3(0 + tile_size.x * (i % columns),
                                      0 - tile_size.y * (i / columns),
                                      0);

            GameObject obj = Instantiate(tile_prefab, pos, Quaternion.identity) as GameObject;
            list.Add(obj.GetComponent<Tile>());
        }

        selected_sprite_image = GameObject.Find("SelectedTile").GetComponent<Image>();

        update_selected_sprite(0);
	}
	
    public void update_selected_sprite(int id)
    {
        this.selected_id = id;
        this.selected_sprite = sprites[id];

        selected_sprite_image.sprite = selected_sprite;
    }

    public void update_selected_sprite()
    {
        update_selected_sprite(selected_id);
    }
}
