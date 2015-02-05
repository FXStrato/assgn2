using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Web.Script.Services;
using System.Web.Services;
using Trie;


/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class wb : System.Web.Services.WebService
{

    private static JTrie gen;
    private static PerformanceCounter theMemCounter = new PerformanceCounter("Memory", "Available MBytes");
    public wb()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> search(string str)
    {
        if (str != "")
        {
            // Define Output HTML Formatting
            //MOVE THIS TO JAVASCRIPT.
            string t = "";
            List<string> original = gen.SearchPrefix(str, 10);
            for (int i = 0; i < original.Count; i++)
            {
                string html = "";
                html += "<ul><li class=\"result\">";
                html += "<a href=\"urlString\" target=\"_blank\">";
                html += "</a><h3>nameString</h3>";
                html += "</li>";
                html += "</ul>";
                html = html.Replace("nameString", original[i]);
                t += html;
                original[i] = html;
            }
            return original;
        }
        else
        {
            return new List<string>();
        }
    }

    [WebMethod]
    public string buildTrie()
    {
        JTrie dict = new JTrie();
        var filePath = System.IO.Path.GetTempPath() + "\\wiki.txt";
        StreamReader streamReader = new StreamReader(filePath);
        float mem = theMemCounter.NextValue();
        int counter = 0;
        while (mem > 50 && streamReader.EndOfStream == false)
        {
            try
            {
                string text = streamReader.ReadLine();
                dict.Add(text);
                counter++;
                if (counter % 1000 == 0)
                {
                    mem = theMemCounter.NextValue();
                }
            }
            catch
            {

            }
        }
        streamReader.Close();
        gen = dict;
        return "Success";
    }

    [WebMethod]
    public string downloadBlob()
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            ConfigurationManager.AppSettings["connectionstring"]);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = blobClient.GetContainerReference("blob");
        if (container.Exists())
        {
            CloudBlockBlob blob = container.GetBlockBlobReference("wiki.txt");
            // Save blob contents to a file.
            var filePath = System.IO.Path.GetTempPath() + "\\wiki.txt";
            blob.DownloadToFile(filePath, FileMode.OpenOrCreate);
        }

        return "Downloaded blob";
    }
}