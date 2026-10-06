param(
    [string]$ProjectDir = "C:\UserData\Projects\hybridCasual\Hybrid"
)

$DataDir = Join-Path $ProjectDir "Assets\Data"
$LevelsDir = Join-Path $DataDir "Levels"

if (-not (Test-Path $LevelsDir)) {
    New-Item -ItemType Directory -Path $LevelsDir -Force | Out-Null
    $levelsMeta = @"
fileFormatVersion: 2
guid: $([guid]::NewGuid().ToString("N"))
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

function Create-BrainAsset($name, $guid, $colorR, $colorG, $colorB, $hitCount = 0, $isBall = $false) {
    $metaPath = Join-Path $DataDir "$name.asset.meta"
    $assetPath = Join-Path $DataDir "$name.asset"

    if (-not (Test-Path $metaPath)) {
        Set-Content -Path $metaPath -Value (New-AssetMeta $guid)
    }

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

    if (-not (Test-Path $metaPath)) {
        Set-Content -Path $metaPath -Value (New-AssetMeta $guid)
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

    if (-not (Test-Path $metaPath)) {
        Set-Content -Path $metaPath -Value (New-AssetMeta $guid)
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
  m_Script: {fileID: 11500000, guid: 82bc7cff34fc4ca6b832bbd366b63a01, type: 3}
  m_Name: $name
  m_EditorClassIdentifier: Assembly-CSharp::Game.Sorcerum.BallDataSo
  _brain: {fileID: 11400000, guid: $brainGuid, type: 2}
  _poolKey:
    _poolKey: simpleBall
"@
    Set-Content -Path $assetPath -Value $content
}

# --- 1. DISCS (Cyan, Pink, Yellow, Purple) ---
# Cyan Disc: 4d4afa9e79a429a4c9ffd57b2100833a (exists)
# Pink Disc: 7b585aff59fc4635a06991169d5cb27f (exists)
$brainYellowGuid = "a1b2c3d4e5f601010101010101010101"
$discYellowGuid  = "a1b2c3d4e5f601010101010101010102"
Create-BrainAsset "StackableItemBrain_Yellow" $brainYellowGuid 1.0 0.85 0.1 0 $false
Create-DiscAsset "SimpleStackableItem_Yellow" $discYellowGuid $brainYellowGuid

$brainPurpleGuid = "a1b2c3d4e5f602020202020202020201"
$discPurpleGuid  = "a1b2c3d4e5f602020202020202020202"
Create-BrainAsset "StackableItemBrain_Purple" $brainPurpleGuid 0.65 0.25 0.95 0 $false
Create-DiscAsset "SimpleStackableItem_Purple" $discPurpleGuid $brainPurpleGuid

# --- 2. BALLS WITH VARIOUS HIT COUNTS ---
$ballRegistry = @{}
# Existing:
$ballRegistry["Cyan_5"] = "179cec27b7322344c98cce586f032401"
$ballRegistry["Cyan_8"] = "c9e303e0ae404fc2818c10bc3fba8bf4"
$ballRegistry["Pink_3"] = "f159a70b6f7e4055b0a68a8064046bb1"

# Helper for creating colored balls
function Register-Ball($colorName, $r, $g, $b, $hitCount, $hexId) {
    $brainG = "b000" + $hexId + "0001"
    $ballG  = "b000" + $hexId + "0002"
    $bName  = "BallBrain_${colorName}_${hitCount}"
    $ballName = "SimpleBall_${colorName}_${hitCount}"
    Create-BrainAsset $bName $brainG $r $g $b $hitCount $true
    Create-BallAsset $ballName $ballG $brainG
    $script:ballRegistry["${colorName}_${hitCount}"] = $ballG
}

# Cyan balls: 1, 2, 3, 4, 6
Register-Ball "Cyan" 0 0.8509804 1 1 "c01"
Register-Ball "Cyan" 0 0.8509804 1 2 "c02"
Register-Ball "Cyan" 0 0.8509804 1 3 "c03"
Register-Ball "Cyan" 0 0.8509804 1 4 "c04"
Register-Ball "Cyan" 0 0.8509804 1 6 "c06"

# Pink balls: 1, 2, 4, 5, 6
Register-Ball "Pink" 1 0.2 0.6 1 "p01"
Register-Ball "Pink" 1 0.2 0.6 2 "p02"
Register-Ball "Pink" 1 0.2 0.6 4 "p04"
Register-Ball "Pink" 1 0.2 0.6 5 "p05"
Register-Ball "Pink" 1 0.2 0.6 6 "p06"

# Yellow balls: 2, 3, 4, 5, 6
Register-Ball "Yellow" 1 0.85 0.1 2 "y02"
Register-Ball "Yellow" 1 0.85 0.1 3 "y03"
Register-Ball "Yellow" 1 0.85 0.1 4 "y04"
Register-Ball "Yellow" 1 0.85 0.1 5 "y05"
Register-Ball "Yellow" 1 0.85 0.1 6 "y06"

# Purple balls: 2, 3, 4, 5, 6
Register-Ball "Purple" 0.65 0.25 0.95 2 "u02"
Register-Ball "Purple" 0.65 0.25 0.95 3 "u03"
Register-Ball "Purple" 0.65 0.25 0.95 4 "u04"
Register-Ball "Purple" 0.65 0.25 0.95 5 "u05"
Register-Ball "Purple" 0.65 0.25 0.95 6 "u06"

# Disc guids
$discCyan   = "4d4afa9e79a429a4c9ffd57b2100833a"
$discPink   = "7b585aff59fc4635a06991169d5cb27f"
$discYellow = $discYellowGuid
$discPurple = $discPurpleGuid

Write-Host "Discs and Balls created successfully!"
