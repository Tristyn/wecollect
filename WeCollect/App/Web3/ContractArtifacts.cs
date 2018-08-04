using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WeCollect.App.Extensions;
using WeCollect.App.Models;

namespace WeCollect.App.Web3
{
    public class ContractArtifacts
    {
        public ContractArtifact Test { get; }

        public ContractArtifact[] All { get; }

        public Dictionary<string, ContractArtifact> ByName { get; }

        private ContractArtifacts(Dictionary<string, ContractArtifact> contracts)
        {
            ByName = contracts;
            All = contracts.Values.ToArray();

            foreach (var method in typeof(ContractArtifacts).GetProperties(BindingFlags.Instance | BindingFlags.GetProperty)
                .Where(property => property.SetMethod.ReturnType.IsAssignableFrom(typeof(ContractArtifact))))
            {
                method.SetValue(this, contracts[method.Name]);
            }
            Test = contracts[nameof(Test)];
        }

        public static async Task<ContractArtifacts> Initialize()
        {
            string dir = Path.Combine(
                new Uri(Assembly.GetEntryAssembly().CodeBase).GetDirectory(),
                "App", "Contracts", "bin");
            
            var contractSourcePaths = Directory.GetFiles(dir, "*.bin", SearchOption.AllDirectories);

            var artifacts = await Task.WhenAll(contractSourcePaths.Select(contractSourcePath => Task.Run(() => new ContractArtifact(contractSourcePath))));
            var contracts = artifacts.ToDictionary(contract => contract.Name);

            return new ContractArtifacts(contracts);

        }
    }

    public class ContractArtifact
    {
        private readonly string _path;

        ILogger _log = Logger.GetLogger<ContractArtifact>();

        public ContractArtifact(string sourcePath)
            : this(sourcePath, sourcePath.Remove(sourcePath.LastIndexOf('.')).Remove(0, sourcePath.LastIndexOf(Path.DirectorySeparatorChar) + 1))
        {

        }

        public ContractArtifact(string path, string name)
        {
            _path = path;
            Name = name;
            Id = nameof(ContractDto) + name;
            try
            {
                Abi = File.ReadAllText(Path.ChangeExtension(_path, ".abi"));
                Bin = File.ReadAllText(Path.ChangeExtension(_path, ".bin"));
            }
            catch (DirectoryNotFoundException ex)
            {
                _log.LogError(ex);
            }
            catch (FileNotFoundException ex)
            {
                _log.LogError(ex);
            }
        }
        
        public string Abi { get; }

        public string Bin { get; }

        public string Source { get; set; }

        public string Name { get; }

        public string Id { get; }
    }
}
