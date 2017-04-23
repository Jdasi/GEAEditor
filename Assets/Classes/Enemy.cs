using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public int tiles_index; // index of the Enemy in the EditableTiles array.
    private int waypoint; // ID of the tile that represents a positional waypoint.

    public Vector3 start_pos; // Used for line render origin.
    private Vector3 waypoint_pos; // Used for line render destination.

    public int get_waypoint()
    {
        return waypoint;
    }

    public void set_waypoint(int _waypoint, Vector3 _waypoint_pos)
    {
        this.waypoint = _waypoint;
        this.waypoint_pos = _waypoint_pos;
    }

    public Vector3 get_waypoint_pos()
    {
        return waypoint_pos;
    }
}
