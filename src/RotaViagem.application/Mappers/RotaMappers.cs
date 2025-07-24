using RotaViagem.application.DTOs;
using RotaViagem.application.Interfaces;
using RotaViagem.domain.Entities;

namespace RotaViagem.application.Mappers
{
    /// <summary>
    /// Implementação do mapeador para conversão entre DTOs e entidade Rota.
    /// </summary>
    public class RotaMapper : IRotaMapper
    {
        public RotaCreateDto MapToCreateDto(Rota rota)
        {
            return new RotaCreateDto()
            {
                Destino = rota.Destino,
                Origem = rota.Origem,
                Preco = rota.Preco
            };
        }

        /// <inheritdoc/>
        public Rota MapToEntity(RotaCreateDto dto)
        {
            return new Rota(dto.Origem, dto.Destino, dto.Preco);
        }

        /// <inheritdoc/>
        public Rota MapToEntity(RotaUpdateDto dto, Guid id)
        {
            var rota = new Rota(dto.Origem, dto.Destino, dto.Preco);
            // Define o Id da rota via reflexão, caso o setter seja privado
            typeof(Rota).GetProperty("Id")?.SetValue(rota, id);
            return rota;
        }

        /// <inheritdoc/>
        public RotaResponseDto MapToResponseDto(Rota rota)
        {
            return new RotaResponseDto
            {
                Id = rota.Id,
                Origem = rota.Origem,
                Destino = rota.Destino,
                Preco = rota.Preco
            };
        }

        public RotaUpdateDto MapToUpdateDto(Rota rota)
        {
            return new RotaUpdateDto()
            {
                Id = rota.Id.ToString(),
                Destino = rota.Destino,
                Origem = rota.Origem,
                Preco = rota.Preco

            };
        }
    }
}
