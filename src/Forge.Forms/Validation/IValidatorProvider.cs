using Forge.Forms.Interfaces;

namespace Forge.Forms.Validation
{
    public interface IValidatorProvider
    {
        FieldValidator GetValidator(IResourceContext context, ValidationPipe pipe);
    }
}
