using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MailTangy
{
    [Serializable()]
    public class Credentials:ISerializable
    {
        string serverURL = Properties.Settings.Default.ServerURL;
        public Credentials()
        {

        }
        public Credentials(SerializationInfo info, StreamingContext ctxt)
        {
            AccessToken = info.GetValue("AccessToken", typeof(string)).ToString();
            RefreshToken = info.GetValue("RefreshToken", typeof(string)).ToString();
            InstanceURL = info.GetValue("InstanceURL", typeof(string)).ToString();
            EmailID = info.GetValue("EmailID", typeof(string)).ToString();
        }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string InstanceURL { get; set; }
        public string EmailID { get; set; }

        public async Task<string> GetTokens(string accessCode)
        {
            string tokenURL = serverURL+"getTokenRefreshToken.token?type=code&code=" + 
                accessCode;
            string tokenResponse=await WebRequestHelper.getResponseAsync(tokenURL);
            var details = JObject.Parse(tokenResponse);
            AccessToken= details["access_token"].ToString();
            RefreshToken = details["refresh_token"].ToString();
            InstanceURL = details["instance_url"].ToString();
            //EmailID = Globals.ThisAddIn.LoggedinUserID;
            return "";
        }

        public async void GetTokenViaRefreshToken(string refreshToken)
        {
            string tokenURL = serverURL+"getTokenRefreshToken.token?type=re_tkn&re_tkn=" + refreshToken;
            string tokenResponse = await WebRequestHelper.getResponseAsync(tokenURL);
            var details = JObject.Parse(tokenResponse);
            try
            {
                AccessToken = details["access_token"].ToString();
                InstanceURL = details["instance_url"].ToString();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Failed while fetching accesstoken using refresh token. " + ex.Message);              
            }
             
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AccessToken", AccessToken);
            info.AddValue("RefreshToken", RefreshToken);
            info.AddValue("InstanceURL", InstanceURL);
            info.AddValue("EmailID", EmailID);
        }

        public void serializeCredentials(Credentials myCredentials)
        {
            var systemPath = System.Environment.
                             GetFolderPath(
                                 Environment.SpecialFolder.CommonApplicationData
                             );
            var complete = Path.Combine(systemPath, "Point5Nyble\\MailTangy");
            if (!Directory.Exists(complete))
            {
                Directory.CreateDirectory(complete);
            }
            Stream stream = File.Open(complete+ "\\Credentials.osl", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, myCredentials);
            stream.Close();
        }

        public Credentials deserializeCredentials()
        {
            var systemPath = System.Environment.
                             GetFolderPath(
                                 Environment.SpecialFolder.CommonApplicationData
                             );
            var complete = Path.Combine(systemPath, "Point5Nyble\\MailTangy\\Credentials.osl");
            if (File.Exists(complete))
            {
                Credentials myCredentials = null;

                Stream stream = File.Open(complete, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                myCredentials = (Credentials)bformatter.Deserialize(stream);
                stream.Close();
                return myCredentials;
            }
            else
                return null;
            
        }

        
    }
}
