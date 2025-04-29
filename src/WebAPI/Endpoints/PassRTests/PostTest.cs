
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
        public record GetLoggedInUserQuery() : IQuery<UserDto>;
        public record UserDto(Guid Id, string Name);

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

        public class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserDto>
        {
            public ValueTask<Result<UserDto>> HandleAsync(GetLoggedInUserQuery request, CancellationToken cancellationToken)
            {
                var user = new UserDto(Guid.NewGuid(), "DemoUser");
                return ValueTask.FromResult(Result.Success(user));
            }
        }
    }
}
