using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class CommanderProfileController : MonoBehaviour
{
    private readonly List<GameObject> pages = new List<GameObject>();
    private GameObject doctrineWindow;

    private void Awake()
    {
        CachePages();
        CacheDoctrineWindow();
        WireNavigation();
        ShowPage(0);
        CloseDoctrine();
    }

    private void CachePages()
    {
        pages.Clear();

        string[] names =
        {
            "PAGE_PROFILE",
            "PAGE_RANK",
            "PAGE_PROGRESSION",
            "PAGE_DOCTRINE",
            "PAGE_STATISTICS",
            "PAGE_ACHIEVEMENTS",
            "PAGE_CAREER"
        };

        foreach (string pageName in names)
        {
            GameObject page = GameObject.Find(pageName);

            if (page != null)
                pages.Add(page);
        }
    }

    private void CacheDoctrineWindow()
    {
        doctrineWindow = GameObject.Find("WINDOW_DOCTRINE_LIBRARY");
    }

    private void WireNavigation()
    {
        WireButton("BUTTON_NAV_PROFILE", delegate { ShowPage(0); });
        WireButton("BUTTON_NAV_RANK", delegate { ShowPage(1); });
        WireButton("BUTTON_NAV_PROGRESSION", delegate { ShowPage(2); });
        WireButton("BUTTON_NAV_DOCTRINE", delegate { ShowPage(3); });
        WireButton("BUTTON_NAV_STATISTICS", delegate { ShowPage(4); });
        WireButton("BUTTON_NAV_ACHIEVEMENTS", delegate { ShowPage(5); });
        WireButton("BUTTON_NAV_CAREER", delegate { ShowPage(6); });

        WireButton("BUTTON_DOCTRINE_LIBRARY", OpenDoctrine);
        WireButton("BUTTON_DOCTRINE_CLOSE", CloseDoctrine);

        WireButton("BUTTON_BACK", delegate
        {
            ShowPage(0);
        });
    }

    private void WireButton(string objectName, UnityEngine.Events.UnityAction action)
    {
        GameObject go = GameObject.Find(objectName);

        if (go == null)
            return;

        Button button = go.GetComponent<Button>();

        if (button == null)
            return;

        button.onClick.AddListener(action);
    }

    public void ShowPage(int index)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            if (pages[i] != null)
                pages[i].SetActive(i == index);
        }

        UpdateNavigationState(index);
    }

    private void UpdateNavigationState(int activeIndex)
    {
        string[] names =
        {
            "BUTTON_NAV_PROFILE",
            "BUTTON_NAV_RANK",
            "BUTTON_NAV_PROGRESSION",
            "BUTTON_NAV_DOCTRINE",
            "BUTTON_NAV_STATISTICS",
            "BUTTON_NAV_ACHIEVEMENTS",
            "BUTTON_NAV_CAREER"
        };

        for (int i = 0; i < names.Length; i++)
        {
            GameObject go = GameObject.Find(names[i]);

            if (go == null)
                continue;

            Image image = go.GetComponent<Image>();

            if (image == null)
                continue;

            image.color = i == activeIndex
                ? new Color(0.05f, 0.34f, 0.42f, 1f)
                : new Color(0.025f, 0.045f, 0.058f, 1f);
        }
    }

    private void OpenDoctrine()
    {
        if (doctrineWindow != null)
            doctrineWindow.SetActive(true);
    }

    private void CloseDoctrine()
    {
        if (doctrineWindow != null)
            doctrineWindow.SetActive(false);
    }
}