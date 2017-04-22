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

	void Start()
    {
        level_manager = GameObject.FindObjectOfType<LevelManager>();

        show_menu(main_menu, true);

        width_height_field_changed();
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
}
