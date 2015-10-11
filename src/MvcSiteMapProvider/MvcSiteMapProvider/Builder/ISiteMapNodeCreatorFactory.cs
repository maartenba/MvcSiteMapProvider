namespace MvcSiteMapProvider.Builder
{
    public interface ISiteMapNodeCreatorFactory
    {
        ISiteMapNodeCreator Create(ISiteMap siteMap);
    }
}
