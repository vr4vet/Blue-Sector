# Fish

This prefab spawns as a child component of the FishSystem prefab when the game is started. It is therefore not necessary to place this prefab in the scene. The prefab has no paramateres that can be set in the Inspector, and gets all necessary data from the parent FishSystem component.

## Behaviour

The fish component swims randomly when idle (before game is started, or when game is completed), swim up when hungry and return when full. These states are decided by the parent FishSystem component. New random directions are generated every 3rd second.
  
## LOD Group

This prafab has LOD (level of detail). There are three levels: 3D-model, sprite, and culled. The exact distances from the camera at which these transition can be adjusted in the LOD Group component.
