﻿#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
//using WW;

[InitializeOnLoad]
public class LogWrapperConsoleWindow : EditorWindow
{

    private List<string> debugNames;

    private int debugFlags = 0;

    private int debugLvl = 0;

    private UnityEngine.Object go = null;

    private bool clearBtn = false;

    [MenuItem("Tools/" + nameof(LogWrapperConsoleWindow))]
    static void Init()
    {
        LogWrapperConsoleWindow pWindow = (LogWrapperConsoleWindow)EditorWindow.GetWindow(typeof(LogWrapperConsoleWindow));
        if (pWindow)

        pWindow.minSize = new Vector2(100, 25);
        pWindow.maxSize = new Vector2(500, 35);
        pWindow.title = "WW Debugger";
    }


    static LogWrapperConsoleWindow()
    {
        // setStaticValues();
    }


    void OnGUI()
    {

        debugNames = new List<string>();
        //for (int c = 0; c < WW.debug.colors.Length; c++)
        //{
        //    debugNames.Insert(c, Enum.GetName(typeof(WW.debug.cat), c));
        //}


        //debugFlags = EditorGUI.MaskField(new Rect(5, 5, 150, 20),
        //                                  //"",
        //                                  EditorPrefs.GetInt("WW_debuggerflags", 0),
        //                                  debugNames.ToArray());


        debugLvl = EditorGUI.Popup(new Rect(180, 5, 150, 20),
                                            EditorPrefs.GetInt("WW_debuggerlvl", 0),
                                            new string[] { "0 - IMPORTANT", "1", "2", "3", "4", "5 - MOST STUFF", "6", "7", "8", "9 - ALL" }
                                        );


        go = EditorGUI.ObjectField(new Rect(15, 25, 250, 15),
                    go,
                    typeof(UnityEngine.Object));

        GUI.color = Color.red;
        clearBtn = EditorGUI.Toggle(new Rect(280, 23, 15, 15),
                        "",
                        clearBtn);


        if (GUI.changed)
        {
            EditorPrefs.SetInt("WW_debuggerflags", debugFlags);

            EditorPrefs.SetInt("WW_debuggerlvl", debugLvl);


            if (go != null)
            {
                if (go.GetType() == typeof(GameObject))
                {
                    GameObject goYes = (GameObject)go as GameObject;
                    EditorPrefs.SetString("WW_debuggerObject", goYes.transform.root.gameObject.name);
                }
                else
                {
                    EditorPrefs.DeleteKey("WW_debuggerObject");
                }
            }

            if (clearBtn)
            {
                go = null;
                EditorPrefs.DeleteKey("WW_debuggerObject");
                clearBtn = false;
            }


            setStaticValues();
        }
    }


    static void setStaticValues()
    {
        //WW.debug.debugFlags = EditorPrefs.GetInt("WW_debuggerflags", 0);
        //WW.debug.debugLevel = EditorPrefs.GetInt("WW_debuggerlvl", 9);
        //WW.debug.importantGO = EditorPrefs.GetString("WW_debuggerObject", "");
        //WW.debug.debuggerActive = true; //always true because all this is #EDITOR_ONLY code
    }
}
#endif