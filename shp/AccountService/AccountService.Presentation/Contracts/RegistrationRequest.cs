namespace AccountService.Presentation.Contracts;

public record RegistrationRequest(
    string Email, string Username, string Password);
