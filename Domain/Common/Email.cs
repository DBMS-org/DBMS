namespace Domain.Common
{
    /// <summary>
    ///     Value Object that represents and validates an e-mail address.
    ///     Implicit conversions are provided so existing code that expects
    ///     a string will continue to work during the migration.
    /// </summary>
    public readonly record struct Email
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty", nameof(value));

            try
            {
                _ = new System.Net.Mail.MailAddress(value);
            }
            catch
            {
                throw new ArgumentException("Invalid email format", nameof(value));
            }

            Value = value.Trim();
        }

        public override string ToString() => Value;

        public static implicit operator string(Email email) => email.Value;

        public static implicit operator Email(string value) => new(value);
    }
} 