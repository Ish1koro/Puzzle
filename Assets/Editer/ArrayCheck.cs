using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArrayCheck : EditorWindow
{
    [MenuItem("Window/Array")]
    private static void Create()
    {
        GetWindow<ArrayCheck>("配列の中身");
    }

    [SerializeField,Header("位置を見たいオブジェクト")] private GameObject Target = default;

    private void OnGUI()
    {
        
    }
}
