namespace ControllerModel.Jobs
{
    public enum JobType : byte
    {
        Differential,
        Full
    }
    public class JobObj
    {
        public int Id { get; set; }
        public string Name { get;}
        public string SourcePath { get;}
        public string TargetPath { get;}
        public JobType Type { get;}

        /// <summary>
        /// Initializes a new instance of JobObj with the specified parameters.
        /// </summary>
        /// <param name="Id">Job ID</param>
        /// <param name="Name">Job name. </param>
        /// <param name="SourcePath">File source path.</param>
        /// <param name="TargetPath">Target path for backup.</param>
        /// <param name="Type">Backup type.</param>
        public JobObj(int Id, string Name, string SourcePath, string TargetPath, JobType Type)
        {
            this.Id = Id;
            this.Name = Name;
            this.SourcePath = SourcePath;
            this.TargetPath = TargetPath;
            this.Type = Type;
        }

    }
}
