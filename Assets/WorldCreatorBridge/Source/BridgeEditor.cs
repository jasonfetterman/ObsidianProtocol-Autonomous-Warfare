// Project: WorldCreatorBridge
// Filename: BridgeEditor.cs
// Copyright (c) 2025 BiteTheBytes GmbH. All rights reserved
// *********************************************************

using System;
using System.Globalization;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace BtB.WC.Bridge
{
    [Serializable]
    public class BridgeEditor : EditorWindow, IHasCustomMenu
    {
        #region Fields

        #region Private

        private readonly string[] toolbarItems =
        {
            "Terrain",
            "Objects",
            "About"
        };

        private bool locked;

        private Vector2 scrollPosGeneralTab;
        //private Vector2 scrollPosObjects; 

        private int selectedToolbarItemIndex;
        private Vector2 folderScrollPos;
        
        public string projectFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/World Creator/Sync/bridge.xml";
        
        #endregion

        #region Public

        public static BridgeEditor Window;

        public ParamsBridge pb;

        public ParamsImport pi;

        private Sprite bannerWorldCreator;
        private Sprite logoYouTube;
        private Sprite logoFacebook;
        private Sprite logoTwitter;
        private Sprite logoDiscord;
        private Sprite logoArtstation;
        private Sprite logoInstagram;
        private Sprite logoVimeo;
        private Sprite logoTwitch;
        
        private float spacePixels = 8;

        static AddRequest Request;

        #endregion Public

        #endregion Fields

        #region Methods (Public)

        public void Awake()
        {
            LoadPbSettings();
            LoadPiSettings();
        }

        #region Settings

        private string GetPbDirectory()
        {
            return Application.dataPath + @"/WorldCreatorBridge/Settings";
        }

        private string GetPiDirectory()
        {
            return Application.dataPath + @"/WorldCreatorTerrains/ImportSettings";
        }
        
        private string GetPbFilePath()
        {
            return GetPbDirectory() + "/ParamsBridge.json";
        }

        private string GetPiFilePath()
        {
            return GetPiDirectory() + "/ParamsImport_" + pb.assetName + ".json";
        }

        private void SaveSettings()
        {
            try
            {
                DirectoryInfo target = new DirectoryInfo(GetPbDirectory());

                if (!target.Exists)
                {
                    target.Create();
                    target.Refresh();
                }

                string pbFilePath = GetPbFilePath();
                string dataAsJson = JsonUtility.ToJson(pb);
                File.WriteAllText(pbFilePath, dataAsJson);

                target = new DirectoryInfo(GetPiDirectory());

                if (!target.Exists)
                {
                    target.Create();
                    target.Refresh();
                }

                string piFilePath = GetPiFilePath();
                dataAsJson = JsonUtility.ToJson(pi);
                File.WriteAllText(piFilePath, dataAsJson);
            }
            catch (Exception e)
            {
                Debug.Log("Couldn't save settings: " + e);
            }
        }

        private void LoadPbSettings()
        {
            try
            {
                string pbFilePath = GetPbFilePath();

                if (File.Exists(pbFilePath))
                {
                    string dataAsJson = File.ReadAllText(pbFilePath);
                    pb = JsonUtility.FromJson<ParamsBridge>(dataAsJson);
                }
                else
                {
                    pb = new ParamsBridge();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Couldn't load settings: " + e);
            }
        }

        private void LoadPiSettings()
        {
            try
            {
                string piFilePath = GetPiFilePath();

                if (File.Exists(piFilePath))
                {
                    string dataAsJson = File.ReadAllText(piFilePath);
                    pi = JsonUtility.FromJson<ParamsImport>(dataAsJson);
                }
                else
                {
                    pi = new ParamsImport();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Couldn't load settings: " + e);
            }
        }

        #endregion Settings

        public void OnGUI()
        {
            if(pb == null)
                LoadPbSettings();

            if(pi == null)
                LoadPiSettings();

            EditorGUILayout.BeginVertical("box");
            {
                selectedToolbarItemIndex = GUILayout.Toolbar(selectedToolbarItemIndex, toolbarItems, GUILayout.Height(32));
            }
            
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            {
                scrollPosGeneralTab = GUILayout.BeginScrollView(scrollPosGeneralTab, false, false, GUIStyle.none, GUIStyle.none);
                {
                    switch (selectedToolbarItemIndex)
                    {
                        case 0:
                            DrawTabTerrain();
                            break;
                        case 1:
                            DrawTabObjects();
                            break;
                        case 2:
                            DrawTabAbout();
                            break;
                    }
                }
                
                GUILayout.EndScrollView();
            }
            
            EditorGUILayout.EndVertical();
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fileInfo in source.GetFiles())
            {
                if (fileInfo.Name.Contains(".xml") || fileInfo.Name.Contains("_thumb")) continue;
                fileInfo.CopyTo(Path.Combine(target.FullName, fileInfo.Name), true);
            }

            foreach(DirectoryInfo dirSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(dirSourceSubDir.Name);
                CopyAll(dirSourceSubDir, nextTargetSubDir);
            }
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Lock"), locked, () => { locked = !locked; });
        }

        #endregion Methods (Public)

        #region Methods (Private)

        private void DrawTabTerrain()
        {
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.alignment = TextAnchor.MiddleLeft;
            boxStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.81f, 0.77f, 0.67f) : Color.black;
            boxStyle.stretchWidth = true;

            GUIStyle warningStyle = new GUIStyle(GUI.skin.box);
            warningStyle.alignment = TextAnchor.MiddleLeft;
            warningStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.988f, 0.746f, 0.02f) : Color.black;
            warningStyle.stretchWidth = true;

            // Reset Button
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Reset Settings", GUILayout.Width(160)))
                    pb = new ParamsBridge
                    {
                        bridgeFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/World Creator/Sync/bridge.xml"
                    };
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Box("If you moved the bridge.xml please select it. Its default location is:\n[USER]/Documents/WorldCreator/Sync/bridge.xml", boxStyle);

            GUILayout.Space(spacePixels);

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("SELECT BRIDGE .xml FILE", GUILayout.Height(30)))
                    SelectProjectFolder(pb);
            }
            GUILayout.EndHorizontal();

            GUI.enabled = false;

            folderScrollPos = EditorGUILayout.BeginScrollView(folderScrollPos, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            {
                string path = pb.IsBridgeFileValid() ? pb.bridgeFilePath : projectFolderPath;

                EditorGUILayout.SelectableLabel(path, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            }
            EditorGUILayout.EndScrollView();

            GUI.enabled = true;

            GUILayout.Space(spacePixels);

            GUILayout.BeginHorizontal();
            {
                pb.assetName = EditorGUILayout.TextField(new GUIContent("Terrain Asset Name", "Name of the GameObject container that holds your terrain GameObject(s)."), pb.assetName, GUILayout.ExpandWidth(true));
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(spacePixels);

            GUILayout.BeginHorizontal();
            {
                pb.deleteUnusedAssets = EditorGUILayout.Toggle(new GUIContent("Delete unused Assets", "If enabled automatically cleans up unused terrain assets."), pb.deleteUnusedAssets);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(spacePixels);

            GUILayout.BeginHorizontal();
            {
                pb.isImportLayers = EditorGUILayout.Toggle(new GUIContent("Import Layers", "Choose whether terrain layers are automatically imported. \n If 'false' the terrain uses only a simple texturemap for texturing."), pb.isImportLayers);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(spacePixels);

            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(new GUIContent("Split Threshold", "Specifies when to split the created Unity terrain. This might be important if you want to split your Unity terrain into smaller chunks (e.g. for streaming)."), GUILayout.Width(160));
                pb.userSplit = Mathf.RoundToInt(GUILayout.HorizontalSlider(pb.userSplit, 0, 5));
                GUILayout.Label((pb.UserSplit).ToString(), GUILayout.Width(36));
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(spacePixels);

            GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField) { alignment = TextAnchor.MiddleRight };

            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(new GUIContent("World Scale", "Allows you to scale the terrain to a different value."), GUILayout.Width(160));

                float oldScale = pb.worldScale;
                float newScale = GUILayout.HorizontalSlider(pb.worldScale, 0, 8, GUILayout.ExpandWidth(true));
                if (oldScale != newScale)
                {
                    pb.worldScaleString = newScale.ToString("#0.00");
                    pb.worldScale = newScale;
                }

                string oldString = pb.worldScaleString;
                pb.worldScaleString = GUILayout.TextField(pb.worldScaleString, textFieldStyle, GUILayout.Width(50));

                if (oldString != pb.worldScaleString)
                    if (float.TryParse(pb.worldScaleString, NumberStyles.Any, CultureInfo.InvariantCulture, out float newVal))
                        pb.worldScale = newVal;

                GUILayout.Label("m", GUILayout.Width(14));
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(spacePixels);

            GUILayout.BeginHorizontal();
            {
                pb.materialType = (MaterialType)EditorGUILayout.EnumPopup(new GUIContent("Material Type", "The rendering pipeline for which the terrain material will be set up for. Chose custom to set you own material."), pb.materialType, GUILayout.ExpandWidth(true));
            }
            GUILayout.EndHorizontal();

            if (pb.layerWarning)
            {
                if (pb.materialType == MaterialType.URP || pb.materialType == MaterialType.Standard)
                {
                    GUILayout.Space(spacePixels);

                    GUILayout.Box("Warning - " + (pb.materialType == MaterialType.URP ? "URP" : "The Built-in Render Pipeline") + " uses additional shader passes with more than 4 terrain layers. This can cause performance issues. To avoid this reduce your material layers in World Creator.", warningStyle);

                    GUILayout.Space(spacePixels);
                }
                else if (pb.materialType == MaterialType.HDRP)
                {
                    GUILayout.Space(spacePixels);

                    GUILayout.Box("Warning - HDRP only supports up to 8 terrain layers. Every additional layer will not be rendered. To avoid this reduce your material layers in World Creator.", warningStyle);

                    GUILayout.Space(spacePixels);
                }
            }

            if (pb.materialType == MaterialType.Custom)
            {
                GUILayout.BeginHorizontal();
                {
                    pb.customMaterial = EditorGUILayout.ObjectField("", pb.customMaterial, typeof(Material), false, GUILayout.ExpandWidth(true)) as Material;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(spacePixels);
            
            // Push sync button to bottom
            GUILayout.FlexibleSpace();
            
            DrawSyncTerrainButton();
        }

        private void DrawTabObjects()
        {
            GUIStyle warningStyle = new GUIStyle(GUI.skin.box);
            warningStyle.alignment = TextAnchor.MiddleCenter;
            warningStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.988f, 0.746f, 0.02f) : Color.black;
            warningStyle.stretchWidth = true;

            GUIStyle infoStyle = new GUIStyle(GUI.skin.box);
            infoStyle.alignment = TextAnchor.MiddleLeft;
            infoStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.5f, 0.8f, 1f) : new Color(0f, 0.3f, 0.6f);
            infoStyle.stretchWidth = true;
            infoStyle.wordWrap = true;

            // check if terrain folder is present
            if (!Directory.Exists(Path.Combine(Application.dataPath, pb.terrainsFolderName)))
            {
                GUILayout.Box("Import a terrain first.", warningStyle);
                return;
            }

            // check if unity package is installed
            if (!IsPackageAvailable("com.unity.cloud.gltfast"))
            {
                GUILayout.Box("Since Unity doesn't support GLTF natively we need the GLTFast package. Please install it via the Package Manager. You can either install it manually or using the button below.\n\nPackage ID: com.unity.cloud.gltfast", warningStyle);

                GUILayout.Space(spacePixels);

                // add button
                if (GUILayout.Button("Install com.unity.cloud.gltfast"))
                {
                    Request = Client.Add("com.unity.cloud.gltfast");
                    EditorApplication.update += Progress;
                }
                return;
            }


            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(new GUIContent("Detail Resolution", "Specifies the detail resolution for the created Unity terrain tiles."), GUILayout.Width(160));
                pb.userDetailResolution = Mathf.RoundToInt(GUILayout.HorizontalSlider(pb.userDetailResolution, 0, 8));
                GUILayout.Label((pb.UserDetailResolution).ToString(), GUILayout.Width(36));
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(spacePixels);

            GUILayout.BeginHorizontal();
            {
                pb.overwritePrefabs = EditorGUILayout.Toggle(new GUIContent("Overwrite Prefabs", "If enabled, existing object prefabs will be overwritten."), pb.overwritePrefabs);
            }
            GUILayout.EndHorizontal();
            
            GUILayout.Space(spacePixels);        
            // Push sync button to bottom
            GUILayout.FlexibleSpace();    
            // Show URP-specific info
            if (pb.materialType == MaterialType.URP)
            {
                GUILayout.Box("URP can sometimes cause issues rendering terrain meshes in the scene viewport, supposedly due to an issue in the renderpipeline. If so check your camera view to see your objects.", infoStyle);
                GUILayout.Space(spacePixels);
            }

            GUILayout.Space(spacePixels);


            DrawSyncObjectsButton();
        }

        static void Progress()
        {
            if(Request.IsCompleted)
            {
                if (Request.Status == StatusCode.Success)
                    Debug.Log("Package installed successfully.");
                else
                    Debug.LogError("Failed to install package: " + Request.Error.message);
                EditorApplication.update -= Progress;
            }
        }

        private void DrawTabAbout()
        {
            string spritesFolder = @"Assets/WorldCreatorBridge/Content/Sprites/";

            bannerWorldCreator = AssetDatabase.LoadAssetAtPath<Sprite>(spritesFolder + (EditorGUIUtility.isProSkin ? "banner_wc.png" : "banner_wc_inv.png"));
            logoYouTube = AssetDatabase.LoadAssetAtPath<Sprite>(spritesFolder + (EditorGUIUtility.isProSkin ? "icon_youtube.png" : "icon_youtube_inv.png"));
            logoFacebook = AssetDatabase.LoadAssetAtPath<Sprite>(spritesFolder + (EditorGUIUtility.isProSkin ? "icon_facebook.png" : "icon_facebook_inv.png"));
            logoTwitter = AssetDatabase.LoadAssetAtPath<Sprite>(spritesFolder + (EditorGUIUtility.isProSkin ? "icon_twitter.png" : "icon_twitter_inv.png"));
            logoInstagram = AssetDatabase.LoadAssetAtPath<Sprite>(spritesFolder + (EditorGUIUtility.isProSkin ? "icon_instagram.png" : "icon_instagram_inv.png"));
            logoVimeo = AssetDatabase.LoadAssetAtPath<Sprite>(spritesFolder + (EditorGUIUtility.isProSkin ? "icon_vimeo.png" : "icon_vimeo_inv.png"));
            logoTwitch = AssetDatabase.LoadAssetAtPath<Sprite>(spritesFolder + (EditorGUIUtility.isProSkin ? "icon_twitch.png" : "icon_twitch_inv.png"));
            logoDiscord = AssetDatabase.LoadAssetAtPath<Sprite>(spritesFolder + (EditorGUIUtility.isProSkin ? "icon_discord.png" : "icon_discord_inv.png"));
            logoArtstation = AssetDatabase.LoadAssetAtPath<Sprite>(spritesFolder + (EditorGUIUtility.isProSkin ? "icon_artstation.png" : "icon_artstation_inv.png"));

            if (bannerWorldCreator != null)
                if (GUILayout.Button(bannerWorldCreator.texture))
                    Application.OpenURL("https://www.world-creator.com");

            GUIStyle guiStyleButton = new GUIStyle(GUI.skin.button) { fontSize = 18 };
            GUIStyle styleLegal = new GUIStyle(GUI.skin.box) { richText = true };
            GUILayoutOption[] guiLayoutOptionsHelpLarge = { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) };

            string col = EditorGUIUtility.isProSkin ? "#D0C6AB" : "#000000";

            GUILayout.Box
            ("<color=" + col + ">\nJoin our community on DISCORD and follow us on our social sites \n to get the latest information of the World Creator product series.\n\n" +
             "Get in touch with the devs and share your ideas and suggestions.\n</color>", styleLegal, guiLayoutOptionsHelpLarge);

            GUILayout.BeginHorizontal();
            {
                if (logoDiscord != null)
                    if (GUILayout.Button(logoDiscord.texture))
                        Application.OpenURL("https://discordapp.com/invite/bjMteus");

                if (logoFacebook != null)
                    if (GUILayout.Button(logoFacebook.texture))
                        Application.OpenURL("https://www.facebook.com/worldcreator3d");

                if (logoTwitter != null)
                    if (GUILayout.Button(logoTwitter.texture))
                        Application.OpenURL("https://twitter.com/worldcreator3d");

                if (logoYouTube != null)
                    if (GUILayout.Button(logoYouTube.texture))
                        Application.OpenURL("https://www.youtube.com/channel/UClabqa6PHVjXzR2Y7s1MP0Q");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                if (logoInstagram != null)
                    if (GUILayout.Button(logoInstagram.texture))
                        Application.OpenURL("https://www.instagram.com/worldcreator3d/");

                if (logoVimeo != null)
                    if (GUILayout.Button(logoVimeo.texture))
                        Application.OpenURL("https://vimeo.com/user82114310");

                if (logoTwitch != null)
                    if (GUILayout.Button(logoTwitch.texture))
                        Application.OpenURL("https://www.twitch.tv/worldcreator3d");

                if (logoArtstation != null)
                    if (GUILayout.Button(logoArtstation.texture))
                        Application.OpenURL("https://www.artstation.com/worldcreator");
            }
            GUILayout.EndHorizontal();

            GUILayout.Box("<color=" + col + ">\nWorld Creator Bridge for Unity \nVersion 2.0.0 \n</color>", styleLegal, guiLayoutOptionsHelpLarge);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("COMPANY", guiStyleButton))
                EditorUtility.DisplayDialog(
                    "About - Company",
                    "BiteTheBytes GmbH\n" + "Mainzer Str. 9\n" + "36039 Fulda\n\n" +
                    "Responsible: BiteTheBytes GmbH\n" + "Commercial Register Fulda: HRB 5804\n" +
                    "VAT / Ust-IdNr: DE 272746606", "OK");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("WEBSITE", guiStyleButton))
                {
                    Application.OpenURL("https://www.world-creator.com");
                }

                if (GUILayout.Button("DISCORD", guiStyleButton))
                    Application.OpenURL("https://discordapp.com/invite/bjMteus");
            }
            GUILayout.EndHorizontal();
        }

        private void DrawSyncTerrainButton()
        {
            // Only show the synchronize button when a project folder has been selected
            if (pb.IsBridgeFileValid())
            {
                if (GUILayout.Button("SYNC TERRAIN", GUILayout.Height(50)))
                {
                    if (!File.Exists(pb.bridgeFilePath))
                    {
                        Debug.LogError("Selected file does not exist");
                        return;
                    }

                    // Get the terrain folder
                    string terrainFolder = Application.dataPath + "/" + pb.terrainsFolderName + "/" + pb.assetName + "/assets/";
                    DirectoryInfo target = new DirectoryInfo(terrainFolder);
                    DirectoryInfo source = new DirectoryInfo(pb.bridgeFilePath).Parent;

                    if (source != null && source.Parent != null)
                        source = new DirectoryInfo(source.FullName + "/Assets/");

                    if (pb.deleteUnusedAssets)
                        CleanupFolder("assets");
                    
                    AssetDatabase.StartAssetEditing();
                    CopyAll(source, target);
                    AssetDatabase.StopAssetEditing();
                    AssetDatabase.Refresh();

                    // Copy color map
                    try
                    {
                        foreach (string fileName in Directory.GetFiles(source.Parent.FullName))
                        {
                            if (Path.GetFileName(fileName).Contains("colormap"))
                                File.Copy(fileName, target.Parent + "/" + Path.GetFileName(fileName), true);
                            else if (Path.GetFileName(fileName).Contains("texturemap"))
                                File.Copy(fileName, target.Parent + "/" + Path.GetFileName(fileName), true);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }

                    AssetDatabase.Refresh();

                    if (!pb.IsBridgeFileValid()) return;

                    pi = UnityTerrainUtility.CreateTerrainFromFile(ref pb);
                    
                    // Clear all terrain objects after recreating terrain
                    GameObject container = GameObject.Find(pb.assetName);
                    if (container != null)
                    {
                        foreach (Transform child in container.transform)
                        {
                            Terrain terrain = child.GetComponent<Terrain>();
                            if (terrain != null)
                            {
                                TerrainData terrainData = terrain.terrainData;
                                
                                // Clear detail layers
                                int detailLayerCount = terrainData.detailPrototypes.Length;
                                if (detailLayerCount > 0)
                                {
                                    for (int layer = 0; layer < detailLayerCount; layer++)
                                    {
                                        int[,] emptyDetailMap = new int[terrainData.detailResolution, terrainData.detailResolution];
                                        terrainData.SetDetailLayer(0, 0, layer, emptyDetailMap);
                                    }
                                }
                                
                                // Clear trees
                                terrainData.treeInstances = new TreeInstance[0];
                                
                                // Reset prototypes
                                terrainData.treePrototypes = new TreePrototype[0];
                                terrainData.detailPrototypes = new DetailPrototype[0];
                            }
                        }
                    }

                    // save the settings for the next time the window is used
                    SaveSettings();
                    AssetDatabase.Refresh();
                }
            }
        }

        private void DrawSyncObjectsButton()
        {
            if (pb.IsBridgeFileValid())
            {
                if (GUILayout.Button("SYNC OBJECTS", GUILayout.Height(50)))
                {
                    if (!File.Exists(pb.bridgeFilePath))
                    {
                        Debug.LogError("Selected file does not exist");
                        return;
                    }

                    string objectFolder = Application.dataPath + "/" + pb.terrainsFolderName + "/" + pb.assetName + "/assets/";
                    DirectoryInfo target = new DirectoryInfo(objectFolder);
                    DirectoryInfo source = new DirectoryInfo(pb.bridgeFilePath).Parent;

                    if (source != null && source.Parent != null)
                        source = new DirectoryInfo(source.FullName + "/Assets/");

                    AssetDatabase.StartAssetEditing();
                    CopyAll(source, target);
                    AssetDatabase.StopAssetEditing();
                    AssetDatabase.Refresh();

                    if (!pb.IsBridgeFileValid()) return;
                    UnityObjectUtility.CreateObjectsFromFile(pb, pi);

                    // save the settings for the next time the window is used
                    SaveSettings();
                }
            }
        }
        
        public void SelectProjectFolder(ParamsBridge bp)
        {
#if UNITY_EDITOR

            string path = bp.bridgeFilePath ?? projectFolderPath;

            path = EditorUtility.OpenFilePanel("Select World Creator Bridge XML File", path, "xml");

            if (!string.IsNullOrEmpty(path))
                bp.bridgeFilePath = path;

#endif
        }

        private void CleanupFolder(string sub_folder)
        {
            if (Directory.Exists(@"Assets/" + pb.terrainsFolderName + "/" + pb.assetName + "/" + sub_folder + "/"))
            {
                foreach (string file in Directory.GetFiles(@"Assets/" + pb.terrainsFolderName + "/" + pb.assetName + "/" + sub_folder + "/"))
                    AssetDatabase.DeleteAsset(file);

                foreach (string subdir in Directory.GetDirectories(@"Assets/" + pb.terrainsFolderName + "/" + pb.assetName + "/" + sub_folder + "/"))
                {
                    foreach (string file in Directory.GetFiles(subdir))
                        AssetDatabase.DeleteAsset(file);
                    AssetDatabase.DeleteAsset(subdir);
                }
            }
        }

        // Checks whether the requested package is installed
        private bool IsPackageAvailable(string packageName)
        {
            var packages = UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages();
            foreach (var package in packages)
                if (package.name == packageName)
                    return true;

            return false;
        }

        #endregion Methods (Private)

        #region Methods (Static / Public)

        [MenuItem("Window/World Creator Bridge")]
        public static void Init()
        {
            Window = (BridgeEditor) GetWindow(typeof(BridgeEditor));
            Window.autoRepaintOnSceneChange = true;
            Window.minSize = new Vector2(425, 480);
            Window.titleContent = new GUIContent("World Creator Bridge", AssetDatabase.LoadAssetAtPath<Texture2D>(@"Assets/WorldCreatorBridge/Content/Sprites/" + (EditorGUIUtility.isProSkin ? "icon_wc.png" : "icon_wc_inv.png")));
            Window.Show();
        }
        
        #endregion Methods (Static / Public)
    }
}

#endif