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
    public partial class Registration1 : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyWeBB"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private int checkPassword(string password)
        {
            int score = 0;
            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                score++;
            }
            return score;
        }

        protected void btn_createacc_Click(object sender, EventArgs e)
        {
            //-----------------------------------------------------------ACCOUNT ENCRYPTION-----------------------------------------------------------
            //string pwd = get value from your Textbox
            string pwd = tb_password.Text.ToString().Trim(); ;

            //Generate random "salt"
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];

            //Fills array of bytes with a cryptographically strong sequence of random values.
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);

            SHA512Managed hashing = new SHA512Managed();

            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));

            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            finalHash = Convert.ToBase64String(hashWithSalt);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

            //-----------------------------------------------------------PASSWORD CHECKING-----------------------------------------------------------
            int scores = checkPassword(tb_password.Text);
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;

            }
            lbl_pwdchecker.Text = "Status :" + status;
            if (scores < 4)
            {
                lbl_checker.Text = "Please make a more secure password!";
                lbl_checker.ForeColor = Color.Red;
                lbl_pwdchecker.ForeColor = Color.Red;
            }
            else
            {
                lbl_pwdchecker.ForeColor = Color.Green;
            }

            System.Diagnostics.Debug.WriteLine("======================");
            System.Diagnostics.Debug.WriteLine(tb_fname.Text);

            //Checking empty textboxes
            if (tb_fname.Text == "")
            {
                lbl_fnamechecker.Text = "First Name cannot be empty";
                lbl_fnamechecker.ForeColor = Color.Red;
            }
            else
            {
                lbl_fnamechecker.Text = "";
            }

            if (tb_lname.Text.Length == 0)
            {
                lbl_lnamechecker.Text = "Last Name cannot be empty";
                lbl_lnamechecker.ForeColor = Color.Red;
            }
            else
            {
                lbl_lnamechecker.Text = "";
            }
            
            //Check if email exists
            if (retrieveEmail(tb_email.Text.Trim()))
            {
                lbl_emailchecker.Text = "Email already used";
                lbl_emailchecker.ForeColor = Color.Red;
            }
            else
            {
                lbl_emailchecker.Text = "";
            }

            if(lbl_checker.Text == "")
            {
                createAccount();
                Response.Redirect("Login.aspx", false);
            }
        }
        protected Boolean retrieveEmail(string email)
        {
            //string h = null;
            Boolean found = false;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Email FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["Email"] != null)
                        {
                            if (reader["Email"] != DBNull.Value)
                            {
                                found = true;
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
            return found;
        }
        public void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FName, @LName, @DOB, @Email, @PwdHash, @PwdSalt, @CCNo, @CCExpDate, @CCCVV, @IV, @Key)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FName", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LName", tb_lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@DOB", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@PwdHash", finalHash);
                            cmd.Parameters.AddWithValue("@PwdSalt", salt);
                            cmd.Parameters.AddWithValue("@CCNo", Convert.ToBase64String(encryptData(tb_ccno.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CCExpDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@CCCVV", Convert.ToBase64String(encryptData(tb_cccvv.Text.Trim())));
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
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
        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }
    }
}