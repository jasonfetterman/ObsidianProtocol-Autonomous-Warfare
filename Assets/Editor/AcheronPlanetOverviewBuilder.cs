using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Linq;

public static class AcheronPlanetOverviewBuilder
{
    [MenuItem("Obsidian Protocol/ACHERON/BUILD PLANET OVERVIEW HUD")]
    public static void Build()
    {
        string root=Application.dataPath.Replace("/Assets","");
        string path=Directory.GetFiles(
            root+"/Assets/Scenes","*.unity",
            SearchOption.AllDirectories)
            .FirstOrDefault(x=>Path.GetFileNameWithoutExtension(x)
            .ToUpperInvariant().Contains("ACHERON"));

        if(string.IsNullOrEmpty(path))
        {
            Debug.LogError("ACHERON scene not found.");
            return;
        }

        path=path.Replace("\\","/");
        path=path.Substring(path.IndexOf("Assets/"));

        var scene=EditorSceneManager.OpenScene(path);

        var hud=Find("PLANET OVERVIEW HUD");
        var top=Find("PLANET TOP BAR");
        var info=Find("PLANET INFORMATION PANEL");
        var location=Find("PLANET LOCATION PANEL");
        var actions=Find("PLANET ACTION PANEL");

        if(!hud)
        {
            Debug.LogError("PLANET OVERVIEW HUD not found.");
            return;
        }

        hud.SetActive(true);

        Prepare(hud,.02f,.02f,.96f,.96f);
        Prepare(top,.02f,.88f,.96f,.08f);
        Prepare(info,.02f,.43f,.46f,.40f);
        Prepare(location,.52f,.43f,.46f,.40f);
        Prepare(actions,.02f,.05f,.96f,.30f);

        Style(top);
        Style(info);
        Style(location);
        Style(actions);

        Display(top,
            "ACHERON  //  PLANET OVERVIEW",28);

        Display(info,
            "PLANET INFORMATION\n\n"+
            "PLANET ID       ACHERON\n"+
            "CLASS           PLANETARY WORLD\n"+
            "FACTION         UNCONTROLLED\n"+
            "WARFARE FOCUS   ACTIVE\n"+
            "THREAT LEVEL    HIGH\n"+
            "STATUS          OPERATIONAL",20);

        Display(location,
            "LOCATION\n\n"+
            "GALAXY          CURRENT GALAXY\n"+
            "SECTOR          CURRENT SECTOR\n"+
            "SYSTEM          CURRENT SYSTEM\n"+
            "STAR            CURRENT STAR\n"+
            "ORBIT           STABLE\n"+
            "ORBITAL DISTANCE --\n"+
            "VELOCITY         --",20);

        Display(actions,
            "PLANET ACTIONS",20);

        var controller=hud.GetComponent<AcheronPlanetOverviewController>();
        if(!controller)
            controller=hud.AddComponent<AcheronPlanetOverviewController>();

        controller.information=info;
        controller.location=location;

        Wire("JumpToPlanetOverviewButton",controller,"ShowOverview");
        Wire("ClosePlanetOverview",controller,"CloseOverview");
        Wire("BackToSystem",controller,"CloseOverview");

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("ACHERON EXISTING PLANET HUD BUILD COMPLETE.");
    }

    static GameObject Find(string name)
    {
        return Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x=>x.scene.IsValid() && x.name==name);
    }

    static void Prepare(GameObject o,float x,float y,float w,float h)
    {
        if(!o)return;

        o.SetActive(true);

        var r=o.GetComponent<RectTransform>();
        if(!r)r=o.AddComponent<RectTransform>();

        r.anchorMin=new Vector2(x,y);
        r.anchorMax=new Vector2(x+w,y+h);
        r.offsetMin=Vector2.zero;
        r.offsetMax=Vector2.zero;
    }

    static void Style(GameObject o)
    {
        if(!o)return;

        var image=o.GetComponent<Image>();
        if(!image)image=o.AddComponent<Image>();

        image.color=new Color(.035f,.055f,.085f,.97f);
    }

    static void Display(GameObject parent,string value,int size)
    {
        if(!parent)return;

        var old=parent.transform.Find("ACHERON_DISPLAY");
        if(old)Object.DestroyImmediate(old.gameObject);

        var g=new GameObject("ACHERON_DISPLAY");
        g.transform.SetParent(parent.transform,false);

        var r=g.AddComponent<RectTransform>();
        r.anchorMin=new Vector2(.04f,.08f);
        r.anchorMax=new Vector2(.96f,.92f);
        r.offsetMin=Vector2.zero;
        r.offsetMax=Vector2.zero;

        var t=g.AddComponent<Text>();
        t.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.text=value;
        t.fontSize=size;
        t.color=Color.white;
        t.alignment=TextAnchor.UpperLeft;
        t.horizontalOverflow=HorizontalWrapMode.Wrap;
        t.verticalOverflow=VerticalWrapMode.Overflow;
    }

    static void Wire(string name,
        AcheronPlanetOverviewController controller,
        string method)
    {
        var go=Find(name);
        if(!go)return;

        var button=go.GetComponent<Button>();
        if(!button)return;

        button.onClick.RemoveAllListeners();

        if(method=="ShowOverview")
            button.onClick.AddListener(controller.ShowOverview);

        if(method=="CloseOverview")
            button.onClick.AddListener(controller.CloseOverview);
    }
}

