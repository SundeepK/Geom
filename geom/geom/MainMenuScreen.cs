

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using System.IO;

using System.Threading;
#endregion

namespace geom
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization
        Game game;
        public string musicFileLocation;
        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen(Game game)
            : base("Main Menu")
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");
            MenuEntry fileLocation = new MenuEntry("Pop file browser");
            MenuEntry fileBrowser = new MenuEntry("In game file browser");
           this.game = game;
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            fileLocation.Selected += SetFileLocation;
            fileBrowser.Selected += displayFileBrowser;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(fileLocation);
            MenuEntries.Add(fileBrowser);
            MenuEntries.Add(exitMenuEntry);
        }


        #endregion

        #region Handle Input

        void displayFileBrowser(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new FileBrowserScreen(game));
        }

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            if (!(musicFileLocation == null))
            {

                LoadingScreen.Load(ScreenManager, false,
                                   new GameplayScreen(game, musicFileLocation));
                //ScreenManager.AddScreen(new GameplayScreen(game));
            }
            else
            {

                const string message = "Please select WAV audio file. Press Enter to exit game or Esc to return";

                MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

                confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

                ScreenManager.AddScreen(confirmExitMessageBox);
            }


            //LoadingScreen.Load(ScreenManager, false,
            //                   new GameplayScreen(game));
            
        }



        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel()
        {
            const string message = "Are you sure you want to exit this sample?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion

        void SetFileLocation(object sender, EventArgs e)
         { 
           
             var t = new Thread((ThreadStart)(() =>
             {
                 OpenFileDialog fileDialog = new OpenFileDialog();

                 fileDialog.InitialDirectory = Environment.CurrentDirectory;
                 fileDialog.Title = "Load Music file";

                 if (fileDialog.ShowDialog() == DialogResult.OK)
                 {
                     musicFileLocation = fileDialog.FileName;
                     
                     return;
                 }

                 if (fileDialog.ShowDialog() == DialogResult.Cancel)
                     return;          
             }));

             t.SetApartmentState(ApartmentState.STA);
             t.Start();
             t.Join();

             Console.WriteLine(musicFileLocation);                                
        }

       private void loadContentImage(string stream)
        {
            FileStream fs = new FileStream(stream, FileMode.Open);

            fs.Close();
        }

    }
}
