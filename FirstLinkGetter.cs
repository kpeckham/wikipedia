using System;
using System.IO;


public class FirstLinkGetter {

    public static void main() {
        string path = "~/put path here.txt";
        using (StreamWriter streamWriter = new StreamWriter(path)) {
            
            using (StreamReader streamReader = new StreamReader("enwiki20180401-pages-articles.xml")) {
                while (streamReader.Peek() > -1) {
                    string line = streamReader.ReadLine();

                }
            }
        }
             
    }
}