// See https://aka.ms/new-console-template for more information

using Publisher;
Console.WriteLine("Hello, World!");

//Direct direct = new Direct();
//await direct.PublishDirect();

//Fanout fanout = new Fanout();
//await fanout.FanoutPublish();

Topic topic = new Topic();
await topic.TopicPublish();