# Fishmerd Controls

## Scripts

This component includes four scripts.

### SlideBridge

#### Summary

A bridge script between a slider and a *FishSystem* GameObject.
Enables controlling feeding rate of FishSystem through slider input.

#### Configuration

*GameObject* FishSystem:
Which FishSystem you want to control with the given slider.

#### Functions
    
***void* Start*

Gets defined FishSystems *FishSystemScript*, and stores it local var
'fishSystemScript'.

***void* OnSlideChanged(*float* position)*

Sets feedingIntensity, foodGivenPerSec, and emission.ratOverTime values for
fishSystemscript based on slider posisiton.

### SliderHelper

#### Summary

A helper class for BNG 3D Sliders.
Enables snap-to slider steps.

#### Configuratiom

*int* steps:
How many steps / snap-points you wish the slider to have.

*Axis(enum)* axis:
Which axis the slider runs along.

**Note:**
Can only be set to main axies (X-Y-Z).

*GameObject* sliderPath:
The object that defines the sliders path.
The script takes the size of this object, coupled with given axis, to calculate
the steps along the slider.

*float* offset:
An arbitrary offset at each end of the slider path to limit the end and
beginning of snap-point calculation.
E.g.: to avoid collision with neighboring object.

#### Functions

***void* OnRelease**

*Summary:*

An override of BNG.GrabbableEvents.
Executes snap point behaviour on release of sliderknob.

***void* SetPosition (*float* axisPosition)**

*Summary:*

Calculates closest snap point along given axis from param axisPosition.
Then updates the position of slider knob to closest snap-point along axis.

*Takes:*
*float* axisPositon: local position of slider-knob along axis.

***float* GetPosition**

*Summary:*

Gets local position of the knob along given axis.

*Returns:*

float value equal to local position of knob along given axis.

### RadioButton

### CustomizableButton

## Prefabs

### Unified Controls

The *Unified Controls* Prefab combines *MerdButton* as well as *Slider* prfabs
into one unified control surface.

### MerdButton

Combines BNG Button with Merdcamea switching.

### Slider

Implements SlideBridge and SlideHelper Scripts on what amounts to a carbon copy
of BNG *Slide* prefab.
