using System.Security.Claims;
using ActivityPub.Types;
using ActivityPub.Types.Conversion;
using Letterbook.Core.Adapters;
using Letterbook.Core.Authorization;
using Letterbook.Core.Models;
using Letterbook.Core.Tests.Mocks;
using Medo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace Letterbook.Core.Tests;

public abstract class WithMocks
{
	public Mock<IActivityAdapter> ActivityAdapterMock;
	public Mock<IPostAdapter> PostAdapterMock;
	public Mock<IAccountProfileAdapter> AccountProfileMock;
	public Mock<IMessageBusAdapter> MessageBusAdapterMock;
	public Mock<IAccountEventPublisher> AccountEventServiceMock;
	public Mock<IActivityPubClient> ActivityPubClientMock;
	public Mock<IActivityPubAuthenticatedClient> ActivityPubAuthClientMock;
	public Mock<IProfileService> ProfileServiceMock;
	public Mock<IAuthzProfileService> ProfileServiceAuthMock;
	public IOptions<CoreOptions> CoreOptionsMock;
	public Mock<MockableMessageHandler> HttpMessageHandlerMock;
	public ServiceCollection MockedServiceCollection;
	public Mock<IPostEventPublisher> PostEventServiceMock;
	public Mock<IPostService> PostServiceMock;
	public Mock<IAuthzPostService> PostServiceAuthMock;
	public Mock<IAuthorizationService> AuthorizationServiceMock;
	public Mock<ITimelineService> TimelineServiceMock;
	public Mock<IAuthzTimelineService> AuthzTimelineServiceMock;
	public Mock<IAccountService> AccountServiceMock;
	public Mock<IJsonLdSerializer> JsonLdSerializerMock;

	protected WithMocks()
	{
		HttpMessageHandlerMock = new Mock<MockableMessageHandler>();
		ActivityAdapterMock = new Mock<IActivityAdapter>();
		PostAdapterMock = new Mock<IPostAdapter>();
		AccountProfileMock = new Mock<IAccountProfileAdapter>();
		MessageBusAdapterMock = new Mock<IMessageBusAdapter>();
		AccountEventServiceMock = new Mock<IAccountEventPublisher>();
		ActivityPubClientMock = new Mock<IActivityPubClient>();
		ActivityPubAuthClientMock = new Mock<IActivityPubAuthenticatedClient>();
		ProfileServiceMock = new Mock<IProfileService>();
		ProfileServiceAuthMock = new Mock<IAuthzProfileService>();
		PostEventServiceMock = new Mock<IPostEventPublisher>();
		PostServiceMock = new Mock<IPostService>();
		PostServiceAuthMock = new Mock<IAuthzPostService>();
		AuthorizationServiceMock = new Mock<IAuthorizationService>();
		TimelineServiceMock = new Mock<ITimelineService>();
		AuthzTimelineServiceMock = new Mock<IAuthzTimelineService>();
		AccountServiceMock = new Mock<IAccountService>();
		JsonLdSerializerMock = new Mock<IJsonLdSerializer>();

		ActivityPubClientMock.Setup(m => m.As(It.IsAny<Profile>())).Returns(ActivityPubAuthClientMock.Object);
		PostServiceMock.Setup(m => m.As(It.IsAny<IEnumerable<Claim>>(), It.IsAny<Uuid7>())).Returns(PostServiceAuthMock.Object);
		ProfileServiceMock.Setup(m => m.As(It.IsAny<IEnumerable<Claim>>())).Returns(ProfileServiceAuthMock.Object);
		TimelineServiceMock.Setup(m => m.As(It.IsAny<IEnumerable<Claim>>())).Returns(AuthzTimelineServiceMock.Object);
		var mockOptions = new CoreOptions
		{
			DomainName = "letterbook.example",
			Port = "80",
			Scheme = "http"
		};
		CoreOptionsMock = Options.Create(mockOptions);

		MockedServiceCollection = new ServiceCollection();
		MockedServiceCollection.AddScoped<IAccountProfileAdapter>(_ => AccountProfileMock.Object);
		MockedServiceCollection.AddScoped<IMessageBusAdapter>(_ => MessageBusAdapterMock.Object);
		MockedServiceCollection.AddScoped<IAccountEventPublisher>(_ => AccountEventServiceMock.Object);
		MockedServiceCollection.AddScoped<IActivityPubClient>(_ => ActivityPubClientMock.Object);
		MockedServiceCollection.AddScoped<IActivityPubAuthenticatedClient>(_ => ActivityPubAuthClientMock.Object);
		MockedServiceCollection.AddScoped<IProfileService>(_ => ProfileServiceMock.Object);
		MockedServiceCollection.TryAddTypesModule();
	}

	public void MockAuthorizeAllowAll()
	{
		// TODO: reflect over the Decision methods instead of individual setups
		AuthorizationServiceMock.Setup(s => s.View(It.IsAny<IEnumerable<Claim>>(), It.IsAny<IFederated>(), It.IsAny<Uuid7>()))
			.Returns(Allow);
		AuthorizationServiceMock.Setup(s => s.Create(It.IsAny<IEnumerable<Claim>>(), It.IsAny<IFederated>(), It.IsAny<Uuid7>()))
			.Returns(Allow);
		AuthorizationServiceMock.Setup(s => s.Delete(It.IsAny<IEnumerable<Claim>>(), It.IsAny<IFederated>(), It.IsAny<Uuid7>()))
			.Returns(Allow);
		AuthorizationServiceMock.Setup(s => s.Publish(It.IsAny<IEnumerable<Claim>>(), It.IsAny<IFederated>(), It.IsAny<Uuid7>()))
			.Returns(Allow);
		AuthorizationServiceMock.Setup(s => s.Update(It.IsAny<IEnumerable<Claim>>(), It.IsAny<IFederated>(), It.IsAny<Uuid7>()))
			.Returns(Allow);
		AuthorizationServiceMock.Setup(s => s.Report(It.IsAny<IEnumerable<Claim>>(), It.IsAny<IFederated>(), It.IsAny<Uuid7>()))
			.Returns(Allow);
		return;

		Decision Allow(IEnumerable<Claim> claims, IFederated _, Uuid7 __) => Decision.Allow("Mock", claims);
	}
}