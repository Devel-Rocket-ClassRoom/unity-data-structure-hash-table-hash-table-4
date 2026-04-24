using UnityEditor;
using UnityEngine;

public class TestSimpleHash : MonoBehaviour
{
    private OpenAddressingHashTable<string, int> hash = new OpenAddressingHashTable<string, int>();
    void Start()
    { 
        hash.Add("dd", 11);
        hash.Add("ad", 17);
        hash.Add("cd", 16);
        hash.Add("ed", 15);
        hash.Add("fd", 14);
        hash.Add("gd", 13);
        hash.Add("hd", 12);
        hash.Add("hh", 12);
        hash.Add("ha", 12);
        hash.Add("he", 12);
        hash.Add("hq", 12);
        hash.Add("hu", 12);
        hash.Add("ht", 12);
        hash.Add("ho", 12);
        
        foreach(var h in hash)
        {
            Debug.Log(h.Value);
        }
        
        hash.Remove("dd");
        Debug.Log("dd");
        foreach( var h in hash)
        {
            Debug.Log(h.Value);
        }
        hash.Clear();
        Debug.Log("¤·¤·");
        foreach( var h in hash)
        {
            Debug.Log(h.Value);
        }
    }
}
