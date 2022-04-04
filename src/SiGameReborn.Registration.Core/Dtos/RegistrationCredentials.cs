using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SiGameReborn.Registration.Core.Dtos;

public record RegistrationCredentials
{
    [Required(ErrorMessage = "Имя пользователя – обязательное поле.")]
    [StringLength(100, ErrorMessage = "Максимальная длина имени пользователя – 100 символов.")]
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Адрес электронной почты – обязательное поле.")]
    [StringLength(100, ErrorMessage = "Максимальная длина адрес электронной почты – 100 символов.")]
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль – обязательное поле.")]
    [StringLength(100, MinimumLength = 12, ErrorMessage = "Длина пароля не должна быть от 12 до 100 символов.")]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}