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

    public partial class Form2 : Form
    {

        private MySqlConnection conn;
        private String server;
        private String database;
        private String userId;
        private String password;
        private String connectionString;

        private String message;

        private Timer delaytimer;
        

        public Form2()
        {

            server = "10.0.2.67";
            database = "projektett";
            userId = "programmering";
            password = "qb5774ltG68m1oh094ZEK4Sz5U2755547i1q";
            connectionString = "SERVER=" + server + "; DATABASE=" + database + "; UID=" + userId + "; PASSWORD=" + password + "";

            conn = new MySqlConnection(connectionString);

            InitializeComponent();
        }

        public void listbox()
        {
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT message from messages";
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            da.Fill(dt);

            foreach(DataRow dr in dt.Rows)
            {
                richTextBox1.Text += dr["message"].ToString() +"\n";
                
            }

            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            message = textMessage.Text;

            try
            {
                if (OpenConnection())//om man har connection till databasen
                {
                    string queryMess = "INSERT INTO messages (message) VALUES ('" + message +"');";
                    
                    MySqlCommand messcmd = new MySqlCommand(queryMess, conn);
                    messcmd.ExecuteScalar();
                    
                    conn.Close();
                }
                else//inte får connection
                {
                    Debug.WriteLine("ingen conn");
                }
            }catch(Exception ex)
            {
                Debug.WriteLine("ingen connection till databas");
            }

            listbox();

            textMessage.Text = "";
            
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

        private void Form2_Load(object sender, EventArgs e)
        {
            listbox();
        }
    }
}
