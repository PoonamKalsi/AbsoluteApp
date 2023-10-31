using FikaAmazonAPI.AmazonSpApiSDK.Models.Token;

namespace FikaAmazonAPI.Search
{
    public interface IParameterBasedPII
    {
         bool IsNeedRestrictedDataToken { get; set; }
         CreateRestrictedDataTokenRequest RestrictedDataTokenRequest { get; set; }
    }
}
