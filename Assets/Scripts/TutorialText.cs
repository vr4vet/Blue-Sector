using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour {
    public int myText;
    private void Update()
    {
        myText = TutorialOverview.count;
        transform.GetComponent<TextMesh>().text ="Flytt fiskene fra den høyre" + "\n" +  "kassen til den venstre kassen " + "\n"+ "" + myText + "/3 fisker flyttet";
    }
}
