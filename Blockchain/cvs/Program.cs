using BlockchainLib;

namespace cvs;

class Program
{
    static void Main(string[] args)
    {
        var blockchain = new VersionControlBlockchain("Tekst zero");
        blockchain.AddNewVersion("Tekst zero\nTekst jeden\nTekst dwa");
        blockchain.AddNewVersion("Tekst zero\nTekst jeden\nTekst 2");
        blockchain.AddNewVersion("Tekst zero\nTekst trzy");
        blockchain.AddNewVersion("Tekst 0\nTekst trzy");

        Console.WriteLine(blockchain.GetDocumentVersion(2));

        Console.WriteLine($"IsChainValid: {blockchain.IsChainValid()}");

        blockchain.printRawBlock(0);
        blockchain.printRawBlock(1);
        blockchain.printRawBlock(2);
        blockchain.printRawBlock(3);
        blockchain.printRawBlock(4);

    }
}
