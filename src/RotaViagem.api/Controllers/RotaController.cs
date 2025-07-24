using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RotaViagem.application.DTOs;
using RotaViagem.application.Interfaces;
using RotaViagem.domain.Entities;
namespace RotaViagem.api.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar as operações relacionadas às rotas de voo.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RotaController : ControllerBase
    {
        private readonly IRotaService _rotaService;
        private readonly IValidator<Rota> _validator;
        private readonly IRotaMapper _mapper;

        /// <summary>
        /// Obtém uma rota pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador da rota.</param>
        /// <returns>Retorna os dados da rota.</returns>
        /// <response code="200">Rota encontrada.</response>
        /// <response code="404">Rota não encontrada.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<RotaResponseDto>> GetById(string id)
        {
            var rota = await _rotaService.GetByIdAsync(id);
            if (rota == null)
                return NotFound();

            var responseDto = _mapper.MapToResponseDto(rota);
            return Ok(responseDto);
        }

        /// <summary>
        /// Obtém todas as rotas cadastradas.
        /// </summary>
        /// <returns>Lista de rotas.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RotaResponseDto>>> GetAll()
        {
            var rotas = await _rotaService.GetAllAsync();
            var responseDtos = rotas.Select(r => _mapper.MapToResponseDto(r));
            return Ok(responseDtos);
        }

        /// <summary>
        /// Construtor do controller de rotas.
        /// </summary>
        /// <param name="rotaService">Serviço para operações de rotas.</param>
        /// <param name="validator">Validador da entidade Rota.</param>
        /// <param name="mapper">Mapper para conversão entre DTOs e entidades.</param>
        public RotaController(IRotaService rotaService, IValidator<Rota> validator, IRotaMapper mapper)
        {
            _rotaService = rotaService;
            _validator = validator;
            _mapper = mapper;
        }

        /// <summary>
        /// Cria uma nova rota.
        /// </summary>
        /// <param name="rotaDto">Dados da rota a ser criada.</param>
        /// <returns>Retorna a rota criada com seu identificador.</returns>
        /// <response code="201">Rota criada com sucesso.</response>
        /// <response code="400">Dados inválidos ou rota nula.</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RotaCreateDto rotaDto)
        {
            if (rotaDto == null)
                return BadRequest("Dados da rota não podem ser nulos.");

            var rota = _mapper.MapToEntity(rotaDto);

            var validationResult = _validator.Validate(rota);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var createdRota = await _rotaService.CreateAsync(rota);
            if (createdRota == null)
                return BadRequest("Não foi possível criar a rota.");

            var responseDto = _mapper.MapToResponseDto(createdRota);
            return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
        }

        /// <summary>
        /// Atualiza uma rota existente.
        /// </summary>
        /// <param name="id">Identificador da rota a ser atualizada.</param>
        /// <param name="rotaDto">Dados atualizados da rota.</param>
        /// <returns>Retorna status 204 se atualizado com sucesso.</returns>
        /// <response code="204">Rota atualizada com sucesso.</response>
        /// <response code="400">Dados inválidos ou id inválido.</response>
        /// <response code="404">Rota não encontrada.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] RotaUpdateDto rotaDto)
        {
            if (rotaDto == null)
                return BadRequest("Dados da rota não podem ser nulos.");

            if (!Guid.TryParse(rotaDto.Id, out var guid))
                return BadRequest("Id inválido.");

            var rotaParaValidar = _mapper.MapToEntity(rotaDto, guid);

            var validationResult = _validator.Validate(rotaParaValidar);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var sucesso = await _rotaService.UpdateAsync(rotaDto.Id, rotaParaValidar);
            if (!sucesso)
                return NotFound("Rota não encontrada ou não foi possível atualizar.");

            return NoContent();
        }
        /// <summary>
        /// Deleta uma rota pelo seu identificador.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var sucesso = await _rotaService.DeleteAsync(id);
            if (!sucesso)
                return NotFound("Rota não encontrada ou não foi possível deletar.");

            return NoContent();
        }

        /// <summary>
        /// Obtém a rota mais barata entre dois aeroportos.
        /// </summary>
        /// <param name="origem">Sigla do aeroporto de origem (3 letras maiúsculas).</param>
        /// <param name="destino">Sigla do aeroporto de destino (3 letras maiúsculas).</param>
        /// <returns>Rota mais barata formatada e custo total.</returns>
        /// <response code="200">Retorna a rota mais barata e o custo.</response>
        /// <response code="400">Se os parâmetros forem inválidos.</response>
        /// <response code="404">Se não existir rota entre origem e destino.</response>
        [HttpGet("rota-mais-barata")]
        public async Task<IActionResult> RotaMaisBarata([FromQuery] string origem, [FromQuery] string destino)
        {
            if (string.IsNullOrWhiteSpace(origem) || origem.Length != 3)
                return BadRequest("Origem deve ser uma sigla de 3 letras.");

            if (string.IsNullOrWhiteSpace(destino) || destino.Length != 3)
                return BadRequest("Destino deve ser uma sigla de 3 letras.");

            origem = origem.ToUpperInvariant();
            destino = destino.ToUpperInvariant();

            var (rota, custo) = await _rotaService.FindRotaMaisBarata(origem, destino);

            if (rota == null || !rota.Any())
                return NotFound("Rota não encontrada entre os aeroportos informados.");

            string rotaFormatada = string.Join(" - ", rota);
            string resposta = $"Resposta: {rotaFormatada} ao custo de ${custo}";

            return Ok(resposta);
        }

    }
}
