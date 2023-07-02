using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LiceInterfaceController : MonoBehaviour
{
    public Slider liceSlider;
    public TMP_Text liceCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountLice() {
        liceCounter.text = liceSlider.value.ToString();
    }

    public void SetLice(int lice) {
        liceCounter.text = lice.ToString();
        liceSlider.value = lice;
    }
}
