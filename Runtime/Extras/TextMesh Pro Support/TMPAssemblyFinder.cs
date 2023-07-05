#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.PackageManager;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor.PackageManager.Requests;
using Unity.Plastic.Newtonsoft.Json;

namespace SametHope.RapidLocalization.Misc
{
    public static class TMPAssemblyFinder
    {

        [MenuItem("Tools/SametHope/Testing/LIST")]
        [InitializeOnLoadMethod]
        public static async void Find()
        {
            var tmpPackageInfo = await GetPackage("com.unity.textmeshpro");
            if (tmpPackageInfo == null)
            {
                //Debug.Log("Tmp not found.");
                return;
            }

            string rlrASMDEFPath = GetRapidLocalizationRuntimeASMDEFPath();
            string rlrASMDEFContent = ReadFile(rlrASMDEFPath);

            string tmpASMDEFMetaPath = $"{tmpPackageInfo.resolvedPath}\\Scripts\\Runtime\\Unity.TextMeshPro.asmdef.meta";
            string tmpASMDEFMetaContent = ReadFile(tmpASMDEFMetaPath);
            string tmpASMDEFGUID = tmpASMDEFMetaContent.Split('\n').Where(i => i != "").ToList()[1].Substring("guid: ".Length);

            if (rlrASMDEFContent.Contains(tmpASMDEFGUID))
            {
                //Debug.Log("Tmp already referenced.");
                return;
            }

            var fileStream = new FileStream(rlrASMDEFPath, FileMode.Open, FileAccess.ReadWrite);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                var rlrASMDEFContentJson = JsonConvert.DeserializeObject(rlrASMDEFContent);
                // Fuck it, this methods wont get called with compilation errors anyway.
            }

        }


        private static async Task<UnityEditor.PackageManager.PackageInfo> GetPackage(string packageName)
        {
            ListRequest listReq = Client.List(offlineMode: true, includeIndirectDependencies: true);
            while (!listReq.IsCompleted && listReq.Error == null)
            {
                await Task.Delay(10);
            }
            UnityEditor.PackageManager.PackageInfo packageInfo = listReq.Result.SingleOrDefault(r => r.name == packageName);
            return packageInfo;
        }

        private static string ReadFile(string filePath)
        {
            string data;
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                data = streamReader.ReadToEnd();
            }
            return data;
        }

        private static string GetRapidLocalizationRuntimeASMDEFPath()
        {
            string knownPath = "Assets\\Plugins\\SametHope\\Rapid-Localization\\Runtime";
            string pluginRuntimeFolderPath = Path.GetFullPath(knownPath);

            string rapidLocalizationAsmdefPath = pluginRuntimeFolderPath + "\\SametHope.RapidLocalization.asmdef";
            
            return rapidLocalizationAsmdefPath;
        }
    }
}

#endif