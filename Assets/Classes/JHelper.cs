using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JHelper
{
    public static Texture2D load_png(string file_path)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(file_path))
        {
            fileData = File.ReadAllBytes(file_path);
            tex = new Texture2D (2, 2, TextureFormat.RGBA32, false);

            tex.LoadImage(fileData);
        }

        return tex;
    }
}
