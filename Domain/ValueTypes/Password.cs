namespace Domain.ValueTypes;

public record Password(string Hash, string Salt);