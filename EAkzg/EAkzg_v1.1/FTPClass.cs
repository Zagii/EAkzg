using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace EAkzg
{
    class FTPClass
    {
        String host;
        String user;
        String haslo;
        public FTPClass() { }
        public void KonfigurujPolaczenie(String h,String u,String p)
        {
            host = h;
            user = u;
            haslo = p;
        }
        public int wyslij(String plik)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(user, haslo);

            // Copy the contents of the file to the request stream.
            System.IO.StreamReader sourceStream = new System.IO.StreamReader(plik);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

            response.Close();
            return 0;
        }
    }
}
