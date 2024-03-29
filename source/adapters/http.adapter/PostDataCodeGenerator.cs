namespace Bespoke.Sph.Integrations.Adapters
{
    public class PostDataCodeGenerator
    {
        public virtual string GenerateCode(HttpOperationDefinition operation)
        {
            return "return string.Empty;";
        }

        public static PostDataCodeGenerator Create(HttpOperationDefinition operation)
        {
            if (operation.HttpMethod == "GET")
                return new PostDataForGet();
            if (operation.RequestMemberCollection.Count == 0)
                return new PostDataCodeGenerator();

            var contentType = operation.GetRequestHeader("Content-Type");

            var multipartPost = operation.HttpMethod == "POST" && 
                                !string.IsNullOrWhiteSpace(contentType) &&
                                contentType.Contains("multipart");
            var ajaxPost = operation.HttpMethod == "POST" && 
                                !string.IsNullOrWhiteSpace(contentType) &&
                                contentType.Contains("application/json;");
            if (multipartPost)
                return new PostDataForPostMultipartEncoded();

            if (ajaxPost)
                return new PostDataAjax();


            if (operation.HttpMethod == "POST")
                return new PostDataForPostUrlEncoded();

            return null;
        }
    }
}