using AutoMapper;
using Ecommerce.Web.Interface;

namespace Ecommerce.Web.Services
{
    public class OrderClient : IOrderClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public OrderClient(IHttpClientFactory httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient.CreateClient("ApiClient");
        }
    }
}
