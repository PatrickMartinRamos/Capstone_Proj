using UnityEditor;
using UnityEngine;

namespace Stellarfarer
{
    public class World2_DatabaseGenerator : EditorWindow
    {
        private enum Database
        {
            Hydrious,
            World2Stage,
        }

        private System.Action<Database> _onConfirm;
        private Database _database = Database.Hydrious;

        [MenuItem("Tools/Stellarfarer/World 2: Database Generator")]
        public static void ShowWindow()
            => ShowWindow(onConfirm: GenerateDatabase);

        private static void ShowWindow(System.Action<Database> onConfirm)
        {
            var window = GetWindow<World2_DatabaseGenerator>("World 2: Database Generator");
            window.minSize = new Vector2(300, 50);
            window._onConfirm = onConfirm;
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Choose Database:", EditorStyles.boldLabel);
            _database = (Database)EditorGUILayout.EnumPopup("Database", _database);

            if (GUILayout.Button("Generate"))
                _onConfirm?.Invoke(_database);
            else if (GUILayout.Button("Cancel"))
                Close();
        }

        private static void GenerateDatabase(Database database)
        {
            string databaseFilePath = $"Assets/_World2/Databases/{database}.csv";
            string soFolderPath = $"Assets/_World2/Data/{database}/";

            if (!System.IO.File.Exists(databaseFilePath))
                throw new System.IO.FileNotFoundException($"Database file not found: {databaseFilePath}", databaseFilePath);

            using var reader = new System.IO.StreamReader(databaseFilePath);

            reader.ReadLine();

            string dictSOFilePath = soFolderPath + $"_{database}DictSO.asset";

            ScriptableObject dictSO = database switch
            {
                Database.Hydrious => LoadOrCreateSO<HydriousDictSO>(dictSOFilePath),
                Database.World2Stage => LoadOrCreateSO<World2StageDictSO>(dictSOFilePath),
                _ => null
            };

            if (dictSO == null)
                throw new System.NullReferenceException($"Failed to create or load ScriptableObject for {database}");

            int undoGroupIndex = SetUndoGroup(database);

            while (!reader.EndOfStream)
            {
                string[] values = reader.ReadLine().Split(',');

                GenerateSO(values, database, soFolderPath, dictSO);
            }

            Undo.CollapseUndoOperations(undoGroupIndex);

            AssetDatabase.SaveAssets();
        }

        private static void GenerateSO<TSO>(
            string[] values,
            Database database,
            string soFolderPath,
            TSO dictSO)
            where TSO : ScriptableObject
        {
            // Update SO
            string soName = database switch
            {
                Database.World2Stage => $"{database}{values[0].Trim()}SO",
                _ => $"{values[0].Trim()}SO"
            };
            string soFilePath = soFolderPath + $"{soName}.asset";

            ScriptableObject so = database switch
            {
                Database.Hydrious => LoadOrCreateSO<HydriousSO>(soFilePath),
                Database.World2Stage => LoadOrCreateSO<World2StageSO>(soFilePath),
                _ => null
            };

            if (so == null)
                throw new System.NullReferenceException($"Failed to create or load ScriptableObject for {database}");

            if (so.name != soName)
                so.name = soName;

            // Find the UpdateSO() method, regardless of parameter count
            var updateSOMethod = so.GetType().GetMethod(
                "UpdateSO",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public
            ) ?? throw new System.MissingMethodException($"{so.GetType().Name} does not implement a public UpdateSO() method.");

            // Prepare arguments dynamically based on database type
            object[] args = database switch
            {
                Database.Hydrious => new object[]
                {
                    Utils.StringToEnumFlag<Species>(values[0]),
                    Utils.StringToEnumFlag<CleansingType>(values[1]),
                    Utils.StringToInt(values[2]),
                    Utils.StringToColor(values[3])
                },
                Database.World2Stage => new object[]
                {
                    Utils.StringToInt(values[0]),
                    Utils.StringToEnumFlag<CleansingType>(values[1]),
                    Utils.StringToInt(values[2]),
                    Utils.StringToFloat(values[3])
                },
                _ => System.Array.Empty<object>()
            };

            // Dynamically invoke UpdateSO(...)
            updateSOMethod.Invoke(so, args);

            EditorUtility.SetDirty(so switch
            {
                HydriousSO hydriousSO => hydriousSO,
                World2StageSO world2StageSO => world2StageSO,
                _ => null
            });
            AssetDatabase.SaveAssetIfDirty(so switch
            {
                HydriousSO hydriousSO => hydriousSO,
                World2StageSO world2StageSO => world2StageSO,
                _ => null
            });

            // Update Dict SO
            var tryAddMethod = dictSO.GetType().GetMethod(
                "TryAdd",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public
            ) ?? throw new System.MissingMethodException($"{dictSO.GetType().Name} does not implement a public Add() method.");

            tryAddMethod.Invoke(dictSO, new object[] { so });

            EditorUtility.SetDirty(dictSO switch
            {
                HydriousDictSO hydriousDictSO => hydriousDictSO,
                World2StageDictSO world2StageDictSO => world2StageDictSO,
                _ => null
            });
            AssetDatabase.SaveAssetIfDirty(dictSO switch
            {
                HydriousDictSO hydriousDictSO => hydriousDictSO,
                World2StageDictSO world2StageDictSO => world2StageDictSO,
                _ => null
            });
        }

        private static int SetUndoGroup(Database database)
        {
            string undoGroupName = $"Generate {database} Database";

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName(undoGroupName);

            return Undo.GetCurrentGroup();
        }

        private static TSO LoadOrCreateSO<TSO>(string soFilePath) where TSO : ScriptableObject
        {
            TSO so = AssetDatabase.LoadAssetAtPath<TSO>(soFilePath);

            if (so == null)
            {
                so = CreateInstance<TSO>();
                AssetDatabase.CreateAsset(so, soFilePath);
            }

            return so;
        }
    }
}