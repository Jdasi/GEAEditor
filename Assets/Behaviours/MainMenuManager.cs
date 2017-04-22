using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject main_menu;
    public GameObject new_level_menu;
    public GameObject load_level_menu;

    public Button save_button;
    public Button create_button;
    public Button load_button;

    public InputField field_width;
    public InputField field_height;
    public InputField field_description;
    public Dropdown dropdown_name;

    private LevelManager level_manager;
    private EditorControls editor_controls;
    private CameraControls camera_controls;

	void Start()
    {
        level_manager = GameObject.FindObjectOfType<LevelManager>();
        editor_controls = GameObject.FindObjectOfType<EditorControls>();
        camera_controls = GameObject.FindObjectOfType<CameraControls>();

        show_menu(main_menu, true);

        width_height_field_changed();
	}

    void Update()
    {
        editor_controls.controls_enabled = camera_controls.controls_enabled = !is_menu_open();
    }

    bool is_menu_open()
    {
        return main_menu.activeSelf || new_level_menu.activeSelf || load_level_menu.activeSelf;
    }

    void hide_all_menus()
    {
        main_menu.SetActive(false);
        new_level_menu.SetActive(false);
        load_level_menu.SetActive(false);
    }

    void show_menu(GameObject menu, bool show)
    {
        hide_all_menus();

        save_button.interactable = level_manager.get_current_level() != null;            

        menu.SetActive(show);
    }

    public void toggle_main_menu()
    {
        show_menu(main_menu, !main_menu.activeSelf);
    }

    public void button_new_level()
    {
        field_width.text = "";
        field_height.text = "";
        field_description.text = "";

        show_menu(new_level_menu, true);
    }

    public void button_save_level()
    {
        level_manager.save_levels();
    }

    public void button_load_level()
    {
        dropdown_name.ClearOptions();
        dropdown_name.AddOptions(level_manager.get_level_names());

        dropdown_name_changed();

        show_menu(load_level_menu, true);
    }

    public void button_create()
    {
        hide_all_menus();

        int width = int.Parse(field_width.text);
        int height = int.Parse(field_height.text);
        string description = field_description.text;

        level_manager.create_level(width, height, description);
    }

    public void button_load()
    {
        hide_all_menus();

        level_manager.load_level(dropdown_name.options[dropdown_name.value].text);
    }

    public void button_delete()
    {
        // Get the current options.
        var options = dropdown_name.options;

        if (options.Count <= 0)
        {
            dropdown_name.value = 0;
            return;
        }

        // Remove the level entry from the level manager.
        level_manager.delete_level(dropdown_name.options[dropdown_name.value].text);

        // Remove the entry from the dropdown options.
        options.RemoveAt(dropdown_name.value);
        dropdown_name.options = options;

        // Update the dropdown selection.
        dropdown_name.value = dropdown_name.value - 1;
        Mathf.Clamp(dropdown_name.value, 0, options.Count - 1);

        // Update the interactivity of the load menu elements.
        dropdown_name_changed();
    }

    public void button_cancel()
    {
        show_menu(new_level_menu, false);
        show_menu(load_level_menu, false);
        show_menu(main_menu, true);
    }

    public void button_quit()
    {
        Application.Quit();
    }

    public void width_height_field_changed()
    {
        if (field_width.text.Length > 0 && field_height.text.Length > 0)
        {
            if (int.Parse(field_width.text) >= 1 && int.Parse(field_height.text) >= 1)
            {
                create_button.interactable = true;
            }
        }
        else
        {
            create_button.interactable = false;
        }
    }

    public void dropdown_name_changed()
    {
        bool can_load = dropdown_name.options.Count > 0;

        dropdown_name.interactable = can_load;
        load_button.interactable = can_load;
    }
}
