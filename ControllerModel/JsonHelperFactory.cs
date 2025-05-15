namespace LibrairieJsonHelper
{
    public class JsonHelperFactory
    {
        public static ILoggerWriter CreateLogger()
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
