using Launcher.src.data;
using Launcher.Widgets;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xwt;
using Xwt.Drawing;
using System.Linq;
using System.Collections.ObjectModel;

namespace Launcher.src
{
    class FilterableGameTilesTile: HBox
    {
        private ObservableCollection<Game> gamesData;
        private List<GameTile> allGameTilesList = new List<GameTile>();
        private Menu menu;
        private List<String> selectedTags = new List<String>();
        private TilePanel tilePanel = new TilePanel();
        private VBox sidebar = new VBox();

        public FilterableGameTilesTile()
        {
            // Sidebar
            foreach (String tag in ConfigurationData.Load().Tags)
            {
                ToggleButton button = new ToggleButton(tag);
                button.Clicked += new EventHandler(delegate (Object o, EventArgs a)
                {
                    if (button.Active)
                        selectedTags.Add(button.Label);
                    else
                        selectedTags.Remove(button.Label);
                    filterChanged();
                });
                sidebar.PackStart(button);
            }
            PackStart(sidebar);

            // Data
            List<Widget> visibleGameTilesList = new List<Widget>();
            gamesData = GameData.getInstance().List;
            foreach (Game game in gamesData)
            {
                var gameTile = new GameTile(game);
                allGameTilesList.Add(gameTile);
                visibleGameTilesList.Add(gameTile);
            }
            gamesData.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                ListChanged(null, null);
            };

            // Tilepanel
            tilePanel.AddWidgets(visibleGameTilesList);
            tilePanel.getTable().BackgroundColor = Color.FromName("black");
            PackStart(tilePanel, true, true);

            // Menus
            menu = new Menu();
            var add = new MenuItem("Add");
            add.Clicked += HandleAdd;
            menu.Items.Add(add);
            var options = new MenuItem("Options");
            options.Clicked += HandleOptions;
            menu.Items.Add(options);

        }

        private void filterChanged()
        {
            List<Widget> visibleGameTilesList = new List<Widget>();
            visibleGameTilesList.Clear();
            foreach (GameTile gameTile in allGameTilesList)
            {
                if (selectedTags.Count() > 0 && gameTile.getGameData().tags.Intersect(selectedTags).Count() != selectedTags.Count())
                    continue;
                visibleGameTilesList.Add(gameTile);
            }
            tilePanel.clearWidgets();
            tilePanel.AddWidgets(visibleGameTilesList);
        }

        private void ListChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            foreach (Game game in args.NewItems)
            {
                GameTile newGameTile = new GameTile(game);
                allGameTilesList.Add(newGameTile);
            }
            List<GameTile> removeGameTilesList = new List<GameTile>();
            foreach (Game game in args.OldItems)
            {
                foreach (GameTile gameTile in allGameTilesList)
                {
                    if(game.name == gameTile.getGameData().name)
                    {
                        removeGameTilesList.Add(gameTile);
                        break;
                    }
                }
            }
            foreach (GameTile gameTile in removeGameTilesList)
            {
                allGameTilesList.Remove(gameTile);
            }
            filterChanged();
        }

        void HandleButtonReleased(object sender, ButtonEventArgs e)
        {
            if (e.Button == PointerButton.Right)
            {
                e.Handled = true;
                menu.Popup();
            }
        }
        void HandleOptions(object sender, EventArgs e)
        {
            OptionsDialog.ShowAddDialog();
        }

        void HandleAdd(object sender, EventArgs e)
        {
            EditDialog.ShowAddDialog();
        }
    }
}
