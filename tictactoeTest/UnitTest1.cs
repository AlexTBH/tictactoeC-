using ConsoleApp6;

namespace tictactoeTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_PlayerCreation_And_SymbolCreation()
        {

            IUserInterface ui = new AutoresponeUi();

            Player player1 = Player.NewPlayer(ui);
            Player player2 = Player.NewPlayer(ui);


            string player1Name = player1.Name;
            string player2Name = player2.Name;
            string player1Symbol = player1.Symbol;
            string player2Symbol = player2.Symbol;

            Assert.AreEqual("Alex", player1.Name);
            Assert.AreEqual("Kalle", player2.Name);
            Assert.AreEqual("O", player1.Symbol);
            Assert.AreEqual("X", player2.Symbol);



        }
    }
}