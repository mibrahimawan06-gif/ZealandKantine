using System.ComponentModel.DataAnnotations;

namespace ZealandKantine.models
{
    /// <summary>
    /// En vare i kantinens sortiment. Indeholder kun katalogdata.
    /// Antal hoerer til i CartItem og OrderLine, ikke paa selve varen.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Varen skal have et navn")]
        [StringLength(60)]
        public string Name { get; set; } = string.Empty;

        // Casen kraever en kort beskrivende tekst pr. ret
        [StringLength(200)]
        public string? Description { get; set; }

        [Range(0.01, 1000, ErrorMessage = "Prisen skal vaere mellem 0,01 og 1000 kr.")]
        public decimal Price { get; set; }

        // Styrer om varen er omfattet af medarbejderrabatten
        public bool IsDrink { get; set; }

        public override string ToString()
            => $"Id: {Id}, Navn: {Name}, Pris: {Price}, Drikkevare: {IsDrink}";
    }
}
