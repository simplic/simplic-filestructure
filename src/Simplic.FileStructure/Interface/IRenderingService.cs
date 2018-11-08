namespace Simplic.FileStructure
{
    /// <summary>
    /// File structure rendering service
    /// </summary>
    public interface IRenderingService
    {
        /// <summary>
        /// Renders the directory tree of a given file structure
        /// </summary>
        /// <param name="fileStructure">File structure to render</param>
        /// <returns>Directory structure in HTML</returns>
        string Render(FileStructure fileStructure);
    }
}
