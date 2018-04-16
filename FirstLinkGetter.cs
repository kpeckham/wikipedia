using System;
using System.IO;


public class FirstLinkGetter {

    enum StateOptions { FINDFIRST, METADATA, REDIRECT, SKIPPAGE, TEXT }; 

    public static void Main() {
        string path = "~/put path here.txt";
        //using (StreamWriter streamWriter = new StreamWriter(path)) {
        string home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            using (StreamReader streamReader = new StreamReader(home + "/enwiki-20180401/enwiki-20180401-pages-articles.xml")) {
                string line = streamReader.ReadLine();
                Console.WriteLine(line);
                //StateOptions state = StateOptions.FINDFIRST;
                //while (true) {
                //    switch (state) {
                //        case StateOptions.FINDFIRST {
                //        }
                //        case StateOptions.METADATA {

                //        }
                //        case StateOptions.REDIRECT {

                //        }
                //        case StateOptions.SKIPPAGE {

                //        }
                //        case StateOptions.TEXT {

                //        }
                //    }
                //}
            }
        //}
             
    }
}
