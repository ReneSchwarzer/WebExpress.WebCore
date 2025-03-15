using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebSettingPage;
using WebExpress.WebCore.WebSettingPage.Model;

namespace WebExpress.WebCore.Test
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
