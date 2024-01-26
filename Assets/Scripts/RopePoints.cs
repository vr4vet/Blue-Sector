using UnityEngine;
using UnityEngine.UI;

public class RopePoints : MonoBehaviour {

    public bool removedOld = false;
    public bool addedNew = false;
    public AddPoints points;
   // public Toggle toggle1;
   // public Toggle toggle2;
    private int teller_new = 0;
    private int teller_old = 0;

    // If removed old/broken rope, give point
    public void removed_Old()
    {
            removedOld = true;
            points.AddTilsyn(1);
            teller_old += 1;
        /*if (teller_old >= 1) {
            toggle2.isOn = true;
        }*/
    }
    //If new rope added where missing, give 1 point
    public void added_New() {
        addedNew = true;
        points.AddTilsyn(1);
        teller_new+=1;

        /*if (teller_new >= 1) {
            toggle1.isOn = true;
        }*/
    }
}
