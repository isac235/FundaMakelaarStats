namespace FundaMakelaarStats.Models.ApiResponse
{
    public record PromoLabel(
        bool HasPromotionLabel,
        List<object> PromotionPhotos,
        object PromotionPhotosSecure,
        int? PromotionType,
        int? RibbonColor,
        string RibbonText,
        string Tagline
        );
}
