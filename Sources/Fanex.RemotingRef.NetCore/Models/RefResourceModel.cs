using PublicEnum.Language;

namespace Fanex.RemotingRef.NetCore.Models;
public class RefResourceModel
{
    public string RefId { get; set; }

    public string ResourceId { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.En)]
    public string ResourceName { get; set; }


    [PublicEnum.Language.Language(LanguageEnum.Th)]
    public string ResourceTh { get; set; }


    [PublicEnum.Language.Language(LanguageEnum.Vn)]
    public string ResourceVn { get; set; }


    [PublicEnum.Language.Language(LanguageEnum.Cn)]
    public string ResourceCs { get; set; }


    [PublicEnum.Language.Language(LanguageEnum.Ch)]
    public string ResourceCh { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Sc2)]
    public string ResourceSc2 => ResourceCs;

    [PublicEnum.Language.Language(LanguageEnum.Id)]
    public string ResourceIn { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Jp)]
    public string ResourceJp { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Ko)]
    public string ResourceKo { get; set; }
}