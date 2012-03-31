using Microsoft.Phone.Tasks;

namespace VidaDeProgramador
{
    public class CurrentTasks
    {
        private static CurrentTasks current;
        private readonly ShareStatusTask shareStatusTask;
        private EmailComposeTask emailComposeTask;

        private CurrentTasks()
        {
            shareStatusTask = new ShareStatusTask();
            emailComposeTask = new EmailComposeTask
            {
                Subject = "VidaDeProgramador", To = "alberto.monteiro@live.com"
            };
        }

        public static CurrentTasks Current
        {
            get { return current ?? (current = new CurrentTasks()); }
        }

        public void Share(string message)
        {
            shareStatusTask.Status = message;
            shareStatusTask.Show();
        }

        public void SendMail(string message)
        {
            emailComposeTask = new EmailComposeTask
            {
                Body = message
            };
            emailComposeTask.Show();
        }
    }
}