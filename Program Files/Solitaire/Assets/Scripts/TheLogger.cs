using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Wrapper class for doing our logging
 */
public class TheLogger : MonoBehaviour
{
    static Logger theLog = new Logger(Debug.unityLogger.logHandler);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
     * Method to log to the console for strings Overloaded for bool's as well
     */
    public static void PrintLog(string log)
    {

        theLog.Log(log);

    }

    public static void PrintLog(bool log)
    {

        theLog.Log(log);

    }
}
