using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUITile : MonoBehaviour
{
    private int index = 0; // index of the tile in the GUITileManager list.
    private int tile_id = 0; // id of the tile that the GUITile represents.

    private Image image;
    private Outline outline;
    private TileSelectionManager manager;

    public void initialise()
    {
        image = GetComponent<Image>();
        outline = GetComponent<Outline>();

        GetComponent<Button>().onClick.AddListener(click);
    }
	
    public void update_info(int tile_id, Sprite sprite)
    {
        this.tile_id = tile_id;
        this.image.sprite = sprite;
    }

    public int get_tile_id()
    {
        return tile_id;
    }

    public void set_manager(TileSelectionManager manager, int index)
    {
        this.manager = manager;
        this.index = index;
    }

    public void click()
    {
        manager.selection_changed_event(index, tile_id);
    }

    public void set_outline_enabled(bool enabled)
    {
        outline.enabled = enabled;
    }
}
