param(
    [string]$ProjectDir = "C:\UserData\Projects\hybridCasual\Hybrid"
)

$DataDir = Join-Path $ProjectDir "Assets\Data"
$LevelsDir = Join-Path $DataDir "Levels"

if (-not (Test-Path $LevelsDir)) {
    New-Item -ItemType Directory -Path $LevelsDir -Force | Out-Null
    $levelsMeta = @"
fileFormatVersion: 2
guid: 10000000000000000000000000000001
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"@
    Set-Content -Path "$LevelsDir.meta" -Value $levelsMeta
}

function New-AssetMeta($guid) {
@"
fileFormatVersion: 2
guid: $guid
NativeFormatImporter:
  externalObjects: {}
  mainObjectFileID: 11400000
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"@
}

function Create-BrainAsset($name, $guid, $colorR, $colorG, $colorB, $hitCount, $isBall) {
    $metaPath = Join-Path $DataDir "$name.asset.meta"
    $assetPath = Join-Path $DataDir "$name.asset"

    Set-Content -Path $metaPath -Value (New-AssetMeta $guid)

    $rid1 = [long]6423604256275955000 + (Get-Random -Minimum 1000 -Maximum 9999)
    $rid2 = $rid1 + 1
    $rid3 = $rid1 + 2
    $rid4 = $rid1 + 3
    $rid5 = $rid1 + 4

    if ($isBall) {
        $content = @"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!114 &11400000
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 0}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: ebf1d917dbcd4d439f6b90d7a77b1d4d, type: 3}
  m_Name: $name
  m_EditorClassIdentifier: Assembly-CSharp::Game.Sorcerum.BrainSo
  _attributeBaseList:
  - rid: $rid1
  - rid: $rid2
  - rid: $rid3
  - rid: $rid4
  - rid: $rid5
  references:
    version: 2
    RefIds:
    - rid: $rid1
      type: {class: JumpAttribute, ns: Game.Sorcerum, asm: Assembly-CSharp}
      data: 
    - rid: $rid2
      type: {class: ToughnessAttribute, ns: , asm: Assembly-CSharp}
      data:
        _hitCount: $hitCount
    - rid: $rid3
      type: {class: ColorIdAttribute, ns: Game.Sorcerum, asm: Assembly-CSharp}
      data:
        AlwaysAccept: -999
        _color: {r: $colorR, g: $colorG, b: $colorB, a: 1}
    - rid: $rid4
      type: {class: MovementAttribute, ns: , asm: Assembly-CSharp}
      data:
        _moveDuration: 0.35
        _ease: 6
    - rid: $rid5
      type: {class: PoolObjectAttribute, ns: Game.Sorcerum, asm: Assembly-CSharp}
      data:
        _poolKey:
          _poolKey: simpleBall
"@
    } else {
        $content = @"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!114 &11400000
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 0}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: ebf1d917dbcd4d439f6b90d7a77b1d4d, type: 3}
  m_Name: $name
  m_EditorClassIdentifier: Assembly-CSharp::Game.Sorcerum.BrainSo
  _attributeBaseList:
  - rid: $rid1
  - rid: $rid2
  references:
    version: 2
    RefIds:
    - rid: $rid1
      type: {class: CrackAttribute, ns: Game.Sorcerum, asm: Assembly-CSharp}
      data: 
    - rid: $rid2
      type: {class: ColorIdAttribute, ns: Game.Sorcerum, asm: Assembly-CSharp}
      data:
        AlwaysAccept: -999
        _color: {r: $colorR, g: $colorG, b: $colorB, a: 1}
"@
    }
    Set-Content -Path $assetPath -Value $content
}

function Create-DiscAsset($name, $guid, $brainGuid) {
    $metaPath = Join-Path $DataDir "$name.asset.meta"
    $assetPath = Join-Path $DataDir "$name.asset"

    Set-Content -Path $metaPath -Value (New-AssetMeta $guid)

    $content = @"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!114 &11400000
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 0}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: e9fc09c2411542b29276aa9e90536226, type: 3}
  m_Name: $name
  m_EditorClassIdentifier: Assembly-CSharp::Game.Sorcerum.StackableItemDataSo
  _brain: {fileID: 11400000, guid: $brainGuid, type: 2}
  _poolKey:
    _poolKey: simpleStackableItem
