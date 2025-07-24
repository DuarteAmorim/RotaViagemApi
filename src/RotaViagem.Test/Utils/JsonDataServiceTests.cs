using RotaViagem.application.Utils;
using RotaViagem.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotaViagem.Test.Utils
{
    public class JsonDataServiceTests : IDisposable
    {
        private const string TestFilePath = "test_voos.json";

        [Fact]
        public async Task SaveAsync_CriaArquivoJsonComDados()
        {
            var dataService = new JsonDataService<Rota>(TestFilePath);
            var rotas = new List<Rota>
            {
                new Rota("GRU", "JFK", 1500),
                new Rota("JFK", "LAX", 800)
            };

            await dataService.SaveAsync(rotas);

            Assert.True(File.Exists(TestFilePath));

            var fileContent = await File.ReadAllTextAsync(TestFilePath);
            Assert.Contains("GRU", fileContent);
            Assert.Contains("JFK", fileContent);
            Assert.Contains("LAX", fileContent);
        }

        [Fact]
        public async Task LoadAsync_RetornaDadosSalvos()
        {
            var dataService = new JsonDataService<Rota>(TestFilePath);
            var rotas = new List<Rota>
            {
                new Rota("GRU", "JFK", 1500),
                new Rota("JFK", "LAX", 800)
            };
            await dataService.SaveAsync(rotas);

            var carregados = await dataService.LoadAsync();

            Assert.NotNull(carregados);
            Assert.Equal(2, carregados.Count);
            Assert.Contains(carregados, v => v.Origem == "GRU" && v.Destino == "JFK");
            Assert.Contains(carregados, v => v.Origem == "JFK" && v.Destino == "LAX");
        }

        public void Dispose()
        {
            if (File.Exists(TestFilePath))
                File.Delete(TestFilePath);
        }
    }
}