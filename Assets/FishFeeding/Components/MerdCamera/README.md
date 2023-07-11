# Merd Camera
## Usage
1. Place down an instance of the **MerdCamera** prefab into the scene.
1. Place down an instance of the **MerdCameraHost** prefab into the scene.
1. Select the **MerdCameraHost** in the inspector, expand the _Merd Camera Controller_ script, expand the _Cameras_ array, drag the **MerdCamera** into the camera array.

## Rendering the Camera
In order to render the outputs of the camera, apply the _MerdCameraTexture_ to a mesh renderer. The **MerdCameraHost** prefab has this texture pre-applied.

## Controlling the Camera
Place down an instance of the **MerdCameraJoystick** prefab. This prefab contains a desk, a monitor, and a control panel for switching between and manipulating the camera. This prefab has some extra functionality specifically related to the fish feeding scenario, e.g., FishFeedingHints and FishFeedingStructuredTutorial, however, these are not required in order to use the camera.

The selected camera is controlled by the _Merd Camera Controller_ script. _Merd Camera Controller_ also contains methods for manipulating the position of the selected camera.
