using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Threading;

namespace TradeIntellect.Updater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args == null) return;
            string zipFile = string.Empty;
            if (args.Length > 0)
                zipFile = args[0];
            string appDir = string.Empty;
            if (args.Length > 1)
                appDir = args[1];
            string exePath = string.Empty;
            if (args.Length > 2)
                exePath = args[2];

            // ждём, пока основное приложение закроется
            if (zipFile.Length > 0 && appDir.Length > 0)
                Thread.Sleep(3000);

            // распаковываем архив в папку приложения
            if (zipFile != null && zipFile.Length > 5)
                if (appDir != null && appDir.Length > 5)
                    ZipFile.ExtractToDirectory(zipFile, appDir);

            // запускаем новое приложение
            if (exePath != null && exePath.Length > 5)
                Process.Start(exePath);

            Environment.Exit(0);
        }
    }
}