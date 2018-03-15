using Forge.Forms.FormBuilding;

namespace Forge.Forms.Validation
{
    public interface IValidatorProvider
    {
        FieldValidator GetValidator(IResourceContext context, ValidationPipe pipe);
    }
}
