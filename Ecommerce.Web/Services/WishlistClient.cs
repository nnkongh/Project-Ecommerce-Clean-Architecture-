using AutoMapper;
using Ecommerce.Web.Interface;

namespace Ecommerce.Web.Services
{
    public class WishlistClient : IWishlistClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public WishlistClient(IHttpClientFactory httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient.CreateClient("ApiClient");
        }
    }
}
