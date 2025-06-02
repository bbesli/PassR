
using PassR.Abstractions;
using PassR.Utilities.Attributes;
using PassR.Utilities.Endpoints;
using PassR.Utilities.Extensions;
using PassR.Utilities.Infrastructure;
using PassR.WebAPI.Messaging;

namespace PassR.WebAPI.Endpoints.PassRTests
{
    [ApiVersion(2)]
    public class PostTestV2 : IEndpoint
    {
        public record GetLoggedInUserQuery() : IQuery<UserDto>;
        public record UserDto(Guid Id, string Name);

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("PostTest", async (
                    IPassR sender,
                    CancellationToken cancellationToken) =>
                {
                    var query = new GetLoggedInUserQuery();

                    var result = await sender.SendAsync(query, cancellationToken);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
                .WithTags(Tags.Tests);
        }

        public class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserDto>
        {
            public ValueTask<Result<UserDto>> HandleAsync(GetLoggedInUserQuery request, CancellationToken cancellationToken)
            {
                var user = new UserDto(Guid.NewGuid(), "DemoUser V2");
                return ValueTask.FromResult(Result.Success(user));
            }
        }
    }
}
