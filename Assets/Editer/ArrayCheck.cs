using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArrayCheck : EditorWindow
{
    [MenuItem("Window/Array")]
    private static void Create()
    {
        GetWindow<ArrayCheck>("�z��̒��g");
    }

    [SerializeField,Header("�ʒu���������I�u�W�F�N�g")] private GameObject Target = default;

    private void OnGUI()
    {
        
    }
}
