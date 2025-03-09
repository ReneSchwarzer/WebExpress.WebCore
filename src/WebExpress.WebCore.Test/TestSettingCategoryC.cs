using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebSettingPage;
using WebExpress.WebCore.WebSettingPage.Model;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy setting category for testing purposes.
    /// </summary>
    [Icon("InfoCircle")]
    [Name("SettingCategory C")]
    [Description("Description of category c.")]
    [SettingSection(SettingSection.Secondary)]
    public sealed class TestSettingCategoryC : ISettingCategory
    {
    }
}
