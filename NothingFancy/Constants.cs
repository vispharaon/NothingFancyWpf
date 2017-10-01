namespace NothingFancy
{
    public class Constants
    {
        #region General constants

        public const string BaseAddressApi = "http://api.7digital.com";
        public const string BaseAddressPreview = "http://previews.7digital.com";
        public const string ApiKey = "7d6exupvksjq";
        public const string ConsumerSecret = "wa7zdhhpwss4g7hm";
        public const string CountryDevTag = "ww";
        public const int NumberOfTracksShown = 15;

        #endregion

        #region TrackList specific constants

        public const string TrackSearchApi = "/1.2/track/search";
        public const string UsageTypes = "download,subscriptionstreaming,adsupportedstreaming";

        #endregion

        #region Track preview

        public const string TrackClipPreview = "/clip";

        //"63807806?country=ww&oauth_consumer_key=7d6exupvksjq&oauth_nonce=307204769&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1506621475&oauth_version=1.0&oauth_signature=we1Pn43wvHSE3znGk7qDG3MbWWs%3D"

        #endregion
    }
}