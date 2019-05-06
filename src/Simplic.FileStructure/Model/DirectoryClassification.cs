using System;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Represents a Directory Classification
    /// </summary>
    public class DirectoryClassification
    {
        /// <summary>
        /// Gets or sets the directory classification GUID
        /// </summary>
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();


        /// <summary>
        /// Gets or sets the classification name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Compares an object with the directory classification instance
        /// </summary>
        /// <param name="obj">Object to compare with</param>
        /// <returns>True if the directory classification has the same guid</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = obj as DirectoryClassification;
            if (other == null)
                return false;

            return other.Id == Id;
        }

        /// <summary>
        /// Get the hash code of the classification Id
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
