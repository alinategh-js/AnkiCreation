using AnkiSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiCreation
{
    public class Program
    {
        static void Main(string[] args)
        {
            Anki testAnki = new Anki("testAnki");
            testAnki.SetFields("Title", "Back");
            testAnki.SetFormat("<div class=main-title>PTE SelfStudy WFD v10</div>\\n <br/> \\n <span class=span-class>{0}</span><br/><br/> \\n {{type:Back}} \\n<hr id=answer>\\n");
            testAnki.SetCss(@"C:/Users/ASUS/Desktop/test.css");

            //testAnki.AddItem("0000 [sound:D:/wfd/0.mp3]", "test0");
            //testAnki.AddItem("0001 [sound:D:/wfd/1.mp3]", "test1");
            //testAnki.AddItem("0002 [sound:D:/wfd/2.mp3]", "test2");
            //testAnki.AddItem("0003 [sound:D:/wfd/3.mp3]", "test3");


            // 
            List<string[]> pathTitles = new List<string[]>();
            using (var connection = new SqlConnection("server=.;database=LectureDB;Trusted_Connection=true"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT TOP 100 AudioPath, AudioText
                    FROM Audio
                ";

                using (var reader = command.ExecuteReader())
                {
                    int i=0;
                    while (reader.Read())
                    {
                        var path = reader.GetString(0);
                        var text = reader.GetString(1);
                        string[] str = new string[2];
                        str[0] = path;
                        str[1] = text;

                        pathTitles.Add(str);
                    }
                }
            }

            foreach(var str in pathTitles)
            {
                testAnki.AddItem($"[sound:{str[0]}]", str[1]);
            }

            testAnki.CreateApkgFile("C:/Users/ASUS/Desktop/");

        }
    }
}
