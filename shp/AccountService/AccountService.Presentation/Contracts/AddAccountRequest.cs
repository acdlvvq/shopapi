using AccountService.Core.Enums;

namespace AccountService.Presentation.Contracts;

public record AddAccountRequest(
    string Email, string Username, string Password, AccountRole Role);
