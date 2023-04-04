using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;


public class LevelLoader : MonoBehaviour {
    XDocument xmlDoc;
    IEnumerable<XElement> modes;
    List<Mode> modesList = new List<Mode>();

    int timeLimit;
    float failureThreshold, modifier;
    bool finishedLoading = false, isUnlocked, tutorial;
    string name;

    void start () {
        DontDestroyOnLoad(gameObject);
        LoadXML();
        StartCoroutine("AssignData");
    }

    void update () {
        if (finishedLoading) {
            SceneManager.LoadScene("testScene");
        }
    }

    void LoadXML() {
        xmlDoc = XDocument.Load(@"LevelList.xml");
        modes = xmlDoc.Descendants("modes").Elements();
    }

    void AssignData () {
        foreach (var mode in modes) {
            name = mode.Parent.Attribute("name").Value;
            tutorial = bool.Parse(mode.Parent.Element("tutorial").Value);
            timeLimit = int.Parse(mode.Parent.Element("time").Value);
            failureThreshold = float.Parse(mode.Parent.Element("threshold").Value);
            isUnlocked = bool.Parse(mode.Parent.Element("unlocked").Value);
            modifier = float.Parse(mode.Parent.Element("modifier").Value);

            modesList.Add(new Mode(name, tutorial, timeLimit, failureThreshold, isUnlocked, modifier));
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