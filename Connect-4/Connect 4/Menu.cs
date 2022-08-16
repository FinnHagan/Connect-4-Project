using System;
using System.Drawing;
using System.Windows.Forms;

namespace Connect_4
{
    public partial class Menu : Form
    {
        //Creating instances of each button
        Button startBtn;
        Button compBtn;
        Button rulesBtn;
        Button exitBtn;     
        System.Media.SoundPlayer bgSound = new System.Media.SoundPlayer("giorno.wav"); //Background music on menu screen

        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            createMenu(); //Makes the main menu display
        }

        public void createMenu()
        {
            //Initialising each button
            startBtn = new Button();
            compBtn = new Button();
            rulesBtn = new Button();
            exitBtn = new Button();

            //Setting the player vs player button
            startBtn.SetBounds(250,50,250,100);
            startBtn.Text = "START";
            startBtn.Font = new Font("Futura Bold", 18);
            startBtn.Click += new EventHandler(this.start_btnEvent_Click);

            //Setting the player vs computer button
            compBtn.SetBounds(250,200,250,100);
            compBtn.Text = "1v1 COMPUTER";
            compBtn.Font = new Font("Futura Bold", 18);
            compBtn.Click += new EventHandler(this.computer_btnEvent_Click);

            //Setting the rules button
            rulesBtn.SetBounds(250,350,250,100);
            rulesBtn.Text = "RULES";
            rulesBtn.Font = new Font("Futura Bold", 18);
            rulesBtn.Click += new EventHandler(this.rules_btnEvent_Click);

            //Setting the exit button
            exitBtn.SetBounds(250,500,250,100);
            exitBtn.Text = "EXIT";
            exitBtn.Font = new Font("Futura Bold", 18);
            exitBtn.Click += new EventHandler(this.exit_btnEvent_Click);
            
            bgSound.Play(); //Gets menu music to play

            this.BackgroundImage = Properties.Resources.bg; //Importing the background image

            //Adding in each button
            Controls.Add(startBtn);
            Controls.Add(compBtn);
            Controls.Add(rulesBtn);
            Controls.Add(exitBtn);
        }

        
        private void start_btnEvent_Click(object sender, EventArgs e)
        {
            Game mainGame = new Game(); //Creates instance of game form
            bgSound.Stop();             //Stops the menu music playing
            this.Hide();                //Hides the current form
            Game.computer = false;      //Means the computer won't play
            mainGame.ShowDialog();      //Runs game form
            this.Close();               //Closes menu form
        }

        private void computer_btnEvent_Click(object sender, EventArgs e)
        {
            Game mainGame = new Game();
            bgSound.Stop();
            this.Hide();            
            Game.computer = true; //Means the computer will play
            mainGame.ShowDialog();        
            this.Close();
        }

        private void rules_btnEvent_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Players must alternate turns, and only one disc can be dropped in each turn. \nOn your turn, drop one of your colored discs from the top into any of the seven slots. \nThe game ends when there is a 4 -in -a - row or a stalemate.\nThe starter of the previous game goes second on the next game.");
        }

        private void exit_btnEvent_Click(object sender, EventArgs e)
        {
            //Closes program
            this.Close();
        }  
    }
}
