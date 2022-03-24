using System.ComponentModel.DataAnnotations;

using SiGameReborn.Common.Domain.Models;

namespace SiGameReborn.User.Core.Models;

public class UserPassword : SoftDeletableEntity
{
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Хэш пароля – обязательное поле.")]
    [StringLength(400, ErrorMessage = "Максимальная длина хэша пароля – 400 символов.")]
    public string PasswordHash { get; set; } = string.Empty;
}