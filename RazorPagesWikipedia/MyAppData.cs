using System.Collections.Generic;
using System;
using RazorPagesWikipedia.DbModels;
using System.Text;
using System.Linq;

public class MyAppData
{

    public Dictionary<int, FirstLinkInfo> ToPhilosophy = new Dictionary<int, FirstLinkInfo>();

    public MyAppData()
    {
        LoopPages();
    }

    public void LoopPages()
    {
        using (var db = new WikiDbContext())
        {
            byte[] CompareText = Encoding.UTF8.GetBytes("Featured_articles");
            var links = db.Categorylinks.Where(cl => cl.ClTo == CompareText);

            int count = 0;
            //List<uint> ids = db.KpFirstlinks.Select(link => link.PageId).ToList();
            //foreach (var id in ids)
            foreach (var id in links.Select(link => link.ClFrom))
            {
                if (++count % 1000 == 0)
                    Console.WriteLine(count);
                FindPhilosophy((int)id);
            }

        }
    }

    public FirstLinkInfo FindPhilosophy(int FromId)
    {

        //Console.WriteLine("entered the void");
        var entry = ToPhilosophy.GetValueOrDefault(FromId);
        if (entry != null)
        {
            //Console.WriteLine("We've seen it before fromId - {0}", FromId);
            if (entry.unProcessed || entry.InALoop)
            {
                entry.InALoop = true;
                //Console.WriteLine("we've put it in a loop");
                entry.unProcessed = false;
            }

            return entry;

        }

        FirstLinkInfo unProcessedEntry = new FirstLinkInfo();
        ToPhilosophy.Add(FromId, unProcessedEntry);


        using (var db = new WikiDbContext())
        {
            var ToTitleBinary = db.KpFirstlinks.Where(fl => fl.PageId == FromId).Select(fl => fl.DestinationTitle).FirstOrDefault();

            if (ToTitleBinary == null)
            {
                FirstLinkInfo nullEntry = new FirstLinkInfo(0, false, false, -1);
                ToPhilosophy[FromId] = nullEntry;

                //Console.WriteLine("Exiting at 114");
                return nullEntry;
            }

            string ToTitle = Encoding.UTF8.GetString(ToTitleBinary);
            var ToId = db.Page.Where(pg => pg.PageNamespace == 0 && pg.PageTitle == ToTitleBinary).Select(pg => pg.PageId).FirstOrDefault();


            if (ToId == 0)
            {
                FirstLinkInfo nullEntry = new FirstLinkInfo(0, false, false, -1);
                ToPhilosophy[FromId] = nullEntry;

                //Console.WriteLine("Exiting at 127");
                return nullEntry;
            }

            FirstLinkInfo childInfo = ToPhilosophy.GetValueOrDefault((int)ToId);

            if (childInfo != null)
            {
                if (childInfo.unProcessed)
                {
                    childInfo.InALoop = true;
                }
                FirstLinkInfo parentInfo = new FirstLinkInfo((int)ToId, childInfo.GoesToPhilosophy, childInfo.InALoop, childInfo.Depth + 1);

                ToPhilosophy[FromId] = parentInfo;
                //Console.WriteLine("Exiting at 142 FromId {0} ToTitle {1} inaloop {2}", FromId,ToTitle, parentInfo.InALoop);
                return parentInfo;
            }

            if (ToTitle == "Philosophy")
            {
                FirstLinkInfo info = new FirstLinkInfo((int)ToId, true, false, 1);
                ToPhilosophy[FromId] = info;
                //Console.WriteLine("Exiting at 152");
                return info;
            }

            childInfo = FindPhilosophy((int)ToId);
            FirstLinkInfo legalGuardianInfo = new FirstLinkInfo((int)ToId, childInfo.GoesToPhilosophy, childInfo.InALoop, childInfo.Depth + 1);
            ToPhilosophy[FromId] = legalGuardianInfo;
            //Console.WriteLine("Exiting at 164 FromId {0} ToTitle {1} inaloop {2}", FromId, ToTitle, legalGuardianInfo.InALoop);
            return legalGuardianInfo;



        }

    }

}

public class FirstLinkInfo
{
    public int Depth;
    public bool GoesToPhilosophy;
    public bool InALoop;
    public int ToId;
    public bool unProcessed;

    public FirstLinkInfo(int ToId, bool GoesToPhilosophy, bool InALoop, int Depth)
    {
        this.Depth = Depth;
        this.ToId = ToId;
        this.InALoop = InALoop;
        this.GoesToPhilosophy = GoesToPhilosophy;
        unProcessed = false;
    }

    public FirstLinkInfo()
    {
        unProcessed = true;
        Depth = -1;
        GoesToPhilosophy = false;
        InALoop = false;
        ToId = 0;

    }
}


