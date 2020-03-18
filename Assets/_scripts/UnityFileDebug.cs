using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UnityFileDebug : MonoBehaviour
{
    public bool useAbsolutePath = false;
    public string fileName = "MyGame";

    public string absolutePath = "/home/yourUsername/UnityLogs";

    public string filePath;
    public string filePathFull;
    public int count = 0;

    System.IO.StreamWriter fileWriter;

    public void Start(){
         string filePath = Application.persistentDataPath + "/testFile.txt";
        StreamWriter sr = File.CreateText (filePath);
        sr.WriteLine ("This is my file.");
            sr.WriteLine ("I can write ints {0} or floats {1}, and so on.", 1, 4.2);
                     sr.Close ();
    }

    void OnEnable()
    {
        UpdateFilePath();
        if (Application.isPlaying)
        {
            count = 0;
            fileWriter = new System.IO.StreamWriter(filePathFull, false);
            fileWriter.AutoFlush = true;
           // fileWriter.WriteLine("[");
            Application.logMessageReceived += HandleLog;
        }
    }

    void OnDisable()
    {
        if (Application.isPlaying)
        {
            Application.logMessageReceived -= HandleLog;
          //  fileWriter.WriteLine("\n]");
            fileWriter.Close();
        }
    }

    public void UpdateFilePath()
    {
        filePath = useAbsolutePath ? absolutePath : Application.persistentDataPath;
        filePathFull = System.IO.Path.Combine(filePath, fileName + "." +
            System.DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + ".csv");
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        //... same as above
        //if(type == LogType.Error)
        {
            fileWriter.Write((count == 0 ? "" : logString + "\n"));
           // Debug.Log(logString);
        }
        
            //+ JsonUtility.ToJson(j));
        count++;
    }
}