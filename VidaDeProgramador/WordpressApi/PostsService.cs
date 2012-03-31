using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using AlbertoMonteiroWP7Tools.Controls;

namespace VidaDeProgramador.WordpressApi
{
    public class PostsService
    {
        const string URL = "http://vidadeprogramador.com.br/category/tirinhas/feed/?paged={0}";
        const string IMAGEM = @"<img src=""(?<imagem>.+)"" a";
        const string CORPO = @"<div class=""transcription"">(?<corpo>(.|\n)+)</div>";

        public async Task<IEnumerable<Tirinha>> GetPosts(int page)
        {
            GlobalLoading.Instance.PushLoading();
            XmlReader reader = null;
            MemoryStream contentSteam = null;
            try
            {
                var webClient = new WebClient();
                var xml = await webClient.DownloadStringTaskAsync(new Uri(string.Format(URL, page)));
                contentSteam = new MemoryStream();

                Encoding.UTF8.GetBytes(xml).ToList().ForEach(contentSteam.WriteByte);
                contentSteam.Seek(0, SeekOrigin.Begin);

                reader = XmlReader.Create(contentSteam);
                var feed = SyndicationFeed.Load(reader);
                
                var imagemRegex = new Regex(IMAGEM);
                var corpoRegex = new Regex(CORPO);
                
                return from syndicationItem in feed.Items
                       let html = syndicationItem.ElementExtensions.ReadElementExtensions<string>("encoded", "http://purl.org/rss/1.0/modules/content/")
                       let srcImagem = imagemRegex.Match(html[0]).Groups["imagem"].Value
                       let body = corpoRegex.Match(html[0]).Groups["corpo"].Value
                       select new Tirinha
                       {
                           Title = syndicationItem.Title.Text,
                           Image = srcImagem,
                           Body = HttpUtility.HtmlDecode(body.Replace("<br />", Environment.NewLine)),
                           Link = syndicationItem.Id
                       };
            }
            catch (WebException e)
            {
                if (e.Response.ContentLength == 0)
                    throw new Exception("Impossível se conectar a internet, verifique sua conexão.");
            }
            finally
            {
                GlobalLoading.Instance.PopLoading();
                if (reader != null)
                {
                    reader.Close();
                    contentSteam.Close();
                }
            }
            return null;
        }
    }
}
