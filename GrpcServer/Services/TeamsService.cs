using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormulaOne;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcServer.Services
{
    public class TeamsService: FormulaOne.TeamsService.TeamsServiceBase
    {
        private ILogger _logger;

        public TeamsService(ILogger<TeamsService> logger)
        {
            _logger = logger;
        }

        public override async Task GetAllTeams(EmptyRequest request, IServerStreamWriter<TeamHeader> responseStream, ServerCallContext context)
        {
            foreach (var team in TeamsData.Teams)
            {
                await responseStream.WriteAsync(new TeamHeader { Id = team.Id, Name = team.Name, Manufacturer = team.Manufacturer });
            }
        }

        public override Task<Team> GetTeamById(GetTeamRequest request, ServerCallContext context)
        {
            var team = TeamsData.Teams.First(t => t.Id == request.Id);
            return Task.FromResult(team);
        }

        public override async Task<Summary> AddDrivers(IAsyncStreamReader<Driver> requestStream, ServerCallContext context)
        {
            var currentDriverCount = DriversData.Drivers.Count;

            while (await requestStream.MoveNext())
            {
                var driver = requestStream.Current;
                DriversData.AddDriver(driver);
            }

            return new Summary {ItemsReceived = DriversData.Drivers.Count - currentDriverCount};
        }

        // Server side implementatie optionele oefening
        public override async Task GetAllDrivers(EmptyRequest request, IServerStreamWriter<Driver> responseStream, ServerCallContext context)
        {
            foreach (var driver in DriversData.Drivers)
            {
                await responseStream.WriteAsync(driver);
            }
        }

        public override async Task TeamChat(IAsyncStreamReader<TeamNote> requestStream, IServerStreamWriter<TeamNote> responseStream, ServerCallContext context)
        {
            var answers = PitwallAnswers();
            while (await requestStream.MoveNext())
            {
                var teamNote = requestStream.Current;
                _logger.Log(LogLevel.Debug, $"Received message: {teamNote.Message}");

                var idx = new Random().Next(1, 9);
                await responseStream.WriteAsync(new TeamNote{ Message = $"Pitwall: {answers[idx]}" });
            }
        }

        private List<string> PitwallAnswers()
        {
            return new List<string>
            {
                "Send it!!",
                "Ok, we'll look into it.",
                "Copy that!",
                "We received a five second penalty for leaving the track and gaining an advantage.",
                "That's Pole position!!!!",
                "Box, box, box!",
                "Stop the car. Stop the car!",
                "Turn of the engine please.",
                "Switch to mode 2-0-7"
            };
        }
    }
}