"@
    Set-Content -Path $assetPath -Value $content
}

function Create-BallAsset($name, $guid, $brainGuid) {
    $metaPath = Join-Path $DataDir "$name.asset.meta"
    $assetPath = Join-Path $DataDir "$name.asset"

    Set-Content -Path $metaPath -Value (New-AssetMeta $guid)

    $content = @"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!114 &11400000
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 0}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: 82bc7cff34fc4ca6b832bbd366b63a01, type: 3}
  m_Name: $name
  m_EditorClassIdentifier: Assembly-CSharp::Game.Sorcerum.BallDataSo
  _brain: {fileID: 11400000, guid: $brainGuid, type: 2}
  _poolKey:
    _poolKey: simpleBall
"@
    Set-Content -Path $assetPath -Value $content
}

# --- 1. DISCS ---
$discCyan   = "4d4afa9e79a429a4c9ffd57b2100833a"
$discPink   = "7b585aff59fc4635a06991169d5cb27f"
$brainYellowGuid = "a1b2c3d4e5f601010101010101010101"
$discYellow      = "a1b2c3d4e5f601010101010101010102"
Create-BrainAsset "StackableItemBrain_Yellow" $brainYellowGuid 1.0 0.85 0.1 0 $false
Create-DiscAsset "SimpleStackableItem_Yellow" $discYellow $brainYellowGuid

$brainPurpleGuid = "a1b2c3d4e5f602020202020202020201"
$discPurple      = "a1b2c3d4e5f602020202020202020202"
Create-BrainAsset "StackableItemBrain_Purple" $brainPurpleGuid 0.65 0.25 0.95 0 $false
Create-DiscAsset "SimpleStackableItem_Purple" $discPurple $brainPurpleGuid

# --- 2. BALLS ---
$ballRegistry = @{}
$ballRegistry["Cyan_5"] = "179cec27b7322344c98cce586f032401"
$ballRegistry["Cyan_8"] = "c9e303e0ae404fc2818c10bc3fba8bf4"
$ballRegistry["Pink_3"] = "f159a70b6f7e4055b0a68a8064046bb1"

function Register-Ball($colorName, $r, $g, $b, $hitCount, $hexId) {
    # 32 hexadecimal characters exactly:
    $brainG = "b00000000000000000000" + $hexId + "00000001"
    $ballG  = "b00000000000000000000" + $hexId + "00000002"
    $bName  = "BallBrain_${colorName}_${hitCount}"
    $ballName = "SimpleBall_${colorName}_${hitCount}"
    Create-BrainAsset $bName $brainG $r $g $b $hitCount $true
    Create-BallAsset $ballName $ballG $brainG
    $script:ballRegistry["${colorName}_${hitCount}"] = $ballG
}

Register-Ball "Cyan" 0 0.8509804 1 1 "c01"
Register-Ball "Cyan" 0 0.8509804 1 2 "c02"
Register-Ball "Cyan" 0 0.8509804 1 3 "c03"
Register-Ball "Cyan" 0 0.8509804 1 4 "c04"
Register-Ball "Cyan" 0 0.8509804 1 6 "c06"

Register-Ball "Pink" 1 0.2 0.6 1 "d01"
Register-Ball "Pink" 1 0.2 0.6 2 "d02"
Register-Ball "Pink" 1 0.2 0.6 4 "d04"
Register-Ball "Pink" 1 0.2 0.6 5 "d05"
Register-Ball "Pink" 1 0.2 0.6 6 "d06"

Register-Ball "Yellow" 1 0.85 0.1 2 "e02"
Register-Ball "Yellow" 1 0.85 0.1 3 "e03"
Register-Ball "Yellow" 1 0.85 0.1 4 "e04"
Register-Ball "Yellow" 1 0.85 0.1 5 "e05"
Register-Ball "Yellow" 1 0.85 0.1 6 "e06"

Register-Ball "Purple" 0.65 0.25 0.95 2 "a02"
Register-Ball "Purple" 0.65 0.25 0.95 3 "a03"
Register-Ball "Purple" 0.65 0.25 0.95 4 "a04"
Register-Ball "Purple" 0.65 0.25 0.95 5 "a05"
Register-Ball "Purple" 0.65 0.25 0.95 6 "a06"

