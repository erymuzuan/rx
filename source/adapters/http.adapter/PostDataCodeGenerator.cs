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
            if (operation.HttpMethod == "POST" && operation.RequestHeaders["Content-Type"].Contains("multipart"))
                return new PostDataForPostMultipartEncoded();

            if (operation.HttpMethod == "POST")
                return new PostDataForPostUrlEncoded();

            return null;
        }
    }
}