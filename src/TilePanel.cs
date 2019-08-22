using Launcher.src.data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xwt;
using Xwt.Drawing;
using System.Linq;

namespace Launcher.src
{
    // Holds GameTiles in horizontal lines. and scolls downward
    class TilePanel : VBox
    {
        private GameData data;
        private List<GameTile> list = new List<GameTile>();
        private Table table;
        private Menu menu;
        private List<String> selectedTags = new List<String>();
        private HBox box;
        private VBox sidebar;

        public TilePanel()
        {
            box = new HBox();
            sidebar = new VBox();
            foreach (String tag in ConfigurationData.Load().Tags)
            {
                ToggleButton button = new ToggleButton(tag);
                button.Clicked += new EventHandler(delegate (Object o, EventArgs a)
                {
                    if (button.Active)
                        selectedTags.Add(button.Label);
                    else
                        selectedTags.Remove(button.Label);
                    ListChanged(null, null);
                });
                sidebar.PackStart(button);
            }

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

        private void Button_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ListChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            list.Clear();
            List<Game> tempList = new List<Game>();
            foreach (Game game in data.List)
            {
                if (selectedTags.Count() > 0 && game.tags.Intersect(selectedTags).Count() == 0)
                    continue;
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
            s.HorizontalScrollPolicy = ScrollPolicy.Automatic;
            box.Clear();
            box.PackStart(sidebar);
            box.PackStart(s, true, true);
            PackStart(box, true, true);
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
