using Launcher.src;
using Launcher.src.data;
using System;
using System.Collections.Generic;
using System.IO;
using Xwt;
using Xwt.Drawing;

namespace Launcher.src
{
    class OptionsDialog : Window
    {
        private static OptionsDialog activeDialog;

        internal static void ShowAddDialog()
        {
            if (activeDialog != null && !activeDialog.IsDisposed)
            {
                return;
            }
            activeDialog = new OptionsDialog();
            activeDialog.Show();
        }

        private ConfigurationData configurationData;
        private VBox VBox = new VBox();
        private VBox TagList = new VBox();
        private TextEntry NewTagEntry = new TextEntry();

        private List<string> Tags;

        private OptionsDialog()
        {
            Title = "Options";
            activeDialog = this;
            activeDialog.Closed += HideDialog;

            configurationData = ConfigurationData.Load();
            this.Tags = configurationData.Tags;

            this.Content = VBox;
            VBox.PackStart(new Label("Tags:"));
            UpdateTagList();
            VBox.PackStart(TagList);
            // AddTag
            HBox addTagBox = new HBox();
            addTagBox.PackStart(new Label("New Tag:"));
            addTagBox.PackStart(NewTagEntry);
            NewTagEntry.WidthRequest = 300;
            VBox.PackStart(addTagBox);
            Button addButton = new Button("Add");
            addButton.Clicked += AddTag;
            addTagBox.PackEnd(addButton);
            // Buttons
            Button saveButton = new Button("Save");
            saveButton.Clicked += SaveAndCloseDialog;
            VBox.PackEnd(saveButton);
        }

        private void UpdateTagList()
        {
            TagList.Clear();
            foreach (String tag in Tags)
            {
                var tagButton = new Button(tag);
                tagButton.Clicked += TagClicked;
                TagList.PackStart(tagButton);
            }
        }

        void TagClicked(object sender, EventArgs e)
        {
            Tags.Remove(((Button)sender).Label);
            UpdateTagList();
        }

        private void AddTag(object sender, EventArgs e)
        {
            Tags.Add(NewTagEntry.Text);
            UpdateTagList();
            NewTagEntry.Text = "";
        }

        private void SaveAndCloseDialog(object sender, EventArgs e)
        {
            HideDialog(null, null);
            configurationData.Save();
        }

        private void HideDialog(object sender, EventArgs e)
        {
            if (activeDialog == null && activeDialog.IsDisposed)
                return;
            activeDialog.Dispose();
            activeDialog = null;
        }
    }
}
