using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FormulaOne;
using Grpc.Core;

namespace GrpcServer.Services
{
    public class TeamsService: FormulaOne.TeamsService.TeamsServiceBase
    {
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
    }
}