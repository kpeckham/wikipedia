using System;
using System.IO;


public class FirstLinkGetter {

    enum StateOptions { FINDFIRST, METADATA, REDIRECT, SKIPPAGE, TEXT, ENDPAGE }; 

    public static void Main() {
        string home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string path = home + "/firstLinks1.txt";
        using (StreamWriter streamWriter = new StreamWriter(path)) {


            using (StreamReader streamReader = new StreamReader(home + "/enwiki-20180401/enwiki-20180401-pages-articles.xml")) {

                StateOptions state = StateOptions.FINDFIRST;
                string line;
                string id;
                string link;
                bool isRedirect;

                while ((line = streamReader.ReadLine()) != null) {
                    switch (state) {
                        case StateOptions.FINDFIRST: 
                            
                            if (line == "  <page>") {
                                state = StateOptions.METADATA;
                            }
                            break;
                        
                        case StateOptions.METADATA:
                            if (line.StartsWith("    <ns>", StringComparison.CurrentCulture)){
                                if (line != "    <ns>0</ns>") {
                                    state = StateOptions.SKIPPAGE;
                                }
                            }

                            //id line format: "    <id>10</id>"
                            else if (line.StartsWith("    <id>", StringComparison.CurrentCulture)) {
                                id = line.Substring(8, line.Length - 8 - 5);
                                Console.WriteLine(id);

                            }
                            //redirect line format: "    <redirect title="Computer accessibility" />"
                            else if (line.StartsWith("    <redirect title=\"", StringComparison.CurrentCulture)) {
                                isRedirect = true;
                                link = line.Substring(21, line.Length - 21 - 4);
                                state = StateOptions.REDIRECT;
                                Console.WriteLine(link);


                            }
                            else if (line.StartsWith("      <format>", StringComparison.CurrentCulture)) {
                                //some types are text/css and text/javascript - we don't want to deal with those
                                if (line != "      <format>text/x-wiki</format>") {
                                    Console.WriteLine(line);
                                    Environment.Exit(0);
                                    state = StateOptions.SKIPPAGE;
                                }
                            }

                            break;
                        
                        case StateOptions.REDIRECT: 
                            break;
                        
                        case StateOptions.SKIPPAGE: 
                            break;
                        
                        case StateOptions.TEXT: 
                            break;

                        case StateOptions.ENDPAGE:
                            break;

                    }
                }
            }
        }
             
    }
}
