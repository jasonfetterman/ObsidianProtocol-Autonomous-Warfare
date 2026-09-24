using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDictionary : MonoBehaviour
{
    private Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();
    public void addSelected(GameObject go)
    {
        int id = go.GetInstanceID();
        if(!(selectedTable.ContainsKey(id)))
        {
            selectedTable.Add(id, go);
            go.GetComponent<Renderer>().material.color = Color.red;
        }
    }
    public void removeSelected(GameObject go)
    {
        int id = go.GetInstanceID();
        if(selectedTable.ContainsKey(id))
        {
            selectedTable.Remove(id);
            go.GetComponent<Renderer>().material.color = Color.white;
        }
    }
    public void clearSelected()
    {
        foreach(KeyValuePair<int, GameObject> entry in selectedTable)
        {
            entry.Value.GetComponent<Renderer>().material.color = Color.white;
        }
        selectedTable.Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
