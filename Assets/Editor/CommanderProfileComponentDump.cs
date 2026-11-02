using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;
using System.Collections.Generic;

public static class CommanderProfileComponentDump
{
    [MenuItem("Obsidian Protocol/Inspection/Dump Commander Profile Components")]
    public static void Dump()
    {
        string scenePath =
            "Assets/Scenes/SCN09_COMMANDER_PROFILE/[HUD] COMMANDER PROFILE HUD/Player_Commander_Profile.unity";

        SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

        if (sceneAsset == null)
        {
            Debug.LogError("[COMMANDER PROFILE] Scene asset not found: " + scenePath);
            return;
        }

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("============================================================");
        sb.AppendLine("OBSIDIAN PROTOCOL — SCN09 COMMANDER PROFILE");
        sb.AppendLine("UI COMPONENT / INTERACTION INVENTORY");
        sb.AppendLine("============================================================");
        sb.AppendLine();
        sb.AppendLine("Scene: " + scene.path);
        sb.AppendLine();

        GameObject[] roots = scene.GetRootGameObjects();

        int buttonCount = 0;
        int toggleCount = 0;
        int sliderCount = 0;
        int dropdownCount = 0;
        int inputCount = 0;
        int scrollCount = 0;
        int selectableCount = 0;
        int textCount = 0;
        int imageCount = 0;

        foreach (GameObject root in roots)
        {
            DumpObject(
                root.transform,
                sb,
                0,
                ref buttonCount,
                ref toggleCount,
                ref sliderCount,
                ref dropdownCount,
                ref inputCount,
                ref scrollCount,
                ref selectableCount,
                ref textCount,
                ref imageCount
            );
        }

        sb.AppendLine();
        sb.AppendLine("============================================================");
        sb.AppendLine("SUMMARY");
        sb.AppendLine("============================================================");
        sb.AppendLine("Buttons:      " + buttonCount);
        sb.AppendLine("Toggles:      " + toggleCount);
        sb.AppendLine("Sliders:      " + sliderCount);
        sb.AppendLine("Dropdowns:    " + dropdownCount);
        sb.AppendLine("InputFields:  " + inputCount);
        sb.AppendLine("ScrollRects:  " + scrollCount);
        sb.AppendLine("Selectables:  " + selectableCount);
        sb.AppendLine("TMP Text:     " + textCount);
        sb.AppendLine("Images:       " + imageCount);

        string output =
            "C:/ObsidianProtocol-Autonomous-Warfare/OPAW/" +
            "_HierarchyInspection/09_COMMANDER_PROFILE_COMPONENTS.txt";

        File.WriteAllText(output, sb.ToString());

        Debug.Log(
            "[COMMANDER PROFILE] Component dump complete: " + output
        );

        AssetDatabase.Refresh();
    }

    private static void DumpObject(
        Transform t,
        StringBuilder sb,
        int depth,
        ref int buttonCount,
        ref int toggleCount,
        ref int sliderCount,
        ref int dropdownCount,
        ref int inputCount,
        ref int scrollCount,
        ref int selectableCount,
        ref int textCount,
        ref int imageCount
    )
    {
        GameObject go = t.gameObject;

        List<string> components = new List<string>();

        Button button = go.GetComponent<Button>();
        if (button != null)
        {
            buttonCount++;

            string listeners =
                button.onClick != null
                    ? button.onClick.GetPersistentEventCount().ToString()
                    : "0";

            components.Add("BUTTON listeners=" + listeners);
        }

        Toggle toggle = go.GetComponent<Toggle>();
        if (toggle != null)
        {
            toggleCount++;
            components.Add(
                "TOGGLE value=" + toggle.isOn
            );
        }

        Slider slider = go.GetComponent<Slider>();
        if (slider != null)
        {
            sliderCount++;

            components.Add(
                "SLIDER value=" +
                slider.value +
                " range=" +
                slider.minValue +
                ".." +
                slider.maxValue
            );
        }

        TMP_Dropdown tmpDropdown = go.GetComponent<TMP_Dropdown>();
        if (tmpDropdown != null)
        {
            dropdownCount++;
            components.Add(
                "TMP_DROPDOWN options=" +
                tmpDropdown.options.Count +
                " value=" +
                tmpDropdown.value
            );
        }

        InputField inputField = go.GetComponent<InputField>();
        if (inputField != null)
        {
            inputCount++;
            components.Add("INPUT_FIELD");
        }

        TMP_InputField tmpInput = go.GetComponent<TMP_InputField>();
        if (tmpInput != null)
        {
            inputCount++;
            components.Add("TMP_INPUT_FIELD");
        }

        ScrollRect scrollRect = go.GetComponent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollCount++;
            components.Add("SCROLL_RECT");
        }

        Selectable selectable = go.GetComponent<Selectable>();
        if (selectable != null)
        {
            selectableCount++;

            if (button == null &&
                toggle == null &&
                slider == null &&
                tmpDropdown == null &&
                inputField == null &&
                tmpInput == null)
            {
                components.Add("SELECTABLE");
            }
        }

        TMP_Text tmpText = go.GetComponent<TMP_Text>();
        if (tmpText != null)
        {
            textCount++;

            string text =
                tmpText.text
                    .Replace("\r", " ")
                    .Replace("\n", " ");

            if (text.Length > 100)
                text = text.Substring(0, 100);

            components.Add(
                "TMP_TEXT=\"" + text + "\""
            );
        }

        Image image = go.GetComponent<Image>();
        if (image != null)
        {
            imageCount++;
            components.Add("IMAGE");
        }

        string indent = new string(' ', depth * 4);

        if (components.Count > 0)
        {
            sb.AppendLine(
                indent +
                "├── " +
                go.name +
                "    [" +
                string.Join(" | ", components.ToArray()) +
                "]"
            );
        }
        else
        {
            sb.AppendLine(
                indent +
                "├── " +
                go.name
            );
        }

        for (int i = 0; i < t.childCount; i++)
        {
            DumpObject(
                t.GetChild(i),
                sb,
                depth + 1,
                ref buttonCount,
                ref toggleCount,
                ref sliderCount,
                ref dropdownCount,
                ref inputCount,
                ref scrollCount,
                ref selectableCount,
                ref textCount,
                ref imageCount
            );
        }
    }
}
