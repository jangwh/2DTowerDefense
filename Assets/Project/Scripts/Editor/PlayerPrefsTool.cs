using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerPrefsTool
{
    [MenuItem("Tools/Delete All Prefs")]
    public static void DeleteAllPrefs()
    {
        string title = "Player Prefs 삭제";
        string message = "모든 Player Prefs 데이터를 삭제합니다.";
        string ok = "예, 삭제합니다.";
        string cancel = "취소";
        if (EditorUtility.DisplayDialog(title, message, ok, cancel))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("모든 Player Prefs 데이터가 삭제되었습니다.");
        }
        else
        {
            Debug.Log("모든 Player Prefs 데이터가 삭제되지 않았습니다.");
        }
    }
}
