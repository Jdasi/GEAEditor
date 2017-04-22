using System.Collections;
using UnityEngine;

public class EditableTile : MonoBehaviour
{
    private SpriteRenderer sprite_renderer;
    private TileType tile_type = new TileType();

    // Should be called by a manager class upon instantiation.
	public void initialise()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
	}

    public void paint(TileType type)
    {
        this.tile_type.id = type.id;
        this.tile_type.sprite = type.sprite;

        this.sprite_renderer.sprite = type.sprite;
    }

    public TileType get_tile_type()
    {
        return tile_type;
    }

    public void set_faded(bool fade)
    {
        Color new_color = sprite_renderer.color;
        new_color.a = fade ? 0.8f : 1.0f;

        sprite_renderer.color = new_color;
    }
}
