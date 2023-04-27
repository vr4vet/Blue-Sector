# Fish System

This prefab is the implementation of the fish cage. It instantiates Fish components as children of itself, and contains all the logic for determining the fish's behaviour. This includes hunger state, dimensions of the swimmable area, and so on.  

The component is shaped like a cylinder which is divided into two zones. The lower half is where fish go when full, and the upper half is where they go when hungry.

## Attributes

The FishSystem component has many attributes which can be set in Unity's Inspector:

- Merd Nr: Identificating number
- Fish: Fish component
- Radius: The system's radius
- Height: The system's height
- Amount Of Fish: The amount of fish which will be instantiated within this system
- Fullness Divider: A float between 0 and 1 which decides where the fish cage is divided between full and hunger zone
- Swim Speed (vertical and horisonal): Float values which decide vertical and horisontal swim speed respectively
