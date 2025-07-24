using Moq;
using RotaViagem.application.Interfaces;
using RotaViagem.application.Services;
using RotaViagem.domain.Entities;

namespace RotaViagem.Test.Services
{

    public class RotaServiceTests
    {
        private readonly Mock<IDataService<Rota>> _dataServiceMock;
        private readonly IRotaService _iRotaService;

        public RotaServiceTests()
        {
            _dataServiceMock = new Mock<IDataService<Rota>>();
            _iRotaService = new RotaService(_dataServiceMock.Object);
        }

        [Fact]
        public async Task CriarAsync_AdicionaRotaEGrava()
        {
            var rotas = new List<Rota>();
            _dataServiceMock.Setup(ds => ds.LoadAsync()).ReturnsAsync(rotas);
            _dataServiceMock.Setup(ds => ds.SaveAsync(It.IsAny<List<Rota>>())).Returns(Task.CompletedTask);

            var novaRota = new Rota("GRU", "JFK", 1500);

            var resultado = await _iRotaService.CreateAsync(novaRota);

            Assert.Contains(novaRota, rotas);
            Assert.Equal(novaRota, resultado);
            _dataServiceMock.Verify(ds => ds.SaveAsync(rotas), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_AtualizaRotaExistente()
        {
            var rotaExistente = new Rota("GRU", "JFK", 1500);
            var rotas = new List<Rota> { rotaExistente };
            _dataServiceMock.Setup(ds => ds.LoadAsync()).ReturnsAsync(rotas);
            _dataServiceMock.Setup(ds => ds.SaveAsync(It.IsAny<List<Rota>>())).Returns(Task.CompletedTask);

            var rotaAtualizada = new Rota("GRU", "LAX", 2000);

            var resultado = await _iRotaService.UpdateAsync(rotaExistente.Id.ToString(), rotaAtualizada);

            Assert.True(resultado);
            Assert.Equal("LAX", rotaExistente.Destino);
            Assert.Equal(2000, rotaExistente.Preco);
            _dataServiceMock.Verify(ds => ds.SaveAsync(rotas), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_RotaNaoExistente_RetornaFalse()
        {
            var rotas = new List<Rota>();
            _dataServiceMock.Setup(ds => ds.LoadAsync()).ReturnsAsync(rotas);

            var rotaAtualizada = new Rota("GRU", "LAX", 2000);

            var resultado = await _iRotaService.UpdateAsync(Guid.NewGuid().ToString(), rotaAtualizada);

            Assert.False(resultado);
            _dataServiceMock.Verify(ds => ds.SaveAsync(It.IsAny<List<Rota>>()), Times.Never);
        }

        [Fact]
        public async Task DeletarAsync_RemoveRotaExistente()
        {
            var rotaExistente = new Rota("GRU", "JFK", 1500);
            var rotas = new List<Rota> { rotaExistente };
            _dataServiceMock.Setup(ds => ds.LoadAsync()).ReturnsAsync(rotas);
            _dataServiceMock.Setup(ds => ds.SaveAsync(It.IsAny<List<Rota>>())).Returns(Task.CompletedTask);

            var resultado = await _iRotaService.DeleteAsync(rotaExistente.Id.ToString());

            Assert.True(resultado);
            Assert.DoesNotContain(rotaExistente, rotas);
            _dataServiceMock.Verify(ds => ds.SaveAsync(rotas), Times.Once);
        }

        [Fact]
        public async Task DeletarAsync_RotaNaoExistente_RetornaFalse()
        {
            var rotas = new List<Rota>();
            _dataServiceMock.Setup(ds => ds.LoadAsync()).ReturnsAsync(rotas);

            var resultado = await _iRotaService.DeleteAsync(Guid.NewGuid().ToString());

            Assert.False(resultado);
            _dataServiceMock.Verify(ds => ds.SaveAsync(It.IsAny<List<Rota>>()), Times.Never);
        }

        [Fact]
        public async Task ObterPorIdAsync_RetornaRotaCorreta()
        {
            var rota = new Rota("GRU", "JFK", 1500);
            var rotas = new List<Rota> { rota };
            _dataServiceMock.Setup(ds => ds.LoadAsync()).ReturnsAsync(rotas);

            var resultado = await _iRotaService.GetByIdAsync(rota.Id.ToString());

            Assert.Equal(rota, resultado);
        }

        [Fact]
        public async Task ObterPorIdAsync_IdInvalido_RetornaNull()
        {
            _dataServiceMock.Setup(ds => ds.LoadAsync()).ReturnsAsync(new List<Rota>());

            var resultado = await _iRotaService.GetByIdAsync("id-invalido");

            Assert.Null(resultado);
        }

        [Fact]
        public async Task ObterPorOrigemDestinoAsync_FiltraCorretamente()
        {
            var rota1 = new Rota("GRU", "JFK", 1500);
            var rota2 = new Rota("GRU", "LAX", 2000);
            var rotas = new List<Rota> { rota1, rota2 };
            _dataServiceMock.Setup(ds => ds.LoadAsync()).ReturnsAsync(rotas);

            var resultado = await _iRotaService.GetByOrigemDestinoAsync("GRU", "JFK");

            Assert.Single(resultado);
            Assert.Contains(rota1, resultado);
        }

        [Fact]
        public async Task EncontrarRotaMaisBarataAsync_RetornaRotaECusto()
        {
            var rotas = new List<Rota>
            {
                new Rota("A", "B", 5),
                new Rota("B", "C", 10),
                new Rota("A", "C", 20)
            };
            _dataServiceMock.Setup(ds => ds.LoadAsync()).ReturnsAsync(rotas);

            var (rota, custo) = await _iRotaService.FindRotaMaisBarata("A", "C");

            Assert.NotNull(rota);
            Assert.Equal(15, custo);
            Assert.Equal(new List<string> { "A", "B", "C" }, rota.ToList());
        }
    }

}
