using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using FormulaOne;
using Grpc.Core;
using Grpc.Net.Client;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new TeamsService.TeamsServiceClient(channel);

            Console.WriteLine("-------------- Unary call -----------");
            Console.WriteLine("-------------- Get one team by its ID -----------");

            Team team = client.GetTeamById(new GetTeamRequest { Id = new Random().Next(1, 10) });
            Console.WriteLine($"Received team: {JsonSerializer.Serialize(team)}");

            Console.WriteLine("-------------- Server stream call -----------");
            var teamsCall = client.GetAllTeams(new EmptyRequest());
            await foreach (var response in teamsCall.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"Received Team {response.Name}");
            }

            Console.WriteLine($" ## Call ended with status {teamsCall.GetStatus().StatusCode} ##");

            Console.WriteLine("-------------- Client stream call -----------");
            var driversCall = client.AddDrivers();

            var drivers = GetDrivers();
            Console.WriteLine("-------------- Start sending drivers -----------");

            foreach (var driver in drivers)
            {
                await driversCall.RequestStream.WriteAsync(driver);
            }

            await driversCall.RequestStream.CompleteAsync();

            Console.WriteLine("-------------- Finished sending drivers -----------");

            Summary summary = await driversCall.ResponseAsync;

            Console.WriteLine($"Summary: Received {summary.ItemsReceived} drivers from client.");

            Console.WriteLine("-------------- Bi-directional stream call -----------");
            Console.WriteLine("-------------- Start chatting ------------");
            Console.WriteLine("## Quit chat by pressing q or Q ##");

            var chatCall = client.TeamChat();

            // Start response reader task
            var responseReaderTask = Task.Run(async () =>
            {
                while (await chatCall.ResponseStream.MoveNext())
                {
                    var response = chatCall.ResponseStream.Current;
                    Console.WriteLine(response.Message);
                }
            });

            bool isChatting = true;
            while (isChatting)
            {
                var message = Console.ReadLine();
                if (message.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    isChatting = false;
                    await chatCall.RequestStream.CompleteAsync();
                }
                else
                {
                    await chatCall.RequestStream.WriteAsync(new TeamNote {Message = message});
                }
            }

            Console.WriteLine("---------- Chat closed ------------");

            Console.WriteLine("*** Press any key to close application ***");
            Console.ReadKey();
        }

        static List<Driver> GetDrivers()
        {
            return new List<Driver>
            {
                new Driver {FirstName = "Lewis", LastName = "Hamilton"},
                new Driver {FirstName = "Valteri", LastName = "Bottas"},
                new Driver {FirstName = "Max", LastName = "Verstappen"},
                new Driver {FirstName = "Sergio", LastName = "Perez"},
                new Driver {FirstName = "Lando", LastName = "Norris"},
                new Driver {FirstName = "Daniel", LastName = "Ricciardo"},
                new Driver {FirstName = "Charles", LastName = "LeClerc"},
                new Driver {FirstName = "Carlos", LastName = "Sainz"},
            };
        }
    }
}
