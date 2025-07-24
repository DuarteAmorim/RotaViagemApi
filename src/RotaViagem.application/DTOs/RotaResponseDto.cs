namespace RotaViagem.application.DTOs
{
    public class RotaResponseDto
    {
        public Guid Id { get; set; }
        public string Origem { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public double Preco { get; set; }
    }
}
