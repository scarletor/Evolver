using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class Data_Enemy_MushRoomAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/8_Data/1_EnemyData.xlsx";
    private static readonly string assetFilePath = "Assets/8_Data/Data_Enemy_MushRoom.asset";
    private static readonly string sheetName = "Data_Enemy_MushRoom";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            Data_Enemy_MushRoom data = (Data_Enemy_MushRoom)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(Data_Enemy_MushRoom));
            if (data == null) {
                data = ScriptableObject.CreateInstance<Data_Enemy_MushRoom> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<Data_Enemy_MushRoomData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<Data_Enemy_MushRoomData>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