# Helper for Level YAML
function Create-LevelAssetFile($levelIndex, $dimX, $dimY, $stackConfigs, $ballKeys) {
    $pad = "{0:D2}" -f $levelIndex
    $levelName = "LevelData_$pad"
    $levelGuid = "f00000000000000000000000000000$pad"
    $metaPath = Join-Path $LevelsDir "$levelName.asset.meta"
    $assetPath = Join-Path $LevelsDir "$levelName.asset"

    Set-Content -Path $metaPath -Value (New-AssetMeta $levelGuid)

    $stacksYaml = ""
    foreach ($s in $stackConfigs) {
        $x = $s.x
        $y = $s.y
        $itemsYaml = ""
        foreach ($itemGuid in $s.items) {
            $itemsYaml += "      - {fileID: 11400000, guid: $itemGuid, type: 2}`n"
        }
        $stacksYaml += @"
    - _gridPos: {x: $x, y: $y}
      _items:
$itemsYaml      _itemDistance: 0.5

"@
    }

    $ballsYaml = ""
    foreach ($bKey in $ballKeys) {
        $bGuid = $script:ballRegistry[$bKey]
        $ballsYaml += "      - {fileID: 11400000, guid: $bGuid, type: 2}`n"
    }

    $content = @"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!114 &11400000
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 0}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: d6002367a8804e3095cbe5ab65b94d71, type: 3}
  m_Name: $levelName
  m_EditorClassIdentifier: Assembly-CSharp::Game.Sorcerum.LevelDataSo
  _levelDataVo:
    _stackDataList:
$stacksYaml
    _ballData:
      _availableBalls:
$ballsYaml
    _mapGrid:
      _dimensions: {x: $dimX, y: $dimY}
      _cellSize: 1
      _padding: 0.5
    _stackPoolKey:
      _poolKey: simpleStack
    _emptyGridCellKey:
      _poolKey: emptyGridCell
    _stackGridCellKey:
      _poolKey: stackGridCell
"@
    Set-Content -Path $assetPath -Value $content
}

# --- 20 LEVELS DEFINITIONS ---

# Level 1: Intro (2 stacks, 1 disc each)
Create-LevelAssetFile 1 3 3 @(
    @{ x=1; y=1; items=@($discCyan) },
    @{ x=2; y=1; items=@($discPink) }
) @("Cyan_1", "Pink_1")

# Level 2: 2 stacks, 2 discs each
Create-LevelAssetFile 2 3 3 @(
    @{ x=1; y=1; items=@($discCyan, $discCyan) },
    @{ x=2; y=1; items=@($discPink, $discPink) }
) @("Cyan_2", "Pink_2")

# Level 3: 2 stacks, layered tops (Pink on Cyan, Cyan on Pink)
Create-LevelAssetFile 3 4 4 @(
    @{ x=1; y=1; items=@($discPink, $discCyan) },
    @{ x=2; y=2; items=@($discCyan, $discPink) }
) @("Cyan_2", "Pink_2")

# Level 4: 3 stacks (Pink, Cyan mix)
Create-LevelAssetFile 4 4 4 @(
    @{ x=1; y=1; items=@($discCyan, $discPink, $discCyan) },
    @{ x=2; y=2; items=@($discPink, $discCyan, $discPink) },
    @{ x=1; y=3; items=@($discCyan, $discPink) }
) @("Cyan_3", "Pink_3", "Pink_2")

# Level 5: 3 stacks, 3 discs each
Create-LevelAssetFile 5 4 4 @(
    @{ x=1; y=1; items=@($discPink, $discPink, $discCyan) },
    @{ x=2; y=1; items=@($discCyan, $discCyan, $discPink) },
    @{ x=3; y=1; items=@($discCyan, $discPink, $discCyan) }
) @("Cyan_4", "Pink_4", "Cyan_2")

# Level 6: Strategic sequence
Create-LevelAssetFile 6 4 4 @(
    @{ x=1; y=1; items=@($discCyan, $discCyan, $discPink) },
    @{ x=2; y=2; items=@($discPink, $discPink, $discCyan) },
    @{ x=3; y=2; items=@($discCyan, $discPink, $discCyan) }
) @("Pink_3", "Cyan_4", "Pink_3")

