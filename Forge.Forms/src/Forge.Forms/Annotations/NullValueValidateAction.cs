using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Specifies the validation action when property value is null.
    /// This option is added because most validations require a value to validate.
    /// This can be useful when an property is not mandatory, 
    /// but it should be within a certain range if it is filled in.
    /// after all, we can add another attribute with NotBeNull to ensure that it is required.
    /// There is no need to set an option such as AlwaysFalse, etc,
    /// because it can be done by adding validation rules
    /// </summary>
    public enum NullValueValidateAction
    {
        /// <summary>
        /// depends on the validator 
        /// </summary>
        Default,
        /// <summary>
        /// stop the validation and return true. 
        /// </summary>
        AlwaysTrue
    }
}
