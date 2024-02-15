using System.IO;
using System;
using UnityGameFramework.Runtime;
using WhiteTea.GameEditor.DataTableTools;

namespace WhiteTea.GameEditor
{
    public static class WhiteTeaEnumIdGenerator
    {
        public static void GeneratorEnumIdFile(string dataTableName , DataTableProcessor dataTableProcessor)
        {
            StreamWriter(dataTableProcessor , WhiteTeaEditorConfigs.HotfixAssemblyMainReadOnly , WhiteTeaEditorConfigs.CSharpResourceCodePath , $"{dataTableName}Id");
        }
        private static void StreamWriter(DataTableProcessor dataTableProcessor , string nmespace , string codePath , string datatableName)
        {
            if(!Directory.Exists($"{codePath}/"))
            {
                Log.Warning($"{codePath}不存在！");
                return;
            }
            using(StreamWriter sw = new StreamWriter($"{codePath}/{datatableName}.cs"))
            {
                sw.WriteLine("//------------------------------------------------------------");
                sw.WriteLine("// 此文件由工具自动生成，请勿直接修改。");
                sw.WriteLine("// 生成时间：" + DateTime.UtcNow.ToLocalTime( ).ToString("yyyy-MM-dd"));
                sw.WriteLine("//------------------------------------------------------------");

                //命名空间
                sw.WriteLine($"namespace {nmespace}");
                sw.WriteLine("{");

                //类名
                sw.WriteLine("\t/// <summary>");
                sw.WriteLine($"\t/// {dataTableProcessor.GetValue(0 , 1)}");
                sw.WriteLine("\t/// </summary>");
                sw.WriteLine($"\tpublic enum {datatableName}");
                sw.WriteLine("\t{");

                //对象
                int start_index = 4;
                for(int i = start_index; i < dataTableProcessor.RawRowCount; i++)
                {
                    sw.WriteLine("\t\t/// <summary>");
                    sw.WriteLine($"\t\t///{dataTableProcessor.GetValue(i , 2)}");
                    sw.WriteLine("\t\t/// </summary>");
                    sw.WriteLine($"\t\t{dataTableProcessor.GetValue(i , 3)} = {dataTableProcessor.GetValue(i , 1)},");
                }

                //end
                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }
        }
    }
}
