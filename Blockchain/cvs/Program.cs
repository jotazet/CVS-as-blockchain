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
        Console.WriteLine(blockchain.GetDocumentVersion(0));
        Console.WriteLine("---");
        Console.WriteLine(blockchain.GetDocumentVersion(1));
        Console.WriteLine("---");
        Console.WriteLine(blockchain.GetDocumentVersion(2));
        Console.WriteLine("---");
        Console.WriteLine(blockchain.GetDocumentVersion(3));
    }
}
