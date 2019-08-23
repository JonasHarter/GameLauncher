using Launcher.src.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;

namespace Launcher.src
{
    class GameTile : VBox
    {
        private static GameTile lastSelectedTile;
        private Game gameData;
        private Menu menu;

        public GameTile(Game data)
        {
            this.gameData = data;

            PackStart(new ImageView(data.GetImage()));

            menu = new Menu();
            var launch = new MenuItem("Launch");
            launch.Clicked += handleLaunch;
            menu.Items.Add(launch);
            var edit = new MenuItem("Edit");
            edit.Clicked += handleEdit;
            menu.Items.Add(edit);
            var delete = new MenuItem("Delete");
            delete.Clicked += handleDelete;
            menu.Items.Add(delete);
            ButtonReleased += HandleButtonReleased;
        }

        public Game getGameData()
        {
            return gameData;
        }

        void HandleButtonReleased(object sender, ButtonEventArgs e)
        {
            // e.MultiplePress == 2
            if (e.Button == PointerButton.Left && lastSelectedTile == this)
            {
                e.Handled = true;
                lastSelectedTile = null;
                handleLaunch(null, null);
            } else if (e.Button == PointerButton.Right)
            {
                e.Handled = true;
                menu.Popup();
            }
            lastSelectedTile = this;
        }

        void handleLaunch(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(gameData.command);
            }
            catch { }
        }

        void handleEdit(object sender, EventArgs e)
        {
            EditDialog.ShowEditDialog(gameData);
        }

        void handleDelete(object sender, EventArgs e)
        {
            GameData.getInstance().List.Remove(gameData);
            GameData.getInstance().Save();
        }
    }
}
