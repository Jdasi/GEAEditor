using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int id = 0;

    private SpriteRenderer sprite_renderer;

	void Start()
    {
		sprite_renderer = GetComponent<SpriteRenderer>();
	}

    public void paint_tile(int id, Sprite sprite)
    {
        this.id = id;
        this.sprite_renderer.sprite = sprite;
    }
}
