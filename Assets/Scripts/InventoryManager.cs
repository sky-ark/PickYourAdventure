using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public HashSet<string> _items = new HashSet<string>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public bool HasItem(List<string> items)
    {
        foreach (var item in items)
        {
            if (!_items.Contains(item))
                return false;
        }
        return true;
    }
    public void AddItem(List<string> items)
    {
        foreach (var item in items)
        {
            _items.Add(item);
        }
    }
    public void RemoveItem(List<string> items)
    {
        foreach (var item in items)
        {
            _items.Remove(item); 
        }
    }
    
    public void ClearInventory()
    {
        _items.Clear();
    }
}
