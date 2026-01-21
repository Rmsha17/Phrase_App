namespace Phrase_App.Core.DTOs.Request
{
    public class UpdateProfileDtoRequest
    {
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string ProfilePicUrl { get; set; } // This will store the "assets/images/..." string
    }
}
