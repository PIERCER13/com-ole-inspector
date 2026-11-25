// @author piercer
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace PiercerComOleInspector
{
    class Program
    {
        static string ExpandEnvPath(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";
            string p = Environment.ExpandEnvironmentVariables(raw.Trim().Trim('"'));
            return p;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Piercer COM / ActiveX & InprocServer32 Inspector ===");

            string filter = args.Length > 0 ? args[0].ToLowerInvariant() : "";
            string outCsv = args.Length > 1 ? args[1] : "piercer_com_ole_audit.csv";

            int total = 0, missing = 0;
            var rows = new List<string> { "CLSID,Name,ServerType,Path,ExistsOnDisk" };

            using (RegistryKey clsidRoot = Registry.ClassesRoot.OpenSubKey("CLSID"))
            {
                if (clsidRoot == null)
                {
                    Console.WriteLine("Erro: HKCR\\CLSID inacessivel.");
                    return;
                }

                foreach (string clsid in clsidRoot.GetSubKeyNames())
                {
                    try
                    {
                        using (RegistryKey k = clsidRoot.OpenSubKey(clsid))
                        {
                            if (k == null) continue;
                            string name = Convert.ToString(k.GetValue("")) ?? "";
                            string srvType = "";
                            string srvPath = "";

                            using (RegistryKey inproc = k.OpenSubKey("InprocServer32"))
                            {
                                if (inproc != null)
                                {
                                    srvType = "InprocServer32";
                                    srvPath = ExpandEnvPath(Convert.ToString(inproc.GetValue("")));
                                }
                            }
                            if (string.IsNullOrEmpty(srvPath))
                            {
                                using (RegistryKey localSrv = k.OpenSubKey("LocalServer32"))
                                {
                                    if (localSrv != null)
                                    {
                                        srvType = "LocalServer32";
                                        srvPath = ExpandEnvPath(Convert.ToString(localSrv.GetValue("")));
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(srvPath)) continue;

                            if (!string.IsNullOrEmpty(filter) &&
                                !name.ToLowerInvariant().Contains(filter) &&
                                !srvPath.ToLowerInvariant().Contains(filter) &&
                                !clsid.ToLowerInvariant().Contains(filter))
                            {
                                continue;
                            }

                            string cleanFile = srvPath.Split(new[] { " -" , " /" }, StringSplitOptions.None)[0].Trim('"');
                            bool exists = File.Exists(cleanFile) || File.Exists(Path.Combine(Environment.SystemDirectory, cleanFile));
                            total++;
                            if (!exists)
                            {
                                missing++;
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[ORFAO] {0} | {1} -> {2}", clsid, name, srvPath);
                                Console.ResetColor();
                            }

                            rows.Add(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",{4}",
                                clsid,
                                name.Replace("\"", "\"\""),
                                srvType,
                                srvPath.Replace("\"", "\"\""),
                                exists ? "YES" : "NO"));
                        }
                    }
                    catch { }
                }
            }

            File.WriteAllLines(outCsv, rows.ToArray(), Encoding.UTF8);
            Console.WriteLine();
            Console.WriteLine("Total analisados: {0} | Orfaos/ausentes no disco: {1}", total, missing);
            Console.WriteLine("CSV exportado para: {0}", Path.GetFullPath(outCsv));
        }
    }
}
