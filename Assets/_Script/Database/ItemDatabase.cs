using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDatabase : ScriptableObject
{
    public List<ObjectData> objectsData;
}

[Serializable]
public class ItemData
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public string Title { get; private set; }
    [field: SerializeField]
    public int Price { get; private set; }
    [field: SerializeField]
    public Sprite icon { get; private set; }
}
