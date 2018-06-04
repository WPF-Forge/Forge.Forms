using Forge.Forms.Annotations;

namespace Forge.Forms.Demo.Models
{
    [Title("Text fields")]
    public class TextFields
    {
        public string SingleLine { get; set; }

        [Password]
        public string Password { get; set; }

        [MultiLine]
        public string MultiLine { get; set; }

    }
}
