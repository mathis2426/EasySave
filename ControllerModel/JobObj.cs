namespace ControllerModel
{
    public enum jobType
    {
        Differential,
        Full
    }
    public class JobObj
    {
        public string _name { get;}
        public string _sourcePath { get;}
        public string _targetPath { get;}
        public jobType _type { get;}

        public JobObj(string name, string sourcePath, string targetPath, jobType type)
        {
            _name = name;
            _sourcePath = sourcePath;
            _targetPath = targetPath;
            _type = type;
        }

    }
}
