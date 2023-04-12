using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using System.IO;


public class ModeLoader : MonoBehaviour {
    public XDocument xmlDoc;
    public IEnumerable<XElement> modes;
    public List<Mode> modesList = new List<Mode>();
    public bool finishedLoading = false;

    private int timeLimit;
    private float failureThreshold, modifier;
    private bool isUnlocked, tutorial;
    private string __name;

    void Start() {
        DontDestroyOnLoad(gameObject);
        LoadXML();
        StartCoroutine(AssignData());
    }

    void LoadXML() {
        xmlDoc = XDocument.Load(@"Assets/FishFeeding/LevelSystem/LevelList.xml");
        modes = xmlDoc.Descendants("modes").Elements();
        Debug.Log(modes);
    }

    IEnumerator AssignData () {
        foreach (var mode in modes) {
            __name = mode.Attribute("name").Value;
            tutorial = bool.Parse(mode.Element("tutorial").Value);
            timeLimit = int.Parse(mode.Element("time").Value);
            failureThreshold = float.Parse(mode.Element("threshold").Value);
            isUnlocked = bool.Parse(mode.Element("unlocked").Value);
            modifier = float.Parse(mode.Element("modifier").Value);

            modesList.Add(new Mode(__name, tutorial, timeLimit, failureThreshold, isUnlocked, modifier));
            yield return null;
        }

        finishedLoading = true;
    }
}

public class Mode {
    public string name;
    public bool tutorial;
    public int timeLimit;
    public float failureThreshold;
    public bool isUnlocked;
    public float modifier;

    public Mode(string _name, bool _tutorial, int _timeLimit, float _failureThreshold, bool _isUnlocked, float _modifier) {
        name = _name;
        tutorial = _tutorial;
        timeLimit = _timeLimit;
        failureThreshold = _failureThreshold;
        isUnlocked = _isUnlocked;
        modifier = _modifier;
    }
}