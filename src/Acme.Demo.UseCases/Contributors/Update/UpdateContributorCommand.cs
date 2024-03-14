using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Acme.Demo.UseCases.Contributors.Update;

public record UpdateContributorCommand(int ContributorId, string NewName) : ICommand<Result<ContributorDTO>>;
