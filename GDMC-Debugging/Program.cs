using GDMCHttp;
using GDMCHttp.Commands;

Console.WriteLine("Hello, World!");

Connection connection = new Connection();

connection.SendCommandSync(new Say("Client connected"));


connection.SendCommandSync(new Say("Client done"));