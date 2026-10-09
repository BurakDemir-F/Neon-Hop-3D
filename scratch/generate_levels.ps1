$discs = @{
    "Cyan" = "4d4afa9e79a429a4c9ffd57b2100833a"
    "Pink" = "7b585aff59fc4635a06991169d5cb27f"
    "Yellow" = "a1b2c3d4e5f601010101010101010102"
    "Purple" = "a1b2c3d4e5f602020202020202020202"
}

$balls = @{
    "Cyan_1" = "b00000000000000000000c0100000002"
    "Cyan_2" = "b00000000000000000000c0200000002"
    "Cyan_3" = "b00000000000000000000c0300000002"
    "Cyan_4" = "b00000000000000000000c0400000002"
    "Cyan_5" = "179cec27b7322344c98cce586f032401"
    "Cyan_6" = "b00000000000000000000c0600000002"
    "Cyan_8" = "c9e303e0ae404fc2818c10bc3fba8bf4"

    "Pink_1" = "b00000000000000000000d0100000002"
    "Pink_2" = "b00000000000000000000d0200000002"
    "Pink_3" = "f159a70b6f7e4055b0a68a8064046bb1"
    "Pink_4" = "b00000000000000000000d0400000002"
    "Pink_5" = "b00000000000000000000d0500000002"
    "Pink_6" = "b00000000000000000000d0600000002"
    "Pink_8" = "b00000000000000000000d0800000002"

    "Yellow_2" = "b00000000000000000000e0200000002"
    "Yellow_3" = "b00000000000000000000e0300000002"
    "Yellow_4" = "b00000000000000000000e0400000002"
    "Yellow_5" = "b00000000000000000000e0500000002"
    "Yellow_6" = "b00000000000000000000e0600000002"
    "Yellow_8" = "b00000000000000000000e0800000002"

    "Purple_2" = "b00000000000000000000a0200000002"
    "Purple_3" = "b00000000000000000000a0300000002"
    "Purple_4" = "b00000000000000000000a0400000002"
    "Purple_5" = "b00000000000000000000a0500000002"
    "Purple_6" = "b00000000000000000000a0600000002"
    "Purple_8" = "b00000000000000000000f0800000002"
}

function Build-LevelYaml($num, $dimX, $dimY, $stackList, $ballList) {
    $sb = [System.Text.StringBuilder]::new()
    [void]$sb.AppendLine("%YAML 1.1")
    [void]$sb.AppendLine("%TAG !u! tag:unity3d.com,2011:")
    [void]$sb.AppendLine("--- !u!114 &11400000")
    [void]$sb.AppendLine("MonoBehaviour:")
    [void]$sb.AppendLine("  m_ObjectHideFlags: 0")
    [void]$sb.AppendLine("  m_CorrespondingSourceObject: {fileID: 0}")
    [void]$sb.AppendLine("  m_PrefabInstance: {fileID: 0}")
    [void]$sb.AppendLine("  m_PrefabAsset: {fileID: 0}")
    [void]$sb.AppendLine("  m_GameObject: {fileID: 0}")
    [void]$sb.AppendLine("  m_Enabled: 1")
    [void]$sb.AppendLine("  m_EditorHideFlags: 0")
    [void]$sb.AppendLine("  m_Script: {fileID: 11500000, guid: d6002367a8804e3095cbe5ab65b94d71, type: 3}")
    [void]$sb.AppendLine("  m_Name: LevelData_$("{0:D2}" -f $num)")
    [void]$sb.AppendLine("  m_EditorClassIdentifier: Assembly-CSharp::Game.Sorcerum.LevelDataSo")
    [void]$sb.AppendLine("  _levelDataVo:")
    [void]$sb.AppendLine("    _stackDataList:")
    foreach ($st in $stackList) {
        [void]$sb.AppendLine("    - _gridPos: {x: $($st.X), y: $($st.Y)}")
        [void]$sb.AppendLine("      _items:")
        foreach ($itemColor in $st.Items) {
            $guid = $discs[$itemColor]
            [void]$sb.AppendLine("      - {fileID: 11400000, guid: $guid, type: 2}")
        }
        [void]$sb.AppendLine("      _itemDistance: 0.5")
    }
    [void]$sb.AppendLine("    _ballData:")
    [void]$sb.AppendLine("      _availableBalls:")
    foreach ($ballName in $ballList) {
        $guid = $balls[$ballName]
        [void]$sb.AppendLine("      - {fileID: 11400000, guid: $guid, type: 2}")
    }
    [void]$sb.AppendLine("    _mapGrid:")
    [void]$sb.AppendLine("      _dimensions: {x: $dimX, y: $dimY}")
    [void]$sb.AppendLine("      _cellSize: 1")
    [void]$sb.AppendLine("      _padding: 0.5")
    [void]$sb.AppendLine("    _stackPoolKey:")
    [void]$sb.AppendLine("      _poolKey: simpleStack")
    [void]$sb.AppendLine("    _emptyGridCellKey:")
    [void]$sb.AppendLine("      _poolKey: emptyGridCell")
    [void]$sb.AppendLine("    _stackGridCellKey:")
    [void]$sb.AppendLine("      _poolKey: stackGridCell")

    return $sb.ToString()
}

