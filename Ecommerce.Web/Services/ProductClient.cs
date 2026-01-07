using AutoMapper;
using Ecommerce.Web.Interface;

namespace Ecommerce.Web.Services
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public ProductClient(IHttpClientFactory httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient.CreateClient("ApiClient");
        }
    }
}
