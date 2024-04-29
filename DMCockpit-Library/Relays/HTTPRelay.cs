using System.Net;

namespace DMCockpit_Library.Relays
{
    public interface IHTTPRelay
    {
        void StartListening();

        event MessageRecived MessageRecivedEvent;
    }

    public class RelayMessage
    {
        public string URL { get; set; }
        public string Body { get; set; }
    }

    public delegate void MessageRecived(RelayMessage request);

    public class DMCockpitHTTPListener : IHTTPRelay
    {
        public event MessageRecived MessageRecivedEvent;

        private HttpListener? listener;

        public void StartListening()
        {
            if (this.listener != null) return;

            this.listener = new HttpListener();
            foreach (string s in new string[] { "http://localhost:8080/" }) listener.Prefixes.Add(s);

            Task.Run(Listen);
        }

        private void Listen()
        {
            if (this.listener == null) throw new NullReferenceException("Listener is null");
            listener.Start();

            var context = listener.GetContext();

            var content = new StreamReader(context.Request.InputStream).ReadToEnd();
            var url = context.Request.Url?.ToString() ?? string.Empty;

            RelayMessage message = new RelayMessage
            {
                URL = url,
                Body = content
            };

            MessageRecivedEvent?.Invoke(message);

            listener.Stop();

            Task.Run(Listen);
        }
    }
}
