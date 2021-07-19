using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
///
///  Author: Tolga K, 07/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
///  This class is intended to keep logs on client side. It can
///  record the logs as well as it can display on Unity Editor.
///  Also an important capability is to be able to upload them
///  to our server so logs can be viewed from the distance
///
/// </summary>

public static class Log
{
    static int maxSizeKB = 64;
    static int maxFiles = 3;
    static string uploadUrl = "http://";

    public static void Info(string message)
    {
        CheckLogFile();
        LogEntry("I", message);
    }

    public static void Error(string message)
    {
        CheckLogFile();
        LogEntry("E", message);
    }

    public static void Warning(string message)
    {
        CheckLogFile();
        LogEntry("W", message);
    }

    static void LogEntry(string v, string message)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter("log.txt", true))
            {
                sw.WriteLine(FormattedDate() + " :: " + v + " :: " + message);
            }
        }
        catch (Exception ex)
        {
#if UNITY_EDITOR
            Debug.Log("Log system encountered an error: " + ex.Message + "\nStack trace:\n" + ex.StackTrace);
#else
            throw new Exception("Log system encountered an error (see inner exception for details)", ex);
#endif
        }
    }

    private static string FormattedDate()
    {
        return DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0') + " " + DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0');
    }

    /// <summary>
    /// Tests whether log file is over the specified size. If it is, it will shift the previously
    /// packed log files like log2.txt to log3.txt, log1.txt to log2.txt and log.txt to log1.txt
    /// </summary>
    static void CheckLogFile()
    {
        try
        {
            if(File.Exists("log.txt"))
            {
                FileInfo info = new FileInfo("log.txt");
                if(info.Length >= maxSizeKB * 1024)
                {
                    // pack and put to the size
                    for (int i = maxFiles; i >= 1; i--)
                    {
                        if(File.Exists("log" + i + ".txt"))
                        {
                            File.Delete("log" + i + ".txt");
                        }

                        if(File.Exists("log" + (i - 1) + ".txt"))
                        {
                            File.Move("log" + (i - 1) + ".txt", "log" + i + ".txt");
                        }
                    }

                    File.Move("log.txt", "log1.txt");
                }
            }
        } catch(Exception ex)
        {
#if UNITY_EDITOR
            Debug.Log("Log system encountered an error: " + ex.Message + "\nStack trace:\n" + ex.StackTrace);
#else
            throw new Exception("Log system encountered an error (see inner exception for details)", ex);
#endif
        }
    }

    /// <summary>
    /// This function will upload logs to the server
    /// </summary>
    public static void UploadLog()
    {
        throw new NotImplementedException();
    }
}
