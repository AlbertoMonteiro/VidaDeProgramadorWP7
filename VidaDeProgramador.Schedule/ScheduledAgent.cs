using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using VidaDeProgramador.Common.Persistence;
using VidaDeProgramador.Common.WordpressApi;

namespace VidaDeProgramador.Schedule
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;
        private readonly PostsService postService;

        /// <remarks>
        ///     ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate { Application.Current.UnhandledException += ScheduledAgent_UnhandledException; });

                var vdpContext = new VDPContext();
                postService = new PostsService(vdpContext);
            }
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        ///     Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        ///     The invoked task
        /// </param>
        /// <remarks>
        ///     This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO: Add code to perform your task in background
            var tirinhas = postService.GetPosts(0);
            tirinhas.ContinueWith(t =>
            {
                if (!t.Result.Any(x => x.Nova)) 
                    return;
                
                var shellToast = new ShellToast { Title = "VidaDeProgramador", Content = "Nova tirinha publicada" };
                shellToast.Show();
                foreach (var activeTile in ShellTile.ActiveTiles)
                {
                    var data = new StandardTileData();
                    data.Count = t.Result.Count(x => x.Nova);
                    activeTile.Update(data);
                }
            }).Wait();

            NotifyComplete();
        }
    }
}
