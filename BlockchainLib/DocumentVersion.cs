using System.Security.Cryptography;

namespace BlockchainLib
{
    public class DocumentVersion
    {
        private int index;
        private DateTime timestamp;
        private string diff;
        private string previous_hash;
        private string hash;

        public int Index => index;
        public DateTime Timestamp => timestamp;
        public string Diff => diff;
        public string PreviousHash => previous_hash;
        public string Hash => hash;

        public DocumentVersion(int index, DateTime timestamp, string diff, string previous_hash)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");
            this.index = index;
            this.timestamp = timestamp;
            this.diff = diff;
            this.previous_hash = previous_hash;
            this.hash = CalculateHash();
        }

        public string CalculateHash()
        {
            string input = index.ToString() + timestamp.ToString() + diff + previous_hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
