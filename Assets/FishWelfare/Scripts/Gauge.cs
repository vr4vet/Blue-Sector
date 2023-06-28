using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    private Slider measuringGauge;
    private TankController tankController;
    // Start is called before the first frame update
    void Start()
    {
        measuringGauge = GetComponentInChildren<Slider>();
        tankController = transform.root.gameObject.GetComponent<TankController>();
    }

    // Update is called once per frame
    void Update()
    {
        measuringGauge.value = tankController.sedativeConsentration;
    }
}
