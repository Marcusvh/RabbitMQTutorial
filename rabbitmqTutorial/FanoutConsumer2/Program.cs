// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");

// Broadcast to everyone
// ignores keys and such, just sends to all bound queues
// used for 
// Game events
// Game state updates
// Lobby announments

// connection
var factory = new ConnectionFactory() { HostName = "localhost", UserName = "user", Password = "pass" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// var
string exchangeKey = "game.fanouts";
// declare exchange type
await channel.ExchangeDeclareAsync(exchangeKey, ExchangeType.Fanout);

// create a new que and gets its name
var que = await channel.QueueDeclareAsync();
string queName = que.QueueName;

// bind the que on what it should listen to. (exchangeKey)
await channel.QueueBindAsync(queName, exchangeKey, "");

// callback for async listening ~~ 
var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
    return Task.CompletedTask;
};

// consumes the que
await channel.BasicConsumeAsync(queName, true, consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();