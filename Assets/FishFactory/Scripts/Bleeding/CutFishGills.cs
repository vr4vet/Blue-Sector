using UnityEngine;
public class CutFishGills : CutFish
{
        /// <summary>
        /// When the knife collides with the fish gills, the player makes a succesful cut
        /// </summary>
        /// <param name="collision"> The knife object </param>
        private void OnTriggerEnter(Collider collider)
        {
            cutEvent(collider.tag, true);
        }
}