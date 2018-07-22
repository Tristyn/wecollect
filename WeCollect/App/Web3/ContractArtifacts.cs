using System.IO;
using WeCollect.App.Models;

namespace WeCollect.App.Web3
{
    public class ContractArtifacts
    {
        public ContractArtifact Test { get; }

        public ContractArtifact[] All { get; }

        public ContractArtifacts()
        {
            string dir = Path.Combine(
                Directory.GetCurrentDirectory(),
                "App", "Contracts", "bin");

            All = new[]
            {
                Test = new ContractArtifact(Path.Combine(dir, "test"), "Test")
            };
        }
    }

    public class ContractArtifact
    {
        private readonly string _path;

        public ContractArtifact(string path, string name)
        {
            _path = path;
            Name = name;
            Id = nameof(ContractDto) + name;
            Abi = File.ReadAllText(Path.ChangeExtension(_path, ".abi"));
            Bin = File.ReadAllText(Path.ChangeExtension(_path, ".bin"));
        }

        public string Abi { get; }

        public string Bin { get; }

        public string Name { get; }

        public string Id { get; }
    }
}
