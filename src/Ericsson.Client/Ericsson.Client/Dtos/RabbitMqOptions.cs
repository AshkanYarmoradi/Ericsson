namespace Ericsson.Client.Dtos
{
    public class RabbitMqOptions
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Exchange { get; set; }
    }
}