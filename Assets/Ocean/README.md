# Ocean
## Usage
Place down an instance of the Ocean prefab.

The underwater effect is achieved though the use of post processing shaders. This must be applied to the camera(s) of interest using a post processing volume and post processing layer. The post-processing layer must be disabled when the player/camera is not underwater. See the TeleportPlayerToCamera.cs script for how to apply this filter programatically. See MerdCamera for how to apply this filter durably.

## Extensibility
This prefab uses the **FasterOceanEffectShader** by default, which is optimized for the Oculus Quest 2. If you are targetting DX11+, use the **AdvancedOceanEffectShader** instead.

The ocean is only visible from one direction, hence, there is a simplified, underwater version of the shader as well. Enable/disable components to suit your needs.


