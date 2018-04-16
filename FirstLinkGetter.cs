using System;
using System.IO;


public class FirstLinkGetter {

    enum StateOptions { FINDFIRST, METADATA, REDIRECT, SKIPPAGE, TEXT }; 

    public static void Main() {
        string path = "~/put path here.txt";
        //using (StreamWriter streamWriter = new StreamWriter(path)) {

            string home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            using (StreamReader streamReader = new StreamReader(home + "/enwiki-20180401/enwiki-20180401-pages-articles.xml")) {

                StateOptions state = StateOptions.FINDFIRST;
                string line;
                while ((line = streamReader.ReadLine()) != null) {
                    switch (state) {
                        case StateOptions.FINDFIRST {
                            
                            if (line == "  <page>") {
                                state = StateOptions.METADATA;
                            }
                            break;
                        }
                        case StateOptions.METADATA {

                            Console.WriteLine(line);
                            Environment.Exit(0);
                        }
                        case StateOptions.REDIRECT {

                        }
                        case StateOptions.SKIPPAGE {

                        }
                        case StateOptions.TEXT {

                        }

                    }
                }
            }
        //}
             
    }
}
