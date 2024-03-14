using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Acme.Demo.UseCases.Contributors.Get;

public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
