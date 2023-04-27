# Modes

## Scripts

This component includes three scripts.

### ModeBtnBridge

#### Summary

A bridge between a BNG button and a Game for selecting modes.

#### Configuration

*ButtonType(enum)* buttonType:
What mode should the button be corrolated to?

#### Functions

***void* Awake:**

**Summary:**

Sets which game the button is tied to.

***void* OnButton:**

**Summary:*

Calls function in *Modes* to change mode in given game on buttonpush.

### ModeLoader

#### Summary

Class which loads list of mode configurations defined in XML.

#### Functions

***void* Start:*

**Summary:**

Loads the XML document, and starts coroutine to convert to instances of *Mode*.

***IEnumerator (Coroutine)* AssignData*

**Summary:**

Assigns data from XML document to instances of *Mode*.

#### Related Classes

#### Mode

**Summary:**

A representation of a mode in the fishFeeding scenario.

Has fields:
 - *string* name
 - *Tut(enum)* tutorial
 - *bool* hud
 - *int* timeLimit
 - *float* failureThreshold
 - *bool* isUnlocked
 - *float* modifier

**Note:**

Some of these fields remain unused in this scenario, however, they remain in
the codebase.
This is a result of the extensibility they provide, and probable usecase in
other scenarios.

### Modes

#### Summary

Binding class between a *Game* and a *ModeLoader*

#### Functions

***void* Start*

**Summary:**

Sets related *ModeLoader*.

***void* Update*

**Summary:**

Stores all modes, as well as current mode if *ModeLoader* has finished loading
from mode configuration file.

## Prefabs

### ModeBtn

### Levels

