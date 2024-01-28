namespace FDRWebsite.Shared.Helpers
{
    public static class FileFormats
    {
        public static readonly string[] SUPPORTED_IMAGES = { ".png", ".jpg", ".jpeg" };

        public static readonly string[] SUPPORTED_VIDEOS = { ".mp4" };

        public static bool IsSupportedMedia(string filename)
        {
            return IsSupportedImage(filename) || IsSupportedVideo(filename);
        }

        public static bool IsSupportedImage(string fileName)
        {
            return SUPPORTED_IMAGES.Contains(Path.GetExtension(fileName.ToLower()));
        }

        public static bool IsSupportedVideo(string fileName)
        {
            return SUPPORTED_VIDEOS.Contains(Path.GetExtension(fileName.ToLower()));
        }
    }
}