$levels = @(
    # Level 1 (3x3, 2 stacks, 2 balls)
    @{
        Num = 1; DimX = 3; DimY = 3
        Stacks = @(
            @{ X = 1; Y = 1; Items = @("Cyan") },
            @{ X = 2; Y = 1; Items = @("Pink") }
        )
        Balls = @("Cyan_2", "Pink_2")
    },
    # Level 2 (3x3, 3 stacks, 2 balls)
    @{
        Num = 2; DimX = 3; DimY = 3
        Stacks = @(
            @{ X = 1; Y = 0; Items = @("Cyan") },
            @{ X = 1; Y = 1; Items = @("Pink") },
            @{ X = 1; Y = 2; Items = @("Cyan") }
        )
        Balls = @("Cyan_3", "Pink_2")
    },
    # Level 3 (4x4, 4 stacks, 3 balls)
    @{
        Num = 3; DimX = 4; DimY = 4
        Stacks = @(
            @{ X = 1; Y = 1; Items = @("Pink", "Cyan") },
            @{ X = 1; Y = 2; Items = @("Cyan") },
            @{ X = 2; Y = 1; Items = @("Pink") },
            @{ X = 2; Y = 2; Items = @("Cyan", "Pink") }
        )
        Balls = @("Cyan_2", "Pink_4", "Cyan_4")
    },
    # Level 4 (4x4, 5 stacks, 3 balls)
    @{
        Num = 4; DimX = 4; DimY = 4
        Stacks = @(
            @{ X = 1; Y = 1; Items = @("Cyan") },
            @{ X = 1; Y = 2; Items = @("Pink", "Cyan") },
            @{ X = 2; Y = 1; Items = @("Pink") },
            @{ X = 2; Y = 2; Items = @("Cyan", "Pink") },
            @{ X = 2; Y = 3; Items = @("Cyan") }
        )
        Balls = @("Cyan_2", "Pink_4", "Cyan_5")
    },
    # Level 5 (4x4, 6 stacks, 3 balls)
    @{
        Num = 5; DimX = 4; DimY = 4
        Stacks = @(
            @{ X = 1; Y = 0; Items = @("Pink") },
            @{ X = 1; Y = 1; Items = @("Cyan", "Pink") },
            @{ X = 1; Y = 2; Items = @("Pink") },
            @{ X = 2; Y = 0; Items = @("Cyan") },
            @{ X = 2; Y = 1; Items = @("Pink", "Cyan") },
            @{ X = 2; Y = 2; Items = @("Cyan") }
        )
        Balls = @("Pink_3", "Cyan_6", "Pink_5")
    },
    # Level 6 (4x4, 6 stacks, 4 balls)
    @{
        Num = 6; DimX = 4; DimY = 4
        Stacks = @(
            @{ X = 1; Y = 1; Items = @("Pink", "Cyan") },
            @{ X = 1; Y = 2; Items = @("Cyan", "Pink") },
            @{ X = 1; Y = 3; Items = @("Pink") },
            @{ X = 2; Y = 1; Items = @("Cyan", "Pink") },
            @{ X = 2; Y = 2; Items = @("Pink", "Cyan") },
            @{ X = 2; Y = 3; Items = @("Cyan") }
        )
        Balls = @("Pink_3", "Cyan_4", "Pink_5", "Cyan_6")
    },
    # Level 7 (4x4, 7 stacks, 4 balls)
    @{
        Num = 7; DimX = 4; DimY = 4
        Stacks = @(
            @{ X = 0; Y = 1; Items = @("Cyan") },
            @{ X = 1; Y = 1; Items = @("Pink", "Cyan") },
            @{ X = 1; Y = 2; Items = @("Cyan") },
            @{ X = 2; Y = 1; Items = @("Cyan", "Pink") },
            @{ X = 2; Y = 2; Items = @("Pink") },
            @{ X = 3; Y = 1; Items = @("Pink") },
            @{ X = 3; Y = 2; Items = @("Cyan") }
        )
        Balls = @("Cyan_3", "Pink_5", "Cyan_5", "Pink_6")
    },
    # Level 8 (4x4, 7 stacks, 4 balls)
    @{
        Num = 8; DimX = 4; DimY = 4
        Stacks = @(
            @{ X = 1; Y = 0; Items = @("Yellow") },
            @{ X = 1; Y = 1; Items = @("Pink", "Yellow") },
            @{ X = 1; Y = 2; Items = @("Cyan") },
            @{ X = 2; Y = 0; Items = @("Pink") },
            @{ X = 2; Y = 1; Items = @("Cyan", "Pink") },
            @{ X = 2; Y = 2; Items = @("Yellow") },
            @{ X = 2; Y = 3; Items = @("Cyan") }
        )
        Balls = @("Yellow_2", "Pink_5", "Cyan_5", "Yellow_6")
    },
    # Level 9 (4x4, 8 stacks, 5 balls)
    @{
        Num = 9; DimX = 4; DimY = 4
        Stacks = @(
            @{ X = 0; Y = 1; Items = @("Yellow") },
            @{ X = 1; Y = 1; Items = @("Cyan", "Yellow") },
            @{ X = 1; Y = 2; Items = @("Pink") },
            @{ X = 2; Y = 1; Items = @("Pink", "Cyan") },
            @{ X = 2; Y = 2; Items = @("Cyan") },
            @{ X = 2; Y = 3; Items = @("Yellow") },
            @{ X = 3; Y = 1; Items = @("Yellow", "Pink") },
            @{ X = 3; Y = 2; Items = @("Pink") }
        )
        Balls = @("Yellow_2", "Cyan_5", "Pink_5", "Yellow_6", "Pink_8")
    },
    # Level 10 (4x4, 8 stacks, 5 balls)
    @{
        Num = 10; DimX = 4; DimY = 4
        Stacks = @(
            @{ X = 1; Y = 0; Items = @("Pink") },
            @{ X = 1; Y = 1; Items = @("Cyan", "Pink") },
            @{ X = 1; Y = 2; Items = @("Yellow") },
            @{ X = 1; Y = 3; Items = @("Cyan") },
            @{ X = 2; Y = 0; Items = @("Yellow") },
            @{ X = 2; Y = 1; Items = @("Pink", "Yellow") },
            @{ X = 2; Y = 2; Items = @("Cyan") },
            @{ X = 2; Y = 3; Items = @("Pink") }
        )
        Balls = @("Pink_2", "Yellow_3", "Cyan_4", "Pink_6", "Yellow_8")
    },
    # Level 11 (5x5, 8 stacks, 5 balls)
    @{
        Num = 11; DimX = 5; DimY = 5
        Stacks = @(
            @{ X = 1; Y = 1; Items = @("Cyan", "Yellow") },
            @{ X = 1; Y = 2; Items = @("Yellow", "Pink") },
            @{ X = 2; Y = 1; Items = @("Pink", "Cyan") },
            @{ X = 2; Y = 2; Items = @("Yellow", "Cyan") },
            @{ X = 2; Y = 3; Items = @("Pink") },
            @{ X = 3; Y = 1; Items = @("Cyan") },
            @{ X = 3; Y = 2; Items = @("Yellow") },
            @{ X = 3; Y = 3; Items = @("Pink", "Yellow") }
        )
        Balls = @("Yellow_2", "Pink_3", "Cyan_5", "Yellow_6", "Pink_8")
    },
    # Level 12 (5x5, 9 stacks, 5 balls)
    @{
        Num = 12; DimX = 5; DimY = 5
        Stacks = @(
            @{ X = 1; Y = 1; Items = @("Cyan") },
            @{ X = 1; Y = 2; Items = @("Pink", "Cyan") },
            @{ X = 1; Y = 3; Items = @("Yellow") },
            @{ X = 2; Y = 1; Items = @("Yellow", "Pink") },
            @{ X = 2; Y = 2; Items = @("Cyan", "Yellow") },
            @{ X = 2; Y = 3; Items = @("Pink") },
            @{ X = 3; Y = 1; Items = @("Cyan", "Pink") },
            @{ X = 3; Y = 2; Items = @("Yellow") },
            @{ X = 3; Y = 3; Items = @("Cyan") }
        )
        Balls = @("Cyan_3", "Pink_4", "Yellow_5", "Pink_6", "Cyan_8")
    },
    # Level 13 (5x5, 9 stacks, 5 balls)
    @{
        Num = 13; DimX = 5; DimY = 5
        Stacks = @(
            @{ X = 1; Y = 1; Items = @("Purple") },
            @{ X = 1; Y = 2; Items = @("Cyan", "Purple") },
            @{ X = 1; Y = 3; Items = @("Yellow") },
            @{ X = 2; Y = 1; Items = @("Pink", "Yellow") },
            @{ X = 2; Y = 2; Items = @("Purple", "Pink") },
            @{ X = 2; Y = 3; Items = @("Cyan") },
            @{ X = 3; Y = 1; Items = @("Yellow", "Cyan") },
            @{ X = 3; Y = 2; Items = @("Purple") },
            @{ X = 3; Y = 3; Items = @("Pink") }
        )
        Balls = @("Purple_2", "Yellow_4", "Pink_5", "Cyan_6", "Purple_8")
    },
    # Level 14 (5x5, 10 stacks, 6 balls)
    @{
        Num = 14; DimX = 5; DimY = 5
        Stacks = @(
            @{ X = 1; Y = 1; Items = @("Cyan") },
            @{ X = 1; Y = 2; Items = @("Purple", "Cyan") },
            @{ X = 1; Y = 3; Items = @("Yellow") },
            @{ X = 2; Y = 0; Items = @("Pink") },
            @{ X = 2; Y = 1; Items = @("Yellow", "Pink") },
            @{ X = 2; Y = 2; Items = @("Cyan", "Purple") },
            @{ X = 2; Y = 3; Items = @("Pink") },
            @{ X = 3; Y = 1; Items = @("Purple", "Yellow") },
            @{ X = 3; Y = 2; Items = @("Cyan") },
            @{ X = 3; Y = 3; Items = @("Purple") }
        )
        Balls = @("Cyan_3", "Pink_4", "Yellow_5", "Purple_6", "Cyan_6", "Pink_8")
    },
    # Level 15 (5x5, 10 stacks, 6 balls)
    @{
        Num = 15; DimX = 5; DimY = 5
        Stacks = @(
            @{ X = 1; Y = 0; Items = @("Yellow") },
            @{ X = 1; Y = 1; Items = @("Pink", "Yellow") },
            @{ X = 1; Y = 2; Items = @("Cyan") },
            @{ X = 1; Y = 3; Items = @("Purple", "Cyan") },
            @{ X = 2; Y = 1; Items = @("Yellow") },
            @{ X = 2; Y = 2; Items = @("Purple", "Yellow") },
            @{ X = 2; Y = 3; Items = @("Pink") },
            @{ X = 3; Y = 1; Items = @("Cyan", "Pink") },
            @{ X = 3; Y = 2; Items = @("Purple") },
            @{ X = 3; Y = 3; Items = @("Yellow") }
        )
        Balls = @("Yellow_2", "Cyan_4", "Pink_5", "Purple_6", "Yellow_6", "Purple_8")
    },
    # Level 16 (5x5, 11 stacks, 6 balls)
    @{
        Num = 16; DimX = 5; DimY = 5
        Stacks = @(
            @{ X = 1; Y = 1; Items = @("Cyan") },
            @{ X = 1; Y = 2; Items = @("Purple", "Cyan") },
            @{ X = 1; Y = 3; Items = @("Yellow") },
            @{ X = 2; Y = 0; Items = @("Pink") },
            @{ X = 2; Y = 1; Items = @("Yellow", "Pink") },
            @{ X = 2; Y = 2; Items = @("Purple", "Yellow") },
            @{ X = 2; Y = 3; Items = @("Cyan") },
            @{ X = 2; Y = 4; Items = @("Pink") },
            @{ X = 3; Y = 1; Items = @("Purple") },
            @{ X = 3; Y = 2; Items = @("Cyan", "Purple") },
            @{ X = 3; Y = 3; Items = @("Yellow") }
        )
        Balls = @("Cyan_3", "Pink_4", "Yellow_5", "Purple_6", "Cyan_8", "Yellow_8")
    },
    # Level 17 (5x5, 11 stacks, 6 balls)
    @{
        Num = 17; DimX = 5; DimY = 5
        Stacks = @(
            @{ X = 1; Y = 1; Items = @("Purple") },
            @{ X = 1; Y = 2; Items = @("Yellow", "Purple") },
            @{ X = 1; Y = 3; Items = @("Pink") },
            @{ X = 2; Y = 1; Items = @("Cyan", "Pink") },
            @{ X = 2; Y = 2; Items = @("Purple", "Cyan") },
            @{ X = 2; Y = 3; Items = @("Yellow") },
            @{ X = 3; Y = 0; Items = @("Cyan") },
            @{ X = 3; Y = 1; Items = @("Pink", "Yellow") },
            @{ X = 3; Y = 2; Items = @("Cyan") },
            @{ X = 3; Y = 3; Items = @("Purple") },
            @{ X = 3; Y = 4; Items = @("Pink") }
        )
        Balls = @("Purple_2", "Yellow_4", "Pink_5", "Cyan_6", "Purple_6", "Cyan_8")
    },
    # Level 18 (5x5, 12 stacks, 7 balls)
    @{
        Num = 18; DimX = 5; DimY = 5
        Stacks = @(
            @{ X = 0; Y = 2; Items = @("Cyan") },
            @{ X = 1; Y = 1; Items = @("Yellow") },
            @{ X = 1; Y = 2; Items = @("Pink", "Yellow") },
            @{ X = 1; Y = 3; Items = @("Purple") },
            @{ X = 2; Y = 0; Items = @("Pink") },
            @{ X = 2; Y = 1; Items = @("Cyan", "Pink") },
            @{ X = 2; Y = 2; Items = @("Purple", "Cyan") },
            @{ X = 2; Y = 3; Items = @("Yellow") },
            @{ X = 2; Y = 4; Items = @("Purple") },
            @{ X = 3; Y = 1; Items = @("Cyan") },
            @{ X = 3; Y = 2; Items = @("Yellow", "Purple") },
            @{ X = 3; Y = 3; Items = @("Pink") }
        )
        Balls = @("Cyan_3", "Yellow_4", "Pink_5", "Purple_6", "Cyan_6", "Yellow_8", "Pink_8")
    },
    # Level 19 (5x5, 12 stacks, 7 balls)
    @{
        Num = 19; DimX = 5; DimY = 5
        Stacks = @(
            @{ X = 1; Y = 0; Items = @("Purple") },
            @{ X = 1; Y = 1; Items = @("Cyan", "Purple") },
            @{ X = 1; Y = 2; Items = @("Yellow") },
            @{ X = 1; Y = 3; Items = @("Pink") },
            @{ X = 2; Y = 1; Items = @("Purple", "Pink") },
            @{ X = 2; Y = 2; Items = @("Yellow", "Purple") },
            @{ X = 2; Y = 3; Items = @("Cyan") },
            @{ X = 2; Y = 4; Items = @("Pink") },
            @{ X = 3; Y = 1; Items = @("Yellow") },
            @{ X = 3; Y = 2; Items = @("Cyan", "Yellow") },
            @{ X = 3; Y = 3; Items = @("Purple") },
            @{ X = 3; Y = 4; Items = @("Cyan") }
        )
        Balls = @("Purple_2", "Cyan_4", "Yellow_5", "Pink_6", "Purple_6", "Cyan_8", "Yellow_8")
    },
    # Level 20 (5x5, 13 stacks, 7 balls)
    @{
        Num = 20; DimX = 5; DimY = 5
        Stacks = @(
            @{ X = 0; Y = 2; Items = @("Yellow") },
            @{ X = 1; Y = 1; Items = @("Cyan") },
            @{ X = 1; Y = 2; Items = @("Pink", "Cyan") },
            @{ X = 1; Y = 3; Items = @("Purple") },
            @{ X = 2; Y = 0; Items = @("Pink") },
            @{ X = 2; Y = 1; Items = @("Yellow", "Pink") },
            @{ X = 2; Y = 2; Items = @("Purple", "Yellow", "Cyan") },
            @{ X = 2; Y = 3; Items = @("Cyan", "Purple") },
            @{ X = 2; Y = 4; Items = @("Yellow") },
            @{ X = 3; Y = 1; Items = @("Purple") },
            @{ X = 3; Y = 2; Items = @("Cyan", "Purple") },
            @{ X = 3; Y = 3; Items = @("Pink") },
            @{ X = 4; Y = 2; Items = @("Purple") }
        )
        Balls = @("Yellow_3", "Cyan_5", "Pink_6", "Purple_6", "Yellow_6", "Cyan_8", "Purple_8")
    }
)

foreach ($lvl in $levels) {
    $filePath = "Assets/Data/Levels/LevelData_$("{0:D2}" -f $lvl.Num).asset"
    $yaml = Build-LevelYaml $lvl.Num $lvl.DimX $lvl.DimY $lvl.Stacks $lvl.Balls
    [System.IO.File]::WriteAllText($filePath, $yaml, [System.Text.Encoding]::UTF8)
    Write-Host "Updated $filePath with $($lvl.Stacks.Count) stacks and $($lvl.Balls.Count) balls."
}
