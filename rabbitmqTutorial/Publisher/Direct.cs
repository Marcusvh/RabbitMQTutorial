using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    public class Direct
    {
        public async Task PublishDirect(string exchangeKey = "game.direct", string routingKey = "player.move")
        {
            // connection
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "user", Password = "pass" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            // var
            string msg = "";

            // declare type of exchange
            await channel.ExchangeDeclareAsync(exchangeKey, ExchangeType.Direct);

            // sends the message
            msg = "Game started";
            await channel.BasicPublishAsync(
                exchangeKey,
                "game.start",
                Encoding.UTF8.GetBytes(msg)
                );
            Console.WriteLine($" [x] Sent {msg}");

            msg = "Player 1 moved";
            await channel.BasicPublishAsync(
                exchange: exchangeKey,
                routingKey: routingKey,
                body: Encoding.UTF8.GetBytes(msg)
                );
            Console.WriteLine($" [x] Sent {msg}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
