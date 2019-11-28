using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using B83.Win32;


public class ImageExample : MonoBehaviour
{
    Texture2D[] textures = new Texture2D[6];
    DropInfo dropInfo = null;
    class DropInfo
    {
        public string file;
        public Vector2 pos;
    }
    void OnEnable ()
    {
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;

    }
    void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();
    }

    void OnFiles(List<string> aFiles, POINT aPos)
    {
        string file = "";
        // scan through dropped files and filter out supported image types
        foreach(var f in aFiles)
        {
            var fi = new System.IO.FileInfo(f);
            var ext = fi.Extension.ToLower();
            if (ext == ".png" || ext == ".jpg" || ext == ".jpeg")
            {
                file = f;
                break;
            }
        }
        // If the user dropped a supported file, create a DropInfo
        if (file != "")
        {
            var info = new DropInfo
            {
                file = file,
                pos = new Vector2(aPos.x, aPos.y)
            };
            dropInfo = info;
        }
    }

    void LoadImage(int aIndex, DropInfo aInfo)
    {
        if (aInfo == null)
            return;
        // get the GUI rect of the last Label / box
        var rect = GUILayoutUtility.GetLastRect();
        // check if the drop position is inside that rect
        if (rect.Contains(aInfo.pos))
        {
            var data = System.IO.File.ReadAllBytes(aInfo.file);
            var tex = new Texture2D(1,1);
            tex.LoadImage(data);
            if (textures[aIndex] != null)
                Destroy(textures[aIndex]);
            textures[aIndex] = tex;
        }
    }

    private void OnGUI()
    {
        DropInfo tmp = null;
        if (Event.current.type == EventType.Repaint && dropInfo!= null)
        {
            tmp = dropInfo;
            dropInfo = null;
        }
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 3; i++)
        {
            if (textures[i] != null)
                GUILayout.Label(textures[i], GUILayout.Width(200), GUILayout.Height(200));
            else
                GUILayout.Box("Drag image here", GUILayout.Width(200), GUILayout.Height(200));
            LoadImage(i, tmp);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 3; i < 6; i++)
        {
            if (textures[i] != null)
                GUILayout.Label(textures[i], GUILayout.Width(200), GUILayout.Height(200));
            else
                GUILayout.Box("Drag image here", GUILayout.Width(200), GUILayout.Height(200));
            LoadImage(i, tmp);
        }
        GUILayout.EndHorizontal();
    }
}
