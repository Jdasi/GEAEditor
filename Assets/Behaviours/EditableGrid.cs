using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditableGrid : MonoBehaviour
{
    public GameObject editable_tile_prefab;
    public GameObject waypoint_line_prefab;
    public GameObject grid_line_prefab;

    private CameraControls camera_controls;
    private TileSelectionManager tile_selection_manager;

    private List<EditableTile> editable_tiles = new List<EditableTile>();
    private Transform tiles_transform; // For parenting the EditableTile prefabs.
    private Transform grid_lines_transform; // For parenting the GridLine prefabs.

    private Vector3 tile_size; // Size of the Unity tiles.
    private bool spawn_placed; // Indicates if the EditableGrid contains a player spawn tile.

    // The key is the index of the enemy in the EditableTiles array.
    private SortedDictionary<int, Enemy> enemy_waypoints = new SortedDictionary<int, Enemy>();

    private Transform waypoints_transform; // For parenting the WaypointLine prefabs;
    private bool waypoint_mode; // Determines interaction with the EditableGrid.

    void Start()
    {
        camera_controls = GameObject.FindObjectOfType<CameraControls>();
        tile_selection_manager = GameObject.FindObjectOfType<TileSelectionManager>();

        tiles_transform = transform.FindChild("Tiles");
        waypoints_transform = transform.FindChild("WaypointLines");
        grid_lines_transform = transform.FindChild("GridLines");
    }

    void add_enemy(EditableTile tile)
    {
        Enemy enemy = new Enemy();

        enemy.tiles_index = tile.get_tiles_index();
        enemy.start_pos = tile.transform.position;
        enemy.set_waypoint(tile.get_tiles_index(), tile.transform.position);

        enemy_waypoints.Add(enemy.tiles_index, enemy);
    }

    void remove_enemy(int tiles_index)
    {
        foreach (var elem in enemy_waypoints)
        {
            if (elem.Value.tiles_index == tiles_index)
            {
                enemy_waypoints.Remove(elem.Key);
                break;
            }
        }
    }

    // Creates a row of grid squares that overlay the EditableTiles.
    void create_grid_line_row(Vector3 origin, int level_width)
    {
        origin.x -= tile_size.x / 2;
        origin.y -= tile_size.y / 2;
        origin.z = -5;

        GameObject obj = Instantiate(grid_line_prefab, origin, Quaternion.identity) as GameObject;
        obj.transform.SetParent(grid_lines_transform);

        LineRenderer line_renderer = obj.GetComponent<LineRenderer>();
        line_renderer.numPositions = 5 * level_width;

        for (int i = 0; i < line_renderer.numPositions; i += 5)
        {
            Vector3 current_origin = origin + (i * new Vector3(tile_size.x / 5, 0, 0));

            line_renderer.SetPosition(i, current_origin);
            line_renderer.SetPosition(i + 1, current_origin + new Vector3(tile_size.x, 0, 0));
            line_renderer.SetPosition(i + 2, current_origin + tile_size);
            line_renderer.SetPosition(i + 3, current_origin + new Vector3(0, tile_size.y, 0));
            line_renderer.SetPosition(i + 4, current_origin);
        }
    }

    void init_enemy_waypoints(int[] waypoints)
    {
        // Abort if error with waypoints.
        if (waypoints == null)
            return;

        int counter = 0;
        foreach (var elem in enemy_waypoints)
        {
            Enemy enemy = elem.Value;

            enemy.set_waypoint(waypoints[counter], editable_tiles[waypoints[counter]].transform.position);

            ++counter;
        }
    }

    public void reset_grid()
    {
        foreach (var tile in editable_tiles)
        {
            Destroy(tile.gameObject);
        }

        // Clear grid lines.
        foreach (Transform child in grid_lines_transform)
        {
            Destroy(child.gameObject);
        }

        editable_tiles.Clear();
        enemy_waypoints.Clear();

        spawn_placed = false;

        disable_waypoint_mode();
    }

    public void init_grid(JSWLevel level)
    {
        // Clear grid if it has already been initialised.
        reset_grid();

        // Get initial editable sprite prefab and get its size.
        Texture2D texture = editable_tile_prefab.GetComponent<SpriteRenderer>().sprite.texture;

        // Divide size by 100 to match Unity's unit measurements.
        tile_size = new Vector3((float)texture.width / 100, (float)texture.height / 100, 1);

        // Create the grid of tiles based on width & height.
        int area = level.width * level.height;
        for (int i = 0; i < area; ++i)
        {
            Vector3 pos = new Vector3(0 + tile_size.x * (i % level.width), 0 - tile_size.y * (i / level.width), 0);
            GameObject obj = Instantiate(editable_tile_prefab, pos, Quaternion.identity) as GameObject;

            if (i % level.width == 0 && i < area)
                create_grid_line_row(pos, level.width);

            obj.transform.SetParent(tiles_transform);
            obj.name = "Tile" + i;

            EditableTile tile = obj.GetComponent<EditableTile>();
            tile.initialise(i);
            paint_tile(tile, tile_selection_manager.get_tile_type_by_id(level.tile_ids[i]));

            editable_tiles.Add(tile);
        }

        // Match JSWLevel waypoints array.
        init_enemy_waypoints(level.enemy_waypoints);

        // Center camera on the grid.
        camera_controls.reset_camera(new Vector2((level.width * tile_size.x) / 2, -((level.height * tile_size.y) / 2)));
    }

    // Called by init_grid() and whenever the user tries to paint a Tile via EditorControls.
    public void paint_tile(EditableTile tile, TileType tile_type_to_paint)
    {
        // Player spawn can only be placed once.
        if (tile_type_to_paint.id == 1) // 1 = Player Spawn.
        {
            if (spawn_placed)
                return;

            spawn_placed = true;
        } else {
            if (tile.get_tile_type().id == 1)
                spawn_placed = false;
        }

        // Enemies require additional waypoint info.
        if (tile_type_to_paint.id == 4) // 4 = Enemy.
        {
            if (tile.get_tile_type().id == 4)
                return;

            add_enemy(tile);
        } else {
            if (tile.get_tile_type().id == 4)
                remove_enemy(tile.get_tiles_index());
        }

        // If we got this far, paint the tile.
        tile.paint(tile_type_to_paint);
    }

    // Used by LevelManager & FileIO to save the level data.
    public int[] get_tile_ids()
    {
        int[] tile_ids = new int[editable_tiles.Count];

        for (int i = 0; i < editable_tiles.Count; ++i)
        {
            tile_ids[i] = editable_tiles[i].get_tile_type().id;
        }

        return tile_ids;
    }

    // Used by LevelManager & FileIO to save the level data.
    public int[] get_enemy_waypoints()
    {
        int[] waypoints = new int[enemy_waypoints.Count];

        int counter = 0;
        foreach (var elem in enemy_waypoints)
        {
            waypoints[counter] = elem.Value.get_waypoint();

            ++counter;
        }

        return waypoints;
    }

    public bool waypoint_mode_enabled()
    {
        return waypoint_mode;
    }

    // Shows waypoint lines for each enemy.
    public void enable_waypoint_mode()
    {
        waypoint_mode = true;

        foreach (var elem in enemy_waypoints)
        {
            GameObject obj = Instantiate(waypoint_line_prefab, transform.position, Quaternion.identity) as GameObject;
            obj.transform.SetParent(waypoints_transform);
            obj.name = "Line" + elem.Value.tiles_index.ToString();
        }

        refresh_waypoints();

        // Fade all non-enemy tiles.
        foreach (EditableTile tile in editable_tiles)
        {
            if (tile.get_tile_type().id != 4)
                tile.set_faded(true);
        }
    }

    public void disable_waypoint_mode()
    {
        waypoint_mode = false;

        // Destroy all waypoint lines.
        foreach (Transform child in waypoints_transform)
        {
            Destroy(child.gameObject);
        }

        // Remove fade from all tiles.
        foreach (EditableTile tile in editable_tiles)
        {
            tile.set_faded(false);
        }
    }

    // Updates the waypoint lines in case any waypoints have been changed since the lines were created.
    public void refresh_waypoints()
    {
        if (!waypoint_mode)
            return;

        foreach (var elem in enemy_waypoints)
        {
            Vector3 line_start = elem.Value.start_pos;
            Vector3 line_end = elem.Value.get_waypoint_pos();

            line_start.z = -9;
            line_end.z = -9;

            string line_name = "Line" + elem.Value.tiles_index.ToString();
            LineRenderer line = waypoints_transform.FindChild(line_name).GetComponent<LineRenderer>();
            line.SetPosition(0, line_start);
            line.SetPosition(1, line_end);
        }
    }

    public Enemy get_enemy(int tile_index)
    {
        foreach (var elem in enemy_waypoints)
        {
            if (elem.Value.tiles_index == tile_index)
                return elem.Value;
        }

        return null;
    }
}
