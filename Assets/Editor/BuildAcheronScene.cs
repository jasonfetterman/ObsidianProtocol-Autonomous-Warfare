using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Linq;

public static class BuildAcheronScene
{
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

        var old=GameObject.Find("ACHERON_PLANET_OVERVIEW_BUILT");
        if(old) Object.DestroyImmediate(old);

        var canvasGO=new GameObject("ACHERON_PLANET_OVERVIEW_BUILT");
        var canvas=canvasGO.AddComponent<Canvas>();
        canvas.renderMode=RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder=500;

        var scaler=canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution=new Vector2(1920,1080);
        scaler.matchWidthOrHeight=.5f;

        canvasGO.AddComponent<GraphicRaycaster>();

        var rootPanel=Panel(canvasGO.transform,"HUD_FRAME",
            new Color(.025f,.035f,.055f,.96f));
        Full(rootPanel);

        var top=Panel(canvasGO.transform,"PLANET_TOP_BAR",
            new Color(.055f,.075f,.11f,.98f));
        Rect(top,.025f,.88f,.95f,.085f);
        Text(top,"ACHERON",30,TextAnchor.MiddleLeft);

        var status=Panel(canvasGO.transform,"STATUS",
            new Color(.04f,.055f,.08f,.98f));
        Rect(status,.70f,.90f,.25f,.045f);
        Text(status,"PLANETARY STATUS  //  OPERATIONAL",
            16,TextAnchor.MiddleCenter);

        var info=Panel(canvasGO.transform,"PLANET_INFORMATION_PANEL",
            new Color(.055f,.075f,.11f,.98f));
        Rect(info,.025f,.43f,.455f,.42f);
        Text(info,
            "PLANET INFORMATION\n\n"+
            "PLANET ID       ACHERON\n"+
            "CLASS           PLANETARY WORLD\n"+
            "FACTION         UNCONTROLLED\n"+
            "WARFARE FOCUS   ACTIVE\n"+
            "THREAT LEVEL    HIGH\n"+
            "STATUS          OPERATIONAL",
            20,TextAnchor.UpperLeft);

        var location=Panel(canvasGO.transform,"PLANET_LOCATION_PANEL",
            new Color(.055f,.075f,.11f,.98f));
        Rect(location,.52f,.43f,.455f,.42f);
        Text(location,
            "LOCATION\n\n"+
            "GALAXY          CURRENT GALAXY\n"+
            "SECTOR          CURRENT SECTOR\n"+
            "SYSTEM          CURRENT SYSTEM\n"+
            "STAR            CURRENT STAR\n"+
            "ORBIT            STABLE\n"+
            "ORBITAL DISTANCE  --\n"+
            "VELOCITY          --",
            20,TextAnchor.UpperLeft);

        var actions=Panel(canvasGO.transform,"PLANET_ACTION_PANEL",
            new Color(.045f,.065f,.095f,.99f));
        Rect(actions,.025f,.055f,.95f,.32f);

        Text(actions,"PLANET ACTIONS",20,TextAnchor.UpperLeft);

        Button(actions.transform,"ENTER PLANET",.03f,.10f,.17f,.42f);
        Button(actions.transform,"INTELLIGENCE",.22f,.10f,.17f,.42f);
        Button(actions.transform,"OPERATIONS",.41f,.10f,.17f,.42f);
        Button(actions.transform,"DEPLOYMENT",.60f,.10f,.17f,.42f);
        Button(actions.transform,"LOGISTICS",.79f,.10f,.17f,.42f);

        var close=Button(canvasGO.transform,
            "CLOSE OVERVIEW",.82f,.895f,.15f,.05f);

        var controller=canvasGO.AddComponent<AcheronHUDController>();
        controller.root=canvasGO;
        close.onClick.AddListener(controller.Close);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("ACHERON PLANET OVERVIEW HUD BUILD COMPLETE.");
    }

    static GameObject Panel(Transform parent,string name,Color color)
    {
        var g=new GameObject(name);
        g.transform.SetParent(parent,false);

        g.AddComponent<RectTransform>();

        var img=g.AddComponent<Image>();
        img.color=color;
        img.raycastTarget=true;

        return g;
    }

    static void Full(GameObject g)
    {
        var r=g.GetComponent<RectTransform>();
        r.anchorMin=Vector2.zero;
        r.anchorMax=Vector2.one;
        r.offsetMin=Vector2.zero;
        r.offsetMax=Vector2.zero;
    }

    static void Rect(GameObject g,float x,float y,float w,float h)
    {
        var r=g.GetComponent<RectTransform>();
        r.anchorMin=new Vector2(x,y);
        r.anchorMax=new Vector2(x+w,y+h);
        r.offsetMin=Vector2.zero;
        r.offsetMax=Vector2.zero;
    }

    static Text Text(GameObject parent,string value,int size,TextAnchor anchor)
    {
        var g=new GameObject("TEXT");
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
        t.color=new Color(.85f,.92f,1f,1f);
        t.alignment=anchor;
        t.horizontalOverflow=HorizontalWrapMode.Wrap;
        t.verticalOverflow=VerticalWrapMode.Overflow;

        return t;
    }

    static Button Button(
        Transform parent,string name,
        float x,float y,float w,float h)
    {
        var g=new GameObject(name);
        g.transform.SetParent(parent,false);

        var r=g.AddComponent<RectTransform>();
        r.anchorMin=new Vector2(x,y);
        r.anchorMax=new Vector2(x+w,y+h);
        r.offsetMin=Vector2.zero;
        r.offsetMax=Vector2.zero;

        var image=g.AddComponent<Image>();
        image.color=new Color(.08f,.12f,.17f,1f);

        var b=g.AddComponent<Button>();
        b.targetGraphic=image;

        var text=new GameObject("TEXT");
        text.transform.SetParent(g.transform,false);

        var tr=text.AddComponent<RectTransform>();
        tr.anchorMin=Vector2.zero;
        tr.anchorMax=Vector2.one;
        tr.offsetMin=Vector2.zero;
        tr.offsetMax=Vector2.zero;

        var t=text.AddComponent<Text>();
        t.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.text=name;
        t.fontSize=17;
        t.color=Color.white;
        t.alignment=TextAnchor.MiddleCenter;

        return b;
    }
}

public class AcheronHUDController : MonoBehaviour
{
    public GameObject root;

    public void Close()
    {
        if(root) root.SetActive(false);
    }
}
