using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace TradeIntellect.Updater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args == null)
                return;

            string zipFile = args.Length > 0 ? args[0] : string.Empty;
            string appDir = args.Length > 1 ? args[1] : string.Empty;
            string exePath = args.Length > 2 ? args[2] : string.Empty;
            string appName = args.Length > 3 ? args[3] : string.Empty;

            // ждём закрытия основного приложения
            if (zipFile.Length > 0 && appDir.Length > 0)
                Thread.Sleep(3000);

            if (!string.IsNullOrEmpty(appName) || !string.IsNullOrWhiteSpace(appName))
            {
                //Убиваем процесс основного приложения, если оно не закрыто
                Process[] processes = Process.GetProcessesByName(appName);
                if (processes.Length > 0)
                {
                    foreach (Process process in processes)
                    {
                        try
                        {
                            // Сначала можно попытаться закрыть вежливо
                            if (!process.CloseMainWindow())
                            {
                                // Если не помогло — принудительно убиваем
                                process.Kill();
                            }

                            // Ожидаем завершения процесса (опционально)
                            process.WaitForExit(3000);
                            process.Kill();
                        }
                        catch (System.ComponentModel.Win32Exception ex)
                        {
                            // Ошибка доступа, если у процесса выше привилегии (например, системный процесс)
                            Console.WriteLine($"Не хватает прав: {ex.Message}");
                        }
                        catch (NotSupportedException ex)
                        {
                            // Процесс находится на удаленном компьютере
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                    }
                }
            }

            // распаковываем архив с перезаписью (совместимо с .NET Framework 4.5)
            if (!string.IsNullOrEmpty(zipFile) && !string.IsNullOrEmpty(appDir))
            {
                using (var archive = ZipFile.OpenRead(zipFile))
                {
                    foreach (var entry in archive.Entries)
                    {
                        string filePath = Path.Combine(appDir, entry.FullName);

                        if (string.IsNullOrEmpty(entry.Name))
                        {
                            Directory.CreateDirectory(filePath);
                            continue;
                        }

                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        entry.ExtractToFile(filePath);
                    }
                }
            }

            // запускаем обновлённое приложение
            if (!string.IsNullOrEmpty(exePath))
                Process.Start(exePath);

            Environment.Exit(0);

            //if (args == null) return;
            //string zipFile = string.Empty;
            //if (args.Length > 0)
            //    zipFile = args[0];
            //string appDir = string.Empty;
            //if (args.Length > 1)
            //    appDir = args[1];
            //string exePath = string.Empty;
            //if (args.Length > 2)
            //    exePath = args[2];

            //// ждём, пока основное приложение закроется
            //if (zipFile.Length > 0 && appDir.Length > 0)
            //    Thread.Sleep(3000);

            //// распаковываем архив в папку приложения
            //if (zipFile != null && zipFile.Length > 5)
            //    if (appDir != null && appDir.Length > 5)
            //        ZipFile.ExtractToDirectory(zipFile, appDir);

            //// запускаем новое приложение
            //if (exePath != null && exePath.Length > 5)
            //    Process.Start(exePath);

            //Environment.Exit(0);
        }
    }
}