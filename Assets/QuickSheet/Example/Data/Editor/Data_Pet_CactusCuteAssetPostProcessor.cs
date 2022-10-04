using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class Data_Pet_CactusCuteAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/8_Data/1_PetData.xlsx";
    private static readonly string assetFilePath = "Assets/8_Data/Data_Pet_CactusCute.asset";
    private static readonly string sheetName = "Data_Pet_CactusCute";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            Data_Pet_CactusCute data = (Data_Pet_CactusCute)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(Data_Pet_CactusCute));
            if (data == null) {
                data = ScriptableObject.CreateInstance<Data_Pet_CactusCute> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<Data_Pet_CactusCuteData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<Data_Pet_CactusCuteData>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
