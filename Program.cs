using Launcher.src;
using System;
using System.IO;
using Xwt;

namespace Launcher
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Directory.CreateDirectory(Path.Combine(directory, Configuration.ApplicationName));
            Application.Initialize(ToolkitType.Wpf);
            var mainWindow = new Launcher();
            mainWindow.Show();
            Application.Run();
        }
    }
}
