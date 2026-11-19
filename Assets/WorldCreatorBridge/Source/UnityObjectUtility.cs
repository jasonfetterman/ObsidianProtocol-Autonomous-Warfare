using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace BtB.WC.Bridge
{
    public static class UnityObjectUtility
    {
        #region Methods (Static / Public)

        public static void CreateObjectsFromFile(ParamsBridge pb, ParamsImport pi)
        {
            string syncDirectory = Path.GetDirectoryName(pb.bridgeFilePath) + "/";
            
            // If overwritePrefabs is enabled, delete all existing prefabs before creating new ones
            if (pb.overwritePrefabs)
            {
                string prefabFolder = "Assets/WorldCreatorTerrains/" + pb.assetName + "/obj_prefab/";
                
                // Convert to absolute path for filesystem operations
                string absolutePrefabFolder = Path.Combine(Application.dataPath, "..", prefabFolder);
                absolutePrefabFolder = Path.GetFullPath(absolutePrefabFolder);
                
                if (Directory.Exists(absolutePrefabFolder))
                {
                    string[] existingPrefabs = Directory.GetFiles(absolutePrefabFolder, "*.prefab");
                    foreach (string prefabPath in existingPrefabs)
                    {
                        string assetPath = prefabPath.Replace('\\', '/');
                        if (assetPath.StartsWith(Application.dataPath))
                            assetPath = "Assets" + assetPath.Substring(Application.dataPath.Length);
                        AssetDatabase.DeleteAsset(assetPath);
                    }
                    Debug.Log($"Overwrite Prefabs enabled - deleted {existingPrefabs.Length} existing prefabs");
                }
            }

            GameObject container = GameObject.Find(pb.assetName);
            
            // Clear all existing terrain objects before syncing
            foreach (Transform child in container.transform)
            {
                Terrain terrain = child.GetComponent<Terrain>();
                if (terrain != null)
                {
                    TerrainData terrainData = terrain.terrainData;
                    
                    // Set detail resolution from ParamsBridge
                    terrainData.SetDetailResolution(pb.UserDetailResolution, 16);
                    
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
            
            Debug.Log("=== Starting Object Sync ===");
            
            // ========== FIRST PASS: Process instance_info.xml ==========
            // Point cloud objects - create prefab, add as tree prototype, place instances immediately
            ProcessInstanceInfoObjectsIncremental(pb, pi, container, syncDirectory);
            
            // ========== SECOND PASS: Process detail_info.xml ==========
            // Density-based objects - create prefab, add as tree/detail prototype, place instances immediately
            ProcessDetailInfoObjectsIncremental(pb, pi, container, syncDirectory);
            
            Debug.Log("=== Object Sync Complete ===");
        }
        
        /// <summary>
        /// Processes instance_info.xml incrementally - creates prefab, adds as tree prototype, places instances immediately
        /// Returns total number of instances placed
        /// </summary>
        private static void ProcessInstanceInfoObjectsIncremental(ParamsBridge pb, ParamsImport pi, GameObject container, string syncDirectory)
        {
            string instanceInfoPath = syncDirectory + "instance_info.xml";
            
            if (!File.Exists(instanceInfoPath))
            {
                Debug.Log("No instance_info.xml found - skipping point cloud objects");
                return;
            }
            
            Debug.Log("Processing instance_info.xml objects...");
            
            XmlDocument doc = new XmlDocument();
            try
            {
                string xmlContent = File.ReadAllText(instanceInfoPath, System.Text.Encoding.UTF8);
                doc.LoadXml(xmlContent);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load instance_info.xml: {ex.Message}");
                return;
            }
            
            // Get both BiomeObjectList (scattered objects) and SceneObjectList (placed objects)
            XmlNodeList biomeObjectLists = doc.GetElementsByTagName("BiomeObjectList");
            XmlNodeList sceneObjectLists = doc.GetElementsByTagName("SceneObjectList");
            
            // Combine both lists
            List<XmlNode> allObjectLists = new List<XmlNode>();
            foreach (XmlNode node in biomeObjectLists)
                allObjectLists.Add(node);
            foreach (XmlNode node in sceneObjectLists)
                allObjectLists.Add(node);
            
            int totalInstancesPlaced = 0;
            int totalEntitiesPlaced = 0;
            
            // Create "Entities" parent container for all entity GameObjects
            GameObject entitiesContainer = null;

            foreach (XmlNode instanceList in allObjectLists)
            {
                string objectName = instanceList["Name"]?.InnerText;
                
                // Try ObjectInfo first, then SubobjectInfo (for child objects)
                XmlNode objectInfo = instanceList["ObjectInfo"];
                if (objectInfo == null)
                    objectInfo = instanceList["SubobjectInfo"];

                if (objectInfo == null || string.IsNullOrEmpty(objectName))
                {
                    Debug.LogWarning("BiomeObjectList entry missing Name or ObjectInfo/SubobjectInfo node");
                    continue;
                }

                // Get the object path
                string objectPath = objectInfo["Path"]?.InnerText;
                if (string.IsNullOrEmpty(objectPath))
                {
                    Debug.LogWarning($"Point cloud object '{objectName}' has no Path");
                    continue;
                }

                // Check if this object should be created as entities (individual GameObjects)
                bool isEntity = false;
                XmlNode isEntityNode = objectInfo["IsEntity"];
                if (isEntityNode != null)
                    bool.TryParse(isEntityNode.InnerText, out isEntity);

                // Use the filename with its original extension
                string fileName = Path.GetFileName(objectPath);
                string meshPath = "Assets/WorldCreatorTerrains/" + pb.assetName + "/assets/" + fileName;

                // Check if the mesh asset exists
                GameObject testAsset = AssetDatabase.LoadAssetAtPath<GameObject>(meshPath);
                if (testAsset == null)
                {
                    Debug.LogWarning($"Point cloud mesh asset not found at: {meshPath}");
                    continue;
                }

                // Generate prefab with "_Instance" suffix (point cloud objects use CSV scaling)
                string modelFileName = Path.GetFileNameWithoutExtension(objectPath);
                string prefabName = objectName + "_" + modelFileName + "_Instance";
                
                // Get ModelScale from XML
                float modelScale = 1f;
                XmlNode modelScaleNode = objectInfo["ModelScale"];
                if (modelScaleNode != null && float.TryParse(modelScaleNode.InnerText, out float scale))
                    modelScale = scale; // Convert from World Creator scale to Unity scale
                
                GameObject meshObject = GeneratePrefabFromInstanceInfo(pb, meshPath, prefabName, objectName, objectInfo, isEntity);

                if (meshObject == null)
                    continue;

                // Get ScaleRange to normalize CSV scale values
                float bakedScaleAverage = 1f;
                bool hasScaleRange = false;
                XmlNode scaleRangeNode = objectInfo["ScaleRange"];
                if (scaleRangeNode != null)
                {
                    float minScale = 1f, maxScale = 1f;
                    XmlNode minNode = scaleRangeNode["Min"];
                    XmlNode maxNode = scaleRangeNode["Max"];
                    if (minNode != null && float.TryParse(minNode.InnerText, out minScale))
                        hasScaleRange = true;
                    if (maxNode != null && float.TryParse(maxNode.InnerText, out maxScale))
                        hasScaleRange = true;
                    
                    if (hasScaleRange)
                        bakedScaleAverage = (minScale + maxScale) / 2f;
                }

                if (isEntity)
                {
                    // Create "Entities" parent container if it doesn't exist yet
                    if (entitiesContainer == null)
                    {
                        entitiesContainer = new GameObject("Entities");
                        entitiesContainer.transform.parent = container.transform;
                        entitiesContainer.transform.localPosition = Vector3.zero;
                        entitiesContainer.transform.localRotation = Quaternion.identity;
                        entitiesContainer.transform.localScale = Vector3.one;
                    }

                    // Create a single parent GameObject for all instances of this object type
                    GameObject instanceContainer = new GameObject(objectName + "_Instances");
                    instanceContainer.transform.parent = entitiesContainer.transform;
                    instanceContainer.transform.localPosition = Vector3.zero;
                    instanceContainer.transform.localRotation = Quaternion.identity;
                    instanceContainer.transform.localScale = Vector3.one;

                    // Process ALL InstanceDataFiles entries (one per terrain tile)
                    XmlNodeList csvFilesList = instanceList.SelectNodes("InstanceDataFiles");
                    if (csvFilesList != null && csvFilesList.Count > 0)
                    {
                        foreach (XmlNode csvFileNode in csvFilesList)
                        {
                            string csvFileName = csvFileNode["FileName"]?.InnerText;
                            if (!string.IsNullOrEmpty(csvFileName))
                            {
                                string csvPath = syncDirectory + csvFileName;
                                if (File.Exists(csvPath))
                                {
                                    int instanceCount = LoadInstancesFromCSVAsEntities(csvPath, meshObject, instanceContainer, pb, pi, bakedScaleAverage, objectName, hasScaleRange, modelScale);
                                    totalEntitiesPlaced += instanceCount;
                                }
                                else
                                {
                                    Debug.LogWarning($"CSV file not found: {csvPath}");
                                }
                            }
                        }
                    }
                }
                else
                {
                    // IsEntity=false: Use terrain tree system
                    
                    // Add as tree prototype to all terrains incrementally
                    TreePrototype newPrototype = new TreePrototype
                    {
                        prefab = meshObject,
                        bendFactor = 0f
                    };

                    int prototypeIndex = -1;
                    foreach (Transform child in container.transform)
                    {
                        Terrain terrain = child.GetComponent<Terrain>();
                        if (terrain != null)
                        {
                            TerrainData terrainData = terrain.terrainData;
                            TreePrototype[] currentPrototypes = terrainData.treePrototypes;
                            TreePrototype[] updatedPrototypes = new TreePrototype[currentPrototypes.Length + 1];
                            currentPrototypes.CopyTo(updatedPrototypes, 0);
                            updatedPrototypes[currentPrototypes.Length] = newPrototype;
                            terrainData.treePrototypes = updatedPrototypes;

                            if (prototypeIndex == -1)
                                prototypeIndex = currentPrototypes.Length;
                        }
                    }

                    // Process ALL InstanceDataFiles entries (one per terrain tile)
                    XmlNodeList csvFilesList = instanceList.SelectNodes("InstanceDataFiles");
                    if (csvFilesList != null && csvFilesList.Count > 0)
                    {
                        foreach (XmlNode csvFileNode in csvFilesList)
                        {
                            string csvFileName = csvFileNode["FileName"]?.InnerText;
                            if (!string.IsNullOrEmpty(csvFileName))
                            {
                                string csvPath = syncDirectory + csvFileName;
                                if (File.Exists(csvPath))
                                {
                                    int instanceCount = LoadInstancesFromCSV(csvPath, prototypeIndex, container, pb, pi, bakedScaleAverage);
                                    totalInstancesPlaced += instanceCount;
                                }
                                else
                                {
                                    Debug.LogWarning($"    CSV file not found: {csvPath}");
                                }
                            }
                        }
                    }
                }
            }
            
            Debug.Log($"Instance sync complete: {totalInstancesPlaced} terrain tree instances, {totalEntitiesPlaced} entity GameObjects");
        }
        
        /// <summary>
        /// Processes detail_info.xml incrementally - creates prefab, adds as tree/detail prototype, places instances immediately
        /// Returns total number of detail instances placed, outputs tree instances placed separately
        /// </summary>
        private static void ProcessDetailInfoObjectsIncremental(ParamsBridge pb, ParamsImport pi, GameObject container, string syncDirectory)
        {
            int treeInstancesPlaced = 0;
            int detailLayersCreated = 0;
            string detailInfoPath = syncDirectory + "detail_info.xml";
            
            if (!File.Exists(detailInfoPath))
            {
                Debug.Log("No detail_info.xml found - skipping density-based objects");
                return;
            }
            
            Debug.Log("Processing detail_info.xml objects...");
            
            XmlDocument doc = new XmlDocument();
            try
            {
                string xmlContent = File.ReadAllText(detailInfoPath, System.Text.Encoding.UTF8);
                doc.LoadXml(xmlContent);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load detail_info.xml: {ex.Message}");
                return;
            }

            XmlNodeList detailLists = doc.GetElementsByTagName("DetailList");

            foreach (XmlNode detailList in detailLists)
            {
                // Try ObjectInfo first, then SubobjectInfo (for child objects)
                XmlNode objectInfo = detailList["ObjectInfo"];
                if (objectInfo == null)
                    objectInfo = detailList["SubobjectInfo"];
                    
                string objectName = detailList["Name"]?.InnerText;

                if (objectInfo == null || string.IsNullOrEmpty(objectName))
                    continue;

                // Get the object path and convert to Unity format
                string objectPath = objectInfo["Path"]?.InnerText;
                if (string.IsNullOrEmpty(objectPath))
                    continue;

                // Use the filename with its original extension
                string fileName = Path.GetFileName(objectPath);
                string meshPath = "Assets/WorldCreatorTerrains/" + pb.assetName + "/assets/" + fileName;

                // Check if the mesh asset exists
                GameObject testAsset = AssetDatabase.LoadAssetAtPath<GameObject>(meshPath);
                if (testAsset == null)
                {
                    Debug.LogWarning($"Mesh asset not found at: {meshPath}");
                    continue;
                }

                // Check material count on the asset to determine type
                MeshRenderer[] renderers = testAsset.GetComponentsInChildren<MeshRenderer>(true);
                int materialCount = 0;
                foreach (MeshRenderer renderer in renderers)
                    if (renderer != null && renderer.sharedMaterials != null)
                        materialCount = Mathf.Max(materialCount, renderer.sharedMaterials.Length);

                bool useAsTree = materialCount > 1;
                string typeSuffix = useAsTree ? "_DetailMultiMat" : "_Detail";

                // Generate prefab with type suffix
                string modelFileName = Path.GetFileNameWithoutExtension(objectPath);
                string prefabName = objectName + "_" + modelFileName + typeSuffix;
                GameObject meshObject = GeneratePrefab(pb, meshPath, prefabName, objectName);

                if (meshObject == null)
                    continue;

                // Extract properties from XML
                bool alignToNormal = false;
                XmlNode alignToNormalNode = objectInfo["AlignToNormal"];
                if (alignToNormalNode != null)
                    bool.TryParse(alignToNormalNode.InnerText, out alignToNormal);

                float minScale = 1f, maxScale = 1f;
                XmlNode scaleRangeNode = objectInfo["ScaleRange"];
                if (scaleRangeNode != null)
                {
                    XmlNode minNode = scaleRangeNode["Min"];
                    XmlNode maxNode = scaleRangeNode["Max"];
                    if (minNode != null) float.TryParse(minNode.InnerText, out minScale);
                    if (maxNode != null) float.TryParse(maxNode.InnerText, out maxScale);
                }

                int prototypeIndex = -1;

                if (useAsTree)
                {
                    // Add as tree prototype to all terrains
                    TreePrototype newPrototype = new TreePrototype
                    {
                        prefab = meshObject,
                        bendFactor = 0f
                    };

                    foreach (Transform child in container.transform)
                    {
                        Terrain terrain = child.GetComponent<Terrain>();
                        if (terrain != null)
                        {
                            TerrainData terrainData = terrain.terrainData;
                            TreePrototype[] currentPrototypes = terrainData.treePrototypes;
                            TreePrototype[] updatedPrototypes = new TreePrototype[currentPrototypes.Length + 1];
                            currentPrototypes.CopyTo(updatedPrototypes, 0);
                            updatedPrototypes[currentPrototypes.Length] = newPrototype;
                            terrainData.treePrototypes = updatedPrototypes;

                            if (prototypeIndex == -1)
                                prototypeIndex = currentPrototypes.Length;
                        }
                    }
                }
                else
                {
                    // Add as detail prototype to all terrains
                    DetailPrototype newPrototype = new DetailPrototype
                    {
                        alignToGround = alignToNormal ? 1f : 0f,
                        usePrototypeMesh = true,
                        prototype = meshObject,
                        minWidth = minScale,
                        maxWidth = maxScale,
                        minHeight = minScale,
                        maxHeight = maxScale,
                        density = 1f,
                        renderMode = DetailRenderMode.VertexLit,
                        useInstancing = true,
                        useDensityScaling = true,
                        healthyColor = Color.white,
                        dryColor = new Color(0.8f, 0.8f, 0.4f, 1f)
                    };

                    foreach (Transform child in container.transform)
                    {
                        Terrain terrain = child.GetComponent<Terrain>();
                        if (terrain != null)
                        {
                            TerrainData terrainData = terrain.terrainData;
                            DetailPrototype[] currentPrototypes = terrainData.detailPrototypes;
                            DetailPrototype[] updatedPrototypes = new DetailPrototype[currentPrototypes.Length + 1];
                            currentPrototypes.CopyTo(updatedPrototypes, 0);
                            updatedPrototypes[currentPrototypes.Length] = newPrototype;
                            terrainData.RefreshPrototypes();
                            terrainData.detailPrototypes = updatedPrototypes;

                            if (prototypeIndex == -1)
                                prototypeIndex = currentPrototypes.Length;
                        }
                    }
                    
                    // Count this detail mesh prototype (created once per object, not per tile)
                    detailLayersCreated++;
                }

                // Process ALL HeatmapFiles entries (one per terrain tile)
                XmlNodeList heatmapFilesList = detailList.SelectNodes("HeatmapFiles");
                if (heatmapFilesList != null && heatmapFilesList.Count > 0)
                {
                    foreach (XmlNode heatmapFiles in heatmapFilesList)
                    {
                        string splatmapFileName = heatmapFiles["FileName"]?.InnerText;

                        // Get tile coordinates for this splatmap
                        int splatmapTileX = 0, splatmapTileY = 0;
                        if (heatmapFiles["TileX"] != null) int.TryParse(heatmapFiles["TileX"].InnerText, out splatmapTileX);
                        if (heatmapFiles["TileY"] != null) int.TryParse(heatmapFiles["TileY"].InnerText, out splatmapTileY);

                        if (!string.IsNullOrEmpty(splatmapFileName))
                        {
                            string splatmapPath = syncDirectory + splatmapFileName;
                            if (File.Exists(splatmapPath))
                            {
                                int instancesPlaced = PlaceInstancesFromSplatmap(container, splatmapPath, prototypeIndex, useAsTree, objectInfo, pb, pi, splatmapTileX, splatmapTileY);

                                if (useAsTree)
                                    treeInstancesPlaced += instancesPlaced;
                            }
                            else
                            {
                                Debug.LogWarning($"Splatmap file not found: {splatmapPath}");
                            }
                        }
                    }
                }
            }
            
            Debug.Log($"Detail object sync complete: {treeInstancesPlaced} tree instances placed, {detailLayersCreated} detail mesh layers created");
        }

        /// <summary>
        /// Loads instances from a CSV file and places them on the appropriate terrain tiles
        /// </summary>
        private static int LoadInstancesFromCSV(string csvPath, int prototypeIndex, GameObject container, ParamsBridge pb, ParamsImport pi, float bakedScaleAverage = 1f)
        {
            int instanceCount = 0;
            
            try
            {
                using (StreamReader reader = new StreamReader(csvPath))
                {                    
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                            continue;
                        
                        string[] values = line.Split(',');
                        if (values.Length < 12) // Expect: tx,ty,tz,sx,sy,sz,qx,qy,qz,qw,gradient,seed
                            continue;
                        
                        // Parse CSV values based on World Creator format
                        // tx,ty,tz,sx,sy,sz,qx,qy,qz,qw,gradient,seed
                        float posX, posY, posZ, scaleX, scaleY, scaleZ;
                        float qx, qy, qz, qw;

                        if (!float.TryParse(values[0], NumberStyles.Float, CultureInfo.InvariantCulture, out posX) ||
                            !float.TryParse(values[1], NumberStyles.Float, CultureInfo.InvariantCulture, out posY) ||
                            !float.TryParse(values[2], NumberStyles.Float, CultureInfo.InvariantCulture, out posZ) ||
                            !float.TryParse(values[3], NumberStyles.Float, CultureInfo.InvariantCulture, out scaleX) ||
                            !float.TryParse(values[4], NumberStyles.Float, CultureInfo.InvariantCulture, out scaleY) ||
                            !float.TryParse(values[5], NumberStyles.Float, CultureInfo.InvariantCulture, out scaleZ) ||
                            !float.TryParse(values[6], NumberStyles.Float, CultureInfo.InvariantCulture, out qx) ||
                            !float.TryParse(values[7], NumberStyles.Float, CultureInfo.InvariantCulture, out qy) ||
                            !float.TryParse(values[8], NumberStyles.Float, CultureInfo.InvariantCulture, out qz) ||
                            !float.TryParse(values[9], NumberStyles.Float, CultureInfo.InvariantCulture, out qw))
                        {
                            continue;
                        }
                        
                        // World Creator uses Y as forward/back, Z as height
                        // Unity uses Z as forward/back, Y as height
                        // So we need to swap Y and Z coordinates
                        float unityX = posX * 1024f * pb.worldScale;
                        float unityY = (posZ * 1024f - pi.minHeight) * pb.worldScale; // Subtract terrain min height to align properly
                        float unityZ = posY * 1024f * pb.worldScale; // World Creator Y (forward) → Unity Z (forward)
                        
                        // Find which terrain tile this instance belongs to
                        Terrain targetTerrain = FindTerrainForPosition(container, unityX, unityZ, pb, pi);
                        
                        // Convert world position to terrain-local normalized coordinates
                        TerrainData terrainData = targetTerrain.terrainData;
                        Vector3 terrainPosition = targetTerrain.transform.position;
                        Vector3 terrainSize = terrainData.size;
                        
                        float normalizedX = (unityX - terrainPosition.x) / terrainSize.x;
                        float normalizedZ = (unityZ - terrainPosition.z) / terrainSize.z;
                        float normalizedY = unityY / terrainSize.y;

                        // Clamp to valid range
                        if (normalizedX < 0f || normalizedX > 1f || normalizedZ < 0f || normalizedZ > 1f)
                            continue;

                        float uniformScale = scaleZ * 100000f / bakedScaleAverage;

                        // Convert quaternion to Unity's coordinate system (same as terrain trees)
                        Quaternion rotation = new Quaternion(-qx, -qz, -qy, qw);
                        // rotate quaternion 90 around y axis to account for forward vector difference
                        rotation *= Quaternion.Euler(0f, 90f, 0f);
                        
                        float rotationDegrees = rotation.eulerAngles.y;
                        float normalizedRotation = (rotationDegrees + 180f) / 360f;
                        
                        // Create tree instance
                        TreeInstance tree = new TreeInstance();
                        tree.position = new Vector3(normalizedX, normalizedY, normalizedZ);
                        tree.prototypeIndex = prototypeIndex;
                        tree.widthScale = uniformScale;
                        tree.heightScale = uniformScale;
                        tree.rotation = normalizedRotation;
                        tree.color = Color.white;
                        tree.lightmapColor = Color.white;
                        
                        // Add to terrain
                        TreeInstance[] existingTrees = terrainData.treeInstances;
                        TreeInstance[] newTrees = new TreeInstance[existingTrees.Length + 1];
                        existingTrees.CopyTo(newTrees, 0);
                        newTrees[existingTrees.Length] = tree;
                        terrainData.treeInstances = newTrees;
                        
                        instanceCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load instances from CSV {csvPath}: {ex.Message}");
            }
            
            return instanceCount;
        }
        
        /// <summary>
        /// Loads instances from CSV and creates individual GameObjects (entities) instead of terrain trees
        /// </summary>
        private static int LoadInstancesFromCSVAsEntities(string csvPath, GameObject prefab, GameObject instanceContainer, ParamsBridge pb, ParamsImport pi, float bakedScaleAverage, string objectName, bool hasScaleRange, float modelScale)
        {
            int instanceCount = 0;
            
            try
            {
                using (StreamReader reader = new StreamReader(csvPath))
                {                    
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                            continue;
                        
                        string[] values = line.Split(',');
                        if (values.Length < 12) // Expect: tx,ty,tz,sx,sy,sz,qx,qy,qz,qw,gradient,seed
                            continue;
                        
                        // Parse CSV values based on World Creator format
                        // tx,ty,tz,sx,sy,sz,qx,qy,qz,qw,gradient,seed
                        float posX, posY, posZ, scaleX, scaleY, scaleZ;
                        float qx, qy, qz, qw;

                        if (!float.TryParse(values[0], NumberStyles.Float, CultureInfo.InvariantCulture, out posX) ||
                            !float.TryParse(values[1], NumberStyles.Float, CultureInfo.InvariantCulture, out posY) ||
                            !float.TryParse(values[2], NumberStyles.Float, CultureInfo.InvariantCulture, out posZ) ||
                            !float.TryParse(values[3], NumberStyles.Float, CultureInfo.InvariantCulture, out scaleX) ||
                            !float.TryParse(values[4], NumberStyles.Float, CultureInfo.InvariantCulture, out scaleY) ||
                            !float.TryParse(values[5], NumberStyles.Float, CultureInfo.InvariantCulture, out scaleZ) ||
                            !float.TryParse(values[6], NumberStyles.Float, CultureInfo.InvariantCulture, out qx) ||
                            !float.TryParse(values[7], NumberStyles.Float, CultureInfo.InvariantCulture, out qy) ||
                            !float.TryParse(values[8], NumberStyles.Float, CultureInfo.InvariantCulture, out qz) ||
                            !float.TryParse(values[9], NumberStyles.Float, CultureInfo.InvariantCulture, out qw))
                        {
                            continue;
                        }
                        
                        // World Creator uses Y as forward/back, Z as height
                        // Unity uses Z as forward/back, Y as height
                        // So we need to swap Y and Z coordinates
                        float unityX = posX * 1024f * pb.worldScale;
                        float unityY = (posZ * 1024f - pi.minHeight) * pb.worldScale; // Subtract terrain min height to align properly
                        float unityZ = posY * 1024f * pb.worldScale; // World Creator Y (forward) → Unity Z (forward)
                        
                        // Calculate scale
                        // World Creator normalizes meshes to max bbox axis = 1, then applies:
                        // finalScale = normalized_mesh * ModelScale * instance_scale
                        // For entities: mesh is only normalized, so we apply ModelScale * instance_scale to GameObject
                        float instanceScale = scaleZ * 1024f; // CSV scale in world units
                        float uniformScale = modelScale * instanceScale * pb.worldScale;
                        Vector3 scale = new Vector3(uniformScale, uniformScale, uniformScale);

                        // Convert quaternion to Unity's coordinate system (same as terrain trees)
                        Quaternion rotation = new Quaternion(-qx, -qz, -qy, qw);
                        // rotate quaternion 90 around y axis to account for forward vector difference
                        rotation *= Quaternion.Euler(0, 90, 0f);
                        
                        // Instantiate GameObject
                        GameObject instance = GameObject.Instantiate(prefab);
                        instance.name = objectName + "_" + instanceCount;
                        instance.transform.parent = instanceContainer.transform;
                        instance.transform.position = new Vector3(unityX, unityY, unityZ);
                        instance.transform.rotation = rotation;
                        instance.transform.localScale = scale;
                        
                        instanceCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load entity instances from CSV {csvPath}: {ex.Message}");
            }
            
            return instanceCount;
        }
        
        /// <summary>
        /// Finds the terrain tile that contains the given world position
        /// </summary>
        private static Terrain FindTerrainForPosition(GameObject container, float worldX, float worldZ, ParamsBridge pb, ParamsImport pi)
        {
            foreach (Transform child in container.transform)
            {
                Terrain terrain = child.GetComponent<Terrain>();
                if (terrain == null)
                    continue;
                
                Vector3 terrainPos = terrain.transform.position;
                Vector3 terrainSize = terrain.terrainData.size;
                
                if (worldX >= terrainPos.x && worldX <= terrainPos.x + terrainSize.x &&
                    worldZ >= terrainPos.z && worldZ <= terrainPos.z + terrainSize.z)
                {
                    return terrain;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Converts a splatmap texture to a detail density map using the same tiling logic as terrain alphamaps
        /// </summary>
        private static int[,] ConvertSplatmapToDetailMapTiled(Texture2D splatmap, int detailResolution, ParamsBridge pb, ParamsImport pi, int tileX, int tileY, float densityMultiplier = 1f, float instanceDistance = 32f, bool useDistributionDensity = false, Vector3 terrainSize = default, int wcTileX = 0, int wcTileY = 0, float minScale = 1f, float maxScale = 1f)
        {
            int[,] detailMap = new int[detailResolution, detailResolution];
            
            // Use actual texture dimensions (matches material alphamap system)
            // Each splatmap file represents one WC tile
            int splatmapFullResX = splatmap.width;
            int splatmapFullResY = splatmap.height;

            // Calculate cell size using the actual Unity terrain world size
            // terrainSize already accounts for InternalSplit (it's the size of this specific Unity tile)
            // The InternalSplit factor affects density because smaller tiles = smaller cells
            // To keep density constant, we need to calculate the world scale factor
            float worldScaleFactor = pb.InternalSplit / (float)pi.maxTileRes;
            float cellSize = terrainSize.x / detailResolution * worldScaleFactor;
            
            // Calculate average scale for this detail mesh
            // Unity's detail system with useDensityScaling=true automatically adjusts instance counts
            // based on minWidth/maxWidth to prevent overlap. We need to compensate for this.
            float avgScale = (minScale + maxScale) / 2f;
            
            // Calculate instances per cell based on world space cell size
            // Factor in the mesh scale - larger meshes need proportionally fewer instances
            // to maintain the same visual coverage density
            // instancesPerCell = (cellSize / instanceDistance) × densityMultiplier / avgScale
            float instancesPerCell = (cellSize / instanceDistance) * densityMultiplier;
            
            // Unity's Coverage mode: scale the density to 0-255 range
            // The 255 represents full coverage (1 instance per instanceDistance²)
            // We need to scale by a factor to get visible results
            // Empirically determined: 256× scaling gives good results
            float unityCoverageScale = 256f;
            
            // Calculate base coverage per cell
            // This gives us the raw density value before splatmap modulation
            float baseCoverage = instancesPerCell * 255f * unityCoverageScale;
            
            int nonZeroCount = 0;
            int totalSampled = 0;
            
            // Calculate WC-tile-relative coordinates for sampling
            // Material splatmaps are full-terrain resolution and use absolute offsets
            // Detail splatmaps are per-WC-tile (4096×4096 each) and need relative offsets
            // Example: Unity tile (1,3) in WC tile (0,1) with InternalSplit=2048
            //   - WC tile (0,1) starts at Unity tile Y=2
            //   - Relative tile: (1, 3-2) = (1, 1)
            //   - Offset: (1*2048, 1*2048) = (2048, 2048) ✓ within 4096×4096 splatmap
            int wcTileSize = pi.maxTileRes / pb.InternalSplit;  // Unity tiles per WC tile dimension
            int relTileX = tileX - (wcTileX * wcTileSize);  // Convert absolute to relative
            int relTileY = tileY - (wcTileY * wcTileSize);  // Convert absolute to relative
            
            // Calculate splatmap resolution per Unity tile
            // Splatmap is always 4096×4096 per WC tile, distributed across Unity tiles
            // Example: InternalSplit=4096, detailRes=1024, wcTileSize=1 → splatResPerTile=4096 (full splatmap for single tile)
            // Example: InternalSplit=2048, detailRes=1024, wcTileSize=2 → splatResPerTile=2048 (half splatmap per tile)
            int splatResPerTile = splatmapFullResX / wcTileSize;
            
            int xOff = relTileX * splatResPerTile;
            int yOff = relTileY * splatResPerTile;
            
            //Debug.Log($"Detail tile ({tileX},{tileY}) for WC tile ({wcTileX},{wcTileY}): relativeTile=({relTileX},{relTileY}), offset=({xOff},{yOff})\nInternalSplit={pb.InternalSplit}, wcTileSize={wcTileSize}, splatResPerTile={splatResPerTile}, splatmap={splatmapFullResX}×{splatmapFullResY}\nterrainSize={terrainSize.x:F1}×{terrainSize.z:F1}, worldScaleFactor={worldScaleFactor:F4}\ndetailRes={detailResolution}, cellSize={cellSize:F4}, instanceDist={instanceDistance:F1}\ninstancesPerCell={instancesPerCell:F6}, baseCoverage={baseCoverage:F2}");
            
            for (int x = 0; x < detailResolution; x++)
            {
                for (int y = 0; y < detailResolution; y++)
                {
                    // Map detail cell [0, detailResolution-1] to splatmap pixel [offset, offset+splatResPerTile-1]
                    // Use proper interpolation to handle resolution differences
                    float splatX = (float)x / detailResolution * splatResPerTile;
                    float splatY = (float)y / detailResolution * splatResPerTile;
                    
                    int realX = xOff + Mathf.RoundToInt(splatX);
                    int realY = yOff + Mathf.RoundToInt(splatY);

                    // Clamp to texture bounds
                    realX = Mathf.Clamp(realX - 1, 0, splatmapFullResX - 1);
                    realY = Mathf.Clamp(realY, 0, splatmapFullResY - 1);

                    // Sample the pixel using GetPixel (note: GetPixel uses bottom-left origin)
                    Color pixelColor = splatmap.GetPixel(realX, realY);

                    // Coverage mode: DetailMapValue (0-255) represents coverage fraction
                    // Splatmap (0-1) modulates this coverage per-cell
                    float splatIntensity = pixelColor.r > 0.00001f ? (useDistributionDensity ? pixelColor.r : 1f) : 0f;
                    
                    // Calculate final coverage value for this cell
                    // Formula: coverage = (cellSize / instanceDistance)² × densityMultiplier × splatIntensity × 255
                    float cellCoverage = baseCoverage * splatIntensity;
                    int density = Mathf.RoundToInt(cellCoverage);
                    detailMap[y, x] = density;

                    totalSampled++;
                    if (density > 0) nonZeroCount++;
                }
            }
            
            return detailMap;
        }

        /// <summary>
        /// Places instances from a splatmap file onto all terrain tiles
        /// Returns total number of instances placed across all terrains
        /// </summary>
        private static int PlaceInstancesFromSplatmap(GameObject container, string splatmapPath, int prototypeIndex, bool isTree, XmlNode objectInfo, ParamsBridge pb, ParamsImport pi, int targetTileX, int targetTileY)
        {
            int totalInstancesPlaced = 0;
            
            // Load splatmap
            byte[] imageData = File.ReadAllBytes(splatmapPath);
            Texture2D splatmap = new Texture2D(2, 2);
            if (!splatmap.LoadImage(imageData))
            {
                Debug.LogWarning($"Failed to load splatmap image: {splatmapPath}");
                UnityEngine.Object.DestroyImmediate(splatmap);
                return 0;
            }
            
            // Extract placement parameters from XML
            float densityMultiplier = 1f;
            float instanceDistance = 32f;
            bool useDistributionDensity = false;
            bool useDistributionScale = false;
            bool alignToNormal = false;
            float minScale = 1f, maxScale = 1f;
            float minRotationY = 0f, maxRotationY = 0f;
            float distributionDensityMin = 0f, distributionDensityMax = 1f;
            float distributionScaleMin = 0f, distributionScaleMax = 1f;
            
            if (objectInfo != null)
            {
                XmlNode densityNode = objectInfo["Density"];
                if (densityNode != null)
                    float.TryParse(densityNode.InnerText, out densityMultiplier);
                
                XmlNode instanceDistanceNode = objectInfo["InstanceDistance"];
                if (instanceDistanceNode != null)
                    float.TryParse(instanceDistanceNode.InnerText, out instanceDistance);

                XmlNode alignToNormalNode = objectInfo["AlignToNormal"];
                if (alignToNormalNode != null)
                    bool.TryParse(alignToNormalNode.InnerText, out alignToNormal);

                XmlNode distributionDensityRangeNode = objectInfo["UseDistributionDensityRange"];
                if (distributionDensityRangeNode != null)
                    bool.TryParse(distributionDensityRangeNode.InnerText, out useDistributionDensity);
                
                XmlNode distributionScaleRangeNode = objectInfo["UseDistributionScaleRange"];
                if (distributionScaleRangeNode != null)
                    bool.TryParse(distributionScaleRangeNode.InnerText, out useDistributionScale);
                
                XmlNode scaleRangeNode = objectInfo["ScaleRange"];
                if (scaleRangeNode != null)
                {
                    XmlNode minNode = scaleRangeNode["Min"];
                    XmlNode maxNode = scaleRangeNode["Max"];
                    if (minNode != null) float.TryParse(minNode.InnerText, out minScale);
                    if (maxNode != null) float.TryParse(maxNode.InnerText, out maxScale);
                }
                
                XmlNode rotationRangeYNode = objectInfo["RotationRangeY"];
                if (rotationRangeYNode != null)
                {
                    XmlNode minNode = rotationRangeYNode["Min"];
                    XmlNode maxNode = rotationRangeYNode["Max"];
                    if (minNode != null) float.TryParse(minNode.InnerText, out minRotationY);
                    if (maxNode != null) float.TryParse(maxNode.InnerText, out maxRotationY);
                }
                
                XmlNode distributionDensityRange = objectInfo["DistributionDensityRange"];
                if (distributionDensityRange != null)
                {
                    XmlNode minNode = distributionDensityRange["Min"];
                    XmlNode maxNode = distributionDensityRange["Max"];
                    if (minNode != null) float.TryParse(minNode.InnerText, out distributionDensityMin);
                    if (maxNode != null) float.TryParse(maxNode.InnerText, out distributionDensityMax);
                }
                
                XmlNode distributionScaleRange = objectInfo["DistributionScaleRange"];
                if (distributionScaleRange != null)
                {
                    XmlNode minNode = distributionScaleRange["Min"];
                    XmlNode maxNode = distributionScaleRange["Max"];
                    if (minNode != null) float.TryParse(minNode.InnerText, out distributionScaleMin);
                    if (maxNode != null) float.TryParse(maxNode.InnerText, out distributionScaleMax);
                }
            }
            
            // Place instances on each terrain
            foreach (Transform child in container.transform)
            {
                Terrain terrain = child.GetComponent<Terrain>();
                if (terrain == null)
                    continue;
                
                // Get terrain tile coordinates from name
                string terrainName = terrain.name;
                string[] parts = terrainName.Split('_');
                if (parts.Length < 3)
                    continue;
                
                int tileX = int.Parse(parts[parts.Length - 2]);
                int tileY = int.Parse(parts[parts.Length - 1]);
                
                // Convert World Creator tile coordinates to Unity tile range
                // WC always exports at max 4096 TileResolution (maxTileRes)
                // Unity uses InternalSplit which may be smaller (e.g., 1024), subdividing WC tiles
                // Example: 4096×8192 terrain
                //   - WC exports: 1×3 tiles (each 4096×4096, last is 4096×1)
                //   - Unity with InternalSplit=1024: 4×8 tiles (subdivides each WC tile into 4×4 Unity tiles)
                //   - WC tile (0,0) → Unity tiles (0-3, 0-3)
                //   - WC tile (0,1) → Unity tiles (0-3, 4-7)
                //   - WC tile (0,2) → Unity tiles (0-3, 8+) if they exist
                int wcTileSize = pi.maxTileRes;  // World Creator's tile size (typically 4096)
                int unityTileSize = pb.InternalSplit;  // Unity's tile size (e.g., 1024)
                
                // Calculate which Unity tiles correspond to this WC tile
                int unityTileStartX = (targetTileX * wcTileSize) / unityTileSize;
                int unityTileStartY = (targetTileY * wcTileSize) / unityTileSize;
                int unityTileEndX = ((targetTileX + 1) * wcTileSize) / unityTileSize;
                int unityTileEndY = ((targetTileY + 1) * wcTileSize) / unityTileSize;
                
                // Check if this Unity tile falls within the WC tile's region
                if (tileX < unityTileStartX || tileX >= unityTileEndX ||
                    tileY < unityTileStartY || tileY >= unityTileEndY)
                    continue;
                
                TerrainData terrainData = terrain.terrainData;
                Vector3 terrainSize = terrainData.size;
                
                if (isTree)
                {
                    // Place tree instances
                    int instanceCount = PlaceTreeInstancesOnTerrain(terrain, splatmap, prototypeIndex, pb, pi, tileX, tileY, densityMultiplier, instanceDistance, terrainSize, minScale, maxScale, minRotationY, maxRotationY, useDistributionDensity, distributionDensityMin, distributionDensityMax, useDistributionScale, distributionScaleMin, distributionScaleMax, targetTileX, targetTileY);
                    totalInstancesPlaced += instanceCount;
                }
                else
                {
                    // Place detail instances
                    int detailResolution = terrainData.detailResolution;
                    int[,] detailMap = ConvertSplatmapToDetailMapTiled(splatmap, detailResolution, pb, pi, tileX, tileY, densityMultiplier, instanceDistance, useDistributionDensity, terrainSize, targetTileX, targetTileY, minScale, maxScale);
                    terrainData.SetDetailLayer(0, 0, prototypeIndex, detailMap);
                    
                    // Increment counter for detail mesh layer created (not counting individual instances for performance)
                    totalInstancesPlaced++;
                }
            }
            
            // Clean up
            UnityEngine.Object.DestroyImmediate(splatmap);
            return totalInstancesPlaced;
        }
        
        /// <summary>
        /// Places tree instances on a single terrain based on splatmap density
        /// Returns number of instances placed
        /// </summary>
        private static int PlaceTreeInstancesOnTerrain(Terrain terrain, Texture2D splatmap, int treePrototypeIndex, ParamsBridge pb, ParamsImport pi, int tileX, int tileY, float densityMultiplier, float instanceDistance, Vector3 terrainSize, float minScale, float maxScale, float minRotationY, float maxRotationY, bool useDistributionDensity, float distributionDensityMin, float distributionDensityMax,bool useDistributionScale, float distributionScaleMin, float distributionScaleMax, int wcTileX = 0, int wcTileY = 0)
        {
            TerrainData terrainData = terrain.terrainData;
            
            // Use actual texture dimensions (per-WC-tile splatmap)
            int splatmapFullResX = splatmap.width;
            int splatmapFullResY = splatmap.height;
            
            // Calculate WC-tile-relative coordinates (same logic as detail meshes)
            int wcTileSize = pi.maxTileRes / pb.InternalSplit;
            int relTileX = tileX - (wcTileX * wcTileSize);
            int relTileY = tileY - (wcTileY * wcTileSize);
            
            int xOff = relTileX * pb.InternalSplit;
            int yOff = relTileY * pb.InternalSplit;
            float scale = pb.InternalSplit <= 4096 ? 1 : (float)pb.InternalSplit / 1024;
                        
            // Calculate grid dimensions based on instance distance
            float countX = terrainSize.x / instanceDistance;
            float countZ = terrainSize.z / instanceDistance;
            int gridCountX = Mathf.CeilToInt(countX);
            int gridCountZ = Mathf.CeilToInt(countZ);
            
            // Use deterministic random based on tile position and prototype for consistency
            System.Random random = new System.Random((tileX * 1000 + tileY) * 100 + treePrototypeIndex);
            List<TreeInstance> newTrees = new List<TreeInstance>();
            
            // Grid-based placement with splatmap filtering
            for (int gx = 0; gx < gridCountX; gx++)
            {
                for (int gz = 0; gz < gridCountZ; gz++)
                {
                    // Calculate base position on grid (in world space meters)
                    float worldX = (gx + 0.5f) * instanceDistance;
                    float worldZ = (gz + 0.5f) * instanceDistance;
                    
                    // Add random offset within grid cell for natural variation
                    float offsetRange = instanceDistance;
                    float randomOffsetX = ((float)random.NextDouble() - 0.5f) * offsetRange;
                    float randomOffsetZ = ((float)random.NextDouble() - 0.5f) * offsetRange;
                    worldX += randomOffsetX;
                    worldZ += randomOffsetZ;
                    
                    // Convert to normalized terrain coordinates [0,1]
                    float normalizedX = worldX / terrainSize.x;
                    float normalizedZ = worldZ / terrainSize.z;
                    
                    // Skip if outside terrain bounds
                    if (normalizedX < 0f || normalizedX > 1f || normalizedZ < 0f || normalizedZ > 1f)
                        continue;
                    
                    // Sample splatmap using the same offset-based logic as alphamap processing
                    // This maps the normalized terrain position to the splatmap texture
                    int detailX = Mathf.RoundToInt(normalizedX * pb.InternalSplit);
                    int detailY = Mathf.RoundToInt(normalizedZ * pb.InternalSplit);
                    
                    int splatX = xOff + Mathf.CeilToInt(detailX * ((float)(pb.InternalSplit + 1) / pb.InternalSplit) * scale);
                    int splatY = yOff + Mathf.CeilToInt(detailY * ((float)(pb.InternalSplit + 1) / pb.InternalSplit) * scale);
                    
                    // Clamp to texture bounds
                    splatX = Mathf.Clamp(splatX - 1, 0, splatmapFullResX - 1);
                    splatY = Mathf.Clamp(splatY, 0, splatmapFullResY - 1);
                    
                    Color splatValue = splatmap.GetPixel(splatX, splatY);
                    float splatIntensity = splatValue.r;
                    
                    // Apply distribution density range if enabled
                    float densityProbability = splatIntensity;
                    if (useDistributionDensity)
                    {
                        densityProbability = Mathf.Lerp(distributionDensityMin, distributionDensityMax, splatIntensity);
                    }
                    
                    // Place tree based on splatmap probability and density multiplier
                    if ((float)random.NextDouble() < densityProbability * densityMultiplier)
                    {
                        // Sample terrain height at this position
                        float terrainHeight = terrainData.GetInterpolatedHeight(normalizedX, normalizedZ);
                        float normalizedHeight = terrainHeight / terrainSize.y;
                        
                        // Calculate tree scale
                        // For _DetailMultiMat trees: the average ScaleRange is already baked into the mesh vertices during prefab creation
                        // So we only apply distribution scale modifier here (base scale = 1.0)
                        // For _Instance trees: scale is applied as a multiplier (not baked into mesh)
                        float treeScale = 1.0f;
                        
                        // Apply random variation only if min != max (otherwise it's already the average)
                        if (Mathf.Abs(maxScale - minScale) > 0.01f)
                        {
                            // For _DetailMultiMat: minScale and maxScale are already baked as average in mesh
                            // This would only apply if we want variation around the baked average
                            // For now, keep scale at 1.0 to match the baked average
                            treeScale = Mathf.Lerp(0.9f, 1.1f, (float)random.NextDouble()); // Small variation around baked size
                        }
                        
                        if (useDistributionScale)
                        {
                            float scaleT = Mathf.Lerp(distributionScaleMin, distributionScaleMax, splatIntensity);
                            treeScale *= scaleT;
                        }
                        
                        // Random rotation
                        float rotationDegrees = Mathf.Lerp(minRotationY, maxRotationY, (float)random.NextDouble());
                        float normalizedRotation = (rotationDegrees + 180f) / 360f;
                        
                        TreeInstance tree = new TreeInstance();
                        tree.position = new Vector3(normalizedX, normalizedHeight, normalizedZ);
                        tree.prototypeIndex = treePrototypeIndex;
                        tree.widthScale = treeScale;
                        tree.heightScale = treeScale;
                        tree.rotation = normalizedRotation;
                        tree.color = Color.white;
                        tree.lightmapColor = Color.white;
                        
                        newTrees.Add(tree);
                    }
                }
            }
            
            // Add the new trees to the terrain
            if (newTrees.Count > 0)
            {
                TreeInstance[] existingTrees = terrainData.treeInstances;
                TreeInstance[] combinedTrees = new TreeInstance[existingTrees.Length + newTrees.Count];
                existingTrees.CopyTo(combinedTrees, 0);
                newTrees.CopyTo(combinedTrees, existingTrees.Length);
                terrainData.treeInstances = combinedTrees;
            }
            
            return newTrees.Count;
        }

        /// <summary>
        /// Generates a prefab for point cloud objects from instance_info.xml
        /// Uses objectInfo directly instead of looking it up in detail_info.xml
        /// </summary>
        public static GameObject GeneratePrefabFromInstanceInfo(ParamsBridge pb, string assetPath, string prefabName, string objectName, XmlNode objectInfo, bool isEntity = false)
        {
            string prefabFolder = "Assets/WorldCreatorTerrains/" + pb.assetName + "/obj_prefab/";
            
            // Convert to absolute path for filesystem operations
            string absolutePrefabFolder = Path.Combine(Application.dataPath, "..", prefabFolder);
            absolutePrefabFolder = Path.GetFullPath(absolutePrefabFolder);
            
            if (!Directory.Exists(absolutePrefabFolder))
            {
                Directory.CreateDirectory(absolutePrefabFolder);
                AssetDatabase.Refresh();
            }

            // Check if enhanced prefab already exists
            string prefabPath = prefabFolder + prefabName + ".prefab";
            string absolutePrefabPath = Path.Combine(Application.dataPath, "..", prefabPath);
            absolutePrefabPath = Path.GetFullPath(absolutePrefabPath);
            bool fileExists = File.Exists(absolutePrefabPath);
            
            GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            
            if (fileExists && existingPrefab == null)
            {
                AssetDatabase.ImportAsset(prefabPath, ImportAssetOptions.ForceUpdate);
                existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            }
            
            if (existingPrefab != null && !pb.overwritePrefabs)
                return existingPrefab;
            else if (existingPrefab != null && pb.overwritePrefabs)
                AssetDatabase.DeleteAsset(prefabPath);

            // Load the asset
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (asset == null)
            {
                Debug.LogWarning($"Could not find asset at path: {assetPath}");
                return null;
            }
            
            // Create prefab with metadata from instance_info.xml
            GameObject prefab = CreateEnhancedPrefab(asset, prefabName, prefabFolder, objectInfo, pb.overwritePrefabs, pb.worldScale, isEntity);

            if (prefab != null)
            {
                GameObject finalCheck = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (finalCheck != null && !pb.overwritePrefabs)
                {
                    Debug.LogWarning($"Safety check: Prefab {prefabName} already exists and overwritePrefabs is disabled. Returning existing prefab.");
                    GameObject.DestroyImmediate(prefab);
                    return finalCheck;
                }
                
                GameObject savedPrefab = PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
                GameObject.DestroyImmediate(prefab);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                return savedPrefab;
            }
            else
            {
                Debug.LogError($"Failed to create prefab object for: {prefabName}");
            }

            return null;
        }

        public static GameObject GeneratePrefab(ParamsBridge pb, string assetPath, string prefabName, string originalObjectName)
        {
            // If originalObjectName not provided, use prefabName for metadata lookup
            if (string.IsNullOrEmpty(originalObjectName))
                originalObjectName = prefabName;
                
            string prefabFolder = "Assets/WorldCreatorTerrains/" + pb.assetName + "/obj_prefab/";
            
            // Convert to absolute path for filesystem operations
            string absolutePrefabFolder = Path.Combine(Application.dataPath, "..", prefabFolder);
            absolutePrefabFolder = Path.GetFullPath(absolutePrefabFolder);
            
            if (!Directory.Exists(absolutePrefabFolder))
            {
                Directory.CreateDirectory(absolutePrefabFolder);
                AssetDatabase.Refresh(); // Refresh to make Unity aware of the new folder
            }

            // Check if enhanced prefab already exists (only reuse if overwritePrefabs is disabled)
            string prefabPath = prefabFolder + prefabName + ".prefab";
            
            // Convert Unity asset path to absolute filesystem path for File.Exists check
            string absolutePrefabPath = Path.Combine(Application.dataPath, "..", prefabPath);
            absolutePrefabPath = Path.GetFullPath(absolutePrefabPath); // Normalize the path
            bool fileExists = File.Exists(absolutePrefabPath);
            
            // Try to load from AssetDatabase
            GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            
            // If file exists but AssetDatabase didn't load it, force a refresh
            if (fileExists && existingPrefab == null)
            {
                AssetDatabase.ImportAsset(prefabPath, ImportAssetOptions.ForceUpdate);
                existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            }
            
            if (existingPrefab != null && !pb.overwritePrefabs)
                return existingPrefab;
            else if (existingPrefab != null && pb.overwritePrefabs)
                AssetDatabase.DeleteAsset(prefabPath);

            // Load the asset from the path
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (asset == null)
            {
                Debug.LogWarning($"Could not find asset at path: {assetPath}");
                return null;
            }

            // Try to get object metadata for enhanced prefab creation
            // Use originalObjectName for metadata lookup since it matches the Name in XML
            XmlNode objectMetadata = GetObjectMetadata(pb, originalObjectName);
            
            if (objectMetadata == null)
            {
                Debug.LogError($"Could not find metadata for object: {originalObjectName}");
                return null;
            }
            
            // Create prefab with metadata
            GameObject prefab = CreateEnhancedPrefab(asset, prefabName, prefabFolder, objectMetadata, pb.overwritePrefabs, pb.worldScale);

            if (prefab != null)
            {
                // Double-check: if overwritePrefabs is disabled and prefab exists, don't overwrite
                // This is a safety check in case we somehow got past the earlier check
                GameObject finalCheck = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (finalCheck != null && !pb.overwritePrefabs)
                {
                    Debug.LogWarning($"Safety check: Prefab {prefabName} already exists and overwritePrefabs is disabled. Returning existing prefab.");
                    GameObject.DestroyImmediate(prefab);
                    return finalCheck;
                }
                
                GameObject savedPrefab = PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
                GameObject.DestroyImmediate(prefab);
                AssetDatabase.SaveAssets(); // Persist changes to disk
                AssetDatabase.Refresh(); // Ensure the prefab is visible in Project window
                return savedPrefab;
            }
            else
            {
                Debug.LogError($"Failed to create prefab object for: {prefabName}");
            }

            return null;
        }

        /// <summary>
        /// Gets object metadata from detail_info.xml
        /// </summary>
        private static XmlNode GetObjectMetadata(ParamsBridge pb, string objectName)
        {
            string syncDirectory = Path.GetDirectoryName(pb.bridgeFilePath) + "/";
            string detailInfoPath = syncDirectory + "detail_info.xml";
            
            if (!File.Exists(detailInfoPath))
                return null;
            
            try
            {
                XmlDocument doc = new XmlDocument();
                // Read with proper encoding handling
                string xmlContent = File.ReadAllText(detailInfoPath, System.Text.Encoding.UTF8);
                doc.LoadXml(xmlContent);
                
                XmlNodeList detailLists = doc.GetElementsByTagName("DetailList");
                foreach (XmlNode detailList in detailLists)
                {
                    string name = detailList["Name"]?.InnerText;
                    if (name == objectName)
                    {
                        // Try ObjectInfo first, then SubobjectInfo (for child objects)
                        XmlNode objectInfo = detailList["ObjectInfo"];
                        if (objectInfo == null)
                            objectInfo = detailList["SubobjectInfo"];
                        return objectInfo;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Failed to parse detail_info.xml: {ex.Message}");
            }
            
            return null;
        }

        /// <summary>
        /// Creates an enhanced prefab using object metadata
        /// </summary>
        private static GameObject CreateEnhancedPrefab(GameObject sourceAsset, string prefabName, string prefabFolder, XmlNode objectInfo, bool overwriteAssets, float worldScale = 1f, bool isEntity = false)
        {
            GameObject prefabRoot = new GameObject(prefabName);

            try
            {
                // Get scale from XML metadata
                float modelScale = 1f;
                XmlNode scaleNode = objectInfo["ModelScale"];
                if (scaleNode != null && float.TryParse(scaleNode.InnerText, out float scale))
                    modelScale = scale / 128f; // Convert from World Creator scale to Unity scale (1 = 1 unit)

                // Find the mesh object using existing LOD0 detection
                GameObject meshObject = FindLOD0Mesh(sourceAsset);
                if (meshObject == null)
                {
                    Debug.LogWarning($"No suitable mesh found in {prefabName}");
                    GameObject.DestroyImmediate(prefabRoot);
                    return null;
                }
                
                // Check material count to determine if this will be a tree (multi-material)
                MeshRenderer sourceRenderer = meshObject.GetComponent<MeshRenderer>();
                int materialCount = sourceRenderer != null && sourceRenderer.sharedMaterials != null ? 
                    sourceRenderer.sharedMaterials.Length : 0;
                bool isMultiMaterial = materialCount > 1;

                // Determine if we should bake scale into mesh:
                // - Entity objects: NO (scale applied to GameObject transform)
                // - _Instance objects: YES (CSV uses scale multipliers)
                // - _DetailMultiMat objects: YES (TreeInstance uses scale multipliers)
                // - _Detail objects: NO (DetailPrototype uses absolute minWidth/maxWidth)
                bool isInstanceObject = prefabName.Contains("_Instance");
                bool shouldBakeScale = !isEntity && (isInstanceObject || isMultiMaterial);

                // Get scale range
                float scaleMultiplier = 1f;
                float minScale = 1f, maxScale = 1f;
                XmlNode scaleRangeNode = objectInfo["ScaleRange"];
                if (scaleRangeNode != null)
                {
                    XmlNode minNode = scaleRangeNode["Min"];
                    XmlNode maxNode = scaleRangeNode["Max"];
                    if (minNode != null) float.TryParse(minNode.InnerText, out minScale);
                    if (maxNode != null) float.TryParse(maxNode.InnerText, out maxScale);
                    
                    // Calculate average scale - only bake if needed
                    if (shouldBakeScale)
                        scaleMultiplier = (minScale + maxScale) / 2f;

                }

                // Create transformed mesh with hierarchy transforms + normalization + ModelScale applied to vertices
                string meshAssetPath = prefabFolder + prefabName + "_mesh.asset";
                Mesh existingMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshAssetPath);
                Mesh transformedMesh = null;
                
                if (existingMesh != null && !overwriteAssets)
                {
                    // Reuse existing mesh asset
                    transformedMesh = existingMesh;
                }
                else
                {
                    // Create new mesh
                    // For entities: only normalize (no ModelScale/ScaleRange baking)
                    // For non-entities: bake ModelScale, ScaleRange average, and worldScale into mesh vertices
                    float meshScale = isEntity ? 1f : (modelScale * scaleMultiplier * worldScale);
                    transformedMesh = CreateTransformedMesh(sourceAsset, meshScale);
                    if (transformedMesh != null)
                    {
                        if (existingMesh != null && overwriteAssets)
                        {
                            // Delete old mesh before creating new one
                            AssetDatabase.DeleteAsset(meshAssetPath);
                        }
                        AssetDatabase.CreateAsset(transformedMesh, meshAssetPath);
                    }
                }

                // Add mesh components to prefab root (transform stays at scale 1,1,1)
                MeshFilter meshFilter = prefabRoot.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = prefabRoot.AddComponent<MeshRenderer>();

                meshFilter.mesh = transformedMesh ?? meshObject.GetComponent<MeshFilter>().sharedMesh;

                // Copy and process materials with metadata
                CopyObjectMaterials(meshObject, meshRenderer, prefabFolder, prefabName, objectInfo, overwriteAssets);

                return prefabRoot;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to create enhanced prefab for {prefabName}: {ex.Message}");
                GameObject.DestroyImmediate(prefabRoot);
                return null;
            }
        }

        /// <summary>
        /// Copies and processes materials from source object with XML metadata
        /// </summary>
        private static void CopyObjectMaterials(GameObject sourceObject, MeshRenderer targetRenderer, string prefabFolder, string objectName, XmlNode objectInfo, bool overwriteAssets)
        {
            MeshRenderer sourceMeshRenderer = sourceObject.GetComponent<MeshRenderer>();
            if (sourceMeshRenderer == null || sourceMeshRenderer.sharedMaterials == null)
            {
                Debug.LogWarning($"No materials found on {objectName}");
                return;
            }
            
            List<Material> copiedMaterials = new List<Material>();
            XmlNodeList materialInfos = objectInfo.SelectNodes("MaterialInfos");
            
            for (int i = 0; i < sourceMeshRenderer.sharedMaterials.Length; i++)
            {
                Material sourceMaterial = sourceMeshRenderer.sharedMaterials[i];
                if (sourceMaterial == null) continue;
                
                // Generate unique name
                string materialName = $"{objectName}_{sourceMaterial.name}_{i:00}";
                string materialPath = prefabFolder + materialName + ".mat";
                
                // Check if material already exists
                Material existingMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                
                if (existingMaterial != null && !overwriteAssets)
                {
                    // Reuse existing material
                    copiedMaterials.Add(existingMaterial);
                    continue;
                }
                
                // Create editable copy
                Material copiedMaterial = new Material(sourceMaterial);
                
                // Enable GPU Instancing for terrain detail rendering
                copiedMaterial.enableInstancing = true;
                
                // Apply XML material properties if available
                if (i < materialInfos.Count)
                {
                    ApplyXMLMaterialProperties(copiedMaterial, materialInfos[i]);
                }
                
                copiedMaterial.name = materialName;
                
                // Save or overwrite material asset
                if (existingMaterial != null && overwriteAssets)
                {
                    AssetDatabase.DeleteAsset(materialPath);
                }
                AssetDatabase.CreateAsset(copiedMaterial, materialPath);
                
                copiedMaterials.Add(copiedMaterial);
            }
            
            // Assign copied materials to renderer
            targetRenderer.sharedMaterials = copiedMaterials.ToArray();
        }

        /// <summary>
        /// Applies material properties from XML metadata
        /// </summary>
        private static void ApplyXMLMaterialProperties(Material material, XmlNode materialInfo)
        {
            // Apply color
            XmlNode colorNode = materialInfo["Color"];
            if (colorNode != null && UnityEngine.ColorUtility.TryParseHtmlString("#" + colorNode.InnerText, out Color color))
            {
                if (material.HasProperty("_BaseColor"))
                    material.SetColor("_BaseColor", color);
                else if (material.HasProperty("_Color"))
                    material.SetColor("_Color", color);
            }
            
            // Apply roughness
            XmlNode roughnessNode = materialInfo["Roughness"];
            if (roughnessNode != null && float.TryParse(roughnessNode.InnerText, out float roughness))
            {
                if (material.HasProperty("_Smoothness"))
                    material.SetFloat("_Smoothness", 1f - roughness);
                else if (material.HasProperty("_Roughness"))
                    material.SetFloat("_Roughness", roughness);
            }
            
            // Apply metalness
            XmlNode metalnessNode = materialInfo["Metalness"];
            if (metalnessNode != null && float.TryParse(metalnessNode.InnerText, out float metalness))
            {
                if (material.HasProperty("_Metallic"))
                    material.SetFloat("_Metallic", metalness);
            }
            
            // Apply emission
            XmlNode emissionColorNode = materialInfo["EmissionColor"];
            XmlNode emissionStrengthNode = materialInfo["EmissionStrength"];
            if (emissionColorNode != null && emissionStrengthNode != null)
            {
                if (UnityEngine.ColorUtility.TryParseHtmlString("#" + emissionColorNode.InnerText, out Color emissionColor) &&
                    float.TryParse(emissionStrengthNode.InnerText, out float emissionStrength))
                {
                    Color finalEmission = emissionColor * emissionStrength;
                    if (material.HasProperty("_EmissionColor"))
                    {
                        material.SetColor("_EmissionColor", finalEmission);
                        material.EnableKeyword("_EMISSION");
                    }
                }
            }
        }


        /// <summary>
        /// Finds the LOD0 mesh in a prefab and creates a new mesh with all transforms applied to the vertices,
        /// normalized so the largest axis = 1, then scaled by modelScale
        /// </summary>
        /// <param name="prefab">The prefab to search through</param>
        /// <param name="modelScale">The ModelScale from World Creator to apply after normalization</param>
        /// <returns>A new mesh with transforms applied, or null if no suitable mesh is found</returns>
        public static Mesh CreateTransformedMesh(GameObject prefab, float modelScale = 1f)
        {
            // Find the LOD0 mesh
            GameObject meshObject = FindLOD0Mesh(prefab);
            if (meshObject == null)
                return null;

            MeshFilter meshFilter = meshObject.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null)
                return null;

            Mesh originalMesh = meshFilter.sharedMesh;
            
            // Calculate the combined transform matrix from prefab root to mesh object
            Matrix4x4 transformMatrix = CalculateTransformMatrix(prefab.transform, meshObject.transform);
            
            // Create a new mesh with transformed vertices, normalized and scaled
            return ApplyTransformToMesh(originalMesh, transformMatrix, modelScale);
        }

        /// <summary>
        /// Finds the LOD0 mesh object in the prefab hierarchy
        /// </summary>
        private static GameObject FindLOD0Mesh(GameObject prefab)
        {
            // Check if the prefab itself has a MeshFilter
            if (prefab.GetComponent<MeshFilter>() != null)
                return prefab;

            // Search through all children for LOD0 variants
            Transform[] allChildren = prefab.GetComponentsInChildren<Transform>();
            GameObject fallbackMesh = null;

            foreach (Transform child in allChildren)
            {
                if (child.GetComponent<MeshFilter>() == null)
                    continue;

                string childName = child.name.ToLower();
                
                // Check for various LOD0 naming conventions
                if (childName.Contains("lod_0") || 
                    childName.Contains("lod0") || 
                    childName.Contains("lod-0") ||
                    childName.Contains("detail_0") ||
                    childName.Contains("detail0"))
                {
                    return child.gameObject;
                }

                // Keep track of any mesh as fallback
                if (fallbackMesh == null)
                    fallbackMesh = child.gameObject;
            }

            return fallbackMesh;
        }

        /// <summary>
        /// Calculates the combined transform matrix from root to target
        /// </summary>
        private static Matrix4x4 CalculateTransformMatrix(Transform root, Transform target)
        {
            Matrix4x4 matrix = Matrix4x4.identity;
            Transform current = target;

            // Walk up the hierarchy until we reach the root's parent
            while (current != null && current != root.parent)
            {
                // Combine local transform matrix
                Matrix4x4 localMatrix = Matrix4x4.TRS(
                    current.localPosition,
                    current.localRotation,
                    current.localScale
                );
                
                matrix = localMatrix * matrix;
                current = current.parent;
            }

            return matrix;
        }

        /// <summary>
        /// Creates a new mesh with the transform applied to all vertices, normalized and scaled
        /// </summary>
        /// <param name="originalMesh">The original mesh</param>
        /// <param name="transformMatrix">Transform matrix to apply</param>
        /// <param name="modelScale">ModelScale to apply after normalization (default 1.0)</param>
        private static Mesh ApplyTransformToMesh(Mesh originalMesh, Matrix4x4 transformMatrix, float modelScale = 1f)
        {
            Mesh newMesh = new Mesh();
            newMesh.name = originalMesh.name + "_Transformed";

            // Copy basic mesh data
            Vector3[] originalVertices = originalMesh.vertices;
            Vector3[] originalNormals = originalMesh.normals;
            Vector4[] originalTangents = originalMesh.tangents;

            // Transform vertices
            Vector3[] transformedVertices = new Vector3[originalVertices.Length];
            for (int i = 0; i < originalVertices.Length; i++)
            {
                transformedVertices[i] = transformMatrix.MultiplyPoint3x4(originalVertices[i]);
            }

            // Assign transformed vertices temporarily to calculate bounds
            newMesh.vertices = transformedVertices;
            newMesh.RecalculateBounds();
            
            // Use mesh bounds to find the maximum extent for normalization
            Bounds bounds = newMesh.bounds;
            Vector3 size = bounds.size;
            float maxExtent = Mathf.Max(size.x, Mathf.Max(size.y, size.z));

            // Normalize so the largest axis = 1, then apply modelScale
            // Keep the original pivot point (don't center the mesh)
            float normalizeAndScaleFactor = maxExtent > 0 ? (modelScale / maxExtent) : modelScale;

            // Apply normalization and scaling to vertices (keeping original pivot)
            for (int i = 0; i < transformedVertices.Length; i++)
            {
                // Only scale, don't center - preserve original pivot point
                transformedVertices[i] = transformedVertices[i] * normalizeAndScaleFactor;
            }

            // Transform normals (use inverse transpose for normals)
            Vector3[] transformedNormals = null;
            if (originalNormals.Length > 0)
            {
                Matrix4x4 normalMatrix = transformMatrix.inverse.transpose;
                transformedNormals = new Vector3[originalNormals.Length];
                for (int i = 0; i < originalNormals.Length; i++)
                {
                    transformedNormals[i] = normalMatrix.MultiplyVector(originalNormals[i]).normalized;
                }
            }

            // Transform tangents
            Vector4[] transformedTangents = null;
            if (originalTangents.Length > 0)
            {
                transformedTangents = new Vector4[originalTangents.Length];
                for (int i = 0; i < originalTangents.Length; i++)
                {
                    Vector3 tangent = new Vector3(originalTangents[i].x, originalTangents[i].y, originalTangents[i].z);
                    Vector3 transformedTangent = transformMatrix.MultiplyVector(tangent).normalized;
                    transformedTangents[i] = new Vector4(transformedTangent.x, transformedTangent.y, transformedTangent.z, originalTangents[i].w);
                }
            }

            // Assign transformed data to new mesh
            newMesh.vertices = transformedVertices;
            if (transformedNormals != null)
                newMesh.normals = transformedNormals;
            if (transformedTangents != null)
                newMesh.tangents = transformedTangents;

            // Copy other mesh data unchanged
            newMesh.uv = originalMesh.uv;
            newMesh.uv2 = originalMesh.uv2;
            newMesh.uv3 = originalMesh.uv3;
            newMesh.uv4 = originalMesh.uv4;
            newMesh.colors = originalMesh.colors;
            newMesh.colors32 = originalMesh.colors32;

            // Copy submeshes and triangles
            newMesh.subMeshCount = originalMesh.subMeshCount;
            for (int i = 0; i < originalMesh.subMeshCount; i++)
            {
                int[] triangles = originalMesh.GetTriangles(i);
                newMesh.SetTriangles(triangles, i);
            }

            // Copy blend shapes if any
            for (int i = 0; i < originalMesh.blendShapeCount; i++)
            {
                string shapeName = originalMesh.GetBlendShapeName(i);
                int frameCount = originalMesh.GetBlendShapeFrameCount(i);
                
                for (int frame = 0; frame < frameCount; frame++)
                {
                    Vector3[] deltaVertices = new Vector3[originalMesh.vertexCount];
                    Vector3[] deltaNormals = new Vector3[originalMesh.vertexCount];
                    Vector3[] deltaTangents = new Vector3[originalMesh.vertexCount];
                    float frameWeight = originalMesh.GetBlendShapeFrameWeight(i, frame);
                    
                    originalMesh.GetBlendShapeFrameVertices(i, frame, deltaVertices, deltaNormals, deltaTangents);
                    
                    // Transform blend shape deltas
                    for (int v = 0; v < deltaVertices.Length; v++)
                    {
                        deltaVertices[v] = transformMatrix.MultiplyVector(deltaVertices[v]);
                        if (deltaNormals[v] != Vector3.zero)
                            deltaNormals[v] = transformMatrix.inverse.transpose.MultiplyVector(deltaNormals[v]).normalized;
                        if (deltaTangents[v] != Vector3.zero)
                            deltaTangents[v] = transformMatrix.MultiplyVector(deltaTangents[v]).normalized;
                    }
                    
                    newMesh.AddBlendShapeFrame(shapeName, frameWeight, deltaVertices, deltaNormals, deltaTangents);
                }
            }

            // Recalculate bounds
            newMesh.RecalculateBounds();

            return newMesh;
        }
        
        #endregion
    }
}