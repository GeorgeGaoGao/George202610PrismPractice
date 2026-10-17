using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
    public static class ModuleScanner
    {
        /// <summary>
        /// 扫描指定目录下所有 DLL，收集其中带有 GeorgeModuleAttribute 的特性实例。
        /// </summary>
        public static List<GeorgeModuleAttribute> GetGeorgeModuleAttributes(string modulesDirectory)
        {
            if (string.IsNullOrWhiteSpace(modulesDirectory))
                throw new ArgumentException("目录不能为空", nameof(modulesDirectory));

            if (!Directory.Exists(modulesDirectory))
                throw new DirectoryNotFoundException($"Modules 目录不存在: {modulesDirectory}");

            var result = new List<GeorgeModuleAttribute>();
            var processed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var dllPath in Directory.GetFiles(modulesDirectory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                Assembly assembly;
                try
                {
                    // LoadFrom 会缓存已加载的同名程序集，避免重复加载
                    assembly = Assembly.LoadFrom(dllPath);
                }
                catch (BadImageFormatException) { continue; } // 非 .NET DLL（如 native）
                catch (FileLoadException) { continue; }       // 已被其他版本占用
                catch (Exception) { continue; }               // 其它加载失败，跳过

                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // 部分类型依赖缺失时，仍可拿到能加载的类型
                    types = ex.Types.Where(t => t != null).ToArray();
                }

                foreach (var type in types)
                {
                    // 特性通常打在类上，可按需扩展到接口/方法
                    if (!type.IsClass) continue;

                    GeorgeModuleAttribute? attr;
                    try
                    {
                        attr = type.GetCustomAttribute<GeorgeModuleAttribute>(inherit: false);
                    }
                    catch
                    {
                        continue; // 该类型特性解析失败，跳过
                    }

                    if (attr != null)
                    {
                        // 同一特性实例理论上只出现一次；用类型全名做去重更稳妥
                        var key = $"{assembly.GetName().Name}|{type.FullName}";
                        if (processed.Add(key))
                            result.Add(attr);
                    }
                }
            }

            return result;
        }
    }
}