# Level 7: 4 stacks (2x2 grid positions)
Create-LevelAssetFile 7 4 4 @(
    @{ x=1; y=1; items=@($discPink, $discCyan) },
    @{ x=2; y=1; items=@($discCyan, $discPink) },
    @{ x=1; y=2; items=@($discPink, $discCyan) },
    @{ x=2; y=2; items=@($discCyan, $discPink) }
) @("Cyan_4", "Pink_4")

# Level 8: Yellow introduced! (3 colors)
Create-LevelAssetFile 8 4 4 @(
    @{ x=1; y=1; items=@($discCyan, $discYellow) },
    @{ x=2; y=2; items=@($discPink, $discYellow) },
    @{ x=3; y=1; items=@($discYellow, $discCyan) }
) @("Yellow_3", "Cyan_2", "Pink_2")

# Level 9: 3 colors, 3 discs per stack
Create-LevelAssetFile 9 4 4 @(
    @{ x=1; y=1; items=@($discPink, $discCyan, $discYellow) },
    @{ x=2; y=2; items=@($discYellow, $discPink, $discCyan) },
    @{ x=3; y=1; items=@($discCyan, $discYellow, $discPink) }
) @("Yellow_3", "Cyan_3", "Pink_3")

# Level 10: 4 stacks, 3 colors
Create-LevelAssetFile 10 4 4 @(
    @{ x=1; y=1; items=@($discYellow, $discCyan, $discPink) },
    @{ x=2; y=1; items=@($discCyan, $discYellow, $discPink) },
    @{ x=1; y=2; items=@($discPink, $discCyan, $discYellow) },
    @{ x=2; y=2; items=@($discYellow, $discPink, $discCyan) }
) @("Pink_4", "Cyan_4", "Yellow_4")

# Level 11: Towers of 4 discs
Create-LevelAssetFile 11 5 5 @(
    @{ x=1; y=1; items=@($discCyan, $discYellow, $discPink, $discYellow) },
    @{ x=2; y=2; items=@($discYellow, $discPink, $discCyan, $discPink) },
    @{ x=3; y=3; items=@($discPink, $discCyan, $discYellow, $discCyan) }
) @("Yellow_4", "Pink_4", "Cyan_4")

# Level 12: 4 stacks, 4 discs each (3 colors)
Create-LevelAssetFile 12 5 5 @(
    @{ x=1; y=2; items=@($discCyan, $discPink, $discYellow, $discCyan) },
    @{ x=2; y=1; items=@($discPink, $discYellow, $discCyan, $discPink) },
    @{ x=3; y=2; items=@($discYellow, $discCyan, $discPink, $discYellow) },
    @{ x=2; y=3; items=@($discPink, $discCyan, $discYellow, $discPink) }
) @("Cyan_4", "Pink_5", "Yellow_4", "Pink_3")

# Level 13: Purple introduced! (4 colors)
Create-LevelAssetFile 13 5 5 @(
    @{ x=1; y=1; items=@($discPurple, $discCyan, $discYellow) },
    @{ x=2; y=2; items=@($discYellow, $discPurple, $discPink) },
    @{ x=3; y=1; items=@($discPink, $discYellow, $discPurple) },
    @{ x=1; y=3; items=@($discCyan, $discPink, $discPurple) }
) @("Purple_4", "Yellow_3", "Pink_3", "Cyan_3")

# Level 14: 4 colors, 4 stacks of 4 discs
Create-LevelAssetFile 14 5 5 @(
    @{ x=1; y=1; items=@($discCyan, $discPurple, $discPink, $discYellow) },
    @{ x=3; y=1; items=@($discPink, $discYellow, $discCyan, $discPurple) },
    @{ x=1; y=3; items=@($discPurple, $discCyan, $discYellow, $discPink) },
    @{ x=3; y=3; items=@($discYellow, $discPink, $discPurple, $discCyan) }
) @("Yellow_4", "Purple_4", "Pink_4", "Cyan_4")

