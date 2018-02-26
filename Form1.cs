using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chatProgram
{
    public partial class Form1 : Form
    { 

        private MySqlConnection conn;
        private String server;
        private String database;
        private String userId;
        private String password;
        private String connectionString;


        public Form1()
        {
            server = "10.0.2.67";
            database = "projektett";
            userId = "programmering";
            password = "qb5774ltG68m1oh094ZEK4Sz5U2755547i1q";
            connectionString = "SERVER="+ server +"; DATABASE="+ database +"; UID=" +userId +"; PASSWORD="+ password +"";

            conn = new MySqlConnection(connectionString);

            InitializeComponent();
        }

        private void exitButton(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginButton_Click(object sender, EventArgs e)//login button
        {

            String user = tbUser.Text;//tar username från username textbox
            String pass = tbPass.Text;//tar password från password textbox
            if (IsLogin(user, pass))//Om funktionen IsLogin går igenom så händer detta
            {
                //öppnar chatProgrammet
                Form2 form2 = new Form2();
                this.Hide();
                form2.ShowDialog();
                this.Show();
            }

            /*else//om användaren inte hittas
            {
                MessageBox.Show("" + user + " does not exist!");
            }*/
            
        }

        private void button1_Click(object sender, EventArgs e)//register button
        {
            String user = tbUser.Text;
            String pass = tbPass.Text;
            try
            {
                if (Register(user, pass))
                {
                    MessageBox.Show("you have register " + user + "");
                }
            }
            catch(Exception ex)
            {
                
            }
            
            /*else
            {
                MessageBox.Show("User "+ user +" has not been created yet!");
            }*/
            
        }

        public bool IsLogin(string user, string pass)//funktion för login
        {
            //gör om användarnamnet och lösenordet till en string så att databasen förstår den
            string query = "SELECT * FROM logins WHERE username='" + user + "' AND passwords='" + pass + "'";

            try
            {
                if (OpenConnection())//om man har connection till databasen
                {
                    //skickar ett komando och läser ifall den användaren finns
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())//om den hittar användaren och både användarnamnet och lösenordet hittas
                        {
                            reader.Close();
                            conn.Close();
                            return true;
                        }
                    }
                    reader.Close();
                    conn.Close();
                    return false;
                }
                else//om den inte får conection
                {
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                return false;

            }
        }

        private void registerLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form3 formReg = new Form3();
            formReg.ShowDialog();
        }

        private bool OpenConnection()//funktion för att connection med databasen
        {
            try//försöker öppna en connection till databanen
            {
                conn.Open();
                return true;
            }
            catch (MySqlException ex)//om den inte for connection med databanen
            {
                switch (ex.Number)
                {
                    case 0:
                    MessageBox.Show("No connection to server");
                    break;

                    case 1045:
                        MessageBox.Show("Server username or password is incorrect");
                        break;
                }
                return false;
            }
        }
        
    }
}
