// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Numerics;
using System.Text;

Console.WriteLine("Hello, World!");
// Routing/sending by key
// Useful for game commands like
// game.start
// game.end
// player.move


// connection
var factory = new ConnectionFactory() { HostName = "localhost", UserName = "user", Password = "pass" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// var
string exchangeKey = "game.direct";

// declare exchange type
await channel.ExchangeDeclareAsync(exchangeKey, ExchangeType.Direct);

// declare a que
var que = await channel.QueueDeclareAsync();
string queName = que.QueueName;

// what the que should listen on
await channel.QueueBindAsync(queName, exchangeKey, "player.move");

// callback for async listening ~~
var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
    return Task.CompletedTask;
};

// consumes/uses the consumer
await channel.BasicConsumeAsync(queName, true, consumer);

Console.WriteLine("press enter to exit");
Console.ReadLine();