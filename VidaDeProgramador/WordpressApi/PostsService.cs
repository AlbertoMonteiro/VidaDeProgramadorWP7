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
        public async Task<IEnumerable<Post>> GetPosts(int page)
        {
            try
            {
                GlobalLoading.Instance.PushLoading();
                var wc = new WebClient();
                var xml = await wc.DownloadStringTaskAsync(new Uri(string.Format("http://vidadeprogramador.com.br/category/tirinhas/feed/?paged={0}", page)));
                var mem = new MemoryStream();

                foreach (var b in Encoding.UTF8.GetBytes(xml)) mem.WriteByte(b);
                mem.Seek(0, SeekOrigin.Begin);

                var reader = XmlReader.Create(mem);
                SyndicationFeed feed = SyndicationFeed.Load(reader);

                const string imagem = @"<img src=""(?<imagem>.+)"" a";
                const string corpo = @"<div class=""transcription"">(?<corpo>(.|\n)+)</div>";
                var imagemRegex = new Regex(imagem);
                var corpoRegex = new Regex(corpo);

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
            return null;
        }
    }
}
