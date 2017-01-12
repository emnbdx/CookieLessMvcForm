namespace CookieLessMvcForm.Models
{
    public class SerializationFormViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public SerializationFormStep1ViewModel Step1 { get; set; }

        public SerializationFormStep2ViewModel Step2 { get; set; }

        public SerializationFormStep3ViewModel Step3 { get; set; }
    }
}