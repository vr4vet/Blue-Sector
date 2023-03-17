using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;

interface ILevel
{
    string Mode { get; set; }
    int LevelNo { get; set; }
    int TimeLimit { get; set; }
    float FailureThreshold { get; set; }
    int BeatThreshold { get; set; }
    ILevel NextLevel { get; set; }
    bool IsUnlocked { get; set; }
    bool IsBeaten { get; set; }
    /*
    private int timeLimit = 480;
    private float modifier = 1.0;
    private bool isUnlocked = false;
    private bool isBeaten = false;
    */
}

public class Levels : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReadXML()
    {
        xmlDoc = XDocument.Load("LevelList.xml");
        System.Xml.Serialization.XmlSerializer reader =
    new System.Xml.Serialization.XmlSerializer(typeof(ILevel));
        System.IO.StreamReader file = new System.IO.StreamReader(
            @"LevelList.xml");
        file.Close();

        Console.WriteLine(test.mode);
    }

    // Move to own class ?
    float CalculateDifficulty(string type, int levelNo, float modifier = 1)
    {
        float difCalc = 0;

        return difCalc;
    }

    void SetDifficulty(float number) { }
}