﻿using Launcher.src.data;
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
        private Game data;
        private Menu menu;

        public GameTile(Game data)
        {
            this.data = data;

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
            return data;
        }

        void HandleButtonReleased(object sender, ButtonEventArgs e)
        {
            // e.MultiplePress == 2
            if (e.Button == PointerButton.Left)
            {
                e.Handled = true;
                handleLaunch(null, null);
            } else if (e.Button == PointerButton.Right)
            {
                e.Handled = true;
                menu.Popup();
            } 
        }

        void handleLaunch(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(data.command);
            }
            catch { }
        }

        void handleEdit(object sender, EventArgs e)
        {
            EditDialog.ShowEditDialog(data);
        }

        void handleDelete(object sender, EventArgs e)
        {
            GameData.getInstance().List.Remove(data);
            GameData.getInstance().Save();
        }
    }
}
