using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LaserTurret))]
public class LaserTurretEditor : Editor
{
    private void OnSceneGUI()
    {
        var LaserTurret = (LaserTurret)target;
        
        LaserTurret.InitLaserPoints();
        
        EditorUtility.SetDirty(LaserTurret);
    }
}
