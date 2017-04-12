using System.Collections;
using UnityEngine;

public class EditableTile : MonoBehaviour
{
    private int id = 0;
    private SpriteRenderer sprite_renderer;

	void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
	}

    public void paint(int id, Sprite sprite)
    {
        this.id = id;
        this.sprite_renderer.sprite = sprite;
    }

    public int get_id()
    {
        return id;
    }

    public Sprite get_sprite()
    {
        return sprite_renderer.sprite;
    }
}
