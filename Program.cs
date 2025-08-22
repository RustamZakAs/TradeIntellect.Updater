using System.Diagnostics;
using System.IO.Compression;
using System.Threading;

namespace TradeIntellect.Updater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string zipFile = args[0];
            string appDir = args[1];
            string exePath = args[2];

            // ждём, пока основное приложение закроется
            Thread.Sleep(2000);

            // распаковываем архив в папку приложения
            ZipFile.ExtractToDirectory(zipFile, appDir);

            // запускаем новое приложение
            Process.Start(exePath);
        }
    }
}