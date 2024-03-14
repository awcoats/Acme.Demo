using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Acme.Demo.UseCases.Contributors.Delete;

public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
