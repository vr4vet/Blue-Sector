# MonitorScore 
Prefab that contains a monitor which shows the score at the end of the game. Attached to the prefab is the Scoring-, Game- and StartGameTooltip-script.  
This prefab is currently being used in the MerdCameraJoystick prefab.

## Scoring
Script that takes care of all the scoring logic in the game. Has an UpdateScore function that updates the score based on the state of the fish system (whether the merd is Full, Hungry or Dying) and the current feeding intensity (High, Medium or Low). 

* UpdateScore  
Updates the score every second updates based on the state of the fish system (whether the merd is Full, Hungry or Dying) and the current feeding intensity (High, Medium or Low). 
Iterates through all of the fishsystems/merds in the scene and gives a score based on its state. Currently the player is awarded most points if the status of the merd is Full and feeding intensity is set to Low. They get less points if the merd is Full but the feeding intensity is set to Medium. Even less if the merd is hungry, regardless of the feeding intensity. The player gets zero points if the merd is Full and the feeding intensity to the merd is set to High, and also zero points if the state of the merd is Dying.

## Game
The script that contains the timer and lets the user start the game. In addition it updates the screen elements in the CanvasHUD prefab. 
Requires the existence of the FishSystem, MerdCameraHost and CanvasHUD prefabs and the Scoring and Modes scripts.

## StartGameTooltip
Makes the StartGameTooltip prefab visible based on the CanStartGame variable in the Game script. If the player is in the activated area and they haven't started the game, then it is set to true. Requires the StartGameTooltip prefab. 

# CanvasHUD
Prefab that contains a canvas with all of the game elements on the screen: Time left, Score, Amount of dead fish, Food wasted, Slider/bar that shows the amount of food wasted and the current merd being shown on the screen.  
This prefab is currently being used in the MerdCameraJoystick prefab.  
This prefab is also used by the Game script to update HUD in the game.