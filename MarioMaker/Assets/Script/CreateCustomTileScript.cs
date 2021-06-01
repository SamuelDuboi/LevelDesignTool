using System.IO;
using UnityEditor;

namespace TileTool{

    public static class CreateCustomTileScript
    {
        public static string CreateSript(string tileName)
        {
            FileStream fs = File.Open("Assets/CustomeTilesScript/" + tileName + "Behavior.cs", FileMode.Create);
            using (StreamWriter outfile = new StreamWriter(fs))
            {
                #region Using
                outfile.WriteLine("using System.Collections;");
                outfile.WriteLine("using System.Collections.Generic;");
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("");
                #endregion
                #region NameSpace
                outfile.WriteLine("namespace TileTool");
                outfile.WriteLine("{");


                #region class
                outfile.WriteLine("public class " + tileName + "Behavior : TileBehavior");
                outfile.WriteLine("{");

                #region overrides
                outfile.WriteLine("");
                outfile.WriteLine("    public override void Start()");
                outfile.WriteLine("    {");
                outfile.WriteLine("        base.Start();");
                outfile.WriteLine("    }");


                outfile.WriteLine("");
                outfile.WriteLine("    public override void OnCollision()");
                outfile.WriteLine("    {");
                outfile.WriteLine("        base.OnCollision();");
                outfile.WriteLine("    }");


                outfile.WriteLine("");
                outfile.WriteLine("    public override void OnInputPlayer(KeyCode code)");
                outfile.WriteLine("    {");
                outfile.WriteLine("        base.OnInputPlayer(code);");
                outfile.WriteLine("    }");

                #endregion

                outfile.WriteLine("}");
                #endregion
                outfile.WriteLine("}");
                #endregion
            }
            AssetDatabase.Refresh();
            return "Assets/CustomeTilesScript/" + tileName + "Behavior.cs";
        }
    }
}
