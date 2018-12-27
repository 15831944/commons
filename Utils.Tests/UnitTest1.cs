using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Utils.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var conn = new MySqlConnection("Server=192.168.0.199;Port=3306;UserId=root;Password=newsys;Database=redcross");
            conn.Open();
            var reader = conn.ExecuteReader("SELECT * FROM `sys_filecontent`");
            while (reader.Read())
            {
                var ms = new MemoryStream((byte[])reader.GetValue(1));
                var bm = new Bitmap(ms);
                bm.Save(@"D:/foo/" + reader.GetString(0) + ".png", ImageFormat.Png);
            }
            reader.Close();
            conn.Close();
        }
    }
}