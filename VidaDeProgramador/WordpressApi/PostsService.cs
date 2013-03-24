using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using AlbertoMonteiroWP7Tools.Controls;
using AlbertoMonteiroWP7Tools.Extensions;

namespace VidaDeProgramador.WordpressApi
{
    public class PostsService
    {
        const string URL = "http://vidadeprogramador.com.br/category/tirinhas/feed/?paged={0}";
        const string IMAGEM = @"<img src=\""(?<imagem>[\w:/.-]+)\""";
        const string CORPO = @"<div class=""transcription"">(?<corpo>(.|\n)+)</div>";

        public async Task<IEnumerable<Tirinha>> GetPosts(int page)
        {
            GlobalLoading.Instance.PushLoading();

            var contentSteam = new MemoryStream();
            try
            {
                var webClient = new WebClient();
                var xml = await webClient.DownloadString(new Uri(string.Format(URL, page)));

                Encoding.UTF8.GetBytes(xml).ToList().ForEach(contentSteam.WriteByte);
                contentSteam.Seek(0, SeekOrigin.Begin);

                XDocument xDocument = XDocument.Load(contentSteam);

                var rss = xDocument.Element("rss");

                var wfw = rss.GetNamespaceOfPrefix(@"wfw");
                var slash = rss.GetNamespaceOfPrefix(@"slash");

                var imagemRegex = new Regex(IMAGEM);
                var corpoRegex = new Regex(CORPO);

                var tirinhas = new List<Tirinha>();

                foreach (var item in rss.Element("channel").Elements("item"))
                {
                    var srcImagem = imagemRegex.Match(item.Element("description").Value).Groups["imagem"].Value;
                    var body = corpoRegex.Match(item.Element("description").Value).Groups["corpo"].Value;

                    var tirinha = new Tirinha()
                    {
                        Title = item.Element("title").Value,
                        Body = HttpUtility.HtmlDecode(body.Replace("<br />", Environment.NewLine)),
                        Image = srcImagem,
                        Link = item.Element("link").Value,
                        PublicadoEm = DateTime.Parse(item.Element("pubDate").Value),
                        LinkComentarios = item.Element(XName.Get("commentRss", wfw.NamespaceName)).Value,
                        TotalComentarios = int.Parse(item.Element(XName.Get("comments", slash.NamespaceName)).Value)
                    };
                    tirinhas.Add(tirinha);
                }
                return tirinhas;
            }
            catch (WebException e)
            {
                if (e.Response.ContentLength == 0)
                    throw new Exception("Impossível se conectar a internet, verifique sua conexão.");
            }
            finally
            {
                GlobalLoading.Instance.PopLoading();
                contentSteam.Close();
            }
            return null;
        }
    }
}
