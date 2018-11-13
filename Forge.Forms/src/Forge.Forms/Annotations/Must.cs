using Forge.Forms.Validation;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Specifies validation type.
    /// </summary>
    public enum Must
    {
        /// <summary>
        /// Property must equal to a value.
        /// </summary>
        BeEqualTo,

        /// <summary>
        /// Property must not equal to a value.
        /// </summary>
        NotBeEqualTo,

        /// <summary>
        /// Property must be greater than a value.
        /// </summary>
        BeGreaterThan,

        /// <summary>
        /// Property must be greater than or equal to a value.
        /// </summary>
        BeGreaterThanOrEqualTo,

        /// <summary>
        /// Property must be less than a value.
        /// </summary>
        BeLessThan,

        /// <summary>
        /// Property must be less than or equal to a value.
        /// </summary>
        BeLessThanOrEqualTo,

        /// <summary>
        /// Property must be empty.
        /// A string is empty if it is null or has length 0.
        /// A collection is empty if it is null or has 0 elements.
        /// </summary>
        BeEmpty,

        /// <summary>
        /// Property must not be empty.
        /// A string is empty if it is null or has length 0.
        /// A collection is empty if it is null or has 0 elements.
        /// </summary>
        NotBeEmpty,

        /// <summary>
        /// Property must be true.
        /// </summary>
        BeTrue,

        /// <summary>
        /// Property must be false.
        /// </summary>
        BeFalse,

        /// <summary>
        /// Property must be null.
        /// </summary>
        BeNull,

        /// <summary>
        /// Property must not be null.
        /// </summary>
        NotBeNull,

        /// <summary>
        /// Property must exist in a collection.
        /// </summary>
        ExistIn,

        /// <summary>
        /// Property must not exist in a collection.
        /// </summary>
        NotExistIn,

        /// <summary>
        /// Property must match a regex pattern.
        /// </summary>
        MatchPattern,

        /// <summary>
        /// Property must not match a regex pattern.
        /// </summary>
        NotMatchPattern,

        /// <summary>
        /// Property value must satisfy model's static method of signature: public static bool &lt;Argument&gt;(
        /// <see cref="ValidationContext" /> context).
        /// Throws if no such method is found.
        /// </summary>
        SatisfyMethod,

        /// <summary>
        /// Property value must satisfy context's static method of signature: public static bool &lt;Argument&gt;(
        /// <see cref="ValidationContext" /> context).
        /// Does nothing if no such method is found.
        /// </summary>
        SatisfyContextMethod,

        /// <summary>
        /// Property will be invalid unless the validator is disabled.
        /// </summary>
        BeInvalid,

        /// <summary>
        /// Property will be invalid unless the validator is disabled. Alias for <see cref="BeInvalid"/>.
        /// </summary>
        Fail = BeInvalid
    }
}
