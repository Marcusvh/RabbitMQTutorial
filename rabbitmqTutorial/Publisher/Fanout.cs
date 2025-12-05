using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    public class Fanout
    {
        public async Task FanoutPublish(string exchangeKey = "game.fanouts")
        {
            // connection
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "user", Password = "pass" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            // var
            string msg = "Player 2 has joined the lobby!";
            await channel.ExchangeDeclareAsync(exchangeKey, ExchangeType.Fanout);
            await channel.BasicPublishAsync(exchangeKey, "", Encoding.UTF8.GetBytes(msg));

            Console.WriteLine($" [x] Sent {msg}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
