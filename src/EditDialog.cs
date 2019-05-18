using Launcher.src;
using Launcher.src.data;
using System;
using System.IO;
using Xwt;
using Xwt.Drawing;

namespace Launcher.src
{
    class EditDialog : Window
    {
        private static EditDialog activeDialog;

        internal static void ShowAddDialog()
        {
            ShowEditDialog(null);
        }

        internal static void ShowEditDialog(Game game)
        {
            if (activeDialog != null && !activeDialog.IsDisposed)
            {
                return;
            }
            activeDialog = new EditDialog(game);
            activeDialog.Show();
        }

        private Game gameToEdit;
        private VBox VBox = new VBox();
        private TextEntry NameEntry = new TextEntry();
        private TextEntry CommandEntry = new TextEntry();
        private ImageView ImageShow = new ImageView();
        private FileSelector ImageSelector = new FileSelector();

        private EditDialog(Game game)
        {
            if (game == null)
            {
                Title = "Add";
            }
            else
            { 
                Title = "Edit";
                gameToEdit = game;
            }
            activeDialog = this;
            activeDialog.Closed += HideEditDialog;

            this.Content = VBox;
            // Name
            HBox nameBox = new HBox();
            nameBox.PackStart(new Label("Name:"));
            nameBox.PackStart(NameEntry);
            NameEntry.WidthRequest = 300;
            VBox.PackStart(nameBox);
            // Command
            HBox commandBox = new HBox();
            commandBox.PackStart(new Label("Command:"));
            commandBox.PackStart(CommandEntry);
            CommandEntry.WidthRequest = 300;
            VBox.PackStart(commandBox);
            // Image
            HBox imageBox = new HBox();
            imageBox.PackStart(new Label("Image:"));
            ImageShow.WidthRequest = 460;
            ImageShow.HeightRequest = 215;
            imageBox.PackStart(ImageShow);
            VBox.PackStart(imageBox);
            // Image select
            ImageSelector = new FileSelector();
            ImageSelector.Filters.Add(new FileDialogFilter("Image files", "*.bmp;*.jpg;*.jpeg;*.png;"));
            ImageSelector.FileChanged += FileChanged;
            VBox.PackStart(ImageSelector);
            // Tags
            VBox tagList = new VBox();
            tagList.PackStart(new Label("Tags:"));
            foreach (String tag in ConfigurationData.Load().Tags)
            {
                var check = new CheckBox(tag);
                if (gameToEdit != null && gameToEdit.tags.Contains(tag))
                    check.State = CheckBoxState.On;
                check.Toggled += CheckBoxClicked;
                tagList.PackStart(check);
            }
            VBox.PackStart(tagList);
            // Buttons
            Button saveButton = new Button("Save");
            saveButton.Clicked += SaveAndCloseDialog;
            VBox.PackEnd(saveButton);

            if (gameToEdit != null)
            {
                NameEntry.Text = game.name;
                CommandEntry.Text = game.command;
                ImageShow.Image = game.GetImage();
            }
            else
                gameToEdit = new Game();
        }

        private void CheckBoxClicked(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if(checkBox.State == CheckBoxState.On)
            {
                gameToEdit.tags.Add(checkBox.Label.ToString());
            }
            else
            {
                gameToEdit.tags.Remove(checkBox.Label.ToString());
            }
        }

        private void FileChanged(object sender, EventArgs e)
        {
            try
            {
                String path = ImageSelector.FileName;
                using (MemoryStream ms = new MemoryStream())
                {
                    System.Drawing.Image.FromFile(path).Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ImageShow.Image = Image.FromStream(ms);
                }
            }
            catch { }
        }

        private void SaveAndCloseDialog(object sender, EventArgs e)
        {
            if (GameData.getInstance().List.Contains(gameToEdit))
                GameData.getInstance().List.Remove(gameToEdit);
            gameToEdit.name = NameEntry.Text;
            gameToEdit.command = CommandEntry.Text;
            if (ImageSelector.FileName.Length == 0)
                gameToEdit.image = gameToEdit.image;
            else
                gameToEdit.SetImage(ImageSelector.FileName);
            HideEditDialog(null, null);
            GameData.getInstance().List.Add(gameToEdit);
            GameData.getInstance().Save();
        }

        private void HideEditDialog(object sender, EventArgs e)
        {
            if (activeDialog == null && activeDialog.IsDisposed)
                return;
            activeDialog.Dispose();
            activeDialog = null;
        }
    }
}
