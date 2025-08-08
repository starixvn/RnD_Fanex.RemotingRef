namespace Fanex.RemotingRef.NetCore.Services.Interface
{
    public interface IRefResourceService
    {
        string GetRefResourceById(string refId, string resourceId, string languageCode);
    }
}