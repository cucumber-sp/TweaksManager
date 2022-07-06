using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ModLoader;
using SFS.IO;
using UnityEngine;

namespace TweaksManager
{
    public class TweaksManagerMod : Mod
    {
        public TweaksManagerMod() : base("tweaksmanager", "TweaksManager", "CucumberSpace", "0.5.7", "1.0.0",
            "description")
        {
        }

        public override void Load()
        {
            /*foreach (FilePath file in new FolderPath(ModFolder).Extend("Tweaks").CreateFolder().GetFilesInFolder(false))
            {
                if (file.Extension != "cs")
                    continue;
                string fileText = file.ReadText();
                string[] namespaces = GetUsing(fileText);
                

                Script<object> script = CSharpScript.Create(fileText,
                    ScriptOptions.Default
                        .WithReferences(AppDomain.CurrentDomain.GetAssemblies().Where(x => x.Location != ""))
                        .WithImports(namespaces));

                try
                {
                    script.Compile();
                    script.RunAsync();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            */
            
            ScriptUtility.LoadScriptsFromPath(new FolderPath(ModFolder).Extend("Tweaks"));
        }

        public override void Unload()
        {
        }

        public static string[] GetUsing(string code)
        {
            MatchCollection matches = Regex.Matches(code, "using ([A-z.0-9]+);");
            List<string> namespaces = new List<string>();

            for (int i = 0; i < matches.Count; i++) namespaces.Add(matches[i].Groups[1].Value);

            return namespaces.ToArray();
        }
    }
}