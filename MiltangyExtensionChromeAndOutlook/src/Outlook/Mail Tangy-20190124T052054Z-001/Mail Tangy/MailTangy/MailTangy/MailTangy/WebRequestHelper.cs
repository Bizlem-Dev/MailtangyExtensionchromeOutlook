using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MailTangy
{
    static class WebRequestHelper
    {

        public static async Task<string> getResponseAsync(string url)
        {
            string html = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization",Properties.Settings.Default.Authorization);
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseString;
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        responseString = reader.ReadToEnd();
                    }
                }
                return responseString;
            }
            else
                return "";
        }

        public static string getResponse(string url)
        {
            string html = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", Properties.Settings.Default.Authorization);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseString;
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        responseString = reader.ReadToEnd();
                    }
                }
                return responseString;
            }
            else
                return "";
        }

        public static async Task<string> HttpPOST(string postData, string endPoint)
        {
            //WebRequest request = WebRequest.Create("http://35.188.249.145:8080/SFDC/featureServletNew");
            WebRequest request = WebRequest.Create(endPoint);
            request.Headers.Add("Authorization", Properties.Settings.Default.Authorization);
            request.Method = "POST";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;


            Stream dataStream = await request.GetRequestStreamAsync();

            dataStream.Write(byteArray, 0, byteArray.Length);

            dataStream.Close();

            WebResponse response = await request.GetResponseAsync();

            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            //Console.WriteLine(responseFromServer);

            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

        #region FORMPOST

        public static string SaveByteDataAsFile(string data,string fileName)
        {
            
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Point5Nyble\Mailtangy\";
            try
            {
                if (File.Exists(filePath + fileName))
                {
                    File.Delete(filePath + fileName);
                }
                // Create a new stream to write to the file
                //var d = File.OpenWrite(filePath + fileName);
                //Writer = new BinaryWriter(d);
                FileStream fs = new FileStream(filePath+fileName, FileMode.Create);
                fs.Write(Encoding.UTF8.GetBytes(data), 0, data.Length);
                fs.Close();
            }
            catch(Exception)
            {
                //throw ex;
            }
            
            return filePath + fileName;

        }


        public async static Task<string> PostFormDataAsync(string URL, Microsoft.Office.Interop.Outlook.MailItem mail)
        {
            Dictionary<string, object> postParameters = new Dictionary<string, object>();

            
            postParameters.Add("source", "Outlook");
            postParameters.Add("id", mail.ConversationID);
            postParameters.Add("to", mail.To);
            postParameters.Add("from",mail.SenderName +"<"+mail.SenderEmailAddress+">" );
            postParameters.Add("subject", mail.Subject);
            
            string seen = String.Format("{0:ddd MMM dd yyyy HH:mm:ss}", mail.SentOn) +" "+TimeZone.CurrentTimeZone;
            //postParameters.Add("seen", "Fri Mar 23 2018 13:38:29 GMT+0530 (India Standard Time)");
            DateTimeOffset local_offset = new DateTimeOffset(mail.SentOn);
            DateTimeOffset utc_offset = local_offset.ToUniversalTime();

            seen = String.Format("{0:ddd MMM dd yyyy HH:mm:ss}", utc_offset) + " GMT+530 (India Standard Time)";
            postParameters.Add("seen", seen);
            string OffSet = " "+ local_offset.ToString("%K").Replace(":","").Trim();
            
            postParameters.Add("sentdate", String.Format("{0:ddd,dd MMM yyyy HH:mm:ss}", mail.SentOn)+ OffSet);
            
            
            postParameters.Add("receiveddate", String.Format("{0:ddd,dd MMM yyyy HH:mm:ss}", mail.ReceivedTime) + OffSet);
            
            postParameters.Add("HtmlMessagebody", mail.HTMLBody);
            postParameters.Add("PlainTextMessagebody", mail.Body);
            
            foreach (Microsoft.Office.Interop.Outlook.Attachment item in mail.Attachments)
            {
                //Save attachments and provide path.
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\Point5Nyble\Mailtangy\";
                if (Directory.Exists(filePath))
                {
                    item.SaveAsFile(filePath+item.FileName);
                }
                else
                {
                    Directory.CreateDirectory(filePath);
                    item.SaveAsFile(filePath + item.FileName);
                }
                
                FileStream fs = new FileStream(filePath + item.FileName, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data,0 , data.Length);
                fs.Close();
                postParameters.Add("attachfiles", new FileParameter(data, item.FileName,getContentType(filePath + item.FileName)));
                File.Delete(filePath+item.FileName);
            }
            
            HttpWebResponse webResponse =await MultipartFormDataPost(URL,"OutlookAddin", postParameters);
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();
            return fullResponse;
        }

        private static string getContentType(string v)
        {
            switch (Path.GetExtension(v))
            {
                case "pdf":
                    return "application/pdf";
                case "docx":
                    return "application/msword";
                case "doc":
                    return "application/msword";
                case "xls":
                    return "application/msexcel";
                case "xlsx":
                    return "application/msexcel";
                case "txt":
                    return "text/plain";
                case "bmp":
                    return "image/bmp";
                case "jpeg":
                    return "image/jpeg";
                default:
                    return "text/plain";
                    
            }
        }

        //new FormUpload.FileParameter(data, "People.doc", "application/msword")
        private static readonly Encoding encoding = Encoding.UTF8;
        public static Task<HttpWebResponse> MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, userAgent, contentType, formData);
        }
        private async static Task<HttpWebResponse> PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            // Set up the request properties.
            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = formData.Length;
            request.Headers.Add("Authorization", Properties.Settings.Default.Authorization);

            // Send the form data to the request.
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData ,0, formData.Length);
                requestStream.Close();
            }

            return await request.GetResponseAsync() as HttpWebResponse;
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"),0 , encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header),0 , encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File,0 , fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData),0 , encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer),0 , encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData,0 , formData.Length);
            formDataStream.Close();

            return formData;
        }

        
        #endregion

    }

    public class FileParameter
    {
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public FileParameter(byte[] file) : this(file, null) { }
        public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
        public FileParameter(byte[] file, string filename, string contenttype)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
        }
    }
    public class Attachment
    {
        public string data { get; set; }
        public string fileName { get; set; }
        public string contentType { get; set; }
    }
    public class CustomFormData
    {
        public string source { get; set; }
        public string id { get; set; }
        public string to { get; set; }
        public string from { get; set; }
        public string seen { get; set; }
        public string sentdate { get; set; }
        public string receiveddate { get; set; }
        public string HtmlMessagebody { get; set; }
        public string PlainTextMessagebody { get; set; }


    }

   
}
