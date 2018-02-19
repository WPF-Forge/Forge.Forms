using System;
using Forge.Forms.Annotations;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Models
{
    [Title("Create account")]
    [Action("cancel", "CANCEL", IsReset = true, IsCancel = true)]
    [Action("register", "REGISTER", Validates = true, IsDefault = true)]
    public class User
    {
        [Heading("Personal details")]
        [Field(Name = "First Name",
            ToolTip = "Enter your first name here.",
            Icon = PackIconKind.Pencil)]
        [Value(Must.NotBeEmpty)]
        public string FirstName { get; set; }

        [Field(Name = "Last Name",
            ToolTip = "Enter your last name here.",
            Icon = "Empty")]
        [Value(Must.NotBeEmpty)]
        public string LastName { get; set; }

        [Field(Icon = PackIconKind.Calendar)]
        [Value(Must.BeLessThan, "2020-01-01",
            Message = "You said you are born in the year {Value:yyyy}. Are you really from the future?")]
        public DateTime? DateOfBirth { get; set; }

        [Heading("Account details")]
        [Field(Name = "Username",
            Icon = PackIconKind.Account)]
        [Value(Must.MatchPattern, "^[a-zA-Z][a-zA-Z0-9]*$",
            Message = "{Value} is not a valid username, usernames must match pattern {Argument}.")]
        [Value(Must.NotExistIn, "{ContextBinding Users}",
            Message = "User {Value} is already taken.")]
        public string Username { get; set; }

        [Field(Icon = PackIconKind.Key)]
        [Value("Length", Must.BeGreaterThanOrEqualTo, 6,
            Message = "Your password has {Value|Length} characters, which is less than the required {Argument}.")]
        [Value("Length", Must.BeGreaterThan, 12,
            When = "{ContextBinding RequireLongPasswords}",
            Message = "The administrator decided that your password must be really long!")]
        public string Password { get; set; }

        [Field(Icon = "Empty")]
        [Value(Must.BeEqualTo, "{Binding Password}",
            Message = "The entered passwords do not match.",
            ArgumentUpdatedAction = ValidationAction.ClearErrors)]
        [Value(Must.NotBeEmpty)]
        public string ConfirmPassword { get; set; }

        [Break]
        [Heading("Review entered information")]
        [Text("Name: {Binding FirstName} {Binding LastName}")]
        [Text("Date of birth: {Binding DateOfBirth:yyyy-MM-dd}")]
        [Text("Username: {Binding Username}")]
        [Break]
        [Heading("License agreement")]
        [Text("By signing up, you agree to our terms of use, privacy policy, and cookie policy.")]
        [Value(Must.BeTrue, Message = "You must accept the license agreement.")]
        public bool AgreeToLicense { get; set; }
    }
}
