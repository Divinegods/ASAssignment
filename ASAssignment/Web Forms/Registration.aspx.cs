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
        static int count;
        static
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
            //Clearing pass textboxes
            lbl_checker.Text = "";
            lbl_fnamechecker.Text = "";
            lbl_lnamechecker.Text = "";
            lbl_dobchecker.Text = "";
            lbl_emailchecker.Text = "";
            lbl_pwdchecker.Text = "";
            lbl_cfmpwdchecker.Text = "";
            lbl_ccnochecker.Text = "";
            lbl_ccexpdatechecker.Text = "";
            lbl_cccvvchecker.Text = ""; 
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
            
            //Checking empty textboxes & Regex Testing
            if (tb_fname.Text.Length == 0)
            {
                lbl_fnamechecker.Text = "First Name cannot be empty!";
                lbl_fnamechecker.ForeColor = Color.Red;
            }

            else if (!Regex.IsMatch(tb_fname.Text, "^[A-Za-z0-9-_ ]*$"))
            {
                lbl_fnamechecker.Text = "Only Letters, Numbers, Underscores and Dashes Allowed!";
                lbl_fnamechecker.ForeColor = Color.Red;
            }

            if (tb_lname.Text.Length == 0)
            {
                lbl_lnamechecker.Text = "Last Name cannot be empty!";
                lbl_lnamechecker.ForeColor = Color.Red;
            }

            else if(!Regex.IsMatch(tb_lname.Text, "^[A-Za-z0-9-_ ]*$"))
            {
                lbl_lnamechecker.Text = "Only Letters, Numbers, Underscores and Dashes Allowed!";
                lbl_lnamechecker.ForeColor = Color.Red;
            }

            if (tb_dob.Text.Length == 0)
            {
                lbl_dobchecker.Text = "Please fill in Birth Date!";
                lbl_dobchecker.ForeColor = Color.Red;
            }

            else if (Convert.ToDateTime(tb_dob.Text) > DateTime.Now)
            {
                lbl_dobchecker.Text = "How come ur birthday is in the future huh?";
                lbl_dobchecker.ForeColor = Color.Red;
            }

            if (tb_email.Text.Length == 0)
            {
                lbl_emailchecker.Text = "Email cannot be empty!";
                lbl_emailchecker.ForeColor = Color.Red;
            }

            else if (!Regex.IsMatch(tb_email.Text, "^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$"))
            {
                lbl_emailchecker.Text = "Invalid Email!";
                lbl_emailchecker.ForeColor = Color.Red;
            }

            if (tb_password.Text.Length == 0)
            {
                lbl_pwdchecker.Text = "Password cannot be empty!";
                lbl_pwdchecker.ForeColor = Color.Red;
            }

            else if (scores < 5)
            {
                lbl_pwdchecker.Text = "Status :" + status;
                lbl_checker.Text = "Please make a more secure password!";
                lbl_checker.ForeColor = Color.Red;
                lbl_pwdchecker.ForeColor = Color.Red;
            }
            else if (scores == 5)
            {
                lbl_pwdchecker.Text = "Status :" + status;
                lbl_pwdchecker.ForeColor = Color.Green;
            }

            if (tb_cfmpassword.Text.Length == 0)
            {
                lbl_cfmpwdchecker.Text = "Confirm Password cannot be empty!";
                lbl_cfmpwdchecker.ForeColor = Color.Red;
            }

            if (tb_ccno.Text.Length == 0)
            {
                lbl_ccnochecker.Text = "Credit Card Number cannot be empty!";
                lbl_ccnochecker.ForeColor = Color.Red;
            }

            //This is only for VISA
            else if (!Regex.IsMatch(tb_ccno.Text, "^4[0-9]{12}(?:[0-9]{3})?$"))
            {
                lbl_ccnochecker.Text = "Invalid Credit Card Number!";
                lbl_ccnochecker.ForeColor = Color.Red;
            }

            if (tb_ccexpdate.Text.Length == 0)
            {
                lbl_ccexpdatechecker.Text = "Credit Card Expiration Date cannot be empty!";
                lbl_ccexpdatechecker.ForeColor = Color.Red;
            }

            else if (!Regex.IsMatch(tb_ccexpdate.Text, "^([2][0][2-3][0-9])[/.-]([0-1][0-9])[/.-]([0-3][0-9])$"))
            {
                lbl_ccexpdatechecker.Text = "Invalid Credit Card Expiry Date!";
                lbl_ccexpdatechecker.ForeColor = Color.Red;
            }

            if (tb_cccvv.Text.Length == 0)
            {
                lbl_cccvvchecker.Text = "Credit Card CVV cannot be empty!";
                lbl_cccvvchecker.ForeColor = Color.Red;
            }

            else if (!Regex.IsMatch(tb_cccvv.Text, "^[0-9]{3,4}$"))
            {
                lbl_cccvvchecker.Text = "Invalid Credit Card CVV!";
                lbl_cccvvchecker.ForeColor = Color.Red;
            }

            //Check if email exists
            if (retrieveEmail(tb_email.Text.Trim()))
            {
                lbl_emailchecker.Text = "Email already used";
                lbl_emailchecker.ForeColor = Color.Red;
            }

            //Check if can pass
            if(lbl_checker.Text == "" && 
                lbl_fnamechecker.Text == "" &&
                lbl_lnamechecker.Text == "" &&
                lbl_dobchecker.Text == "" &&
                lbl_emailchecker.Text == "" &&
                lbl_pwdchecker.Text == "Status :Excellent" && 
                lbl_cfmpwdchecker.Text == "" &&
                lbl_ccnochecker.Text == "" &&
                lbl_ccexpdatechecker.Text == "" &&
                lbl_cccvvchecker.Text == "")
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
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FName, @LName, @DOB, @Email, @PwdHash, @PwdSalt, @PwdMinAge, @PwdMaxAge,@CCNo, @CCExpDate, @CCCVV, @IV, @Key, @LockoutCount, @LockoutEnd)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FName", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LName", tb_lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(tb_dob.Text.Trim()));
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@PwdHash", finalHash);
                            cmd.Parameters.AddWithValue("@PwdSalt", salt);
                            cmd.Parameters.AddWithValue("@PwdMinAge", DateTime.Now.AddMinutes(5));
                            cmd.Parameters.AddWithValue("@PwdMaxAge", DateTime.Now.AddMinutes(15));
                            cmd.Parameters.AddWithValue("@CCNo", Convert.ToBase64String(encryptData(tb_ccno.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CCExpDate", Convert.ToDateTime(tb_ccexpdate.Text.Trim()));
                            cmd.Parameters.AddWithValue("@CCCVV", Convert.ToBase64String(encryptData(tb_cccvv.Text.Trim())));
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@LockoutCount", Convert.ToInt32(count));
                            cmd.Parameters.AddWithValue("@LockoutEnd", DBNull.Value);

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

        protected void btn_login_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }

        protected void btn_404_Click(object sender, EventArgs e)
        {
            Response.Redirect("RandomPage.aspx", false);
        }
    }
}