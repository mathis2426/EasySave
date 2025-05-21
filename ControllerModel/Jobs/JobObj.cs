namespace ControllerModel.Jobs
{
    public enum JobType : byte
    {
        Differential,
        Full
    }
    public class JobObj
    {
        public string Name { get;}
        public string SourcePath { get;}
        public string TargetPath { get;}
        public JobType Type { get;}

        /// <summary>
        /// Initialise une nouvelle instance de JobObj avec les paramètres spécifiés.
        /// </summary>
        /// <param name="Name">Nom du job.</param>
        /// <param name="SourcePath">Chemin source des fichiers.</param>
        /// <param name="TargetPath">Chemin cible pour la sauvegarde.</param>
        /// <param name="Type">Type de sauvegarde.</param>
        public JobObj(string Name, string SourcePath, string TargetPath, JobType Type)
        {
            this.Name = Name;
            this.SourcePath = SourcePath;
            this.TargetPath = TargetPath;
            this.Type = Type;
        }

    }
}
