using System;
using System.Collections.Generic;
using System.Text;
using Version = Lucene.Net.Util.Version;
using Directory = Lucene.Net.Store.Directory;
using FSDirectory = Lucene.Net.Store.FSDirectory;


namespace TestLucene
{
    using Lucene.Net.Index;
    using Lucene.Net.Store;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Search;
    using Lucene.Net.QueryParsers;

    class Program
    {
        static void Main(string[] args)
        {
            Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_23);
            //IndexWriter writer = new IndexWriter("IndexDirectory", analyzer, true);
            //AddDocument(writer, "SQL Server 2008 的发布", "SQL Server 2008 的新特性");
            //AddDocument(writer, "ASP.Net MVC框架配置与分析", "而今，微软推出了新的MVC开发框架，也就是Microsoft ASP.NET 3.5 Extensions");
            //writer.Optimize();
            //writer.Close();
           
            while (true)
            {
                Console.Write("key:");

                string key = Console.ReadLine().ToString();
                
                string indexPath = @"D:\workspace\C#\lucene\lucene\lucene\IndexDirectory";
                Directory directory = null;
                directory = FSDirectory.Open(new System.IO.DirectoryInfo(indexPath));
                IndexSearcher searcher = new IndexSearcher(directory);
                MultiFieldQueryParser parser = new MultiFieldQueryParser(Version.LUCENE_23, new string[] { "title", "content" }, analyzer);
                Query query = parser.Parse(key);
                TopDocs hits = searcher.Search(query, (Filter)null, 1000);

             
                foreach (ScoreDoc hit in hits.ScoreDocs)
                {
                    Document doc = searcher.Doc(hit.Doc);
                    string id = doc.Get("id");
                    string title = doc.Get("title");
                    string content = doc.Get("content");

                    Console.WriteLine("title:{0},content:{1},id:{2}", title, content, id);

                }
                searcher.Dispose();
                Console.WriteLine("=================================");
            }
            

            Console.ReadKey();
        }
        //static void AddDocument(IndexWriter writer, string title, string content)
        //{
        //    Document document = new Document();
        //    document.Add(new Field("title", title, Field.Store.YES, Field.Index.TOKENIZED));
        //    document.Add(new Field("content", content, Field.Store.YES, Field.Index.TOKENIZED));
        //    writer.AddDocument(document);
        //}
    }
}
