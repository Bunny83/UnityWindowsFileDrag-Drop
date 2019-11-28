# UnityWindowsFileDrag&Drop
Adds file drag and drop support for Unity standalone builds on windows.

It uses the GetMessage hook to intercept the WM_DROPFILES message

See the "FileDragAndDrop.cs" or "ImageExample.cs" file for an example usage.

Due to too many issues in the Unity editor (causes random silent crashes) I have disabled the hook when tested in playmode inside the editor. So this feature only works in a build.
