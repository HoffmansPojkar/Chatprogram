using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chatProgram
{
    public partial class Form3 : Form
    {

        private MySqlConnection conn;
        private String server;
        private String database;
        private String userId;
        private String password;
        private String connectionString;

        public Form3()
        {
            server = "10.0.2.67";
            database = "projektett";
            userId = "programmering";
            password = "qb5774ltG68m1oh094ZEK4Sz5U2755547i1q";
            connectionString = "SERVER=" + server + "; DATABASE=" + database + "; UID=" + userId + "; PASSWORD=" + password + "";

            conn = new MySqlConnection(connectionString);

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //CONFIRM PASSWORD
            //IF THE CONFIRM-PASSWORDBOX MATCHES THE PASSWORD BOX, REGISTER ACCOUNT 
            if (tbPass.Text == textBox1.Text)
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
                catch (Exception ex)
                {

                }

                /*else
                {
                    MessageBox.Show("User "+ user +" has not been created yet!");
                }*/
            }
            //IF NOT SHOW THEY DON'T
            else
            {
                MessageBox.Show("Passwords do not match");
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            String user = textBox1.Text;
        }
        private void tbPass_TextChanged(object sender, EventArgs e)
        {

        }

        public bool Register(string user, string pass)//funktion för regestrering
        {
            string query = "SELECT * FROM logins WHERE username='" + user + "'";//ser så att det inte finns en användare med samma lösenord och användarnamn


            try
            {
                if (OpenConnection())//om man har connection till databasen
                {
                    //skickar ett komando och läser ifall den användaren finns
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())//om den hittar en exesterande användaren
                    {
                        MessageBox.Show("användarnamnet finns redan");
                        reader.Close();
                        conn.Close();
                        return true;
                    }
                    else// om den inte hittar användaren
                    {
                        reader.Close();

                        string queryReg = "INSERT INTO logins (username, passwords) VALUES ('" + user + "', '" + pass + "');";
                        MySqlCommand regcmd = new MySqlCommand(queryReg, conn);
                        MySqlDataReader readerReg = regcmd.ExecuteReader();

                        readerReg.Close();
                        reader.Close();
                        conn.Close();

                        MessageBox.Show("Du har registrerat " + user);

                        return false;
                    }
                }
                else
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

        private void exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


