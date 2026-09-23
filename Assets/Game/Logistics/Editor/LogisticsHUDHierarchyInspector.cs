using UnityEditor;
using UnityEngine;
public static class LogisticsHUDHierarchyInspector {
[MenuItem("Obsidian Protocol/Logistics/Inspect HUD")]
static void Inspect() {
var h=GameObject.Find("LOGISTICS HUD");
if(h==null){Debug.LogError("HUD NOT FOUND");return;}
Print(h.transform,0);
}
static void Print(Transform t,int d) {
Debug.Log(new string(' ',d*2)+t.name);
for(int i=0;i<t.childCount;i++) Print(t.GetChild(i),d+1);
}}
