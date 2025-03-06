namespace Vizvezetek.API.DTOs
{
    public class MunkalapDTO
    {
        public required int Id { get; set; }

        public required DateTime beadas_datum { get; set; }

        public required DateTime javitas_datum { get; set; }

        public required string Helyszin { get; set; }

        public required string Szerelo { get; set; }

        public required int Munkaora { get; set; }

        public required int Anyagar { get; set; }
    }
}
