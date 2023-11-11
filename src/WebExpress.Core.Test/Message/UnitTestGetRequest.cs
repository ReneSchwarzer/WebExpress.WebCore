using System.IO;
using System.Net.Http;
using Xunit;

namespace WebExpress.Core.Test.Message
{
    public class UnitTestGetRequest
    {
        [Fact]
        public void Get_General()
        {
            var client = new HttpClient();
            //client.

            //client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            //Stream data = client.OpenRead("http://localhost/");
            //StreamReader reader = new StreamReader(data);
            //string s = reader.ReadToEnd();
            //Console.WriteLine(s);
            //data.Close();
            //reader.Close();

            //using var reader = new BinaryReader(new FileStream(Path.Combine("test", "general.get"), FileMode.Open));
            //var request = Request.Create(reader, "127.0.0.1");

            //Assert.True
            //(
            //    request.Uri?.ToString() == "/abc/xyz/A7BCCCA9-4C7E-4117-9EE2-ECC3381B605A",
            //    "Fehler in der Funktion Get_General"
            //);
        }

        [Fact]
        public void Get_Less()
        {
            //using var reader = new BinaryReader(new FileStream(Path.Combine("test", "less.get"), FileMode.Open));
            //var request = Request.Create(reader, "127.0.0.1");

            //Assert.True
            //(
            //    request.Uri?.ToString() == "/abc/xyz/A7BCCCA9-4C7E-4117-9EE2-ECC3381B605A",
            //    "Fehler in der Funktion Get_Less"
            //);
        }

        [Fact]
        public void Get_Massive()
        {
            //using var reader = new BinaryReader(new FileStream(Path.Combine("test", "massive.get"), FileMode.Open));
            //var request = Request.Create(reader, "127.0.0.1");

            //Assert.True
            //(
            //    request.Uri?.ToString() == "/abc/xyz/A7BCCCA9-4C7E-4117-9EE2-ECC3381B605A",
            //    "Fehler in der Funktion Get_Massive"
            //);
        }

        [Fact]
        public void Get_Param()
        {
            //using var reader = new BinaryReader(new FileStream(Path.Combine("test", "param.get"), FileMode.Open));
            //var request = Request.Create(reader, "127.0.0.1");
            //var param = request?.GetParameter("a")?.Value;

            //Assert.True
            //(
            //    request.Uri?.ToString() == "/abc/xyz/A7BCCCA9-4C7E-4117-9EE2-ECC3381B605A" &&
            //    param != null && param == "1",
            //    "Fehler in der Funktion Get_Param"
            //);
        }

        [Fact]
        public void Get_Param_Umlaut()
        {
            //using var reader = new BinaryReader(new FileStream(Path.Combine("test", "param_umlaut.get"), FileMode.Open));
            //var request = Request.Create(reader, "127.0.0.1");
            //var a = request?.GetParameter("a")?.Value;
            //var b = request?.GetParameter("b")?.Value;

            //Assert.True
            //(
            //    request.Uri?.ToString() == "/abc/xyz/A7BCCCA9-4C7E-4117-9EE2-ECC3381B605A" &&
            //    a != null && a == "ä" &&
            //    b != null && b == "ö ü",
            //    "Fehler in der Funktion Get_Param_Umlaut"
            //);
        }
    }
}
