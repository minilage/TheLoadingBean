using System.ComponentModel.DataAnnotations;

namespace TheLoadingBean.Shared.DTOs
{
    public class CustomerResponseDto
    {
        public CustomerResponseDto()
        {
            Id = string.Empty;
            Email = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Phone = string.Empty;
            Address = string.Empty;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class CreateCustomerDto
    {
        public CreateCustomerDto()
        {
            Email = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Phone = string.Empty;
            Address = string.Empty;
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }
    }

    public class UpdateCustomerDto
    {
        public UpdateCustomerDto()
        {
            Email = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Phone = string.Empty;
            Address = string.Empty;
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
