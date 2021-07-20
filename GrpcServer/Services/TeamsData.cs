using FormulaOne;

namespace GrpcServer.Services
{
    public static class TeamsData
    {
        public static Team[] Teams
        {
            get
            {
                return new[]
                {
                    new Team {Id = 1, Name = "Red Bull Racing", Manufacturer = "Honda", FirstDriver = "Max Verstappen", SecondDriver = "Sergio Perez", Country = "Oostenrijk"},
                    new Team {Id = 2, Name = "Mercedes-AMG Petronas Motorsport", Manufacturer = "Mercedes", FirstDriver = "Lewis Hamilton", SecondDriver = "Valtteri Bottas", Country = "Duitsland"},
                    new Team {Id = 3, Name = "Alfa Romeo Racing", Manufacturer = "Ferrari", FirstDriver = "Kimi Raikkonen", SecondDriver = "Antonio Giovinazzi", Country = "Italië"},
                    new Team {Id = 4, Name = "Scuderia Alpha Tauri", Manufacturer = "Honda", FirstDriver = "Pierre Gasly", SecondDriver = "Yuki Tsunoda", Country = "Italië"},
                    new Team {Id = 5, Name = "Scuderia Ferrari Mission Winnow", Manufacturer = "Ferrari", FirstDriver = "Charles Leclerc", SecondDriver = "Carlos Sainz", Country = "Italië"},
                    new Team {Id = 6, Name = "McLaren Racing", Manufacturer = "Mercedes", FirstDriver = "Daniel Ricciardo", SecondDriver = "Lando Norris", Country = "Groot Brittanië"},
                    new Team {Id = 7, Name = "Aston Martin F1 Team", Manufacturer = "Mercedes", FirstDriver = "Sebastian Vettel", SecondDriver = "Lance Stroll", Country = "Groot Brittanië"},
                    new Team {Id = 8, Name = "Alpine F1", Manufacturer = "Renault", FirstDriver = "Esteban Ocon", SecondDriver = "Fernando Alonso", Country = "Groot Brittanië"},
                    new Team {Id = 9, Name = "Williams Racing", Manufacturer = "Mercedes", FirstDriver = "George Russel", SecondDriver = "Nicholas Latifi", Country = "Groot Brittanië"},
                    new Team {Id = 10, Name = "Haas F1", Manufacturer = "Ferrari", FirstDriver = "Nikita Mazepin", SecondDriver = "Mick Schumacher", Country = "Verenigde Staten" }
                };
            }
        }
    }
}