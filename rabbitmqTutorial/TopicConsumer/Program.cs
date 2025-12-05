// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
Console.WriteLine("Hello, World!");

// Wildcard Filtering
// Routing keys can contain words separated by "." such as:
// - game.123.turn.start
// - game.123.player.p1.move
// - chat.global.message
// - game.*.turn.*
// - game.# (everything starting with game)
// - *.123.*

// connection
var factory = new ConnectionFactory() { HostName = "localhost", UserName = "user", Password = "pass" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// var
string exchangeKey = "game.topic";

// declare the exchange type
await channel.ExchangeDeclareAsync(exchangeKey, ExchangeType.Topic);

// declare the que and gets its name
var que = await channel.QueueDeclareAsync();
string queName = que.QueueName;

// binds what the que should listen on
await channel.QueueBindAsync(queName, exchangeKey, "game.123.#");
await channel.QueueBindAsync(queName, exchangeKey, "game.#.attack");
await channel.QueueBindAsync(queName, exchangeKey, "*.player.*.move");
await channel.QueueBindAsync(queName, exchangeKey, "player.#");


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