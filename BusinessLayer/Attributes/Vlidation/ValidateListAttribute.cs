using System.ComponentModel.DataAnnotations;
namespace BusinessLayer.Attributes.Vlidation
{
    public class ValidateListAttribute : ValidationAttribute
    {
        private readonly int _minElements;

        public ValidateListAttribute(int minElements = 1)
        {
            _minElements = minElements;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IList<int>;
            if (list == null || list.Count < _minElements)
            {
                return new ValidationResult($"The list must contain at least {_minElements} elements.");
            }
            return ValidationResult.Success;
        }

    }
}
