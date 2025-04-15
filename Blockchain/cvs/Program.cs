using BlockchainLib;
using System.Text.Json;

namespace cvs;

class Program
{
    static void Main(string[] args)
    {
        RunConsoleUI();
    }
    static void RunConsoleUI()
    {
        var blockchain = new VersionControlBlockchain("");
        // Add file name
        var file = string.Empty;
        var documentContent = string.Empty;

        Console.WriteLine("File version control using blockchain");
        while (true)
        {
            Console.Write("> ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "h":
                case "help":
                    Console.WriteLine("""
                           help - show all commands
                           add - add file to blochchain
                           file - export blockchain to file
                           update - add new version of file to blockchain
                           isvalid - check if blockchain is valid
                           save - save blockchain to json file
                           load - load blockchain from json file
                           clear - clear console history
                           exit - exit program
                           (You can write single letter of command to exectute it too)
                           """);
                    break;

                case "e":
                case "exit":
                    return;

                case "a":
                case "add":
                    Console.WriteLine("Text file path: ");
                    file = Console.ReadLine();
                    try
                    {
                        documentContent = File.ReadAllText(file);
                        blockchain = new VersionControlBlockchain(documentContent);
                        Console.WriteLine("File added");
                    }
                    catch
                    {
                        Console.WriteLine("File do not found...");
                    }
                    break;

                case "f":
                case "file":

                    break;
                case "u":
                case "update":
                    try
                    {
                        var newDocumentContent = File.ReadAllText(file);
                        if (newDocumentContent != documentContent)
                        {
                            documentContent = newDocumentContent;
                            blockchain.AddNewVersion(documentContent);
                            Console.WriteLine("New block created");
                        }
                        else
                        {
                            Console.WriteLine("Content do not changed");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("File do not found...");
                    }
                    break;

                case "i":
                case "isvalid":
                    Console.WriteLine($"Chain is {(blockchain.IsChainValid() ? "valid" : "invalid")}");
                    break;

                case "s":
                case "save":
                    var mainFolder = Path.Combine(Directory.GetCurrentDirectory(), "saves");
                    Directory.CreateDirectory(mainFolder);

                    var fileName = Path.GetFileName(file) + ".json";
                    var savePath = Path.Combine(mainFolder, fileName);

                    File.WriteAllText(savePath, blockchain.GetBlockchainJson());
                    Console.WriteLine($"Blockchain saved to: {savePath}");
                    break;

                case "l":
                case "load":
                    var savesFolder = Path.Combine(Directory.GetCurrentDirectory(), "saves");
                    if (!Directory.Exists(savesFolder))
                    {
                        Console.WriteLine("No saved blocks found.");
                        break;
                    }

                    var blockFiles = Directory.GetFiles(savesFolder, "*.json");
                    if (blockFiles.Length == 0)
                    {
                        Console.WriteLine("No saved blocks found.");
                        break;
                    }

                    Console.WriteLine("Available blocks:");
                    for (int i = 0; i < blockFiles.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}: {Path.GetFileName(blockFiles[i])}");
                    }

                    Console.Write("Enter the number of the block to load: ");
                    if (int.TryParse(Console.ReadLine(), out int blockChoice) && blockChoice > 0 && blockChoice <= blockFiles.Length)
                    {
                        var selectedBlockPath = blockFiles[blockChoice - 1];
                        try
                        {
                            var blockchainJsonData = File.ReadAllText(selectedBlockPath);
                            blockchain.LoadBlochchainJson(blockchainJsonData);
                            if (blockchain.IsChainValid())
                            {
                                Console.WriteLine($"Blockchain loaded from: {selectedBlockPath}");
                            }
                            else
                            {
                                Console.WriteLine("Corupted block file");
                            }
                        }
                        catch
                        {
                        Console.WriteLine("Failed to load the selected block.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                    break;

                case "c":
                case "clear":
                    Console.Clear();
                    break;

                default:
                    Console.WriteLine("Command do not found");
                    break;
            }
        }
    }
}
