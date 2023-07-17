Note 1: The Renderer P1, P2 (in the Canvas GameObject of the UI prefab) and individual agent Indicators are UI prefabs that belong to a free asset in the Unity Asset store. 
The Scene will work without it, but you won't get HUD indicators for player and enemy and you will get a missing prefab reference in the UI prefab for them.
To fix, just delete the references or download the asset at:
https://assetstore.unity.com/packages/tools/gui/hud-indicator-220695

Note 2: Some of the fonts we use are from a free Unity Asset. You do not have to download this asset, Unity should fall back to it's default font. Or it can be found at
https://assetstore.unity.com/packages/tools/gui/mk-easy-text-light-140840