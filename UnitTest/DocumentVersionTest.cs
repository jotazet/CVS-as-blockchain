using BlockchainLib;

namespace UnitTest
{
    [TestClass]
    public sealed class DocumentVersionTest
    {
        [TestMethod]
        public void Constructor_ValidParameters_ShouldInitializeProperties()
        {
            // Arrange
            int index = 1;
            DateTime timestamp = DateTime.UtcNow;
            string diff = "Sample diff";
            string previousHash = "abc123";

            // Act
            DocumentVersion documentVersion = new DocumentVersion(index, timestamp, diff, previousHash);

            // Assert
            Assert.AreEqual(index, documentVersion.Index);
            Assert.AreEqual(timestamp, documentVersion.Timestamp);
            Assert.AreEqual(diff, documentVersion.Diff);
            Assert.AreEqual(previousHash, documentVersion.PreviousHash);
            Assert.IsNotNull(documentVersion.Hash);
        }

        [TestMethod]
        public void Constructor_NegativeIndex_ShouldThrowException()
        {
            // Arrange
            int index = -1;
            DateTime timestamp = DateTime.UtcNow;
            string diff = "Sample diff";
            string previousHash = "abc123";

            // Act
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DocumentVersion(index, timestamp, diff, previousHash));
        }

        [TestMethod]
        public void CalculateHash_ShouldReturnConsistentHash()
        {
            // Arrange
            int index = 1;
            DateTime timestamp = DateTime.UtcNow;
            string diff = "Sample diff";
            string previousHash = "abc123";
            DocumentVersion documentVersion = new DocumentVersion(index, timestamp, diff, previousHash);

            // Act
            string hash1 = documentVersion.CalculateHash();
            string hash2 = documentVersion.CalculateHash();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void CalculateHash_ShouldReturnDifferentHashForDifferentInputs()
        {
            // Arrange
            int index1 = 1;
            DateTime timestamp1 = DateTime.UtcNow;
            string diff1 = "Sample diff 1";
            string previousHash1 = "abc123";
            int index2 = 2;
            DateTime timestamp2 = DateTime.UtcNow.AddMinutes(1);
            string diff2 = "Sample diff 2";
            string previousHash2 = "def456";
            DocumentVersion documentVersion1 = new DocumentVersion(index1, timestamp1, diff1, previousHash1);
            DocumentVersion documentVersion2 = new DocumentVersion(index2, timestamp2, diff2, previousHash2);
            // Act
            string hash1 = documentVersion1.CalculateHash();
            string hash2 = documentVersion2.CalculateHash();
            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
