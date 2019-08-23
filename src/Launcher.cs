using System;
using Launcher.src;
using Launcher.src.data;
using Xwt;

namespace Launcher
{
    class Launcher : Window
    {
        public Launcher()
        {
            Title = Configuration.ApplicationName;
            Width = 1500;
            Height = 720;

            Closed += HandleClose;

            Content = new FilterableGameTilesTile();
        }

        private void HandleClose(object sender, EventArgs e)
        {
            GameData.getInstance().Save();
            Dispose();
            Application.Exit();
        }
    }
}
