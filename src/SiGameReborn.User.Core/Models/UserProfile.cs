using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SiGameReborn.Common.Domain.Models;

namespace SiGameReborn.User.Core.Models;

public class UserProfile : SoftDeletableEntity
{
    [Required(ErrorMessage = "Имя пользователя – обязательное поле.")]
    [StringLength(100, ErrorMessage = "Длина имени пользователя не может превышать 100 символов.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Адрес электронной почты – обязательное поле.")]
    [StringLength(250, ErrorMessage = "Длина адреса электронной почты не может превышать 100 символов.")]
    [EmailAddress(ErrorMessage = "Введённый почтовый адрес недействителен.")]
    public string EmailAddress { get; set; } = string.Empty;

    [StringLength(300, ErrorMessage = "Длина ссылки на аватар профиля не может превышать 300 символов.")]
    public Uri? ProfileImageUrl { get; set; }

    public void CopyDetails(UserProfile userProfile)
    {
        Username = userProfile.Username;
        EmailAddress = userProfile.EmailAddress;
        ProfileImageUrl = userProfile.ProfileImageUrl;
    }
}