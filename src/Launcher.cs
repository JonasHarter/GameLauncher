using System;
using Launcher.src;
using Xwt;
using Xwt.Drawing;

namespace Launcher
{
    class Launcher : Window
    {
        readonly VBox stage;

        public Launcher()
        {
            Title = Configuration.ApplicationName;
            Width = 1280;
            Height = 720;

            this.Content = stage = new VBox();
            stage.BackgroundColor = Color.FromName("green");
            var list = new TilePanel();
            Closed += HandleClose;
            stage.PackStart(list, true, true);
        }

        private void HandleClose(object sender, EventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }
    }
}
