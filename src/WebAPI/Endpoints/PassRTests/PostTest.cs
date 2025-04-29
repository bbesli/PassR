
using Application.Abstractions;
using Application.Messaging;
using Domain.Abstractions;
using Microsoft.AspNetCore.Hosting.Server;
using WebAPI.Extenstions;
using WebAPI.Infrastructure;

namespace WebAPI.Endpoints.PassRTests
{
    public class PostTest : IEndpoint
    {
        public record GetLoggedInUserQuery() : IQuery<Result>;

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("PostTest", async (
                IPassR sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetLoggedInUserQuery(); // assumes this is a parameterless query

                var result = await sender.SendAsync(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Tests);
        }

        public class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, Result>
        {
            ValueTask<Result<Result>> IRequestHandler<GetLoggedInUserQuery, Result<Result>>.HandleAsync(GetLoggedInUserQuery request, CancellationToken cancellationToken)
            {
                // Simulate returning a logged-in user
                var user = new { Id = Guid.NewGuid(), Name = "DemoUser" };
                return ValueTask.FromResult(Result.Success(user));
            }
        }
    }
}
