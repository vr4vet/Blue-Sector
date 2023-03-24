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
    IEnumerable<XElement> levels;
    List<XMLData> data = new List<XMLData>();

    int iteration = 0, levelNo = 0, timeLimit = 0, beatenThreshold = 0;
    float failureThreshold = 0;
    bool finishedLoading = false, isUnlocked = false, isBeaten = false;

    void start () {
        DontDestroyOnLoad(gameObject);
        LoadXML("arcadelevels");
        StartCoroutine("AssignData");
    }

    void update () {
        if (finishedLoading) {
            SceneManager.LoadScene("testScene");
        }
    }

    void LoadXML(string levelType) {
        xmlDoc = XDocument.Load(@"LevelList.xml");
        levels = xmlDoc.Descendants(levelType).Elements();
    }

    void AssignData () {
        foreach (var level in levels) {
            levelNo = iteration;
            timeLimit = int.Parse(level.Parent.Element("time").Value);
            failureThreshold = float.Parse(level.Parent.Element("failure_threshold").Value);
            beatenThreshold = int.Parse(level.Parent.Element("beaten_threshold").Value);
            isUnlocked = bool.Parse(level.Parent.Element("is_unlocked").Value);
            isBeaten = bool.Parse(level.Parent.Element("is_beaten").Value);

            data.Add(new XMLData(levelNo, timeLimit, failureThreshold, beatenThreshold, isUnlocked, isBeaten));
            Debug.Log(data[iteration].levelNo);
            iteration ++;
        }

        finishedLoading = true;
    }


}

public class XMLData {
    public int levelNo;
    public int timeLimit;
    public float failureThreshold;
    public int beatenThreshold;
    public bool isUnlocked;
    public bool isBeaten;

    public XMLData(int level, int time, float failure, int beat, bool unlocked, bool beaten) {
        levelNo = level;
        timeLimit = time;
        failureThreshold = failure;
        beatenThreshold = beat;
        isUnlocked = unlocked;
        isBeaten = beaten;
    }
}