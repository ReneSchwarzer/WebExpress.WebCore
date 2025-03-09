using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebSettingPage;
using WebExpress.WebCore.WebSettingPage.Model;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy setting category for testing purposes.
    /// </summary>
    [Icon("InfoCircle")]
    [Name("SettingCategory B")]
    [Description("Description of category b.")]
    [SettingSection(SettingSection.Primary)]
    public sealed class TestSettingCategoryB : ISettingCategory
    {
    }
}