# Level 15: 5 stacks arena (4 colors)
Create-LevelAssetFile 15 5 5 @(
    @{ x=1; y=1; items=@($discPurple, $discPink, $discCyan) },
    @{ x=3; y=1; items=@($discYellow, $discCyan, $discPurple) },
    @{ x=2; y=2; items=@($discPink, $discPurple, $discYellow) },
    @{ x=1; y=3; items=@($discCyan, $discYellow, $discPink) },
    @{ x=3; y=3; items=@($discYellow, $discPink, $discPurple) }
) @("Cyan_4", "Purple_4", "Yellow_4", "Pink_4", "Yellow_2")

# Level 16: 5 stacks, 4 discs each
Create-LevelAssetFile 16 5 5 @(
    @{ x=1; y=1; items=@($discPink, $discPurple, $discCyan, $discYellow) },
    @{ x=2; y=2; items=@($discYellow, $discCyan, $discPurple, $discPink) },
    @{ x=3; y=3; items=@($discCyan, $discPink, $discYellow, $discPurple) },
    @{ x=1; y=3; items=@($discPurple, $discYellow, $discPink, $discCyan) },
    @{ x=3; y=1; items=@($discPink, $discCyan, $discPurple, $discYellow) }
) @("Yellow_5", "Cyan_5", "Purple_5", "Pink_5")

# Level 17: 5 stacks tower puzzle
Create-LevelAssetFile 17 5 5 @(
    @{ x=1; y=1; items=@($discCyan, $discYellow, $discPurple, $discPink) },
    @{ x=2; y=1; items=@($discPurple, $discPink, $discYellow, $discCyan) },
    @{ x=3; y=2; items=@($discYellow, $discCyan, $discPink, $discPurple) },
    @{ x=1; y=3; items=@($discPink, $discPurple, $discCyan, $discYellow) },
    @{ x=2; y=3; items=@($discCyan, $discPink, $discPurple, $discYellow) }
) @("Pink_5", "Yellow_5", "Purple_5", "Cyan_5", "Pink_2")

# Level 18: 6 stacks puzzle
Create-LevelAssetFile 18 5 5 @(
    @{ x=1; y=1; items=@($discPurple, $discYellow, $discCyan) },
    @{ x=2; y=1; items=@($discCyan, $discPink, $discPurple) },
    @{ x=3; y=1; items=@($discYellow, $discPurple, $discPink) },
    @{ x=1; y=3; items=@($discPink, $discCyan, $discYellow) },
    @{ x=2; y=3; items=@($discPurple, $discYellow, $discPink) },
    @{ x=3; y=3; items=@($discCyan, $discPurple, $discCyan) }
) @("Cyan_5", "Purple_5", "Yellow_5", "Pink_5", "Purple_3")

# Level 19: 6 stacks, 4 discs each
Create-LevelAssetFile 19 5 5 @(
    @{ x=1; y=1; items=@($discCyan, $discPurple, $discYellow, $discPink) },
    @{ x=2; y=2; items=@($discYellow, $discPink, $discCyan, $discPurple) },
    @{ x=3; y=1; items=@($discPurple, $discCyan, $discPink, $discYellow) },
    @{ x=1; y=3; items=@($discPink, $discYellow, $discPurple, $discCyan) },
    @{ x=3; y=3; items=@($discYellow, $discPurple, $discCyan, $discPink) },
    @{ x=2; y=4; items=@($discCyan, $discPink, $discYellow, $discPurple) }
) @("Pink_6", "Yellow_6", "Purple_6", "Cyan_6", "Pink_3")

# Level 20: Grand Finale (6 stacks of 5 discs!)
Create-LevelAssetFile 20 5 5 @(
    @{ x=1; y=1; items=@($discPurple, $discCyan, $discYellow, $discPink, $discPurple) },
    @{ x=2; y=1; items=@($discYellow, $discPink, $discPurple, $discCyan, $discYellow) },
    @{ x=3; y=2; items=@($discPink, $discPurple, $discCyan, $discYellow, $discPink) },
    @{ x=1; y=3; items=@($discCyan, $discYellow, $discPink, $discPurple, $discCyan) },
    @{ x=2; y=3; items=@($discPurple, $discPink, $discYellow, $discCyan, $discPurple) },
    @{ x=3; y=4; items=@($discYellow, $discCyan, $discPurple, $discPink, $discYellow) }
) @("Purple_6", "Yellow_6", "Pink_6", "Cyan_6", "Purple_4", "Yellow_4")

Write-Host "All 20 progressive level assets generated successfully!"
