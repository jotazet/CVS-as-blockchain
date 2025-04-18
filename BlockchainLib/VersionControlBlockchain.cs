﻿using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BlockchainLib
{
    public class VersionControlBlockchain
    {
        private List<DocumentVersion> chain = new List<DocumentVersion>();

        public VersionControlBlockchain(string init)
        {
            CreateGenesisBlock(init);
        }

        private void CreateGenesisBlock(string init)
        {
            DocumentVersion genesisBlock = new DocumentVersion(0, DateTime.Now, init, "0");
            chain.Add(genesisBlock);
        }

        public string GetLatestVersionBlock()
        {
            if (chain.Count == 0)
                throw new InvalidOperationException("The blockchain is empty.");
            return chain.Last().Hash;
        }

        public bool IsChainValid()
        {
            for (int i = 1; i < chain.Count; i++)
            {
                var currentBlock = chain[i];
                var previousBlock = chain[i - 1];
                if (currentBlock.Hash != currentBlock.CalculateHash())
                    return false;
                if (currentBlock.PreviousHash != previousBlock.Hash)
                    return false;
            }
            return true;
        }

        public string GetDocumentVersion(int index)
        {
            if (index == -1)
            {
                index = chain.Count - 1;
            } 
            else if (index < 0 || index >= chain.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Invalid version index.");

            string document = chain[0].Diff;
            for (int i = 0; i <= index; i++)
            {
                document = ApplyDiff(document, chain[i].Diff);
            }
            return document;
        }

        public string GetBlockchainJson()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(chain, options);
            return json;
        }

        public void LoadBlochchainJson(string content)
        {
            using (JsonDocument document = JsonDocument.Parse(content))
            {
                chain.Clear();

                foreach (JsonElement element in document.RootElement.EnumerateArray())
                {
                    int index = element.GetProperty("Index").GetInt32();
                    DateTime timestamp = element.GetProperty("Timestamp").GetDateTime();
                    string diff = element.GetProperty("Diff").GetString();
                    string previousHash = element.GetProperty("PreviousHash").GetString();
                    string hash = element.GetProperty("Hash").GetString();

                    DocumentVersion block = new DocumentVersion(index, timestamp, diff, previousHash);
                    chain.Add(block);
                }
                
            }
        }

        public void PrintRawBlock(int index)
        {
            Console.WriteLine("--START--");
            Console.WriteLine($"Index: {chain[index].Index}\nTimestamp: {chain[index].Timestamp}\nDiff: {chain[index].Diff}\nPreviousHash: {chain[index].PreviousHash}\nHash: {chain[index].Hash}");
            Console.WriteLine("--STOP--");
        }

        public void AddNewVersion(string document)
        {
            string previousDocument = GetDocumentVersion(chain.Count - 1);
            string diff = CalculateDiff(previousDocument, document);
            DocumentVersion newVersion = new DocumentVersion(
                chain.Count,
                DateTime.Now,
                diff,
                GetLatestVersionBlock()
            );

            chain.Add(newVersion);
        }

        private string ApplyDiff(string original, string diff)
        {
            var originalLines = original.Split('\n');
            var diffLines = diff.Split('\n');
            var result = new StringBuilder();

            for (int lineIndex = 0; lineIndex < diffLines.Length; lineIndex++)
            {
                if (diffLines[lineIndex].StartsWith("~"))
                {
                    result.Append($"{diffLines[lineIndex].Substring(1)}");
                }
                else
                {
                    result.Append(originalLines[lineIndex]);
                }

                if (lineIndex < diffLines.Length - 1)
                {
                    result.Append("\n");
                }
            }

            return result.ToString();
        }

        private string CalculateDiff(string oldDocument, string newDocument)
        {
            var oldLines = oldDocument.Split('\n');
            var newLines = newDocument.Split('\n');
            var diff = new StringBuilder();

            for (int lineIndex = 0; lineIndex < newLines.Length; lineIndex++)
            {
                if (lineIndex >= oldLines.Length || oldLines[lineIndex] != newLines[lineIndex])
                {
                    diff.Append($"~{newLines[lineIndex]}");
                }
                else
                {
                    diff.Append("");
                }

                if (lineIndex < newLines.Length - 1)
                {
                    diff.Append("\n");
                }
            }

            return diff.ToString();
        }
    }
}