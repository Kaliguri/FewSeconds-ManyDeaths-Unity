#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;



public class DBMenu : OdinMenuEditorWindow
{
    [MenuItem("Tools/FewSecondsManyDeaths/DBMenu")]
    private static void OpenWindow()
    {
        GetWindow<DBMenu>().Show();
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        throw new System.NotImplementedException();
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif