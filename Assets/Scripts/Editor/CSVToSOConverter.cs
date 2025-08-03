using System;
using System.Collections.Generic;
using System.IO;
using CapstoneProj.EnumSystem;
using CapstoneProj.MiscSystem;
using CapstoneProj.ScriptableObjectSystem;
using UnityEditor;
using UnityEngine;

namespace CapstoneProject.EditorSystem
{
    public enum CSVType
    {
        Worlds,
        Stages,
        Bombs,
    }

    public class CSVToSOConverter : EditorWindow
    {
        Action<CSVType> _onConfirm;
        CSVType _csv = CSVType.Worlds;

        static void ShowWindow(Action<CSVType> onConfirm)
        {
            CSVToSOConverter window = GetWindow<CSVToSOConverter>("CSV to SO Converter");
            window.minSize = new(300, 50);
            window._onConfirm = onConfirm;
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("CSV", EditorStyles.boldLabel);
            _csv = (CSVType)EditorGUILayout.EnumPopup("CSV Type", _csv);

            if (GUILayout.Button("Confirm")) _onConfirm?.Invoke(_csv);
            else if (GUILayout.Button("Cancel")) Close();
        }

        [MenuItem("Tools/Capstone Project/CSV to SO Converter")]
        public static void ShowRebuildWindow()
        {
            ShowWindow(onConfirm: ConvertCSVToSO);
        }

        #region Convert CSV to Scriptable Objects
        private static void ConvertCSVToSO(CSVType csvType)
        {
            string csvFilePath = Application.dataPath + $"/CSVs/{csvType}.csv";
            string scriptableObjectFilePath = $"Assets/ScriptableObjects/{csvType}/";

            if (!File.Exists(csvFilePath)) return;

            using StreamReader reader = new(csvFilePath);

            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string[] values = reader.ReadLine().Split(',');

                CreateScriptableObject(values: values, csvType: csvType, scriptableObjectFilePath: scriptableObjectFilePath);
            }

            AssetDatabase.SaveAssets();
        }

