using BlockchainLib;

namespace UnitTest
{
    [TestClass]
    public sealed class VersionControlBlockchainTest
    {
        [TestMethod]
        public void Constructor_ValidParameters_ShouldInitializeProperties()
        {
            // Arrange
            string init = "Test";
            VersionControlBlockchain blockchain = new VersionControlBlockchain(init);
            // Act
            string latestVersionHash = blockchain.GetLatestVersionBlock();
            // Assert
            Assert.IsNotNull(latestVersionHash);
        }
    }
}
