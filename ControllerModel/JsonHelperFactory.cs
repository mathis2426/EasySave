namespace LibrairieJsonHelper
{
    public class JsonHelperFactory
    {
        public ILoggerWriter CreateLogger()
        {      
            return new JsonHelperClassLogger();   
        }
        public static JsonHelperClassJsonUpdate CreateJsonUpdate()
        {     
            return new JsonHelperClassJsonUpdate();
        }
        public static JsonHelperClassJsonReadMultipleObj CreateJsonReadMultipleObj()
        {
            return new JsonHelperClassJsonReadMultipleObj();
        }
        public static JsonHelperClassJsonReadSingleObj CreateJsonReadSingleObj()
        {
            return new JsonHelperClassJsonReadSingleObj();
        }
    }
}
