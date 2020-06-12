using ApiGateways.Telegram.Sender.Models;

namespace ApiGateways.Telegram.Sender.Config
{
    public class UrlsConfig
    {
        public string Session { get; set; }
        public string Receiver { get; set; }
        public string ReceiverEn { get; set; }

        public string Receivers(string input) =>
         input switch
         {
             "ru" => Receiver,
             "en" => ReceiverEn,
             _ => Receiver
         };
    }
}