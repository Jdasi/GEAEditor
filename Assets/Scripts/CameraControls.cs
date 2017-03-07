using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float move_speed = 3.0f;
    public float scroll_speed = 50.0f;
    public float shift_modifier = 3.0f;

    private Camera cam;
    private float current_modifier = 1.0f;
    private float original_zoom;

	void Start()
    {
		cam = GetComponent<Camera>();
        original_zoom = cam.orthographicSize;
	}
	
	void Update()
    {
        handle_modifier();
        handle_movement();
        handle_zoom();
	}

    void handle_modifier()
    {
        if (Input.GetButton("CameraSpeedModifier"))
        {
            current_modifier = shift_modifier;
        }
        else
        {
            current_modifier = 1.0f;
        }
    }

    void handle_movement()
    {
        Vector3 temp = transform.position;

        temp.x += Input.GetAxis("Horizontal") * move_speed * Time.deltaTime * current_modifier;
        temp.y += Input.GetAxis("Vertical") * move_speed * Time.deltaTime * current_modifier;

        transform.position = temp;
    }

    void handle_zoom()
    {
        cam.orthographicSize -= Input.GetAxis("MouseScroll") * scroll_speed * Time.deltaTime * current_modifier;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1, 10);

        if (Input.GetButtonDown("ResetCameraZoom"))
        {
            cam.orthographicSize = original_zoom;
        }
    }
}
