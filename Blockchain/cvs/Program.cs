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
        string fileName = string.Empty; // Declare and initialize fileName here  
        string filePath = string.Empty; // Declare and initialize filePath here  
        string documentContent = string.Empty; // Declare and initialize documentContent here  

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
                    var fileDir = Console.ReadLine();
                    fileName = Path.GetFileName(fileDir);
                    filePath = Path.GetDirectoryName(fileDir);

                    try
                    {
                        documentContent = File.ReadAllText(Path.Combine(filePath, fileName));
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
                    if (fileName != "")
                    {
                        Console.WriteLine($"Save {fileName} in: ");
                        filePath = Console.ReadLine();
                        var saveLocalization = Path.Combine(filePath, fileName);
                        File.WriteAllText(saveLocalization, blockchain.GetDocumentVersion(-1));
                        Console.WriteLine($"File saved to: {saveLocalization}");
                    }
                    else
                    {
                        Console.WriteLine("Your file do not have name!");
                    }
                    break;

                case "u":
                case "update":
                    try
                    {
                        var newDocumentContent = File.ReadAllText(Path.Combine(filePath, fileName));
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
                    // Remove vulnerability Path Traversal (../../)
                    // Insted using Console.ReadLine write new function

                    var mainFolder = Path.Combine(Directory.GetCurrentDirectory(), "saves");
                    Directory.CreateDirectory(mainFolder);

                    var saveFileName = fileName + ".json";
                    var savePath = Path.Combine(mainFolder, saveFileName);

                    if (File.Exists(savePath))
                    {
                        Console.WriteLine($"A file named '{Path.GetFileNameWithoutExtension(saveFileName)}' already exists.\nEnter a new name or press Enter to overwrite:");
                        while (true)
                        {
                            var newFileName = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(newFileName))
                            {
                                Console.WriteLine("Overwriting the existing file...");
                                break;
                            }

                            saveFileName = newFileName + ".json";
                            savePath = Path.Combine(mainFolder, saveFileName);

                            if (!File.Exists(savePath))
                            {
                                break;
                            }

                            Console.WriteLine($"A file named '{Path.GetFileNameWithoutExtension(newFileName)}' already exists.\nPlease enter a different name or press Enter to overwrite:");
                        }
                    }

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
                                filePath = string.Empty;
                                fileName = Path.GetFileNameWithoutExtension(selectedBlockPath);
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