        private static void CreateScriptableObject(string[] values, CSVType csvType, string scriptableObjectFilePath)
        {
            switch (csvType)
            {
                case CSVType.Worlds:
                    // WORLD SO
                    string worldSOName = $"{Utils.RemoveWhiteSpaceAndDash(values[1])}SO";
                    string worldSOFilePath = scriptableObjectFilePath + $"{worldSOName}.asset";

                    WorldSO worldSO = LoadOrCreateAsset<WorldSO>(worldSOFilePath);

                    if (worldSO.name != worldSOName)
                        worldSO.name = worldSOName;

                    if (int.TryParse(values[0], out int worldSOWorldID))
                    {
                        if (worldSO.WorldID != worldSOWorldID)
                            worldSO.WorldID = worldSOWorldID;
                    }

                    string worldSOWorldName = values[1];
                    if (worldSO.WorldName != worldSOWorldName)
                        worldSO.WorldName = worldSOWorldName;

                    string worldSOWorldDescription = values[2];
                    if (worldSO.WorldDescription != worldSOWorldDescription)
                        worldSO.WorldDescription = worldSOWorldDescription;

                    EditorUtility.SetDirty(worldSO);
                    AssetDatabase.SaveAssetIfDirty(worldSO);

                    // WORLD LIST SO
                    string worldListSOName = "WorldListSO";
                    string worldListSOFilePath = scriptableObjectFilePath + $"{worldListSOName}.asset";

                    WorldListSO worldListSO = LoadOrCreateAsset<WorldListSO>(worldListSOFilePath);

                    int index = worldListSO.WorldSOList.FindIndex(worldSOListElement => worldSOListElement == worldSO);

                    if (index != -1)
                        worldListSO.WorldSOList[index] = worldSO;

                    EditorUtility.SetDirty(worldListSO);
                    AssetDatabase.SaveAssetIfDirty(worldListSO);

                    break;
                case CSVType.Stages:
                    // STAGE SO
                    string stageSOName = $"{Utils.RemoveWhiteSpaceAndDash(values[2])}SO";
                    string stageSOFilePath = scriptableObjectFilePath + $"{stageSOName}.asset";

                    StageSO stageSO = LoadOrCreateAsset<StageSO>(stageSOFilePath);

                    if (stageSO.name != stageSOName)
                        stageSO.name = stageSOName;

                    if (int.TryParse(values[0], out int stageSOStageID))
                    {
                        if (stageSO.StageID != stageSOStageID)
                            stageSO.StageID = stageSOStageID;
                    }

                    worldListSOFilePath = $"Assets/ScriptableObjects/Worlds/WorldListSO.asset";
                    worldListSO = LoadOrCreateAsset<WorldListSO>(worldListSOFilePath);
                    if (int.TryParse(values[1], out int stageSOWorldID))
                    {
                        worldSO = worldListSO.WorldSOList.Find(worldSO => worldSO.WorldID == stageSOWorldID);
                        if (worldSO != null)
                            stageSO.WorldSO = worldSO;
                    }

                    string stageSOStageName = values[2];
                    if (stageSO.StageName != stageSOStageName)
                        stageSO.StageName = stageSOStageName;

                    if (float.TryParse(values[3], out float stageSOStageDuration))
                    {
                        if (stageSO.StageDuration != stageSOStageDuration)
                            stageSO.StageDuration = stageSOStageDuration;
                    }
                    
                    if (int.TryParse(values[4], out int stageSOInitialBombCount))
                    {
                        if (stageSO.InitialBombCount != stageSOInitialBombCount)
                            stageSO.InitialBombCount = stageSOInitialBombCount;
                    }

                    stageSO.DefusalType = DefusalType.None;

                    if (bool.TryParse(values[5], out bool isPlottingDefusalTypeIncluded))
                    {
                        if (isPlottingDefusalTypeIncluded)
                            stageSO.DefusalType |= DefusalType.Plotting;
                    }

                    if (bool.TryParse(values[6], out bool isMidpointDefusalTypeIncluded))
                    {
                        if (isMidpointDefusalTypeIncluded)
                            stageSO.DefusalType |= DefusalType.Midpoint;
                    }

                    if (bool.TryParse(values[7], out bool isDistanceDefusalTypeIncluded))
                    {
                        if (isDistanceDefusalTypeIncluded)
                            stageSO.DefusalType |= DefusalType.Distance;
                    }

                    stageSO.BombType = BombType.None;

                    if (bool.TryParse(values[8], out bool IsGrayBombIncluded))
                    {
                        if (IsGrayBombIncluded)
                            stageSO.BombType |= BombType.GrayBomb;
                    }

                    if (bool.TryParse(values[9], out bool IsBlueBombIncluded))
                    {
                        if (IsBlueBombIncluded)
                            stageSO.BombType |= BombType.BlueBomb;
                    }

                    if (bool.TryParse(values[10], out bool IsGreenBombIncluded))
                    {
                        if (IsGreenBombIncluded)
                            stageSO.BombType |= BombType.GreenBomb;
                    }

                    if (bool.TryParse(values[11], out bool IsYellowBombIncluded))
                    {
                        if (IsYellowBombIncluded)
                            stageSO.BombType |= BombType.YellowBomb;
                    }

                    if (bool.TryParse(values[12], out bool IsVioletBombIncluded))
                    {
                        if (IsVioletBombIncluded)
                            stageSO.BombType |= BombType.VioletBomb;
                    }

                    if (bool.TryParse(values[13], out bool IsRainbowBombIncluded))
                    {
                        if (IsRainbowBombIncluded)
                            stageSO.BombType |= BombType.RainbowBomb;
                    }

                    stageSO.ProgressStarTypeBombSpawnIntervalList ??= new List<ProgressStarTypeBombSpawnInterval>();

                    if (float.TryParse(values[14], out float zeroStarBombSpawnInterval))
                    {
                        index = stageSO.ProgressStarTypeBombSpawnIntervalList.FindIndex(x => x.progressStarType == ProgressStarType.Star0);
                        if (index != -1)
                        {
                            var item = stageSO.ProgressStarTypeBombSpawnIntervalList[index];
                            if (item.BombSpawnInterval != zeroStarBombSpawnInterval)
                            {
                                item.BombSpawnInterval = zeroStarBombSpawnInterval;
                                stageSO.ProgressStarTypeBombSpawnIntervalList[index] = item; // Reassign the modified copy
                            }
                        }
                        else
                        {
                            stageSO.ProgressStarTypeBombSpawnIntervalList.Add(new ProgressStarTypeBombSpawnInterval
                            {
                                progressStarType = ProgressStarType.Star0,
                                BombSpawnInterval = zeroStarBombSpawnInterval
                            });
                        }
                    }

                    if (float.TryParse(values[15], out float oneStarBombSpawnInterval))
                    {
                        index = stageSO.ProgressStarTypeBombSpawnIntervalList.FindIndex(x => x.progressStarType == ProgressStarType.Star1);
                        if (index != -1)
                        {
                            var item = stageSO.ProgressStarTypeBombSpawnIntervalList[index];
                            if (item.BombSpawnInterval != oneStarBombSpawnInterval)
                            {
                                item.BombSpawnInterval = oneStarBombSpawnInterval;
                                stageSO.ProgressStarTypeBombSpawnIntervalList[index] = item; // Reassign the modified copy
                            }
                        }
                        else
                        {
                            stageSO.ProgressStarTypeBombSpawnIntervalList.Add(new ProgressStarTypeBombSpawnInterval
                            {
                                progressStarType = ProgressStarType.Star1,
                                BombSpawnInterval = oneStarBombSpawnInterval
                            });
                        }
                    }

                    if (float.TryParse(values[16], out float twoStarBombSpawnInterval))
                    {
                        index = stageSO.ProgressStarTypeBombSpawnIntervalList.FindIndex(x => x.progressStarType == ProgressStarType.Star2);
                        if (index != -1)
                        {
                            var item = stageSO.ProgressStarTypeBombSpawnIntervalList[index];
                            if (item.BombSpawnInterval != twoStarBombSpawnInterval)
                            {
                                item.BombSpawnInterval = twoStarBombSpawnInterval;
                                stageSO.ProgressStarTypeBombSpawnIntervalList[index] = item; // Reassign the modified copy
                            }
                        }
                        else
                        {
                            stageSO.ProgressStarTypeBombSpawnIntervalList.Add(new ProgressStarTypeBombSpawnInterval
                            {
                                progressStarType = ProgressStarType.Star2,
                                BombSpawnInterval = twoStarBombSpawnInterval
                            });
                        }
                    }

                    if (int.TryParse(values[17], out int stageSORequiredPoints))
                    {
                        if (stageSO.RequiredPoints != stageSORequiredPoints)
                            stageSO.RequiredPoints = stageSORequiredPoints;
                    }

                    EditorUtility.SetDirty(stageSO);
                    AssetDatabase.SaveAssetIfDirty(stageSO);

                    // STAGE LIST SO
                    string stageListSOName = "StageListSO";
                    string stageListSOFilePath = scriptableObjectFilePath + $"{stageListSOName}.asset";

                    StageListSO stageListSO = LoadOrCreateAsset<StageListSO>(stageListSOFilePath);

                    index = stageListSO.StageSOList.FindIndex(stageSOListElement => stageSOListElement == stageSO);

                    if (index != -1)
                        stageListSO.StageSOList[index] = stageSO;

                    EditorUtility.SetDirty(stageListSO);
                    AssetDatabase.SaveAssetIfDirty(stageListSO);

                    break;
                case CSVType.Bombs:
                    // BOMB SO
                    string bombSOName = $"{values[1]}SO";
                    string bombSOFilePath = scriptableObjectFilePath + $"{bombSOName}.asset";

                    BombSO bombSO = LoadOrCreateAsset<BombSO>(bombSOFilePath);

                    if (bombSO.name != bombSOName)
                        bombSO.name = bombSOName;

                    string bombSOBombName = values[0];
                    if (bombSO.BombName != bombSOBombName)
                        bombSO.BombName = bombSOBombName;

                    string bombSOBombType = values[1];
                    if (Enum.TryParse(bombSOBombType, out BombType bombType))
                    {
                        if (bombSO.BombType != bombType)
                            bombSO.BombType = bombType;
                    }
                    else
                        Debug.LogWarning($"{bombSO.name}'s BombType, {bombSOBombType}, is invalid!");

                    string bombSOBombSprite = values[2];
                    string bombSpriteAssetPath = $"Assets/Gallery/Images/Bombs/{bombSOBombSprite}.png";
                    if (AssetDatabase.AssetPathExists(bombSpriteAssetPath))
                    {
                        Sprite bombSprite = AssetDatabase.LoadAssetAtPath<Sprite>(bombSpriteAssetPath);

                        if (bombSO.BombSprite != bombSprite)
                            bombSO.BombSprite = bombSprite;
                    }
                    else
                        Debug.LogWarning($"{bombSO.name}'s BombSprite, {bombSOBombSprite}, is not found!");

                    string bombSOBombPrefab = values[3];
                    string bombPrefabAssetPath = $"Assets/Prefabs/Bombs/{bombSOBombPrefab}.prefab";
                    if (AssetDatabase.AssetPathExists(bombPrefabAssetPath))
                    {
                        Transform bombPrefab = AssetDatabase.LoadAssetAtPath<Transform>(bombPrefabAssetPath);

                        if (bombSO.BombPrefab != bombPrefab)
                            bombSO.BombPrefab = bombPrefab;
                    }
                    else
                        Debug.LogWarning($"{bombSO.name}'s BombPrefab, {bombSOBombPrefab}, is not found!");

                    string bombSOExplosionType = values[4];
                    if (Enum.TryParse(bombSOExplosionType, out ExplosionType explosionType))
                    {
                        if (bombSO.ExplosionType != explosionType)
                            bombSO.ExplosionType = explosionType;
                    }
                    else
                        Debug.LogWarning($"{bombSO.name}'s ExplosionType, {bombSOExplosionType}, is invalid!");

                    string bombSODefusalType = values[5];
                    if (Enum.TryParse(bombSODefusalType, out DefusalType defusalType))
                    {
                        if (bombSO.DefusalType != defusalType)
                            bombSO.DefusalType = defusalType;
                    }
                    else
                        Debug.LogWarning($"{bombSO.name}'s DefusalType, {bombSODefusalType}, is invalid!");

                    string bombSOCorrectPoints = values[6];
                    if (int.TryParse(bombSOCorrectPoints, out int correctPoints))
                    {
                        if (bombSO.CorrectPoints != correctPoints)
                            bombSO.CorrectPoints = correctPoints;
                    }
                    else
                        Debug.LogWarning($"{bombSO.name}'s CorrectPoints, {bombSOCorrectPoints}, is invalid!");

                    string bombSOIncorrectPoints = values[7];
                    if (int.TryParse(bombSOIncorrectPoints, out int incorrectPoints))
                    {
                        if (bombSO.IncorrectPoints != incorrectPoints)
                            bombSO.IncorrectPoints = incorrectPoints;
                    }
                    else
                        Debug.LogWarning($"{bombSO.name}'s IncorrectPoints, {bombSOIncorrectPoints}, is invalid!");

                    EditorUtility.SetDirty(bombSO);
                    AssetDatabase.SaveAssetIfDirty(bombSO);

                    // BOMB LIST SO
                    string bombListSOName = "BombListSO";
                    string bombListSOFilePath = scriptableObjectFilePath + $"{bombListSOName}.asset";

                    BombListSO bombListSO = LoadOrCreateAsset<BombListSO>(bombListSOFilePath);

                    index = bombListSO.BombSOList.FindIndex(bombSOListElement => bombSOListElement == bombSO);

                    if (index != -1)
                        bombListSO.BombSOList[index] = bombSO;

                    EditorUtility.SetDirty(bombListSO);
                    AssetDatabase.SaveAssetIfDirty(bombListSO);

                    break;
                default:
                    break;
            }

            AssetDatabase.SaveAssets();
        }
        
        private static T LoadOrCreateAsset<T>(string path) where T : ScriptableObject
        {
            if (AssetDatabase.AssetPathExists(path))
                return AssetDatabase.LoadAssetAtPath<T>(path);
            else
            {
                T asset = CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, path);
                return asset;
            }
        }
        #endregion
    }
}