using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FormulaOne;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcServer.Services
{
    public class TeamsService: FormulaOne.TeamsService.TeamsServiceBase
    {
        private DriversData _drivers = new DriversData();
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
            var currentDriverCount = _drivers.Drivers.Count;

            while (await requestStream.MoveNext())
            {
                var driver = requestStream.Current;
                _drivers.AddDriver(driver);
            }

            return new Summary {ItemsReceived = _drivers.Drivers.Count - currentDriverCount};
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