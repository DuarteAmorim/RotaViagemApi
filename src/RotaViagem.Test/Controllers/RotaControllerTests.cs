using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RotaViagem.api.Controllers;
using RotaViagem.application.DTOs;
using RotaViagem.application.Interfaces;
using RotaViagem.application.Mappers;
using RotaViagem.application.Validations;
using RotaViagem.domain.Entities;

namespace RotaViagem.Test.Controllers
{
    public class RotaControllerTests
    {
        private RotaController _iRotaController;
        private readonly Mock<IRotaService> _iRotaServiceMock;
        private readonly Mock<IValidator<Rota>> _iValidatorMock;
        private readonly Mock<IRotaMapper> _mapperMock;
        public RotaControllerTests()
        {
            _iRotaServiceMock = new Mock<IRotaService>();
            _iValidatorMock = new Mock<IValidator<Rota>>();
            _mapperMock = new Mock<IRotaMapper>();
            _iRotaController = new RotaController(_iRotaServiceMock.Object, new RotaValidation(), new RotaMapper());
        }

        [Fact]
        public async Task Post_RotaValida_RetornaCreatedAtAction()
        {
            var rota = new Rota("MAO", "GRU", 450);

            _iRotaServiceMock
                .Setup(service => service.CreateAsync(It.IsAny<Rota>()))
                .ReturnsAsync(rota);
            _mapperMock
                .Setup(m => m.MapToCreateDto(It.IsAny<Rota>()))
                .Returns((Rota r) => new RotaCreateDto
                {
                    Origem = r.Origem,
                    Destino = r.Destino,
                    Preco = r.Preco
                });
            // Act

            var rotaCreateDto = _mapperMock.Object.MapToCreateDto(rota);
            var result = await _iRotaController.Create(rotaCreateDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

            var RotaRetornada = Assert.IsType<RotaResponseDto>(createdAtActionResult.Value);

            Assert.Equal("MAO", RotaRetornada.Origem);
            Assert.Equal("GRU", RotaRetornada.Destino);
            Assert.Equal(450, RotaRetornada.Preco);

        }

        [Fact]
        public async Task RotaMaisBarata_RetornaRespostaFormatadaCorretamente()
        {
            var origem = "GRU";
            var destino = "CDG";
            var rota = new List<string> { "GRU", "BRC", "SCL", "ORL", "CDG" };
            var custo = 40.0;

            _iRotaServiceMock
                .Setup(s => s.FindRotaMaisBarata(origem, destino))
                .ReturnsAsync((rota, custo));

            var result = await _iRotaController.RotaMaisBarata(origem, destino);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var resposta = Assert.IsType<string>(okResult.Value);

            Assert.Equal("Resposta: GRU - BRC - SCL - ORL - CDG ao custo de $40", resposta);
        }


        [Fact]
        public async Task Create_RotaNula_RetornaBadRequest()
        {
            // Arrange
            Rota rota = null;

            // Act

            RotaCreateDto rotaCreateDto = null;

            var result = await _iRotaController.Create(rotaCreateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Dados da rota não podem ser nulos.", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_RotaValida_RetornaCreatedAtAction()
        {
            // Arrange
            var rota = new Rota("GRU", "JFK", 1500);

            // Configura o validador para validar com sucesso
            _iValidatorMock
                .Setup(v => v.Validate(It.IsAny<Rota>()))
                .Returns(new ValidationResult());

            // Configura o serviço para retornar a rota criada
            _iRotaServiceMock
                .Setup(s => s.CreateAsync(It.IsAny<Rota>()))
                .ReturnsAsync(rota);

            _mapperMock
               .Setup(m => m.MapToCreateDto(It.IsAny<Rota>()))
               .Returns((Rota r) => new RotaCreateDto
               {
                   Origem = r.Origem,
                   Destino = r.Destino,
                   Preco = r.Preco
               });
            // Act

            var rotaCreateDto = _mapperMock.Object.MapToCreateDto(rota);
            var result = await _iRotaController.Create(rotaCreateDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var rotaRetornada = Assert.IsType<RotaResponseDto>(createdAtActionResult.Value);
            Assert.Equal("GRU", rotaRetornada.Origem);
            Assert.Equal("JFK", rotaRetornada.Destino);
            Assert.Equal(1500, rotaRetornada.Preco);
        }

        [Fact]
        public async Task Create_RotaInvalida_RetornaBadRequestComErros()
        {
            // Arrange
            var rota = new Rota("GR", "JKF", -10);

            var failures = new[]
            {
                new ValidationFailure("Origem", "Origem deve ter exatamente 3 caracteres."),
                new ValidationFailure("Preco", "Preço deve ser maior que zero.")
            };
            var validationResult = new ValidationResult(failures);

            _iValidatorMock
                .Setup(v => v.Validate(It.IsAny<Rota>()))
                .Returns(validationResult);
            _mapperMock
               .Setup(m => m.MapToCreateDto(It.IsAny<Rota>()))
               .Returns((Rota r) => new RotaCreateDto
               {
                   Origem = r.Origem,
                   Destino = r.Destino,
                   Preco = r.Preco
               });
            // Act

            var rotaCreateDto = _mapperMock.Object.MapToCreateDto(rota);
            var result = await _iRotaController.Create(rotaCreateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // tratando erro com uma mensagem simples
            //var mensagem = Assert.IsType<string>(badRequestResult.Value);

            //tratar erros com múltiplas mensagens
            var erros = Assert.IsAssignableFrom<System.Collections.Generic.IEnumerable<string>>(badRequestResult.Value);

            Assert.Contains("A origem deve ter exatamente 3 caracteres.", erros);
            Assert.Contains("O preço deve ser maior que zero.", erros);
        }
    }
}
