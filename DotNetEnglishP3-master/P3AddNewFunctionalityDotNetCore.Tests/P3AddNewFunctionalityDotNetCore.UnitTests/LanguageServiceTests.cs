using P3AddNewFunctionalityDotNetCore.Models.Services;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.P3AddNewFunctionalityDotNetCore.UnitTests
{
    public class LanguageServiceTests
    {
        [Fact]
        public void SetCulture_GetCultureFrench_ReturnFr()
        {
            // Arrange
            ILanguageService languageService = new LanguageService();
            string language = "French";

            // Act
            string culture = languageService.SetCulture(language);

            // Assert
            Assert.Equal("fr", culture);
        }

        [Fact]
        public void SetCulture_GetCultureEnglishAndDefault_ReturnEn()
        {
            // Arrange
            ILanguageService languageService = new LanguageService();
            string language = "English";
            string languageDefault = "";

            // Act
            string culture = languageService.SetCulture(language);
            string cultureDefault = languageService.SetCulture(languageDefault);

            // Assert
            Assert.Equal("en", culture);
            Assert.Equal("en", cultureDefault);
        }

        [Fact]
        public void SetCulture_GetCultureSpanish_ReturnEs()
        {
            // Arrange
            ILanguageService languageService = new LanguageService();
            string language = "Spanish";

            // Act
            string culture = languageService.SetCulture(language);

            // Assert
            Assert.Equal("es", culture);
        }
    }
}