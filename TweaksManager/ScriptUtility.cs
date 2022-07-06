using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SFS.IO;
using UnityEngine;

namespace TweaksManager
{
    public static class ScriptUtility
    {
        public static void LoadScriptsFromPath(FolderPath path)
        {
            List<string> methodsNames = new List<string>();
            HashSet<string> usingNamespaces = new HashSet<string>();
            List<string> codeFragments = new List<string>();
            List<string> classNames = new List<string>();

            foreach (FilePath file in path.CreateFolder().GetFilesInFolder(false))
            {
                if (file.Extension != "cs")
                    continue;
                string fileText = file.ReadText();
                string[] _usingNamespaces = GetUsing(fileText, out string[] _usingLines);
                foreach (string usingNamespace in _usingNamespaces)
                    usingNamespaces.Add(usingNamespace);
                foreach (string usingLine in _usingLines)
                    fileText = fileText.Replace(usingLine, String.Empty);
                codeFragments.Add(fileText);
                classNames.Add(Regex.Match(fileText, @"//MainClassName: ([A-z._]+)").Groups[1].Value);
            }


            StringBuilder sourceCode = new StringBuilder(string.Join(System.Environment.NewLine, codeFragments));
            sourceCode.AppendLine(Environment.NewLine);
            foreach (string className in classNames)
                sourceCode.AppendLine($"{className}.Load();");
            Script<object> script = CSharpScript.Create(sourceCode.ToString(),
                ScriptOptions.Default
                    .WithReferences(AppDomain.CurrentDomain.GetAssemblies().Where(x => x.Location != ""))
                    .WithImports(usingNamespaces.ToArray()));
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
        
        public static string[] GetUsing(string code, out string[] usingLines)
        {
            List<string> _usingLines = new List<string>();
            MatchCollection matches = Regex.Matches(code, "using ([A-z.0-9]+);");
            List<string> namespaces = new List<string>();

            for (int i = 0; i < matches.Count; i++)
            {
                namespaces.Add(matches[i].Groups[1].Value);
                _usingLines.Add(matches[i].Value);
            }
            usingLines = _usingLines.ToArray();

            return namespaces.ToArray();
        }
    }
}