using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
    public static class AttributeDiagnostics
    {
        public static void Dump(string modulesDirectory)
        {
            var targetFullName = typeof(GeorgeModuleAttribute).FullName;
            Debug.WriteLine($"期望的特性类型: {targetFullName}");
            Debug.WriteLine($"Shell 侧类型程序集: {typeof(GeorgeModuleAttribute).Assembly.Location}");
            Debug.WriteLine($"Shell 侧类型版本: {typeof(GeorgeModuleAttribute).Assembly.GetName().Version}");

            foreach (var dllPath in Directory.GetFiles(modulesDirectory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                Assembly asm;
                try { asm = Assembly.LoadFrom(dllPath); }
                catch (Exception ex) { Debug.WriteLine($"[加载失败] {Path.GetFileName(dllPath)}: {ex.Message}"); continue; }

                Type[] types;
                try { types = asm.GetTypes(); }
                catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).ToArray(); }

                foreach (var type in types.Where(t => t.IsClass))
                {
                    var datas = type.GetCustomAttributesData()
                                    .Where(d => d.AttributeType.FullName == targetFullName)
                                    .ToList();
                    foreach (var data in datas)
                    {
                        Debug.WriteLine($"=== {Path.GetFileName(dllPath)} :: {type.FullName} ===");
                        Debug.WriteLine($"  特性类型所在程序集: {data.AttributeType.Assembly.Location}");
                        Debug.WriteLine($"  构造函数参数: {string.Join(", ", data.ConstructorArguments.Select(a => a.Value ?? "<null>"))}");
                        foreach (var named in data.NamedArguments)
                            Debug.WriteLine($"  命名参数: {named.MemberName} = {named.TypedValue.Value ?? "<null>"}");
                    }
                }
            }
        }
    }
}
