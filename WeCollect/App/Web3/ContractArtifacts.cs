﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WeCollect.App.Models;

namespace WeCollect.App.Web3
{
    public class ContractArtifacts
    {
        public static ContractArtifacts artifacts;

        public ContractArtifact Test { get; }

        public ContractArtifact Cards { get; }

        public ContractArtifact[] All { get; }
        
        public Dictionary<string, ContractDto> DocumentsByName { get; set; }

        public Dictionary<string, ContractArtifact> ByName { get; }
        

        private ContractArtifacts(Dictionary<string, ContractArtifact> contracts)
        {
            ByName = contracts;
            All = contracts.Values.ToArray();
            
            //Test = contracts[nameof(Test)];
            Cards = contracts.Where(contract => contract.Key.EndsWith(nameof(Cards))).FirstOrDefault().Value;
        }

        public static async Task<ContractArtifacts> Initialize()
        {
            string dir = Path.Combine(
                new Uri(Assembly.GetEntryAssembly().Location).GetDirectory(),
                "App", "Contracts");

            var contractSourcePaths = Directory.GetFiles(dir, "*.bin", SearchOption.TopDirectoryOnly);

            var artifacts = await Task.WhenAll(contractSourcePaths.Select(contractSourcePath => Task.Run(() => new ContractArtifact(contractSourcePath))));
            var contracts = artifacts.ToDictionary(contract => contract.Name);

            return new ContractArtifacts(contracts);

        }
    }

    public class ContractArtifact
    {
        private readonly string _path;

        ILogger _log = Log.GetLogger<ContractArtifact>();

        public ContractArtifact(string sourcePath)
            : this(sourcePath, Path.GetFileNameWithoutExtension(sourcePath))
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
