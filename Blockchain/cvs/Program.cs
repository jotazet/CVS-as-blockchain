using BlockchainLib;

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
                           add - add file to program
                           update - add new version of file to blockchain
                           isvalid - check if blockchain is valid
                           save - save blockchain to file
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
                // Save not implemented yet
                case "s":
                case "save":
                    Console.WriteLine(blockchain.getEntireBlockchain());
                    Console.WriteLine("File saved");
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
