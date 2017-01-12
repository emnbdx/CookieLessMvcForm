using System;

namespace CookieLessMvcForm.Models
{
    public class FormModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public string Email { get; set; }

        public int Tip { get; set; }
    }
}