using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VidaDeProgramador.Core
{
    public class PostsService
    {
        private const string URL = "http://vidadeprogramador.com.br/category/tirinhas/feed/?paged={0}";
        private const string IMAGEM = @"<img src=\""(?<imagem>[\w:/.-]+)\""";
        private const string CORPO = @"<div class=""transcription"">(?<corpo>(.|\n)+)</div>";

        public Task<IEnumerable<Tirinha>> GetPosts(int page, Func<Tirinha,bool> tirinhaLida)
        {
            var contentSteam = new MemoryStream();
            var taskCompletion = new TaskCompletionSource<IEnumerable<Tirinha>>();
            try
            {
                var webClient = WebRequest.CreateHttp(new Uri(string.Format(URL, page)));

                webClient.BeginGetResponse(ar =>
                {
                    var webResponse = webClient.EndGetResponse(ar);

                    if (ar.IsCompleted)
                    {
                        using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            var xml = streamReader.ReadToEnd();

                            foreach (var b in Encoding.UTF8.GetBytes(xml))
                                contentSteam.WriteByte(b);

                            contentSteam.Seek(0, SeekOrigin.Begin);

                            var xDocument = XDocument.Load(contentSteam);

                            var rss = xDocument.Element("rss");

                            var wfw = rss.GetNamespaceOfPrefix(@"wfw");
                            var slash = rss.GetNamespaceOfPrefix(@"slash");

                            var imagemRegex = new Regex(IMAGEM);

                            var tirinhas = new List<Tirinha>();

                            foreach (var item in rss.Element("channel").Elements("item"))
                            {
                                string imgHtml = item.Element("description").Value;
                                string bodyHtml = item.Elements("div").FirstOrDefault(element => element.Attribute("class").Value == "transcription").Value;

                                string srcImagem = imagemRegex.Match(imgHtml).Groups["imagem"].Value;
                                string body = bodyHtml;
                                
                                var tirinha = new Tirinha(
                                    item.Element("title").Value,
                                    body.Replace("<br />", Environment.NewLine),
                                    srcImagem,
                                    item.Element("link").Value,
                                    DateTime.Parse(item.Element("pubDate").Value),
                                    item.Element(XName.Get("commentRss", wfw.NamespaceName)).Value,
                                    int.Parse(item.Element(XName.Get("comments", slash.NamespaceName)).Value)
                                );
                                tirinha.Nova = tirinhaLida(tirinha);
                                tirinhas.Add(tirinha);
                            }
                            taskCompletion.SetResult(tirinhas);
                        }
                    }
                },null);
            }
            catch (WebException e)
            {
                taskCompletion.SetException(e);
            }
            return taskCompletion.Task;
        }

        public Task<IEnumerable<Comentario>> GetComments(string linkComentarios)
        {
            var contentSteam = new MemoryStream();
            var taskCompletion = new TaskCompletionSource<IEnumerable<Comentario>>();
            try
            {
                var webClient = WebRequest.CreateHttp(new Uri(linkComentarios));

                webClient.BeginGetResponse(ar =>
                {
                    var webResponse = webClient.EndGetResponse(ar);

                    if (ar.IsCompleted)
                    {
                        using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            var xml = streamReader.ReadToEnd();

                            foreach (var b in Encoding.UTF8.GetBytes(xml))
                                contentSteam.WriteByte(b);

                            contentSteam.Seek(0, SeekOrigin.Begin);

                            var xDocument = XDocument.Load(contentSteam);

                            var rss = xDocument.Element("rss");

                            var dc = rss.GetNamespaceOfPrefix(@"dc");

                            var comentarios = new List<Comentario>();

                            foreach (var item in rss.Element("channel").Elements("item"))
                            {
                                var creator = item.Element(XName.Get("creator", dc.NamespaceName)).Value;
                                var pubDate = DateTime.Parse(item.Element("pubDate").Value);
                                var description = item.Element("description").Value;
                                
                                var comentario = new Comentario(creator,pubDate,description);
                                comentarios.Add(comentario);
                            }
                            taskCompletion.SetResult(comentarios);
                        }
                    }
                }, null);
            }
            catch (WebException e)
            {
                taskCompletion.SetException(e);
            }
            return taskCompletion.Task;

        }
    }
}
