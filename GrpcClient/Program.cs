using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;
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

            var teamsCall = client.GetAllTeams(new EmptyRequest());

            Console.WriteLine("-------------- Unary call -----------");
            Console.WriteLine("-------------- Get one team by its ID -----------");

            var team = client.GetTeamById(new GetTeamRequest { Id = (ulong)new Random().Next(1, 10) });
            Console.WriteLine($"Received team: {JsonSerializer.Serialize(team)}");

            Console.WriteLine("-------------- Server stream call -----------");
            await foreach (var response in teamsCall.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"Received Team {response.Name}");
            }
            
            Console.ReadKey();
        }
    }
}
