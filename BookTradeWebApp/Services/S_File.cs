namespace BookTradeWebApp.Services
{
    public interface IS_File
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
    public class S_File : IS_File
    {
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        string imageUploadFolder = "uploads";

        public S_File(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            bool isValidExtension = _allowedExtensions.Contains(extension);
            if (isValidExtension)
            {
                var folder = Path.Combine("images", imageUploadFolder);
                var uploadPath = Path.Combine(_env.WebRootPath, folder);
                Directory.CreateDirectory(uploadPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Path.Combine(folder, fileName).Replace("\\", "/");
            }
            return string.Empty;
        }
    }
}
