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

        public JobObj(string _name, string _sourcePath, string _targetPath, jobType _type)
        {
            this._name = _name;
            this._sourcePath = _sourcePath;
            this._targetPath = _targetPath;
            this._type = _type;
        }

    }
}
