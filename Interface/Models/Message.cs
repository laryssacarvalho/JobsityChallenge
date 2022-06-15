using System.ComponentModel.DataAnnotations;

namespace Interface.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
    }
}
