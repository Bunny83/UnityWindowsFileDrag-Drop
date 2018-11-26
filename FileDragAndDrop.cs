using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using B83.Win32;
using UnityEngine.Events;

public class FileDragAndDrop : MonoBehaviour
{

    public UnityEventFileDropInfo OnFilesDropped;

    Queue<FileDropInfo> importQueue = new Queue<FileDropInfo>();

    void QueueFiles(List<string> files, POINT point)
    {
        importQueue.Enqueue(new FileDropInfo(files, point));
    }

    void LateUpdate()
    {
        if (importQueue.Count > 0)
        {
            var files = importQueue.Dequeue();
            OnFilesDropped.Invoke(files);
        }
    }

    // important to keep the instance alive while the hook is active.
    UnityDragAndDropHook hook;
    void OnEnable()
    {
        // must be created on the main thread to get the right thread id.
        hook = new UnityDragAndDropHook();
        hook.InstallHook();
        hook.OnDroppedFiles += OnFiles;
    }
    void OnDisable()
    {
        hook.UninstallHook();
    }

    void OnFiles(List<string> aFiles, POINT aPos)
    {
        // do something with the dropped file names. aPos will contain the 
        // mouse position within the window where the files has been dropped.
        Debug.Log("Dropped " + aFiles.Count + " files at: " + aPos + "\n" +
                  aFiles.Aggregate((a, b) => a + "\n" + b));
        QueueFiles(aFiles, aPos);
    }

    [Serializable]
    public class FileDropInfo
    {
        public List<string> Files { get; private set; }
        public POINT Point { get; private set; }

        public FileDropInfo(List<string> files, POINT point)
        {
            Files = files;
            Point = point;
        }
    }

    [Serializable]
    public class UnityEventFileDropInfo : UnityEvent<FileDropInfo>
    {
    }
}