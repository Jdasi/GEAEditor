using System.Collections;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float move_speed = 20.0f;
    public float scroll_speed = 300.0f;
    public float shift_modifier = 3.0f;
    public float drag_speed = 4.0f;

    private MainMenuManager main_menu_manager;
    private float working_drag_speed;

    private float current_modifier = 1.0f;
    private float original_zoom;

    private Vector3 old_cam_pos;
    private Vector3 drag_origin;

	void Start()
    {
        main_menu_manager = GameObject.FindObjectOfType<MainMenuManager>();
        original_zoom = Camera.main.orthographicSize;
	}
	
	void Update()
    {
        if (main_menu_manager.is_menu_open())
            return;

        handle_speed_modifier();
        handle_keyboard_movement();
        handle_mouse_movement();
        handle_zoom();
	}

    // Camera move and scroll speed is based on shift key press.
    void handle_speed_modifier()
    {
        current_modifier = Input.GetButton("CameraSpeedModifier") ? shift_modifier : 1.0f;
    }

    void handle_keyboard_movement()
    {
        Vector3 temp = Camera.main.transform.position;

        temp.x += Input.GetAxis("Horizontal") * move_speed * Time.deltaTime * current_modifier;
        temp.y += Input.GetAxis("Vertical") * move_speed * Time.deltaTime * current_modifier;

        Camera.main.transform.position = temp;
    }

    // Allows dragging of the camera with RMB.
    void handle_mouse_movement()
    {
        working_drag_speed = drag_speed * Camera.main.orthographicSize;

        if (Input.GetMouseButtonDown(1))
        {
            old_cam_pos = Camera.main.transform.position;
            drag_origin = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 drag_difference = Camera.main.ScreenToViewportPoint(Input.mousePosition - drag_origin);
            Vector3 move = new Vector3(drag_difference.x * working_drag_speed, drag_difference.y * (working_drag_speed / 1.5f), 0);

            Camera.main.transform.position = old_cam_pos - move;
        }
    }

    void handle_zoom()
    {
        Camera.main.orthographicSize -= Input.GetAxis("MouseScroll") * scroll_speed * Time.deltaTime * current_modifier;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1, 10);

        if (Input.GetButtonDown("ResetCameraZoom"))
        {
            reset_zoom();
        }
    }

    void reset_zoom()
    {
        Camera.main.orthographicSize = original_zoom;
    }

    public void reset_camera(Vector2 pos)
    {
        Vector3 new_pos = pos;
        new_pos.z = Camera.main.transform.position.z;

        Camera.main.transform.position = new_pos;

        reset_zoom();
    }
}
