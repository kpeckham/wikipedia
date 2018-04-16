using System;
using System.IO;


public class FirstLinkGetter {

    enum StateOptions { FINDFIRST, METADATA, REDIRECT, SKIPPAGE, TEXT }; 

    public static void Main() {
        string home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string path = home + "/firstLinks1.txt";
        using (StreamWriter streamWriter = new StreamWriter(path)) {


            using (StreamReader streamReader = new StreamReader(home + "/enwiki-20180401/enwiki-20180401-pages-articles.xml")) {

                StateOptions state = StateOptions.FINDFIRST;
                string line;
                string id;
                string link;
                while ((line = streamReader.ReadLine()) != null) {
                    switch (state) {
                        case StateOptions.FINDFIRST: 
                            
                            if (line == "  <page>") {
                                state = StateOptions.METADATA;
                            }
                            break;
                        
                        case StateOptions.METADATA:

                            if (line.StartsWith("    <id>", StringComparison.CurrentCulture)) {
                                id = line.Substring(8, line.Length - 8 - 4);
                                Console.WriteLine(id);
                                Environment.Exit(0);

                            }

                            break;
                        
                        case StateOptions.REDIRECT: 
                            break;
                        
                        case StateOptions.SKIPPAGE: 
                            break;
                        
                        case StateOptions.TEXT: 
                            break;
                        

                    }
                }
            }
        }
             
    }
}
