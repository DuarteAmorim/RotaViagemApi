using RotaViagem.application.Utils;

namespace RotaViagem.Test.Utils
{
    public class DijkstraServiceTests
    {
        [Fact]
        public void EncontrarRotaMaisBarata_RetornaRotaSimples()
        {
            // Arrange
            var grafo = new Dictionary<string, List<(string, double)>>
            {
                { "A", new List<(string, double)> { ("B", 5) } },
                { "B", new List<(string, double)>() }
            };
            var dijkstra = new DijkstraService(grafo);

            // Act
            var (rota, custo) = dijkstra.FindRotaMaisBarata("A", "B");

            // Assert
            Assert.Equal(new List<string> { "A", "B" }, rota);
            Assert.Equal(5, custo);
        }

        [Fact]
        public void EncontrarRotaMaisBarata_RetornaRotaComMultiplosSaltos()
        {
            // Arrange
            var grafo = new Dictionary<string, List<(string, double)>>
            {
                { "A", new List<(string, double)> { ("B", 5), ("C", 10) } },
                { "B", new List<(string, double)> { ("C", 3) } },
                { "C", new List<(string, double)>() }
            };
            var dijkstra = new DijkstraService(grafo);

            // Act
            var (rota, custo) = dijkstra.FindRotaMaisBarata("A", "C");

            // Assert
            Assert.Equal(new List<string> { "A", "B", "C" }, rota);
            Assert.Equal(8, custo);
        }

        [Fact]
        public void EncontrarRotaMaisBarata_RetornaRotaVaziaSeNaoEncontrar()
        {
            // Arrange
            var grafo = new Dictionary<string, List<(string, double)>>
            {
                { "A", new List<(string, double)> { ("B", 5) } },
                { "B", new List<(string, double)>() }
            };
            var dijkstra = new DijkstraService(grafo);

            // Act
            var (rota, custo) = dijkstra.FindRotaMaisBarata("A", "C");

            // Assert
            Assert.Empty(rota);
            Assert.Equal(double.PositiveInfinity, custo);
        }

        [Fact]
        public void EncontrarRotaMaisBarata_RetornaRotaQuandoOrigemEDestinoSaoIguais()
        {
            // Arrange
            var grafo = new Dictionary<string, List<(string, double)>>
            {
                { "A", new List<(string, double)> { ("B", 5) } },
                { "B", new List<(string, double)>() }
            };
            var dijkstra = new DijkstraService(grafo);

            // Act
            var (rota, custo) = dijkstra.FindRotaMaisBarata("A", "A");

            // Assert
            Assert.Equal(new List<string> { "A" }, rota);
            Assert.Equal(0, custo);
        }
    }
}
