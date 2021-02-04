using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASAssignment.Web_Forms
{
    public partial class ForgetPassword : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyWeBB"].ConnectionString;
        static string finalHash;
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_message.ForeColor = Color.Red;
            lbl_emailchecker.ForeColor = Color.Red;
            lbl_oldpasswordchecker.ForeColor = Color.Red;
            lbl_newpasswordchecker.ForeColor = Color.Red;
            lbl_cfmnewpasswordchecker.ForeColor = Color.Red;
        }

        protected void lbl_reset_Click(object sender, EventArgs e)
        {
            lbl_message.Text = "";
            string pwd = tb_oldpassword.Text.ToString().Trim();
            string email = tb_email.Text.ToString().Trim();
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(email);
            string dbSalt = getDBSalt(email);
            if (tb_email.Text.Length == 0)
            {
                lbl_emailchecker.Text = "Email cannot be empty!";
            }

            else if (!Regex.IsMatch(tb_email.Text, "^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$"))
            {
                lbl_emailchecker.Text = "Invalid Email!";
            }

            if (tb_oldpassword.Text.Length == 0)
            {
                lbl_oldpasswordchecker.Text = "Old Password cannot be empty!";
            }

            if (tb_newpassword.Text.Length == 0)
            {
                lbl_newpasswordchecker.Text = "New Password cannot be empty!";
            }

            if (tb_cfmnewpassword.Text.Length == 0)
            {
                lbl_cfmnewpasswordchecker.Text = "Confirm New Password cannot be empty!";
            }

            if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
            {
                string pwdWithSalt = pwd + dbSalt;
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                string emailHash = Convert.ToBase64String(hashWithSalt);
                DateTime passwordminage = Convert.ToDateTime(getPwdMinAge(email));
                if (!emailHash.Equals(dbHash))
                {
                    lbl_message.Visible = true;
                    //The reason why put both user id or password is invalid is to lessen chance of leak of user information check if this is correct
                    lbl_message.Text = "Userid or password is not valid. Please try again.";
                    tb_oldpassword.Text = "";
                    tb_newpassword.Text = "";
                    tb_cfmnewpassword.Text = "";
                }
                else if (passwordminage > DateTime.Now)
                {
                    double timegap = Math.Round(passwordminage.Subtract(DateTime.Now).TotalMinutes, MidpointRounding.AwayFromZero);
                    lbl_message.Text = "Please wait " + timegap + " minutes before resetting your password";
                }
                //else if (checked password history)
                if (lbl_message.Text.Length == 0 &&
                    lbl_emailchecker.Text.Length == 0 &&
                    lbl_oldpasswordchecker.Text.Length == 0 &&
                    lbl_newpasswordchecker.Text.Length == 0 &&
                    lbl_cfmnewpasswordchecker.Text.Length == 0)
                {
                    string pwdWithSalts = tb_newpassword.Text + getDBSalt(email);
                    byte[] hashWithSalts = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalts));
                    finalHash = Convert.ToBase64String(hashWithSalts);
                    //UPDATE INTO HISTORY
                    //System.Diagnostics.Debug.WriteLine("=" + getPasswordHisory(email) + "=");
                    if (getPasswordHisory(email) == "" || getPasswordHisory(email).Length == 1)
                    {
                        //System.Diagnostics.Debug.WriteLine("Null went through!");
                        createPasswordHistory(email, getDBSalt(email), getDBHash(email));
                    }
                    else
                    {
                        int id1 = Convert.ToInt32(getPasswordHisory(email)[0]);
                        int id2 = Convert.ToInt32(getPasswordHisory(email)[1]);
                        string gethash = getDBHash(email);
                        if (id1 < id2)
                        {
                            updatePasswordHistory(id1, gethash);
                            updatePasswordHistoryCount(id1);
                        }

                        else if (id1 > id2)
                        {
                            updatePasswordHistory(id2, gethash);
                            updatePasswordHistoryCount(id2);
                        }
                    }
                    //UPDATE INTO NEW PASSOWRD
                    updatePwdHash(email, finalHash);
                }
            }
            else
            {
                lbl_message.Visible = true;
                lbl_message.Text = "Userid or password is not valid. Please try again.";
                tb_email.Text = "";
                tb_oldpassword.Text = "";
                tb_newpassword.Text = "";
                tb_cfmnewpassword.Text = "";
            }
        }

        protected void lbl_return_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }

        protected string getDBHash(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PwdHash FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PwdHash"] != null)
                        {
                            if (reader["PwdHash"] != DBNull.Value)
                            {
                                h = reader["PwdHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }

        protected string getDBSalt(string email)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PwdSalt FROM ACCOUNT WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PwdSalt"] != null)
                        {
                            if (reader["PwdSalt"] != DBNull.Value)
                            {
                                s = reader["PwdSalt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }

        protected string getPwdMinAge(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PwdMinAge FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PwdMinAge"] != null)
                        {
                            if (reader["PwdMinAge"] != DBNull.Value)
                            {
                                h = reader["PwdMinAge"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }

        public void updatePwdHash(string email, string pwdhash)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE Account SET PwdHash=@PwdHash where Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@PwdHash", pwdhash);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void createPasswordHistory(string email, string pwdsalt, string pwdhash)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO PasswordHistory VALUES(@Email, @PwdSalt, @PwdHash)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@PwdSalt", pwdsalt);
                            cmd.Parameters.AddWithValue("@PwdHash", pwdhash);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void updatePasswordHistory(int id, string pwdhash)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE PasswordHistory SET PwdHash=@PwdHash where Id=@Id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@PwdHash", pwdhash);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void updatePasswordHistoryCount(int id)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE PasswordHistory SET Id=@Id where Id=@Id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id + 2);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        protected string getPasswordHisory(string email)
        {
            string h = "";
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Id FROM PasswordHistory WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["Id"] != null)
                        {
                            if (reader["Id"] != DBNull.Value)
                            {
                                h += reader["Id"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }
    }
}