using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItem
{
    public int ID;

    public string Name;

    public string Path;

    public int ParentID;

    public int Hierarchy;

    public bool isFolder;

    public void SelectAndHighlight()
    {
        GameManager.HighlightedResourceItem = this;
    }
}
