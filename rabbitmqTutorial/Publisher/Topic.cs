using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    public class Topic
    {
        public async Task TopicPublish(string exchangeKey = "game.topic")
        {
            // connection
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "user", Password = "pass" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            // var
            string msg = "player 1 moved in game 123";

            // declare exchange type
            await channel.ExchangeDeclareAsync(exchangeKey, ExchangeType.Topic);

            // sends/broadcast the message
            await channel.BasicPublishAsync(
                exchangeKey,
                "game.123.player.p1.move",
                body: Encoding.UTF8.GetBytes(msg));
            Console.WriteLine($" [x] Sent {msg}");

            msg = "Game 100 turn started";
            await channel.BasicPublishAsync(
                exchangeKey,
                "game.100.turn.start",
                body: Encoding.UTF8.GetBytes(msg));
            Console.WriteLine($" [x] Sent {msg}");

            msg = "game 100 player p1 has attacked";
            await channel.BasicPublishAsync(
                exchangeKey,
                "game.100.player.p1.attack",
                body: Encoding.UTF8.GetBytes(msg));
            Console.WriteLine($" [x] Sent {msg}");

            msg = "Player p2 has disconnected";
            await channel.BasicPublishAsync(
                exchangeKey,
                "player.p2.disconnect",
                body: Encoding.UTF8.GetBytes(msg));
            Console.WriteLine($" [x] Sent {msg}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
