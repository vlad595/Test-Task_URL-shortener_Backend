using System;

namespace Test_Task_URL_shortener_Backend.Models
{
    public class AboutText
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}