using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;


public class FirstLinkGetter {

    enum StateOptions { NEXTPAGE, METADATA, TEXT, ENDPAGE }; 

    public static void Main() {
        string home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string path = home + "/firstLinks1.txt";
        using (StreamWriter streamWriter = new StreamWriter(path)) {


            using (StreamReader streamReader = new StreamReader(home + "/data/10g.xml")) {

                StateOptions state = StateOptions.NEXTPAGE;
                Regex spaceRegex = new Regex(" +", RegexOptions.Compiled);

                string line = "";
                string id = "";
                string link = null;

                bool isRedirect = false;
                bool skipLinkRemainder = false;

                int curlyLevel = 0;
                int squareLevel = 0;
                int progressCheck = 0;

                // checking for ENDPAGE in while because it's easier than figuring out how to backtrack a line
                // once we figured out that we'd hit the end of a page.
                // this way we can just switch to the end page state without reading in a new line.
                while (state == StateOptions.ENDPAGE || (line = streamReader.ReadLine()) != null) {
                    switch (state) {
                        case StateOptions.NEXTPAGE: 
                            
                            if (line == "  <page>") {
                                id = "";
                                link = null;

                                isRedirect = false;
                                skipLinkRemainder = false;

                                curlyLevel = 0;
                                squareLevel = 0;

                                state = StateOptions.METADATA;
                            }
                            break;
                        
                        case StateOptions.METADATA:
                            if (line.StartsWith("    <ns>", StringComparison.CurrentCulture)){
                                if (line != "    <ns>0</ns>") {

                                    state = StateOptions.NEXTPAGE;
                                }
                            }

                            //id line format: "    <id>10</id>"
                            else if (line.StartsWith("    <id>", StringComparison.CurrentCulture)) {
                                id = line.Substring(8, line.Length - 8 - 5);
                                if (Convert.ToInt32(id) > progressCheck) {
                                    Console.WriteLine("Progress: {0}", id);
                                    progressCheck += 1000000;
                                }

                            }
                            //redirect line format: "    <redirect title="Computer accessibility" />"
                            else if (line.StartsWith("    <redirect title=\"", StringComparison.CurrentCulture)) {
                                isRedirect = true;
                                link = line.Substring(21, line.Length - 21 - 4);
                                state = StateOptions.ENDPAGE;

                            }
                            else if (line.StartsWith("      <format>", StringComparison.CurrentCulture)) {
                                //some types are text/css and text/javascript - we don't want to deal with those
                                if (line != "      <format>text/x-wiki</format>") {
                                    state = StateOptions.NEXTPAGE;
                                }
                                else {
                                    state = StateOptions.TEXT;
                                }
                            }

                            break;
                       
                        case StateOptions.TEXT:
                            
                            if (line == "  </page>") {
                                state = StateOptions.ENDPAGE;
                            }
                            else {
                                foreach (char item in line) {
                                    bool linkEndFlag = false;

                                    switch (item) {
                                        case '[':
                                            squareLevel += 1;
                                            break;
                                        case ']':
                                            linkEndFlag |= (squareLevel == 2 && curlyLevel == 0);
                                            squareLevel -= 1;
                                            break;
                                        case '{':
                                            curlyLevel += 1;
                                            break;
                                        case '}':
                                            curlyLevel -= 1;
                                            break;
                                    }



                                    if (curlyLevel == 0 && squareLevel == 2 && item != '[' && !skipLinkRemainder) {
                                        if (item == '|' || item == '#') {
                                            skipLinkRemainder = true;
                                        }

                                        else {
                                            if (link == null ) {
                                                link = "";
                                            }
                                            link += item;
                                        }
                                    }

                                    if (linkEndFlag) {
                                        skipLinkRemainder = false;
                                        if (link.StartsWith("File:") || link.StartsWith("Image:")) {
                                            link = null;
                                        }
                                        else {
                                            state = StateOptions.ENDPAGE;
                                            break;
                                        }

                                    }
                                }
                            }
                            break;

                        case StateOptions.ENDPAGE:
                            if (link == null) {
                                link = "(no link found)";
                            }
                            if (squareLevel > 0 || curlyLevel > 0) {
                                link = "(bad wikitext)";
                            }
                            //Canonicalization - https://en.wikipedia.org/wiki/Help:Link#Conversion_to_canonical_form

                            link = link.Replace('_', ' ');
                            link = link.Trim();

                            if (link.Length > 0) {
                                link = char.ToUpper(link[0]) + link.Substring(1);
                            }
                                
                            link = spaceRegex.Replace(link, " ");
                            link = WebUtility.HtmlDecode(link);
                               
                            streamWriter.WriteLine(id + "\t" + (isRedirect ? "t" : "f") + "\t" + link);
                            state = StateOptions.NEXTPAGE;
                            //check for broken wikitext and figure out array out of bounds exception

                            break;
                    }
                }
            }
        }
             
    }
}
