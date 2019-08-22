using Launcher.src.data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xwt;
using Xwt.Drawing;

namespace Launcher.src
{
    // Holds GameTiles in horizontal lines. and scolls downward
    class TilePanel : VBox
    {
        private  GameData data;
        private List<GameTile> list = new List<GameTile>();
        private Table table;
        private Menu menu;
        private int currentColumns = 0;

        public TilePanel()
        {
            data = GameData.getInstance();
            ListChanged(null, null);
            ReorderItems(null, null);
            BoundsChanged += ReorderItems;
            data.List.CollectionChanged += ListChanged;

            menu = new Menu();
            var add = new MenuItem("Add");
            add.Clicked += HandleAdd;
            menu.Items.Add(add);
            var options = new MenuItem("Options");
            options.Clicked += HandleOptions;
            menu.Items.Add(options);

        }

        private void ListChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            list.Clear();
            List<Game> tempList = new List<Game>();
            foreach (Game game in data.List)
            {
                tempList.Add(game);
            }
            tempList.Sort((tile1, tile2) => String.Compare(tile1.name, tile2.name));
            foreach (Game game in tempList)
            {
                GameTile gt = new GameTile(game);
                list.Add(gt);
            }
            ReorderItems(null, null);
        }

        void HandleOptions(object sender, EventArgs e)
        {
            OptionsDialog.ShowAddDialog();
        }

        void HandleAdd(object sender, EventArgs e)
        {
            EditDialog.ShowAddDialog();
        }

        public void Add(List<Game> items)
        {
            foreach (Game game in items)
            {
                GameTile gt = new GameTile(game);
                list.Add(gt);
                data.List.Add(game);
            }
            ReorderItems(null, null);
            data.Save();
        }

        private void ReorderItems(object sender, EventArgs e)
        {
            int columns = Convert.ToInt32(Math.Floor((Size.Width - 50) / 460));
            if (currentColumns == columns)
                return;
            currentColumns = columns;
            Clear();
            if (table != null)
                table.Clear();
            table = new Table();
            table.ButtonReleased += HandleButtonReleased;
            table.BackgroundColor = Color.FromName("black");
            int columnCounter = 0;
            int rowCounter = 0;
            foreach (GameTile item in list)
            {
                table.Add(item, columnCounter, rowCounter);
                columnCounter++;
                if (columnCounter >= columns)
                {
                    columnCounter = 0;
                    rowCounter++;
                }
            }
            rowCounter++;
            table.HeightRequest = rowCounter * 215;
            ScrollView s = new ScrollView();
            s.Content = table;
            s.VerticalScrollPolicy = ScrollPolicy.Automatic;
            s.HorizontalScrollPolicy = ScrollPolicy.Never;
            PackStart(s, true, true);
        }

        void HandleButtonReleased(object sender, ButtonEventArgs e)
        {
            if (e.Button == PointerButton.Right)
            {
                e.Handled = true;
                menu.Popup();
            }
        }
    }
}
