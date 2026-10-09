using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Sorcerum.LevelEditor
{
    public static class LevelDataGenerator
    {
        [MenuItem("Tools/Generate All 20 Levels")]
        public static void GenerateAllLevels()
        {
            // Discs
            var discCyan = AssetDatabase.LoadAssetAtPath<StackableItemDataSo>("Assets/Data/SimpleStackableItem.asset");
            var discPink = AssetDatabase.LoadAssetAtPath<StackableItemDataSo>("Assets/Data/SimpleStackableItem_Pink.asset");
            var discYellow = AssetDatabase.LoadAssetAtPath<StackableItemDataSo>("Assets/Data/SimpleStackableItem_Yellow.asset");
            var discPurple = AssetDatabase.LoadAssetAtPath<StackableItemDataSo>("Assets/Data/SimpleStackableItem_Purple.asset");

            // Ball Dictionary
            var balls = new Dictionary<string, BallDataSo>
            {
                ["Cyan_1"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Cyan_1.asset"),
                ["Cyan_2"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Cyan_2.asset"),
                ["Cyan_3"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Cyan_3.asset"),
                ["Cyan_4"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Cyan_4.asset"),
                ["Cyan_5"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall.asset"),
                ["Cyan_6"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Cyan_6.asset"),
                ["Cyan_8"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_8.asset"),

                ["Pink_1"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Pink_1.asset"),
                ["Pink_2"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Pink_2.asset"),
                ["Pink_3"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_3.asset"),
                ["Pink_4"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Pink_4.asset"),
                ["Pink_5"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Pink_5.asset"),
                ["Pink_6"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Pink_6.asset"),
                ["Pink_8"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Pink_8.asset"),

                ["Yellow_2"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Yellow_2.asset"),
                ["Yellow_3"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Yellow_3.asset"),
                ["Yellow_4"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Yellow_4.asset"),
                ["Yellow_5"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Yellow_5.asset"),
                ["Yellow_6"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Yellow_6.asset"),
                ["Yellow_8"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Yellow_8.asset"),

                ["Purple_2"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Purple_2.asset"),
                ["Purple_3"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Purple_3.asset"),
                ["Purple_4"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Purple_4.asset"),
                ["Purple_5"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Purple_5.asset"),
                ["Purple_6"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Purple_6.asset"),
                ["Purple_8"] = AssetDatabase.LoadAssetAtPath<BallDataSo>("Assets/Data/SimpleBall_Purple_8.asset")
            };

            var stackPoolKey = new PoolKey("simpleStack");
            var emptyGridCellKey = new PoolKey("emptyGridCell");
            var stackGridCellKey = new PoolKey("stackGridCell");

            for (int i = 1; i <= 20; i++)
            {
                string path = $"Assets/Data/Levels/LevelData_{i:D2}.asset";
                var levelSo = AssetDatabase.LoadAssetAtPath<LevelDataSo>(path);
                if (levelSo == null)
                {
                    levelSo = ScriptableObject.CreateInstance<LevelDataSo>();
                    AssetDatabase.CreateAsset(levelSo, path);
                }

                List<StackableItemStackData> stacks;
                List<BallDataSo> availableBalls;
                Vector2Int dimensions;

                switch (i)
                {
                    case 1:
                        dimensions = new Vector2Int(3, 3);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discPink })
                        };
                        availableBalls = new List<BallDataSo> { balls["Cyan_2"], balls["Pink_2"] };
                        break;

                    case 2:
                        dimensions = new Vector2Int(3, 3);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 0), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discCyan })
                        };
                        availableBalls = new List<BallDataSo> { balls["Cyan_3"], balls["Pink_2"] };
                        break;

                    case 3:
                        dimensions = new Vector2Int(4, 4);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discPink, discCyan }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discCyan, discPink })
                        };
                        availableBalls = new List<BallDataSo> { balls["Cyan_2"], balls["Pink_4"], balls["Cyan_4"] };
                        break;

                    case 4:
                        dimensions = new Vector2Int(4, 4);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discPink, discCyan }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discCyan, discPink }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discCyan })
                        };
                        availableBalls = new List<BallDataSo> { balls["Cyan_2"], balls["Pink_4"], balls["Cyan_5"] };
                        break;

                    case 5:
                        dimensions = new Vector2Int(4, 4);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 0), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discCyan, discPink }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(2, 0), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discPink, discCyan }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discCyan })
                        };
                        availableBalls = new List<BallDataSo> { balls["Pink_3"], balls["Cyan_6"], balls["Pink_5"] };
                        break;

                    case 6:
                        dimensions = new Vector2Int(4, 4);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discPink, discCyan }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discCyan, discPink }),
                            new(new Vector2Int(1, 3), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discCyan, discPink }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discPink, discCyan }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discCyan })
                        };
                        availableBalls = new List<BallDataSo> { balls["Pink_3"], balls["Cyan_4"], balls["Pink_5"], balls["Cyan_6"] };
                        break;

                    case 7:
                        dimensions = new Vector2Int(4, 4);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(0, 1), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discPink, discCyan }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discCyan, discPink }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(3, 1), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(3, 2), new List<StackableItemDataSo> { discCyan })
                        };
                        availableBalls = new List<BallDataSo> { balls["Cyan_3"], balls["Pink_5"], balls["Cyan_5"], balls["Pink_6"] };
                        break;

                    case 8:
                        dimensions = new Vector2Int(4, 4);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 0), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discPink, discYellow }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(2, 0), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discCyan, discPink }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discCyan })
                        };
                        availableBalls = new List<BallDataSo> { balls["Yellow_2"], balls["Pink_5"], balls["Cyan_5"], balls["Yellow_6"] };
                        break;

                    case 9:
                        dimensions = new Vector2Int(4, 4);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(0, 1), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discCyan, discYellow }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discPink, discCyan }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(3, 1), new List<StackableItemDataSo> { discYellow, discPink }),
                            new(new Vector2Int(3, 2), new List<StackableItemDataSo> { discPink })
                        };
                        availableBalls = new List<BallDataSo> { balls["Yellow_2"], balls["Cyan_5"], balls["Pink_5"], balls["Yellow_6"], balls["Pink_8"] };
                        break;

                    case 10:
                        dimensions = new Vector2Int(4, 4);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 0), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discCyan, discPink }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(1, 3), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(2, 0), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discPink, discYellow }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discPink })
                        };
                        availableBalls = new List<BallDataSo> { balls["Pink_2"], balls["Yellow_3"], balls["Cyan_4"], balls["Pink_6"], balls["Yellow_8"] };
                        break;

                    case 11:
                        dimensions = new Vector2Int(5, 5);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discCyan, discYellow }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discYellow, discPink }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discPink, discCyan }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discYellow, discCyan }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(3, 1), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(3, 2), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(3, 3), new List<StackableItemDataSo> { discPink, discYellow })
                        };
                        availableBalls = new List<BallDataSo> { balls["Yellow_2"], balls["Pink_3"], balls["Cyan_5"], balls["Yellow_6"], balls["Pink_8"] };
                        break;

                    case 12:
                        dimensions = new Vector2Int(5, 5);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discPink, discCyan }),
                            new(new Vector2Int(1, 3), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discYellow, discPink }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discCyan, discYellow }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(3, 1), new List<StackableItemDataSo> { discCyan, discPink }),
                            new(new Vector2Int(3, 2), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(3, 3), new List<StackableItemDataSo> { discCyan })
                        };
                        availableBalls = new List<BallDataSo> { balls["Cyan_3"], balls["Pink_4"], balls["Yellow_5"], balls["Pink_6"], balls["Cyan_8"] };
                        break;

                    case 13:
                        dimensions = new Vector2Int(5, 5);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discPurple }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discCyan, discPurple }),
                            new(new Vector2Int(1, 3), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discPink, discYellow }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discPurple, discPink }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(3, 1), new List<StackableItemDataSo> { discYellow, discCyan }),
                            new(new Vector2Int(3, 2), new List<StackableItemDataSo> { discPurple }),
                            new(new Vector2Int(3, 3), new List<StackableItemDataSo> { discPink })
                        };
                        availableBalls = new List<BallDataSo> { balls["Purple_2"], balls["Yellow_4"], balls["Pink_5"], balls["Cyan_6"], balls["Purple_8"] };
                        break;

                    case 14:
                        dimensions = new Vector2Int(5, 5);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discPurple, discCyan }),
                            new(new Vector2Int(1, 3), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(2, 0), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discYellow, discPink }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discCyan, discPurple }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(3, 1), new List<StackableItemDataSo> { discPurple, discYellow }),
                            new(new Vector2Int(3, 2), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(3, 3), new List<StackableItemDataSo> { discPurple })
                        };
                        availableBalls = new List<BallDataSo> { balls["Cyan_3"], balls["Pink_4"], balls["Yellow_5"], balls["Purple_6"], balls["Cyan_6"], balls["Pink_8"] };
                        break;

                    case 15:
                        dimensions = new Vector2Int(5, 5);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 0), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discPink, discYellow }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(1, 3), new List<StackableItemDataSo> { discPurple, discCyan }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discPurple, discYellow }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(3, 1), new List<StackableItemDataSo> { discCyan, discPink }),
                            new(new Vector2Int(3, 2), new List<StackableItemDataSo> { discPurple }),
                            new(new Vector2Int(3, 3), new List<StackableItemDataSo> { discYellow })
                        };
                        availableBalls = new List<BallDataSo> { balls["Yellow_2"], balls["Cyan_4"], balls["Pink_5"], balls["Purple_6"], balls["Yellow_6"], balls["Purple_8"] };
                        break;

                    case 16:
                        dimensions = new Vector2Int(5, 5);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discPurple, discCyan }),
                            new(new Vector2Int(1, 3), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(2, 0), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discYellow, discPink }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discPurple, discYellow }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(2, 4), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(3, 1), new List<StackableItemDataSo> { discPurple }),
                            new(new Vector2Int(3, 2), new List<StackableItemDataSo> { discCyan, discPurple }),
                            new(new Vector2Int(3, 3), new List<StackableItemDataSo> { discYellow })
                        };
                        availableBalls = new List<BallDataSo> { balls["Cyan_3"], balls["Pink_4"], balls["Yellow_5"], balls["Purple_6"], balls["Cyan_8"], balls["Yellow_8"] };
                        break;

                    case 17:
                        dimensions = new Vector2Int(5, 5);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discPurple }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discYellow, discPurple }),
                            new(new Vector2Int(1, 3), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discCyan, discPink }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discPurple, discCyan }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(3, 0), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(3, 1), new List<StackableItemDataSo> { discPink, discYellow }),
                            new(new Vector2Int(3, 2), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(3, 3), new List<StackableItemDataSo> { discPurple }),
                            new(new Vector2Int(3, 4), new List<StackableItemDataSo> { discPink })
                        };
                        availableBalls = new List<BallDataSo> { balls["Purple_2"], balls["Yellow_4"], balls["Pink_5"], balls["Cyan_6"], balls["Purple_6"], balls["Cyan_8"] };
                        break;

                    case 18:
                        dimensions = new Vector2Int(5, 5);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(0, 2), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discPink, discYellow }),
                            new(new Vector2Int(1, 3), new List<StackableItemDataSo> { discPurple }),
                            new(new Vector2Int(2, 0), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discCyan, discPink }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discPurple, discCyan }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(2, 4), new List<StackableItemDataSo> { discPurple }),
                            new(new Vector2Int(3, 1), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(3, 2), new List<StackableItemDataSo> { discYellow, discPurple }),
                            new(new Vector2Int(3, 3), new List<StackableItemDataSo> { discPink })
                        };
                        availableBalls = new List<BallDataSo> { balls["Cyan_3"], balls["Yellow_4"], balls["Pink_5"], balls["Purple_6"], balls["Cyan_6"], balls["Yellow_8"], balls["Pink_8"] };
                        break;

                    case 19:
                        dimensions = new Vector2Int(5, 5);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(1, 0), new List<StackableItemDataSo> { discPurple }),
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discCyan, discPurple }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(1, 3), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discPurple, discPink }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discYellow, discPurple }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(2, 4), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(3, 1), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(3, 2), new List<StackableItemDataSo> { discCyan, discYellow }),
                            new(new Vector2Int(3, 3), new List<StackableItemDataSo> { discPurple }),
                            new(new Vector2Int(3, 4), new List<StackableItemDataSo> { discCyan })
                        };
                        availableBalls = new List<BallDataSo> { balls["Purple_2"], balls["Cyan_4"], balls["Yellow_5"], balls["Pink_6"], balls["Purple_6"], balls["Cyan_8"], balls["Yellow_8"] };
                        break;

                    case 20:
                        dimensions = new Vector2Int(5, 5);
                        stacks = new List<StackableItemStackData>
                        {
                            new(new Vector2Int(0, 2), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(1, 1), new List<StackableItemDataSo> { discCyan }),
                            new(new Vector2Int(1, 2), new List<StackableItemDataSo> { discPink, discCyan }),
                            new(new Vector2Int(1, 3), new List<StackableItemDataSo> { discPurple }),
                            new(new Vector2Int(2, 0), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(2, 1), new List<StackableItemDataSo> { discYellow, discPink }),
                            new(new Vector2Int(2, 2), new List<StackableItemDataSo> { discPurple, discYellow, discCyan }),
                            new(new Vector2Int(2, 3), new List<StackableItemDataSo> { discCyan, discPurple }),
                            new(new Vector2Int(2, 4), new List<StackableItemDataSo> { discYellow }),
                            new(new Vector2Int(3, 1), new List<StackableItemDataSo> { discPurple }),
                            new(new Vector2Int(3, 2), new List<StackableItemDataSo> { discCyan, discPurple }),
                            new(new Vector2Int(3, 3), new List<StackableItemDataSo> { discPink }),
                            new(new Vector2Int(4, 2), new List<StackableItemDataSo> { discPurple })
                        };
                        availableBalls = new List<BallDataSo> { balls["Yellow_3"], balls["Cyan_5"], balls["Pink_6"], balls["Purple_6"], balls["Yellow_6"], balls["Cyan_8"], balls["Purple_8"] };
                        break;

                    default:
                        continue;
                }

                var mapGrid = new GridDataVo(dimensions, 1f, 0.5f);
                var ballData = new AvailableBallData(availableBalls);
                var vo = new LevelDataVo(stacks, ballData, mapGrid, stackPoolKey, emptyGridCellKey, stackGridCellKey);

                levelSo.Initialize(vo);
                EditorUtility.SetDirty(levelSo);
                Debug.Log($"[LevelDataGenerator] Level {i:D2} generated successfully with {availableBalls.Count} balls and {stacks.Count} stacks!");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[LevelDataGenerator] All 20 levels generated and saved successfully!");
        }
    }
}
