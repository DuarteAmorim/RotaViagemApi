using RotaViagem.application.DTOs;
using RotaViagem.domain.Entities;

namespace RotaViagem.application.Interfaces
{
    /// <summary>
    /// Interface para mapeamento entre DTOs e entidade Rota.
    /// </summary>
    public interface IRotaMapper
    {

        /// <summary>
        /// Mapeia um DTO de criação para a entidade Rota.
        /// </summary>
        /// <param name="dto">DTO contendo dados para criação da rota.</param>
        /// <returns>Entidade Rota criada a partir do DTO.</returns>
        Rota MapToEntity(RotaCreateDto dto);
        /// <summary>
        /// Mapeia um DTO de atualização para a entidade Rota, atribuindo o Id informado.
        /// </summary>
        /// <param name="dto">DTO contendo dados para atualização da rota.</param>
        /// <param name="id">Identificador da rota.</param>
        /// <returns>Entidade Rota atualizada a partir do DTO e Id.</returns>
        Rota MapToEntity(RotaUpdateDto dto, Guid id);
        /// <summary>
        /// Mapeia a entidade Rota para o DTO de resposta.
        /// </summary>
        /// <param name="rota">Entidade Rota.</param>
        /// <returns>DTO contendo os dados para resposta.</returns>
        RotaResponseDto MapToResponseDto(Rota rota);
        RotaUpdateDto MapToUpdateDto(Rota rota);
        RotaCreateDto MapToCreateDto(Rota rota);
    }
}
