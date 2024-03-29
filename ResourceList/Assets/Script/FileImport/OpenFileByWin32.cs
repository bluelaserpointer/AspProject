﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


public class OpenFileByWin32 : MonoBehaviour
{
    //List<ResourceItem> ResourceItemList => GameManager.ResourceItems;

    public void OpenFile()
    {
        FileOpenDialog dialog = new FileOpenDialog();

        dialog.structSize = Marshal.SizeOf(dialog);

        dialog.filter = "obj files\0*.obj\0All Files\0*.*\0\0";

        dialog.file = new string(new char[256]);

        dialog.maxFile = dialog.file.Length;

        dialog.fileTitle = new string(new char[64]);

        dialog.maxFileTitle = dialog.fileTitle.Length;

        dialog.initialDir = UnityEngine.Application.dataPath;  //默认路径

        dialog.title = "Open File Dialog";

        dialog.defExt = "obj";
        
        dialog.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;  //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

        if (DialogShow.GetOpenFileName(dialog))
        {
            Debug.Log(dialog.file);
            // 插入TreeViewControl
            ResourceItem item = new ResourceItem();
            item.Path = dialog.file;
            item.Name = dialog.fileTitle;
            item.isFolder = false;
            GameManager.ResourceItems.Add(item);
        }
    }
}
