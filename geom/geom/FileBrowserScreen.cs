using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TomShane.Neoforce.Controls;

namespace geom
{
    class FileBrowserScreen : GameScreen
    {



        GraphicsDeviceManager graphics;  
        List<string> subDirectories;
        List<FileInfo> musicFiles;
        int lastDirecPosition = 0;

        GroupPanel gp;
        GroupPanel gp2;
        List<string> directoryList;
        Manager manager; // GUI manager
        private Window window;  // Main window   

        Texture2D image; // Image we wanna display in the window
        Texture2D image2;

        string directory;
        string previousDirectory;
        bool musicFileSelected = false;
        bool isChanged = false;

        List<GroupPanel> gpList;
        Label selectedMusicFile;
        Label currentSelectedFile;
        Button backButton;
        Button selectAudio;
        Game game;

        public FileBrowserScreen(Game game)
        {
            this.game = game;
            graphics = GameServices.GetService<GraphicsDeviceManager>();
            // Setting up the shared skins directory

            manager = new Manager(game, graphics, "Green");
          
            manager.SkinDirectory = @"..\..\Skins\";
            manager.Initialize();
             
            gpList = new List<GroupPanel>();
            Initialize();
        
        }

        public  void Initialize()
        {

           
            base.Initialize();
            image = GameServices.GetService<ContentManager>().Load<Texture2D>(@"folder");
            image2 = GameServices.GetService<ContentManager>().Load<Texture2D>(@"music1");
            directoryList = new List<string>();
            musicFiles = new List<FileInfo>();

            // Creates and initializes window
            window = new Window(manager);
            window.Init();
            window.Width = 600;
            window.Height = 500;
            window.Center();
            window.Visible = true;
            window.CaptionVisible = false;
            window.Resizable = false;
            window.Movable = false;
            window.BorderVisible = false;
            window.AutoScroll = true;


            // Add window to manager processing
            manager.Add(window);
            gpList.Add(new GroupPanel(manager)); gpList.Add(new GroupPanel(manager));
            gp = gpList[0];
            gp.Text = "File Browser";
            gp.AutoScroll = true;
            gp.Width = window.Width;
            gp.Height = window.Height;
            gp.Left = 0;
            gp.Top = 0;
            gp.Parent = window;

            currentSelectedFile = new Label(manager);



            backButton = new Button(manager);
            backButton.Init();
            backButton.Top = 5;
            backButton.Text = "Back";
            backButton.Parent = gp;
            //backButton.Enabled = false;
            backButton.Click += new TomShane.Neoforce.Controls.EventHandler(EnterPreviousDirectory);

            selectAudio = new Button(manager);
            selectAudio.Init();
            selectAudio.Top = 5;
            selectAudio.Left = backButton.Width + 10;
            //selectAudio.Width = 150;
            selectAudio.Text = "Select";
            selectAudio.Parent = gp;
            selectAudio.Enabled = false;
            selectAudio.Click += new TomShane.Neoforce.Controls.EventHandler(StartGame);


            previousDirectory = @"C:\";
            directory = @"C:\";
            directoryList.Add(directory);
            subDirectories = new List<string>();
            var directories = Directory.EnumerateDirectories(directory);
            //System.Console.WriteLine(subDirectories.Count);
            foreach (string dir in directories)
            {
                subDirectories.Add((string)dir);
            }

            createBrowserControls();

            System.Console.WriteLine(subDirectories.Count);

            manager.Add(window);
        }

        void EnterDirectory(object sender, System.EventArgs e)
        {

            System.Console.WriteLine("fefes");
        }

