/*
 * Created by SharpDevelop.
 * User: Kosyo
 * Date: 14.10.2014 г.
 * Time: 16:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Pickanum
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
        bool isRunning = false;
		bool inGame=false;
		bool connected = false;
		bool turn = false;
		byte turnCount = 0;
		Button last;
		int score, opScore;
		TcpClient client;
		
		void MainFormLoad(object sender, EventArgs e)
		{
			client = new TcpClient();
		}
		
		void undoLast()
		{
			last.Enabled=true;
			turn=true;
		}
		
		void updateScore()
		{
			this.Invoke((MethodInvoker)delegate {
			    label2.Text = score.ToString(); // runs on UI thread
			});
			
			this.Invoke((MethodInvoker)delegate {
			    label3.Text = opScore.ToString(); // runs on UI thread
			});
		}
		
		void endGame()
		{
			if(opScore==score)
			{
				MessageBox.Show("Draw!", "Game result");
			}
			else if (opScore>score) MessageBox.Show("Opponent wins!", "Game result");
			else MessageBox.Show("You win!", "Game result");
			button12.Enabled=true;
			
			score=0;
			opScore=0;
			updateScore();
			button1.Enabled=true;
			button2.Enabled=true;
			button3.Enabled=true;
			button4.Enabled=true;
			button5.Enabled=true;
			button6.Enabled=true;
			button7.Enabled=true;
			button8.Enabled=true;
			button9.Enabled=true;
			button10.Enabled=true;
		}
		
		void listen()
		{
			byte[] rb = new Byte[1];
			NetworkStream stream = client.GetStream();
			while(isRunning)
			{
                try
                {
                    if (stream.DataAvailable)
                    {
                        stream.Read(rb, 0, 1);
                        if (rb[0] == 0xFF)//Старт на игра
                        {
							this.Invoke((MethodInvoker)delegate {
							    statusLablel.Text = "Game is on"; // runs on UI thread
							});
                            //MessageBox.Show("Game is on. ");
                            inGame = true;
                            turn = true;
                            turnCount = 10;
                            this.Invoke((MethodInvoker)delegate {
                            button12.Enabled=false;
                            });
                        }
                        else
                        {
                            if (rb[0] == 0x40)//Невалиден ход
                            {
                                MessageBox.Show("Invalid move!", "Error");
                                undoLast();
                            }
                            else//Точки
                            {
                                byte sc = (byte)(rb[0] & 0x3F);
                                if ((rb[0] & 0x80) != 0) score += sc;
                                else opScore += sc;
                                updateScore();
                                //MessageBox.Show(score.ToString());
                                turnCount--;
                                if (turnCount > 0) turn = true;
                                else
                                {
                                	endGame();
                                }
                            }
                        }
                    }
                }
                catch (ThreadAbortException ex) { 
                    
                }
				Thread.Sleep(25);
			}
		}
		
		void sendNumber(byte num)
		{
			byte[] sb = new Byte[1];
			sb[0]=num;
			NetworkStream ns = client.GetStream();
			ns.Write(sb, 0, 1);
			ns.Flush();
		}
		
		void Label2Click(object sender, EventArgs e)
		{
			
		}
		
		void Button11Click(object sender, EventArgs e)
		{
			if(!connected)
			{
				IPAddress ip = IPAddress.Parse(textBox1.Text);
	            int port = 777;
				try {
					client.Connect(ip, port);
				} catch (SocketException se) {
					
					MessageBox.Show(se.Message, "Socket error");
					return;
				}
	            
	            t = new Thread(listen);
	            t.Start();
	            isRunning = true;
	            button11.Text="Disconnect";
	            connected=true;
	            button12.Enabled=true;
			}
			else
			{
				client.Close();
				button11.Text="Connect";
				connected=false;
				isRunning=false;
				t.Abort();
				button12.Enabled=false;
			}			
		}
		
        Thread t;
        
		void Button1Click(object sender, EventArgs e)
		{
			if(!turn) return;
			sendNumber(1);
			button1.Enabled=false;
			turn=false;
			last=button1;
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			if(!turn) return;
			sendNumber(2);
			button2.Enabled=false;
			turn=false;
			last=button2;
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			if(!turn) return;
			sendNumber(3);
			button3.Enabled=false;
			turn=false;
			last=button3;
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			if(!turn) return;
			sendNumber(4);
			button4.Enabled=false;
			turn=false;
			last=button4;
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			if(!turn) return;
			sendNumber(5);
			button5.Enabled=false;
			turn=false;
			last=button5;
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			if(!turn) return;
			sendNumber(6);
			button6.Enabled=false;
			turn=false;
			last=button6;
		}
		
		void Button7Click(object sender, EventArgs e)
		{
			if(!turn) return;
			sendNumber(7);
			button7.Enabled=false;
			turn=false;
			last=button7;
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			if(!turn) return;
			sendNumber(8);
			button8.Enabled=false;
			turn=false;
			last=button8;
		}
		
		void Button9Click(object sender, EventArgs e)
		{
			if(!turn) return;
			sendNumber(9);
			button9.Enabled=false;
			turn=false;
			last=button9;
		}
		
		void Button10Click(object sender, EventArgs e)
		{
			if(!turn) return;
			sendNumber(10);
			button10.Enabled=false;
			turn=false;
			last=button10;
		}

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //client.GetStream().Dispose();
            isRunning = false;
           // t.Suspend();
            if (t != null)
            {
                t.Abort();
            }
        }
		
		void Button12Click(object sender, EventArgs e)
		{
			if(connected) sendNumber(255);
		}
	}
}
