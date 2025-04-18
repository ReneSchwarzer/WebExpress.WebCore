using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.Test.WWW.Settings
{
    /// <summary>
    /// A dummy setting group for testing purposes.
    /// </summary>
    [Name("SettingGroup C")]
    [Description("Description of group c.")]
    [SettingSection(SettingSection.Secondary)]
    public sealed class TestSettingGroupC : ISettingGroup
    {
    }
}
