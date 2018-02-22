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

    class Program
    {
        static void Main(string[] args)
        {

            //Analyzer analyzer = new StandardAnalyzer();
            Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_23);
            Directory directory = null;
            //directory.CreateOutput(@"D:\workspace\C#\lucene\lucene\lucene\IndexDirectory");
           
            while (true)
            {
                Console.Write("id:");
                string id = Console.ReadLine().ToString();
                Console.Write("title:");
                string title = Console.ReadLine().ToString();
                Console.Write("content:");
                string content = Console.ReadLine().ToString();
                Console.WriteLine("=================================");
                string indexPath = @"D:\workspace\C#\lucene\lucene\lucene\IndexDirectory";
                directory = FSDirectory.Open(new System.IO.DirectoryInfo(indexPath));

                IndexWriter writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
                AddDocument(writer,id,title,content);
                writer.Optimize();
                writer.Dispose();
            }
            //AddDocument(writer,"1","name","ada");
            //AddDocument(writer, "2","SQL Server 2008 的发布ada", "SQL Server 2008 的新特性");
            //AddDocument(writer, "3","ASP.Net MVC框架配置与分析", "而今，微软推出了新的MVC开发框架，也就是Microsoft ASP.NET 3.5 Extensions");
           
          
        }

        static void AddDocument(IndexWriter writer, string id,string title, string content)
        {
            Document document = new Document();
            document.Add(new Field("id", id, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("title", title, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("content", content, Field.Store.YES, Field.Index.ANALYZED));
            writer.AddDocument(document);
        }
    }
}

