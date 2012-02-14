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
using VidaDeProgramador.Controls;

namespace VidaDeProgramador.WordpressApi
{
    public class PostsService
    {
        const string Url = "http://vidadeprogramador.com.br/category/tirinhas/feed/?paged={0}";
        const string Imagem = @"<img src=""(?<imagem>.+)"" a";
        const string Corpo = @"<div class=""transcription"">(?<corpo>(.|\n)+)</div>";

        public async Task<IEnumerable<Post>> GetPosts(int page)
        {
            XmlReader reader = null;
            MemoryStream contentSteam = null;
            try
            {
                GlobalLoading.Instance.PushLoading();
                var webClient = new WebClient();
                var xml = await webClient.DownloadStringTaskAsync(new Uri(string.Format(Url, page)));
                contentSteam = new MemoryStream();

                Encoding.UTF8.GetBytes(xml).ToList().ForEach(contentSteam.WriteByte);
                contentSteam.Seek(0, SeekOrigin.Begin);

                reader = XmlReader.Create(contentSteam);
                var feed = SyndicationFeed.Load(reader);
                
                var imagemRegex = new Regex(Imagem);
                var corpoRegex = new Regex(Corpo);

                GlobalLoading.Instance.PopLoading();
                return from syndicationItem in feed.Items
                       let html = syndicationItem.ElementExtensions.ReadElementExtensions<string>("encoded", "http://purl.org/rss/1.0/modules/content/")
                       let srcImagem = imagemRegex.Match(html[0]).Groups["imagem"].Value
                       let body = corpoRegex.Match(html[0]).Groups["corpo"].Value
                       select new Post
                       {
                           Title = syndicationItem.Title.Text,
                           Image = srcImagem,
                           Body = HttpUtility.HtmlDecode(body.Replace("<br />", Environment.NewLine))
                       };
            }
            catch (WebException e)
            {
                if (e.Response.ContentLength == 0)
                    throw new Exception("Impossível se conectar a internet, verifique sua conexão.");
            }
            finally
            {
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
