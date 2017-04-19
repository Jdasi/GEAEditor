using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUITile : MonoBehaviour
{
    private int index = 0; // index of the tile in the GUITileManager list.
    private TileType tile_type; // Type of tile that the GUITile represents.

    private Image image;
    private Outline outline;
    private TileSelectionManager manager;

    // Should be called by a manager class upon instantiation.
    public void initialise()
    {
        image = GetComponent<Image>();
        outline = GetComponent<Outline>();

        GetComponent<Button>().onClick.AddListener(click);
    }

    // Sets the tile representation info of the GUITile.
    public void update_info(TileType type)
    {
        this.tile_type = type;
        this.image.sprite = type.sprite;
    }

    public TileType get_tile_type()
    {
        return tile_type;
    }

    // Informs the GUITile what is managing it and what its index is in that manager's list.
    public void set_manager(TileSelectionManager manager, int index)
    {
        this.manager = manager;
        this.index = index;
    }

    public void click()
    {
        manager.selection_changed_event(index, tile_type.id);
    }

    // Highlights or fades the GUITile based on the passed bool.
    public void is_current_selection(bool current)
    {
        if (current)
        {
            outline.effectColor = Color.red;
            gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.0f);
            image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else
        {
            outline.effectColor = Color.black;
            gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 1.0f);
            image.color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
        }
    }
}
