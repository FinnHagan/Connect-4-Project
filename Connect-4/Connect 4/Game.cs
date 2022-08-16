using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect_4
{
    public partial class Game : Form
    {
        Button[] btn = new Button[7]; //Puts in 7 buttons to select on the grid
        PictureBox[,] P;
        char currentTurn = 'R';
        int height = 6, width = 7;
        public static bool computer; //If true, computer takes turn
        
        public Game()
        {  
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            createGrid();   //Makes a blank grid
        }

        public void createGrid()
        {
            this.BackgroundImage = Properties.Resources.wood;

            P = new PictureBox[height, width];
            //Creates grid
            int left, top = 10; //10px from the top
            for (int i = 0; i < height; i++)
            {
                left = 159; //159px from the left
                for (int j = 0; j < width; j++)
                {
                    P[i, j] = new PictureBox();
                    P[i, j].BackColor = Color.White;
                    P[i, j].Location = new Point(left, top);
                    P[i, j].Size = new Size(60, 60);
                    left += 62;
                    Controls.Add(P[i, j]);
                }
                top += 62; //62px from the top
            }

            //Displays buttons in a straight line
            for (int x = 0; x < btn.Length; x++)
            { 
                btn[x] = new Button();
                btn[x].SetBounds(159 + (62 * x), 400, 62, 62);
                btn[x].Click += new EventHandler(this.btnEvent_Click);
                btn[x].Text = Convert.ToString(x + 1);
                Controls.Add(btn[x]);
            }

            //Menu button
            Button menuBtn = new Button();
            menuBtn.SetBounds(250, 506, 250, 100);
            menuBtn.Text = "Main Menu";
            menuBtn.Font = new Font("Futura Bold", 18);
            menuBtn.Click += new EventHandler(this.menu_btnEvent_Click);
            Controls.Add(menuBtn);
        }

        //Hides the connect 4 form, creates instance of menu form, runs menu and closes current form.
        private void menu_btnEvent_Click(object sender, EventArgs e)
        {
            this.Hide();
            Menu menu = new Menu();
            menu.ShowDialog();
            this.Close();
        }

        //Async function so computer can wait for its turn
        private async void btnEvent_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < btn.Length; i++)
            {
                btn[i].Enabled = false; //Disable all buttons
            }

            int x = Convert.ToInt32(((Button)sender).Text); //Used for which column
            int h = this.findHeight(x-1);   //Finds the next available square for piece

            this.makeMove(h, x);
            await Task.Delay(1500);

            //If the computer is playing then take its turn
            if (computer)
            {
                Random rng = new Random();
                x = rng.Next(1,8);
                h = this.findHeight(x-1);
                this.makeMove(h, x);
            }

            for (int i = 0; i < btn.Length; i++)
            {
                btn[i].Enabled = true;  //Re-enable all buttons
            }
        }

        //makes the players move with animation, async so that animation is visible
        private async void makeMove(int h, int x)
        {
            DialogResult playAgain;
            System.Media.SoundPlayer gameOver = new System.Media.SoundPlayer("game_over.wav");
            if (h!=-1) //If no errors, then place
            {
                if (currentTurn == 'R')
                {                    
                    if (h>0) //If it isnt the very top piece
                    {
                        for (int i = 0; i < h; i++)
                        {
                            P[i, x - 1].BackColor = Color.Red; //Place red piece
                            await Task.Delay(100); //wait
                            if (i<h)
                            {
                                P[i, x - 1].BackColor = Color.White;//replace it with white
                                await Task.Delay(100); //wait, and will go again until it reaches the bottom
                            } 
                        }
                    }
                    P[h, x - 1].BackColor = Color.Red; //Place final piece
                }
                else
                {
                    if (h>0)
                    {
                        for (int i = 0; i < h; i++)
                        {
                            P[i, x - 1].BackColor = Color.Yellow;
                            await Task.Delay(100);
                            if (i<h)
                            {
                                P[i, x - 1].BackColor = Color.White;
                                await Task.Delay(100);
                            }
                        }
                    }
                    P[h, x - 1].BackColor = Color.Yellow;
                }
                //If winning move returns true then display dialog box and sound effect
                if (this.winningMove())
                {
                    gameOver.Play();
                    playAgain = MessageBox.Show("Player " + currentTurn + " Wins, Return to menu and play again?", "GAME OVER", MessageBoxButtons.YesNo);
                    if (playAgain == DialogResult.Yes)
                    {
                        this.resetGrid();   //Resets grid to play again

                    }
                    if (playAgain == DialogResult.No)
                    {
                        this.Close();   //Closes application
                    }
                }
                trackTurn();    //Alternates turn                
            }
        }

        private int findHeight(int w)
        {
            int h;
            for (int i = 5; i >= 0; i--)//Counts down from top of picturebox array (visually the bottom)
            {
                if (P[i, w].BackColor == Color.White) //If backcolor is blank then return h
                {
                    h = i;
                    return h; 
                }
            }
            return -1; //If no available space then return -1
        }
        //Function for if the turn is red then make it yellow and vice versa.
        private void trackTurn()
        {
            
            if (currentTurn=='R')
            {
                currentTurn = 'Y';
            }
            else
            {
                currentTurn = 'R';
            }
        }

        //Calculates winning move by checking rows, columns, pos and neg diagonals.
        private bool winningMove()
        {
            DialogResult playAgain;
            System.Media.SoundPlayer gameOver = new System.Media.SoundPlayer("game_over.wav");
            PictureBox temp = new PictureBox(); //Temp picturebox to compare to current tiles backcolor
            int winLength = 4; //Variable used for checking diagonals
            int drawCounter = 0;

            //Changes temp backcolor to current turns color for comparison later on
            if (currentTurn=='R')
            {
                temp.BackColor = Color.Red;
            }
            else
            {
                temp.BackColor = Color.Yellow;
            }

            //Check Rows
            for (int j = 0; j < width - 3; j++)
            {
                for (int i = 0; i < height; i++)
                {  
                    //If the current, and 3 other pieces to the right have the same backcolor as temp then its a four in a row
                    if (P[i, j].BackColor == temp.BackColor && P[i, j + 1].BackColor == temp.BackColor && P[i, j + 2].BackColor == temp.BackColor && P[i, j + 3].BackColor == temp.BackColor)
                    {
                        //Changes backcolor to winning tiles to blue                    
                        P[i, j].BackColor = Color.Blue;
                        P[i, j + 1].BackColor = Color.Blue;
                        P[i, j + 2].BackColor = Color.Blue;
                        P[i, j + 3].BackColor = Color.Blue;
                        return true;
                    }
                }
            }

            //Check Columns
            for (int i = 0; i < height - 3; i++)
            {
                for (int j = 0; j < width; j++)
                {   //If the current, and 3 other pieces to the right have the same backcolor as temp then its a four in a row
                    if (P[i, j].BackColor == temp.BackColor && P[i + 1, j].BackColor == temp.BackColor && P[i + 2, j].BackColor == temp.BackColor && P[i + 3, j].BackColor == temp.BackColor)
                    {
                        //Changes backcolor to winning tiles to blue
                        P[i, j].BackColor = Color.Blue;
                        P[i + 1, j].BackColor = Color.Blue;
                        P[i + 2, j].BackColor = Color.Blue;
                        P[i + 3, j].BackColor = Color.Blue;
                        return true;
                    }
                }
            }
            
            //posiive slope
            for (int j = 0; j < width - winLength + 1; j++)
            {
                for (int i = 0; i < height - winLength + 1; i++)
                {   //If the current piece is not white and the 3 diagonally positive from it are the same color then its a four in a row
                    if (!P[i, j].BackColor.Equals(Color.White) && P[i + 1, j + 1].BackColor == temp.BackColor && P[i + 2, j + 2].BackColor == temp.BackColor && P[i + 3, j + 3].BackColor == temp.BackColor)
                    {
                        P[i, j].BackColor = Color.Blue;
                        P[i + 1, j + 1].BackColor = Color.Blue;
                        P[i + 2, j + 2].BackColor = Color.Blue;
                        P[i + 3, j + 3].BackColor = Color.Blue;
                        return true;
                    }
                }
            }
            //negative slope
            for (int j = winLength - 1; j < width; j++)
            {
                for (int i = 0; i < height - winLength + 1; i++)
                {   //If the current piece is not white and the 3 diagonally negative from it are the same color then its a four in a row
                    if (!P[i, j].BackColor.Equals(Color.White) && P[i + 1, j - 1].BackColor == temp.BackColor && P[i + 2, j - 2].BackColor == temp.BackColor && P[i + 3, j - 3].BackColor == temp.BackColor)
                    {
                        P[i, j].BackColor = Color.Blue;
                        P[i + 1, j - 1].BackColor = Color.Blue;
                        P[i + 2, j - 2].BackColor = Color.Blue;
                        P[i + 3, j - 3].BackColor = Color.Blue;
                        return true;
                    }
                }
            }
            //Check for draw
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (P[i, j].BackColor == Color.Red || P[i, j].BackColor == Color.Yellow)
                        drawCounter++;
                    if (drawCounter == 42)  //No white boxes found
                    {
                        gameOver.Play();
                        playAgain = MessageBox.Show("No winner, this game is a DRAW, Would you like to play again?", "GAME OVER", MessageBoxButtons.YesNo);
                        if (playAgain == DialogResult.Yes)
                        {
                            this.resetGrid();   //Resets grid to play again
                        }
                        if (playAgain == DialogResult.No)
                        {
                            this.Close();   //Closes application
                        }
                    }

                }
            }

            //Return false if no win condition was met
            return false;
        }


        private void resetGrid()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    P[i, j].BackColor = Color.White;
                }
            }
        }
    }
}
