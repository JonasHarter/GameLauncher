using System;
using System.Collections.Generic;
using Xwt;

namespace Launcher.Widgets
{
    // Holds Widgets in a table, fills horizontally, overflowing to the next line if the horizontal limit is reached
    public class TilePanel : VBox
    {
        private List<Widget> widgetList = new List<Widget>();
        private ScrollView scrollView = new ScrollView();
        private Table widgetTable = new Table();
        private readonly Double spacing = 5;

        public TilePanel()
        {
            widgetTable.DefaultRowSpacing = spacing;
            widgetTable.DefaultColumnSpacing = spacing;
            scrollView.Content = widgetTable;
            scrollView.VerticalScrollPolicy = ScrollPolicy.Automatic;
            scrollView.HorizontalScrollPolicy = ScrollPolicy.Automatic;
            PackStart(scrollView, true, true);
            BoundsChanged += delegate (object sender, EventArgs e)
            {
                reDraw();
            };
            reDraw();
        }

        public void AddWidgets(List<Widget> newWidgets)
        {
            widgetList.AddRange(newWidgets);
            reDraw();
        }

        public void AddWidget(Widget newWidget)
        {
            widgetList.Add(newWidget);
            reDraw();
        }

        public void clearWidgets()
        {
            widgetList.Clear();
            reDraw();
        }

        public Table getTable()
        {
            return widgetTable;
        }

        private void reDraw()
        {
            widgetTable.Clear();
            int columnCounter = 0;
            int rowCounter = 0;
            double rowSize = 0;
            foreach (Widget widget in widgetList)
            {
                rowSize += widget.Size.Width + spacing;
                if (rowSize >= scrollView.Size.Width)
                {
                    columnCounter = 0;
                    rowCounter++;
                    rowSize = 0;
                }
                widgetTable.Add(widget, columnCounter, rowCounter);
                columnCounter++;
            }
        }
    }
}