        void MouseOverDirectory(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {

            if (sender is Label)
            {
                Label tb = (Label)sender;
                tb.TextColor = Color.DarkOrange;
                tb.Color = Color.Red;
            }

        }

        void MouseOffDirectory(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {

            if (sender is Label)
            {
                Label tb = (Label)sender;
                tb.TextColor = Color.White;
                tb.BackColor = Color.Red;
                tb.Refresh();

                tb.Invalidate();
            }

        }

        /// <summary>
        /// Event handler for back button to navigate to previous directory. Makes a simple check to ensure if current directory
        /// is not same as previous directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EnterPreviousDirectory(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {

            if (!(previousDirectory == directory))
            {

                directoryList.RemoveAt(directoryList.Count() - 1);
                directory = directoryList[directoryList.Count() - 1];
                previousDirectory = directoryList.Count() != 1 ? directoryList[directoryList.Count() - 2] : directoryList[0];

                createGroupPanlControl();

            }
        }

        void StartGame(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (!(currentSelectedFile == null))
            {

                LoadingScreen.Load(ScreenManager, false,
                                   new GameplayScreen(game, (string)currentSelectedFile.Tag));
                //ScreenManager.AddScreen(new GameplayScreen(game));
            }
        }

        /// <summary>
        /// Help method to delete old group panel and create new one to be drawn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createGroupPanlControl()
        {
            var directories = GetFiles(directory);
            subDirectories.Clear();
            musicFiles.Clear();
            lastDirecPosition = 0;
            foreach (string dir in directories)
            {
                subDirectories.Add((string)dir);
            }
            lastDirecPosition = subDirectories.Count();

            List<FileInfo> musicFilestemp = GetMusicFiles(directory, "*.mp3", "*.wav");
            foreach (FileInfo dir in musicFilestemp)
            {
                musicFiles.Add(dir);
                System.Console.WriteLine(dir);
            }


            isChanged = true;

            gpList.Add(new TomShane.Neoforce.Controls.GroupPanel(manager));

            gpList.RemoveAt(0);

            gp2 = gp;
            gp2.Dispose();
            gp = gpList[0];
            gp.Text = "File Browser";
            gp.AutoScroll = true;

            gp.Width = window.Width;
            gp.Height = window.Height;
            gp.Left = 0;
            gp.Top = 0;
            gp.Parent = window;
            gp.Add(backButton);
            gp.Add(selectAudio);

            //if (selectedMusicFile != null)
            //{
            //    selectedMusicFile.Left = 500;
            //    gp.Add(selectedMusicFile);
            //}


            if (subDirectories.Count() > 0 || musicFiles.Count() > 0)
            {

                createBrowserControls();
                createMusicControls();
            }
            else
            {
                Label noDirectories = new Label(manager);
                noDirectories.Text = "Music files";
                noDirectories.Top = 30;
                noDirectories.Parent = gp;
            }
            gp.Invalidate();

        }


        /// <summary>
        /// Event handler for on mouse press for labels and folder images. The current directory is passed in
        /// through the sender and is made the current directory. Adds the current direcotry to the list to 
        /// keep track of directories.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="direc"></param>
        void EnterDirectory(object sender, string direc)
        {

            System.Console.WriteLine("Preivous " + previousDirectory);
            directoryList.Add(direc);
            previousDirectory = directoryList.Count() != 1 ? directoryList[directoryList.Count() - 2] : directoryList[0];

            directory = directoryList[directoryList.Count() - 1];
            createGroupPanlControl();

            backButton.Invalidate();


        }



        public IEnumerable<string> GetFiles(string root)
        {
            Stack<string> pending = new Stack<string>();
            pending.Push(root);
            while (pending.Count < 4 && pending.Count != 0)
            {
                var path = pending.Pop();
                string[] next = null;
                try
                {
                    next = Directory.GetDirectories(path);

                }
                catch { }
                if (next != null && next.Length != 0)
                    foreach (var file in next) yield return file;
                try
                {
                    next = Directory.GetDirectories(path);
                    foreach (var subdir in next)
                    //pending.Push(subdir);
                    {
                        if (pending.Last() != subdir)
                            pending.Push(subdir);
                    }
                }
                catch { }
            }
        }


        public static IEnumerable<FileInfo> GetFileInfoFiles(string root, string fileExt)
        {
            Stack<string> pending = new Stack<string>();
            pending.Push(root);
            DirectoryInfo direcInfo = new DirectoryInfo(root);
            while (pending.Count < 2 && pending.Count != 0)
            {
                var path = pending.Pop();
                FileInfo[] next = null;
                try
                {
                    next = direcInfo.GetFiles(fileExt);

                }
                catch { }
                if (next != null && next.Length != 0)
                    foreach (var file in next) yield return file;
                try
                {
                    next = direcInfo.GetFiles(fileExt);
                    foreach (var subdir in next)
                    //pending.Push(subdir.FullName);
                    {
                        if (pending.Last() != subdir.FullName)
                            pending.Push(subdir.FullName);
                    }
                }
                catch { }
            }
        }

        private void createBrowserControls()
        {
            selectAudio.Enabled = false;
            musicFileSelected = false;
            selectedMusicFile = null;
            for (int i = 0; i < subDirectories.Count; i++)
            {


                ImageBox box = new ImageBox(manager);

                box.Init();
                box.DoubleClicks = true;
                box.Parent = gp;
                box.Left = 0;

                box.Passive = false;
                box.Top = 0;
                box.Width = window.Width - 50;
                box.Focused = true;
                box.Height = 30;
                box.SetPosition(10, (30 + (i * 25)));
                lastDirecPosition = (30 + (i * 25));
                box.Image = image;
                box.Text = subDirectories[i];
                box.MousePress += (sender, args) => EnterDirectory(sender, box.Text);

                Label tx = new Label(manager);
                tx.Init();
                tx.Passive = false;
                string subD = (string)subDirectories[i];
                tx.Text = subD;
                tx.DoubleClicks = true;
                //tx.SetPosition(Graphics.GraphicsDevice.Viewport.Width / 2 - 150, (80 + (i * 25)));
                tx.Height = 20;
                tx.Width = window.Width - 50;
                tx.Color = Color.Red;
                tx.BackColor = Color.Red;
                //tx.DoubleClick += (sender, args) => EnterDirectory(sender, box.Text);
                //tx.MouseOver += new TomShane.Neoforce.Controls.MouseEventHandler(EnterDirectory);
                tx.Left = 25;
                tx.Top = 4;
                tx.Parent = box;
                tx.StayOnTop = true;

                tx.MousePress += (sender, args) => EnterDirectory(sender, box.Text);
                tx.MouseOver += (sender, args) => MouseOverDirectory(sender, args);
                tx.MouseOut += (sender, args) => MouseOffDirectory(sender, args);


            }

        }

        private void createMusicControls()
        {
            musicFileSelected = false;
            selectedMusicFile = null;
            selectAudio.Enabled = false;
            for (int i = 0; i < musicFiles.Count; i++)
            {

                ImageBox box = new ImageBox(manager);

                box.Init();
                box.DoubleClicks = true;
                box.Parent = gp;
                box.Left = 0;

                box.Passive = false;
                box.Top = 0;
                box.Width = window.Width - 50;
                box.Focused = true;
                box.Height = 30;
                box.SetPosition(10, (30 + ((lastDirecPosition) + i * 25 )));

                box.Image = image2;
                box.Text = musicFiles[i].Name;

                Label tx = new Label(manager);
                tx.Init();
                tx.Passive = false;
                string subD = musicFiles[i].Name;
                tx.Text = subD;
                tx.Tag = musicFiles[i].FullName;
                tx.DoubleClicks = true;

                tx.Height = 20;
                tx.Width = window.Width - 50;

                tx.Left = 25;
                tx.Top = 4;
                tx.Parent = box;
                tx.StayOnTop = true;

                tx.MousePress += new TomShane.Neoforce.Controls.MouseEventHandler(SelectAudioFile);

            }

        }

        void SelectAudioFile(object sender, TomShane.Neoforce.Controls.MouseEventArgs e)
        {
            Label tmpl = (Label)sender;

            if (musicFileSelected == false)
            {
                selectedMusicFile = tmpl;
                tmpl.TextColor = Color.Blue;
                musicFileSelected = true;
                selectAudio.Enabled = true;
                currentSelectedFile.Tag = tmpl.Tag;
                currentSelectedFile.Left = 350;
                currentSelectedFile.Width = 200;
                currentSelectedFile.Top = 10;
                currentSelectedFile.Text = "Selected file: " + selectedMusicFile.Text;
                currentSelectedFile.Parent = gp;


            }
            else if (musicFileSelected && (selectedMusicFile == tmpl))
            {
                selectedMusicFile = null;
                tmpl.TextColor = Color.White;
                musicFileSelected = false;
                selectAudio.Enabled = false;
                currentSelectedFile.Text = "No music file selected";
            }

        }
 

        public static List<FileInfo> GetMusicFiles(string direc, params string[] fileExtensions)
        {

            List<FileInfo> musicFiles = new List<FileInfo>();
            DirectoryInfo direcInfo = new DirectoryInfo(direc);
            foreach (string fileext in fileExtensions)
            {

                IEnumerable<FileInfo> musicFiles2 = GetFileInfoFiles(direc, fileext);
                foreach (FileInfo ss in musicFiles2)
                {
                    musicFiles.Add(ss);
                }
            }
            return musicFiles;

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                               bool coveredByOtherScreen)
        {
            manager.Update(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            ScreenManager.Game.ResetElapsedTime();
            
        }


        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            //ScreenManager.SpriteBatch
     
            manager.BeginDraw(gameTime);
            //manager.GraphicsDevice.Clear(Color.Black);
            manager.EndDraw();

        }

    }
}
