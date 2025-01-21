using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    private Slider measuringGauge;
    private TankController tankController;
    private bool _anaestheticStepCompleted = false;
    public UnityEvent m_OnEnoughAnaesthetic; // event to complete anaesthetic step of welfare scenario

    // Start is called before the first frame update
    void Start()
    {
        if (m_OnEnoughAnaesthetic == null)
            m_OnEnoughAnaesthetic = new();

        measuringGauge = GetComponentInChildren<Slider>();
        //tankController = transform.root.gameObject.GetComponent<TankController>();
        tankController = GetComponentInParent<TankController>();
    }

    // Update is called once per frame
    void Update()
    {
        measuringGauge.value = tankController.sedativeConsentration;

        if (!_anaestheticStepCompleted && measuringGauge.value * (1 / measuringGauge.maxValue) >= .6) // invoke event to tell tablet to complete anaesthetising step
        {
            _anaestheticStepCompleted = true;
            m_OnEnoughAnaesthetic.Invoke();
        }
    }
}
