using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Next_Generation_School_System_by_Anton {
    public partial class Social_Network : Form {
        string Name_;
        string Surname;
        string Email;
        string Password;
        string School;
        string Class;
        string connection;

        bool isMine;
        public Social_Network(string name, string surname, string email, string pass, string school, string class_, string conn, bool _isMine) {

            Name_ = name;
            Surname = surname;
            Email = email;
            Password = pass;
            School = school;
            Class = class_;
            connection = conn;

            isMine = _isMine;

            InitializeComponent();
        }
        private void SaveDesc(object sender, MouseEventArgs e) {
            if (isMine)
            {
                var result = MessageBox.Show("Do you want to save changes?", "Answer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection sql = new SqlConnection(connection))
                    {
                        sql.Open();
                        SqlCommand command = new SqlCommand(@"UPDATE [NextGenSchoolSystem_Diary].[dbo].[Accounts] 
                    SET About = '" + richTextBox1.Text + @"' WHERE Classes = '" + Class + "' " +
                        "AND School = '" + School + "' AND UserSurName = '" + Surname + "' AND " +
                        "UserName = '" + Name_ + "' AND UserEmail = '" + Email + "' AND UserPassword = '" + Password + "'", sql);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        private void SaveStatus(object sender, MouseEventArgs e) {
            if (isMine)
            {
                var result = MessageBox.Show("Do you want to save changes?", "Answer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection sql = new SqlConnection(connection))
                    {
                        sql.Open();
                        SqlCommand command = new SqlCommand(@"UPDATE [NextGenSchoolSystem_Diary].[dbo].[Accounts] 
                    SET Status = '" + richTextBox2.Text + @"' WHERE Classes = '" + Class + "' " +
                        "AND School = '" + School + "' AND UserSurName = '" + Surname + "' AND " +
                        "UserName = '" + Name_ + "' AND UserEmail = '" + Email + "' AND UserPassword = '" + Password + "'", sql);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        private void ChangeImage(object sender, EventArgs e) {

        }
        private void ChangeStatus(object sender, EventArgs e) {

        }
        private void Social_Network_Load(object sender, EventArgs e) {
            //  MessageBox.Show(isMine.ToString());
            label3.Text = Name_ + " " + Surname;
            label4.Text = School;
            if (isMine)
            {
                richTextBox1.ReadOnly = true;
                richTextBox2.ReadOnly = true;
            }
            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [NextGenSchoolSystem_Diary].[dbo].[Accounts]", sql);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(0).Equals(Name_) &&
                            reader.GetString(1).Equals(Surname) &&
                            reader.GetString(2).Equals(Password) &&
                            reader.GetString(3).Equals(Email) &&
                            // here was a class :D
                            reader.GetString(7).Equals(School))
                        {

                            byte[] buffer = (byte[])reader.GetValue(9);
                            MemoryStream stream = new MemoryStream(buffer);
                            picture.SizeMode = PictureBoxSizeMode.StretchImage;
                            picture.Image = Image.FromStream(stream);

                            richTextBox1.Text = reader.GetString(10);
                            richTextBox2.Text = reader.GetString(11);

                            if (reader.GetString(12).Equals("0"))
                            {
                                panel6.BackColor = Color.Black;
                            } else if (reader.GetString(12).Equals("1"))
                            {
                                panel6.BackColor = Color.Blue;
                            }
                        }
                    }
                }
            }
        }
    }
}
