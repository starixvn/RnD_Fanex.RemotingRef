using PublicEnum.Language;

namespace Fanex.RemotingRef.NetCore.Models;
public class RefResourceModel
{
    public string RefId { get; set; }

    public string ResourceId { get; set; }

    [Language(LanguageEnum.En)]
    public string ResourceName { get; set; }

    [Language(LanguageEnum.Th)]
    public string ResourceTh { get; set; }


    [Language(LanguageEnum.Vn)]
    public string ResourceVn { get; set; }

    [Language(LanguageEnum.Cn)]
    public string ResourceCs { get; set; }

    [Language(LanguageEnum.Ch)]
    public string ResourceCh { get; set; }

    [Language(LanguageEnum.Sc2)]
    public string ResourceSc2 => ResourceCs;

    [Language(LanguageEnum.Id)]
    public string ResourceIn { get; set; }

    [Language(LanguageEnum.Jp)]
    public string ResourceJp { get; set; }

    [Language(LanguageEnum.Ko)]
    public string ResourceKo { get; set; }
}