using UnityEditor;
using UnityEngine;

public class TestSimpleHash : MonoBehaviour
{
    private SImpleHashTable<string, int> hash = new SImpleHashTable<string, int>();
    void Start()
    { 
        hash.Add("dd", 11);
        hash.Add("ad", 17);
        hash.Add("cd", 16);
        hash.Add("ed", 15);
        hash.Add("fd", 14);
        hash.Add("gd", 13);
        hash.Add("hd", 12);
        
        foreach(var h in hash)
        {
            Debug.Log(h.Value);
        }
        hash.Add("dd", 20);
    }

}
