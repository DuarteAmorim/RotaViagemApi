using RotaViagem.application.DTOs;
using RotaViagem.application.Mappers;
using RotaViagem.domain.Entities;

namespace RotaViagem.Test.Mappers
{
    public class RotaMapperTests
    {
        private readonly RotaMapper _mapper;

        public RotaMapperTests()
        {
            _mapper = new RotaMapper();
        }

        [Fact]
        public void MapToEntity_DeveMapearRotaCreateDtoParaRota()
        {
            var dto = new RotaCreateDto
            {
                Origem = "GRU",
                Destino = "JFK",
                Preco = 1500
            };

            var rota = _mapper.MapToEntity(dto);

            Assert.Equal(dto.Origem, rota.Origem);
            Assert.Equal(dto.Destino, rota.Destino);
            Assert.Equal(dto.Preco, rota.Preco);
            Assert.NotEqual(Guid.Empty, rota.Id); // Id deve ser gerado no construtor
        }

        [Fact]
        public void MapToEntity_DeveMapearRotaUpdateDtoParaRotaComId()
        {
            var dto = new RotaUpdateDto
            {
                Origem = "GRU",
                Destino = "JFK",
                Preco = 1500
            };
            var id = Guid.NewGuid();

            var rota = _mapper.MapToEntity(dto, id);

            Assert.Equal(dto.Origem, rota.Origem);
            Assert.Equal(dto.Destino, rota.Destino);
            Assert.Equal(dto.Preco, rota.Preco);
            Assert.Equal(id, rota.Id);
        }

        [Fact]
        public void MapToResponseDto_DeveMapearRotaParaRotaResponseDto()
        {
            var id = Guid.NewGuid();
            var rota = new Rota("GRU", "JFK", 1500);
            // Ajusta Id via reflexão se necessário
            typeof(Rota).GetProperty("Id")?.SetValue(rota, id);

            var dto = _mapper.MapToResponseDto(rota);

            Assert.Equal(id, dto.Id);
            Assert.Equal(rota.Origem, dto.Origem);
            Assert.Equal(rota.Destino, dto.Destino);
            Assert.Equal(rota.Preco, dto.Preco);
        }
    }
}
