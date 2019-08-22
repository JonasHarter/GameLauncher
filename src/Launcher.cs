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
            Width = 1500;
            Height = 720;

            this.Content = stage = new VBox();
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
