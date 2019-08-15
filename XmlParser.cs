using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Xml;
using System.Text;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sample.ParseXml
{
    public static class XmlParser
    {
        [FunctionName("XmlParser")]
         public static async void Run([BlobTrigger("xmlfiles/{name}.xml", Connection = "BLOB_STORAGE")]CloudBlockBlob xmlData, string name, Binder binder, TraceWriter log)
        {
            string node = "ParseNode";
            log.Info($"C# Blob trigger function Processed blob\n Name:{name}\n Node:{node}");

            string path = $"{name}";        
            string json = "";

            MemoryStream ms = new MemoryStream();
           
            await xmlData.DownloadToStreamAsync(ms);
            ms.Seek(0, 0);
            XmlDocument doc = new XmlDocument();
            doc.Load(ms);


            XmlNodeList nodes = doc.SelectNodes("node");

            for(int i=0; i< nodes.Count; i++) { json += JsonConvert.SerializeXmlNode(nodes[i], Newtonsoft.Json.Formatting.Indented, true) ; }
           
            byte[] b = Encoding.UTF8.GetBytes(json);
       

            /////// Save JSON  ///////////
            var  attributes = new System.Attribute[]
           {
               new BlobAttribute($"jsonfiles/{path}.json", FileAccess.Write),
               new StorageAccountAttribute("BLOB_STORAGE")               
           };            

            using (var stream = await binder.BindAsync<Stream>(attributes))
            {
                stream.Write(b, 0, b.Length);
                stream.Flush();
            }

            await xmlData.DeleteIfExistsAsync();

        }
    }
}
