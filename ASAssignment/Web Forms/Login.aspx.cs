using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASAssignment.Web_Forms
{
    public partial class Login1 : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyWeBB"].ConnectionString;

        /*public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LfTH0UaAAAAAPQLaqFY42QkbQJ1oYWEDAZcF01U &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        lbl_gScore.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }*/

        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_message.ForeColor = Color.Red;
        }
        protected void btn_login_Click(object sender, EventArgs e)
        {
            // if(ValidateCaptcha())
            //{
            lbl_message.Text = "";
            string pwd = tb_password.Text.ToString().Trim();
            string email = tb_email.Text.ToString().Trim();
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(email);
            string dbSalt = getDBSalt(email);
            int lockoutcount = Convert.ToInt32(getLockoutCount(email));
            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string emailHash = Convert.ToBase64String(hashWithSalt);

                    //Check for correct email and password
                    if (!emailHash.Equals(dbHash))
                    {
                        lbl_message.Visible = true;
                        //The reason why put both user id or password is invalid is to lessen chance of leak of user information check if this is correct
                        lbl_message.Text = "Userid or password is not valid. Please try again.";
                        tb_password.Text = "";
                        //Increase LockoutCount
                        int new_lockoutcount = lockoutcount + 1;
                        if (new_lockoutcount >= 3)
                        {
                            DateTime lockoutend = Convert.ToDateTime(getLockoutEnd(email));
                            //System.Diagnostics.Debug.WriteLine(lockoutend == Convert.ToDateTime("1 / 1 / 0001 12:00:00 AM"));
                            //System.Diagnostics.Debug.WriteLine(lockoutend > lockoutend);
                            //System.Diagnostics.Debug.WriteLine(DateTime.Now.AddMonths(0));
                            if (lockoutend != Convert.ToDateTime("1 / 1 / 0001 12:00:00 AM"))
                            {
                                if (DateTime.Now > lockoutend)
                                {
                                    updateLockoutCount(email, 0);
                                    updateLockoutEnd(email, 0);
                                }
                                else
                                {
                                    double timegap = Math.Round(lockoutend.Subtract(DateTime.Now).TotalMinutes, MidpointRounding.AwayFromZero);
                                    lbl_message.Text = "Account has been locked for the next " + timegap + " Minutes";
                                }
                            }
                            else
                            {
                                updateLockoutCount(email, new_lockoutcount);
                                updateLockoutEnd(email, 15);
                            }
                        }
                        else
                        {
                            updateLockoutCount(email, new_lockoutcount);
                        }
                    }

                    else if (DateTime.Now > Convert.ToDateTime(getPwdMaxAge(email)))
                    {
                        lbl_message.Visible = true;
                        lbl_message.Text = "Password expired please create a new passowrd!";
                    }

                    if (lbl_message.Text.Length == 0)
                    {
                        Session["Email"] = email;

                        string guid = Guid.NewGuid().ToString();
                        Session["AuthToken"] = guid;
                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                        Response.Redirect("HomePage.aspx", false);
                    }
                }
                else
                {
                    lbl_message.Visible = true;
                    lbl_message.Text = "Userid or password is not valid. Please try again.";
                    tb_email.Text = "";
                    tb_password.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            //}
        }

        protected void btn_forgotpassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgetPassword.aspx", false);
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

        protected string getLockoutCount(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select LockoutCount FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["LockoutCount"] != null)
                        {
                            if (reader["LockoutCount"] != DBNull.Value)
                            {
                                h = reader["LockoutCount"].ToString();
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

        protected string getLockoutEnd(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select LockoutEnd FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["LockoutEnd"] != null)
                        {
                            if (reader["LockoutEnd"] != DBNull.Value)
                            {
                                h = reader["LockoutEnd"].ToString();
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

        protected string getPwdMaxAge(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PwdMaxAge FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PwdMaxAge"] != null)
                        {
                            if (reader["PwdMaxAge"] != DBNull.Value)
                            {
                                h = reader["PwdMaxAge"].ToString();
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

        public void updateLockoutCount(string email, int lockoutcount)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE Account SET LockoutCount=@LockoutCount where Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@LockoutCount", lockoutcount);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void updateLockoutEnd(string email, int lockoutduration)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE Account SET LockoutEnd=@LockoutEnd where Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            if (lockoutduration == 0)
            {
                command.Parameters.AddWithValue("@LockoutEnd", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@LockoutEnd", DateTime.Now.AddMinutes(lockoutduration));
            }
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        protected void lbl_register_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx", false);
        }
    }
}